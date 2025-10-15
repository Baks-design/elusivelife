using System.Threading;
using Cysharp.Threading.Tasks;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Scenes;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;
using VContainer.Unity;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Bootstrap.Initializers
{
    public class BootstrapInitializer : IAsyncStartable
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IPlayerInputService _playerInputService;
        private readonly IUiInputService _uiInputService;
        private readonly IInputSystemManager _inputSystemManager;

        public BootstrapInitializer(
            ISceneLoader sceneLoader,
            IPlayerInputService playerInputService,
            IUiInputService uiInputService,
            IInputSystemManager inputSystemManager)
        {
            _sceneLoader = sceneLoader;
            _playerInputService = playerInputService;
            _uiInputService = uiInputService;
            _inputSystemManager = inputSystemManager;
        }

        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            _playerInputService.Initialize();
            _uiInputService.Initialize();

            await _sceneLoader.LoadSceneAsync("GameScene", cancellation);
            
            await UniTask.WaitForEndOfFrame(cancellation);
            
            await UniTask.NextFrame(cancellation);

            _inputSystemManager.SwitchToPlayerInput();
            GameSystem.SetCursor(true);
        }
    }
}