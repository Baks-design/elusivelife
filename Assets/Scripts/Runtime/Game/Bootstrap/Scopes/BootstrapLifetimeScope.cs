using Alchemy.Inspector;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Services;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Bootstrap.Initializers;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Scenes;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Sound;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Bootstrap.Scopes
{
    public class BootstrapLifetimeScope : LifetimeScope
    {
        [SerializeField, AssetsOnly, Required] private CinemachineBrain _cinemachineBrain;
        [SerializeField, AssetsOnly, Required] private EventSystem _eventSystem;
        [SerializeField, AssetsOnly, Required] private SoundManager _soundManager;
        [SerializeField, AssetsOnly, Required] private MusicManager _musicManager;

        private void Start()
        {
            DontDestroyOnLoad(Instantiate(_cinemachineBrain));
            DontDestroyOnLoad(Instantiate(_eventSystem));
            DontDestroyOnLoad(Instantiate(_soundManager));
            DontDestroyOnLoad(Instantiate(_musicManager));
        }

        protected override void Configure(IContainerBuilder builder)
        {
            // Services
            //builder.Register<ISoundServices, SoundManager>(Lifetime.Singleton);
            builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
            builder.Register<IPlayerInputService, PlayerInputService>(Lifetime.Singleton);
            builder.Register<IUiInputService, UiInputService>(Lifetime.Singleton);
            builder.Register<IInputSystemManager, InputSystemManager>(Lifetime.Singleton);

            // Init
            builder.RegisterEntryPoint<BootstrapInitializer>();
        }
    }
}
