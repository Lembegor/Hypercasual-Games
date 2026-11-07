using Game.Core;
using Game.Input;
using Game.Level;
using Game.Player;
using Game.Shots;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private BallConfig _ballConfig;
        [SerializeField] private PlayerBall _player;
        [SerializeField] private Shot _shotPrefab;

        public override void InstallBindings()
        {
            Container.BindInstance(_levelConfig);
            Container.BindInstance(_ballConfig);
            Container.BindInstance(_player);

            Container.Bind<GameController>().AsSingle();
            Container.BindInterfacesAndSelfTo<TapInput>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShotCharger>().AsSingle();
            Container.BindFactory<Shot, Shot.Factory>().FromComponentInNewPrefab(_shotPrefab);
        }
    }
}
