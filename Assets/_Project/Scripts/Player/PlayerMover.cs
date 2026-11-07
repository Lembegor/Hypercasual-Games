using Game.Core;
using Game.Level;
using Game.Shots;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    public class PlayerMover : MonoBehaviour
    {
        private const float VisualCenterHeight = 0.5f;

        [SerializeField] private PlayerBall _player;
        [SerializeField] private Transform _visual;
        [SerializeField] private LayerMask _obstacleMask;

        private BallConfig _ballConfig;
        private LevelConfig _levelConfig;
        private GameController _game;
        private ShotCharger _charger;

        private Vector3 _hopStart;
        private Vector3 _hopEnd;
        private float _hopProgress = 1f;

        [Inject]
        private void Construct(
            BallConfig ballConfig,
            LevelConfig levelConfig,
            GameController game,
            ShotCharger charger)
        {
            _ballConfig = ballConfig;
            _levelConfig = levelConfig;
            _game = game;
            _charger = charger;
        }

        private void Update()
        {
            if (_game.State != GameState.Playing)
                return;

            if (_hopProgress < 1f)
                UpdateHop();
            else if (!_charger.IsBusy)
                TryMove();
        }

        private void TryMove()
        {
            float doorDistance = Vector3.Distance(transform.position, _levelConfig.DoorPosition);

            if (doorDistance <= _ballConfig.MinHopLength)
            {
                _game.Win();
                return;
            }

            float freeDistance = GetFreeDistance(doorDistance);

            if (freeDistance >= _ballConfig.MinHopLength)
                StartHop(Mathf.Min(freeDistance, _ballConfig.HopLength));
            else if (!_player.CanShoot)
                _game.Lose();
        }

        private float GetFreeDistance(float maxDistance)
        {
            float radius = _player.PathWidth / 2f;
            Vector3 origin = transform.position + Vector3.up * radius;
            Vector3 direction = GetDirection();
            float freeDistance = maxDistance;

            foreach (RaycastHit hit in Physics.SphereCastAll(origin, radius, direction, maxDistance, _obstacleMask))
                freeDistance = Mathf.Min(freeDistance, hit.distance);

            return freeDistance;
        }

        private void StartHop(float length)
        {
            _hopStart = transform.position;
            _hopEnd = _hopStart + GetDirection() * length;
            _hopProgress = 0f;
        }

        private void UpdateHop()
        {
            _hopProgress = Mathf.Min(_hopProgress + Time.deltaTime / _ballConfig.HopDuration, 1f);
            transform.position = Vector3.Lerp(_hopStart, _hopEnd, _hopProgress);

            float height = Mathf.Sin(_hopProgress * Mathf.PI) * _ballConfig.HopHeight;
            _visual.localPosition = Vector3.up * (VisualCenterHeight + height / _player.Size);
        }

        private Vector3 GetDirection()
        {
            return (_levelConfig.DoorPosition - transform.position).normalized;
        }
    }
}
