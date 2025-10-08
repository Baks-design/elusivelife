using Alchemy.Inspector;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Services;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Time;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Controllers;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Setup
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField, Required] private PlayerView _playerView;
        private GameInputActions _inputActionsAsset;

        protected override void Configure(IContainerBuilder builder)
        {
            InputInitalize(builder);

            // Views
            builder.RegisterComponent(_playerView).As<IPlayerView>();
            builder.UseEntryPoints(config =>
            {
                config.Add<PlayerController>();
                config.Add<TimeService>().As<ITimeService>();
            });
        }

        private void InputInitalize(IContainerBuilder builder)
        {
            _inputActionsAsset = new GameInputActions();
            _inputActionsAsset.Initialize();
            builder.RegisterInstance(_inputActionsAsset);
            builder.Register<IPlayerInputService, PlayerInputService>(Lifetime.Singleton)
                .WithParameter(typeof(GameInputActions), _inputActionsAsset);
        }
    }
}
