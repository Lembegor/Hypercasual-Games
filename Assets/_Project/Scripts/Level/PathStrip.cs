using Game.Player;
using UnityEngine;
using Zenject;

namespace Game.Level
{
    public class PathStrip : MonoBehaviour
    {
        private LevelConfig _config;
        private PlayerBall _player;

        [Inject]
        private void Construct(LevelConfig config, PlayerBall player)
        {
            _config = config;
            _player = player;
        }

        private void Start()
        {
            Vector3 path = _config.DoorPosition - _config.StartPosition;
            transform.SetPositionAndRotation(_config.StartPosition + path / 2f, Quaternion.LookRotation(path));

            _player.SizeChanged += UpdateSize;
            UpdateSize();
        }

        private void OnDestroy()
        {
            _player.SizeChanged -= UpdateSize;
        }

        private void UpdateSize()
        {
            float length = Vector3.Distance(_config.StartPosition, _config.DoorPosition);
            transform.localScale = new Vector3(_player.PathWidth, transform.localScale.y, length);
        }
    }
}
