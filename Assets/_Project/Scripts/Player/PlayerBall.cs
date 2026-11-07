using System;
using Game.Level;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    public class PlayerBall : MonoBehaviour
    {
        private BallConfig _ballConfig;
        private LevelConfig _levelConfig;

        public event Action SizeChanged;

        public float Size { get; private set; }
        public float PathWidth => Size * _ballConfig.PathWidthPerSize;
        public bool IsCritical => Size <= _ballConfig.CriticalSize;
        public bool CanShoot => Size - _ballConfig.MinShotSize > _ballConfig.CriticalSize;

        [Inject]
        private void Construct(BallConfig ballConfig, LevelConfig levelConfig)
        {
            _ballConfig = ballConfig;
            _levelConfig = levelConfig;
        }

        private void Awake()
        {
            transform.position = _levelConfig.StartPosition;
            SetSize(_ballConfig.StartSize);
        }

        public void Shrink(float amount)
        {
            SetSize(Mathf.Max(Size - amount, 0f));
        }

        private void SetSize(float size)
        {
            Size = size;
            transform.localScale = Vector3.one * size;
            SizeChanged?.Invoke();
        }
    }
}
