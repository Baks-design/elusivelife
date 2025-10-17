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
        [Header("UI Prefabs")]
        [SerializeField, AssetsOnly, Required] private SlotSelectionUI slotSelectionUIPrefab;
        [SerializeField, AssetsOnly, Required] private SlotButton slotButtonPrefab;
        
        [Header("Player Prefabs")]
        [SerializeField, AssetsOnly, Required] private PlayerView _playerPrefab;
        [SerializeField, Required] private Transform _playerSpawnPoint;

        protected override void Configure(IContainerBuilder builder)
        {
            // Register UI
            builder.RegisterInstance(slotButtonPrefab.gameObject).As<GameObject>();
            builder.RegisterComponentInNewPrefab(slotSelectionUIPrefab, Lifetime.Singleton);
           
            // Register Player
            builder.RegisterInstance(_playerPrefab);
            builder.RegisterInstance(_playerSpawnPoint);
            builder.Register<PlayerFactory>(Lifetime.Singleton);
            builder.Register<PlayerControllerFactory>(Lifetime.Singleton);
            builder.Register<PlayerSpawner>(Lifetime.Singleton);
            builder.Register<PlayerControllerProxy>(Lifetime.Singleton) 
                .As<IPlayerController, ITickable, ILateTickable>();

            // Register entry point
            builder.RegisterEntryPoint<GameInitializer>();
        }
    }
}