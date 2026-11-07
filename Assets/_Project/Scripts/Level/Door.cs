using Game.Player;
using UnityEngine;
using Zenject;

namespace Game.Level
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Transform _leftLeaf;
        [SerializeField] private Transform _rightLeaf;
        [SerializeField] private float _openOffset = 1.4f;
        [SerializeField] private float _openSpeed = 3f;

        private LevelConfig _config;
        private PlayerBall _player;
        private Vector3 _leftTarget;
        private Vector3 _rightTarget;
        private bool _isOpen;

        [Inject]
        private void Construct(LevelConfig config, PlayerBall player)
        {
            _config = config;
            _player = player;
        }

        private void Start()
        {
            Vector3 path = _config.DoorPosition - _config.StartPosition;
            transform.SetPositionAndRotation(_config.DoorPosition, Quaternion.LookRotation(path));

            _leftTarget = _leftLeaf.localPosition + Vector3.left * _openOffset;
            _rightTarget = _rightLeaf.localPosition + Vector3.right * _openOffset;
        }

        private void Update()
        {
            if (_isOpen)
                MoveLeaves();
            else
                _isOpen = Vector3.Distance(_player.transform.position, transform.position) <= _config.DoorOpenDistance;
        }

        private void MoveLeaves()
        {
            float step = _openSpeed * Time.deltaTime;
            _leftLeaf.localPosition = Vector3.MoveTowards(_leftLeaf.localPosition, _leftTarget, step);
            _rightLeaf.localPosition = Vector3.MoveTowards(_rightLeaf.localPosition, _rightTarget, step);
        }
    }
}
