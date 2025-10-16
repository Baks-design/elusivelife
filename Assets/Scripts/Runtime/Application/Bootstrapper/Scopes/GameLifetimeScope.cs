using Alchemy.Inspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ElusiveLife.Runtime.Game.Player.Views;
using ElusiveLife.Runtime.Game.Player.Controllers;
using ElusiveLife.Runtime.Game.Spawner;
using ElusiveLife.Runtime.Application.Game_State;
using ElusiveLife.Runtime.Game.Player.Interfaces;
using ElusiveLife.Runtime.Application.Bootstrapper.Initializers;

namespace ElusiveLife.Runtime.Application.Bootstrapper.Scopes
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

            // Use a proxy pattern to avoid null references
            builder.Register<PlayerControllerProxy>(Lifetime.Singleton)
                .As<IPlayerController, ITickable, ILateTickable>();

            // Register entry point
            builder.RegisterEntryPoint<GameInitializer>();
        }
    }
}