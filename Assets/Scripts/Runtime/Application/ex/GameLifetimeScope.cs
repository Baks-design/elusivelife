using EngineRoom.Examples.Controllers;
using EngineRoom.Examples.Handlers;
using EngineRoom.Examples.Interfaces;
using EngineRoom.Examples.Services;
using EngineRoom.Examples.Settings;
using EngineRoom.Examples.Utils;
using EngineRoom.Examples.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace EngineRoom.Examples
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Settings")]
        [SerializeField]
        ControlsSettings controlsSettings;

        [SerializeField]
        GameSettings gameSettings;

        [Header("Views")]
        [SerializeField]
        PlayerView playerView;

        [SerializeField]
        BulletView bulletViewPrefab;

        [SerializeField]
        TargetView[] targetSpawnPointViews;

        [Header("Various")]
        [SerializeField]
        Transform bulletSpawnPoint;

        protected override void Configure(IContainerBuilder builder)
        {
            // Services
            builder.Register<IBulletsService, BulletsService>(Lifetime.Singleton);
            builder.Register<IScoreService, ScoreService>(Lifetime.Singleton);

            // Various
            builder.Register<IInputHandler, KeyboardInputHandler>(Lifetime.Singleton);
            builder.RegisterInstance(bulletSpawnPoint).Keyed(TransformKey.BulletSpawn);
            builder.Register<ITargetController, TargetController>(Lifetime.Transient);

            // Settings
            builder.RegisterInstance(controlsSettings).As<IControlsSettings>();
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
