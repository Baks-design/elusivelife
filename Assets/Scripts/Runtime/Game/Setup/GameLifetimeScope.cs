using ElusiveLife.Application.Input;
using ElusiveLife.Application.Time;
using ElusiveLife.Game.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ElusiveLife.Game.Setup
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Settings")]
        [SerializeField] GameInputActions inputActionsAsset;

        [Header("Views")]
        [SerializeField] PlayerView playerView;

        protected override void Configure(IContainerBuilder builder)
        {
            // Input
            inputActionsAsset.Initialize();
            builder.RegisterInstance(inputActionsAsset);
            builder.Register<IPlayerInputService, PlayerInputService>(Lifetime.Singleton)
                   .WithParameter(typeof(GameInputActions), inputActionsAsset);

            // Views
            builder.RegisterComponent(playerView).As<IPlayerView>();
            builder.UseEntryPoints(config =>
            {
                config.Add<PlayerController>();
                config.Add<TimeService>().As<ITimeService>();
            });
        }
    }
}
