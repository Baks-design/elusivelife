using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;
using VContainer.Unity;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Bootstrap
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
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
            _playerInputService = playerInputService ?? throw new ArgumentNullException(nameof(playerInputService));
            _uiInputService = uiInputService ?? throw new ArgumentNullException(nameof(uiInputService));
            _inputSystemManager = inputSystemManager ?? throw new ArgumentNullException(nameof(inputSystemManager));
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            try
            {
                _playerInputService.Initialize();
                _uiInputService.Initialize();

                await _sceneLoader.LoadSceneAsync("GameScene", cancellation);

                await UniTask.NextFrame(cancellation);

                _inputSystemManager.SwitchToPlayerInput();
                GameSystem.SetCursor(true);
            }
            catch (OperationCanceledException)
            {
                Logging.Log("Bootstrap initialization cancelled");
                throw;
            }
            catch (Exception ex)
            {
                Logging.LogError($"Bootstrap initialization failed: {ex.Message}");
                throw;
            }
        }
    }
}