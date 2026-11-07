using System;
using Game.Core;
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
        private readonly GameController _game;
        private readonly Shot.Factory _shotFactory;

        private Shot _shot;
        private int _activeShots;

        public bool IsCharging => _shot != null;
        public bool IsBusy => IsCharging || _activeShots > 0;

        public ShotCharger(
            TapInput input,
            PlayerBall player,
            BallConfig ballConfig,
            LevelConfig levelConfig,
            GameController game,
            Shot.Factory shotFactory)
        {
            _input = input;
            _player = player;
            _ballConfig = ballConfig;
            _levelConfig = levelConfig;
            _game = game;
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
            if (IsCharging)
                Transfer(_ballConfig.ChargeSpeed * Time.deltaTime);
        }

        private void OnPressed()
        {
            if (_game.State != GameState.Playing)
                return;

            _shot = _shotFactory.Create();
            Transfer(_ballConfig.MinShotSize);
        }

        private void OnReleased()
        {
            if (!IsCharging)
                return;

            _activeShots++;
            _shot.Finished += OnShotFinished;
            _shot.Launch(GetDirection(), _player.PathWidth);
            _shot = null;
        }

        private void OnShotFinished()
        {
            _activeShots--;
        }

        private void Transfer(float amount)
        {
            _player.Shrink(amount);
            _shot.Grow(amount);
            PlaceShot();

            if (_player.IsCritical)
            {
                _shot = null;
                _game.Lose();
            }
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
