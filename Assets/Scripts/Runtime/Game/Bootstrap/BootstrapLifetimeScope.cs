using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Services;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Controllers;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Bootstrap
{
    public class BootstrapLifetimeScope : LifetimeScope
    {
        [SerializeField] private PlayerView _playerView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
            builder.Register<IPlayerInputService, PlayerInputService>(Lifetime.Singleton);
            builder.Register<IUiInputService, UiInputService>(Lifetime.Singleton);
            builder.Register<IInputSystemManager, InputSystemManager>(Lifetime.Singleton);

            builder.Register<PlayerController>(Lifetime.Singleton).As<IPlayerController, ITickable>();

            builder.RegisterComponent(_playerView).As<IPlayerView>();

            builder.RegisterEntryPoint<BootstrapInitializer>();
        }
    }
}
