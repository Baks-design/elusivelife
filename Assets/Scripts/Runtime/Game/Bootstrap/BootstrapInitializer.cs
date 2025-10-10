using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;
using VContainer.Unity;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Bootstrap
{
    public class BootstrapInitializer : IAsyncStartable, IDisposable
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IPlayerInputService _playerInputService;
        private readonly IUiInputService _uiInputService;
        private readonly IInputSystemManager _inputSystemManager;
        private bool _disposed;

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
            if (_disposed)
                throw new ObjectDisposedException(nameof(BootstrapInitializer));

            try
            {
                Logging.Log("Bootstrap initialization started");

                _playerInputService.Initialize();
                _uiInputService.Initialize();

                await _sceneLoader.LoadSceneAsync("GameScene", cancellation);

                // Wait for GameLifetimeScope to initialize
                await UniTask.NextFrame(cancellation);

                _inputSystemManager.SwitchToPlayerInput();
                GameSystem.SetCursor(true);

                Logging.Log("Bootstrap initialization completed");
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

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                _playerInputService?.Dispose();
                _uiInputService?.Dispose();
                _inputSystemManager?.Dispose();
            }
            finally
            {
                _disposed = true;
            }
        }
    }
}