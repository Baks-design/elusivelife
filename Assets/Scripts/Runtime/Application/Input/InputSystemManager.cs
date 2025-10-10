using System;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;

namespace ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input
{
    public class InputSystemManager : IInputSystemManager
    {
        private readonly IPlayerInputService _playerInputService;
        private readonly IUiInputService _uiInputService;
        private bool _disposed;

        public InputSystemManager(
            IPlayerInputService playerInputService,
            IUiInputService uiInputService)
        {
            _playerInputService = playerInputService ?? throw new ArgumentNullException(nameof(playerInputService));
            _uiInputService = uiInputService ?? throw new ArgumentNullException(nameof(uiInputService));
        }

        public void SwitchToPlayerInput()
        {
            CheckDisposed();

            try
            {
                _uiInputService.Disable();
                _playerInputService.Enable();
                Logging.Log("Switched to Player input");
            }
            catch (Exception ex)
            {
                Logging.LogError($"Failed to switch to player input: {ex.Message}");
                throw;
            }
        }

        public void SwitchToUiInput()
        {
            CheckDisposed();

            try
            {
                _playerInputService.Disable();
                _uiInputService.Enable();
                Logging.Log("Switched to UI input");
            }
            catch (Exception ex)
            {
                Logging.LogError($"Failed to switch to UI input: {ex.Message}");
                throw;
            }
        }

        public void DisableAllInput()
        {
            CheckDisposed();

            try
            {
                _playerInputService.Disable();
                _uiInputService.Disable();
                Logging.Log("All input disabled");
            }
            catch (Exception ex)
            {
                Logging.LogError($"Failed to disable all input: {ex.Message}");
                throw;
            }
        }

        private void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(InputSystemManager));
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                DisableAllInput();
                _playerInputService?.Dispose();
                _uiInputService?.Dispose();
            }
            finally
            {
                _disposed = true;
            }
        }
    }
}