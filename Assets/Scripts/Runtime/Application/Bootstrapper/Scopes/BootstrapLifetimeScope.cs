using Alchemy.Inspector;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;
using ElusiveLife.Runtime.Application.Bootstrapper.Initializers;
using ElusiveLife.Runtime.Application.Scene_Management;
using ElusiveLife.Runtime.Application.Input.Interfaces;
using ElusiveLife.Runtime.Application.Input.Services;
using ElusiveLife.Runtime.Application.Input;
using ElusiveLife.Runtime.Application.Game_State;
using ElusiveLife.Runtime.Application.UI;
using ElusiveLife.Runtime.Application.Persistence;

namespace ElusiveLife.Runtime.Application.Bootstrapper.Scopes
{
    public class BootstrapLifetimeScope : LifetimeScope
    {
        [SerializeField, AssetsOnly, Required] 
        private CinemachineBrain _cinemachineBrain;
        [SerializeField, AssetsOnly, Required] 
        private EventSystem _eventSystem;

        private void Start()
        {
            DontDestroyOnLoad(Instantiate(_cinemachineBrain));
            DontDestroyOnLoad(Instantiate(_eventSystem));
        }

        protected override void Configure(IContainerBuilder builder)
        {
            // Services
            builder.Register<SceneLoader>(Lifetime.Singleton).As<ISceneLoader>();
            builder.Register<PlayerInputService>(Lifetime.Singleton).As<IPlayerInputService>();
            builder.Register<UiInputService>(Lifetime.Singleton).As<IUiInputService>();
            builder.Register<InputSystemManager>(Lifetime.Singleton).As<IInputSystemManager>();
            builder.Register<GameLifecycle>(Lifetime.Singleton).As<IGameLifecycle, ITickable>();
            builder.Register<GameDataService>(Lifetime.Singleton).As<IGameDataService>();
            builder.Register<UIManager>(Lifetime.Singleton).As<IUIManager>();
    
            // Init
            builder.RegisterEntryPoint<BootstrapInitializer>();
        }
    }
}