using System;
using Game.Input;
using Game.Level;
using Game.Player;
using UnityEngine;
using Zenject;

namespace Game.Shots
{
    public class ShotCharger : IInitializable, ITickable, IDisposable
    {
        private readonly TapInput _input;
        private readonly PlayerBall _player;
        private readonly BallConfig _ballConfig;
        private readonly LevelConfig _levelConfig;
        private readonly Shot.Factory _shotFactory;

        private Shot _shot;

        public ShotCharger(
            TapInput input,
            PlayerBall player,
            BallConfig ballConfig,
            LevelConfig levelConfig,
            Shot.Factory shotFactory)
        {
            _input = input;
            _player = player;
            _ballConfig = ballConfig;
            _levelConfig = levelConfig;
            _shotFactory = shotFactory;
        }

        public void Initialize()
        {
            _input.Pressed += OnPressed;
            _input.Released += OnReleased;
        }

        public void Dispose()
        {
            _input.Pressed -= OnPressed;
            _input.Released -= OnReleased;
        }

        public void Tick()
        {
            if (_shot == null)
                return;

            Transfer(_ballConfig.ChargeSpeed * Time.deltaTime);
        }

        private void OnPressed()
        {
            _shot = _shotFactory.Create();
            Transfer(_ballConfig.MinShotSize);
        }

        private void OnReleased()
        {
            if (_shot == null)
                return;

            _shot.Launch(GetDirection());
            _shot = null;
        }

        private void Transfer(float amount)
        {
            _player.Shrink(amount);
            _shot.Grow(amount);
            PlaceShot();
        }

        private void PlaceShot()
        {
            float distance = (_player.Size + _shot.Size) / 2f;
            _shot.transform.position = _player.transform.position + GetDirection() * distance;
        }

        private Vector3 GetDirection()
        {
            return (_levelConfig.DoorPosition - _player.transform.position).normalized;
        }
    }
}
