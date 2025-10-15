using Alchemy.Inspector;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Bootstrap.Initializers;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Manager;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Views;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Spawner;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Bootstrap.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField, AssetsOnly, Required] private PlayerView _playerPrefab;
        [SerializeField, Required] private Transform _playerSpawnPoint;

        protected override void Configure(IContainerBuilder builder)
        {
            // Register instances
            builder.RegisterInstance(_playerPrefab);
            builder.RegisterInstance(_playerSpawnPoint);

            // Register factories
            builder.Register<PlayerFactory>(Lifetime.Singleton);
            builder.Register<PlayerControllerFactory>(Lifetime.Singleton);

            // Register managers and spawners
            builder.Register<PlayerSpawner>(Lifetime.Singleton);
            builder.Register<GameManager>(Lifetime.Singleton);

            // Use a proxy pattern to avoid null references
            builder.Register<PlayerControllerProxy>(Lifetime.Singleton)
                .As<IPlayerController, ITickable, ILateTickable>();

            // Register entry point
            builder.RegisterEntryPoint<GameInitializer>();
        }
    }
}