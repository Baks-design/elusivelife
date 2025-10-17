using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using ElusiveLife.Runtime.Application.Scene_Management;
using ElusiveLife.Runtime.Application.Input.Interfaces;
using ElusiveLife.Runtime.Application.Game_State;
using ElusiveLife.Runtime.Application.Persistence;

namespace ElusiveLife.Runtime.Application.Bootstrapper.Initializers
{
    public class BootstrapInitializer : IAsyncStartable
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IPlayerInputService _playerInputService;
        private readonly IUiInputService _uiInputService;
        private readonly IGameLifecycle _gameLifecycle;
        private readonly IDataService _gameDataService;
        
        public BootstrapInitializer(
            ISceneLoader sceneLoader,
            IPlayerInputService playerInputService,
            IUiInputService uiInputService,
            IGameLifecycle gameLifecycle,
            IDataService gameDataService) 
        {
            _sceneLoader = sceneLoader;
            _playerInputService = playerInputService;
            _uiInputService = uiInputService;
            _gameLifecycle = gameLifecycle;
            _gameDataService = gameDataService;
        }

        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            _playerInputService.Initialize();
            _uiInputService.Initialize();
            await _gameDataService.LoadGameAsync();
            await _sceneLoader.LoadSceneAsync("GameScene", cancellation);
            _gameLifecycle.Initialize();
        }
    }
}