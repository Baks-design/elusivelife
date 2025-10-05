using ElusiveLife.Application.Input;
using ElusiveLife.Application.Time;
using ElusiveLife.Game.Player;
using ElusiveLife.Game.Score;
using ElusiveLife.Game.Settings;
using ElusiveLife.Game.Target;
using ElusiveLife.Game.Weapon;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ElusiveLife.Game.Setup
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Settings")]
        [SerializeField] GameSettings gameSettings;
        [SerializeField] GameInputActions inputActionsAsset;

        [Header("Views")]
        [SerializeField] PlayerView playerView;
        [SerializeField] BulletView bulletViewPrefab;
        [SerializeField] TargetView[] targetSpawnPointViews;

        [Header("Various")]
        [SerializeField] Transform bulletSpawnPoint;

        protected override void Configure(IContainerBuilder builder)
        {
            // Services
            builder.Register<IBulletsService, BulletsService>(Lifetime.Singleton);
            builder.Register<IScoreService, ScoreService>(Lifetime.Singleton);

            // Various
            inputActionsAsset.Initialize();
            builder.RegisterInstance(inputActionsAsset);
            builder.Register<IPlayerInputService, PlayerInputService>(Lifetime.Singleton)
                   .WithParameter(typeof(GameInputActions), inputActionsAsset);
            builder.RegisterInstance(bulletSpawnPoint).Keyed(TransformKey.BulletSpawn);
            builder.Register<ITargetController, TargetController>(Lifetime.Transient);

            // Settings
            builder.RegisterInstance<IGameSettings>(gameSettings);

            // Views
            builder.RegisterComponent(playerView).As<IPlayerView>();
            builder.RegisterInstance(bulletViewPrefab);

            foreach (var targetSpawnPointView in targetSpawnPointViews)
                builder
                    .RegisterInstance<ITargetView>(targetSpawnPointView)
                    .Keyed(targetSpawnPointView.name);

            builder.UseEntryPoints(config =>
            {
                config.Add<PlayerController>();
                config.Add<TargetsService>();
                config.Add<TimeService>().As<ITimeService>();
            });
        }
    }
}
