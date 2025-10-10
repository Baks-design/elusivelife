using System;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Maps;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;

namespace ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Services
{
    public class UiInputService : IUiInputService
    {
        private UiMapInputActions _inputActions;
        private bool _isInitialized;
        private bool _disposed;

        public void Initialize()
        {
            if (_isInitialized)
                return;

            try
            {
                _inputActions = new UiMapInputActions();
                _inputActions.Initialize();

                ValidateInputActions();
                _isInitialized = true;

                Logging.Log("UiInputService initialized successfully");
            }
            catch (Exception ex)
            {
                Logging.LogError($"Failed to initialize input system: {ex.Message}");
                throw;
            }
        }

        private void ValidateInputActions()
        {
            if (_inputActions.Ui == null)
                throw new InvalidOperationException("UI action map not found");

            if (_inputActions.ClosePause == null)
                throw new InvalidOperationException("ClosePause action not found");
        }

        public void Dispose()
        {
            if (_disposed) return;

            _inputActions?.Ui?.Disable();
            _inputActions = null;
            _isInitialized = false;
            _disposed = true;

            Logging.Log("UiInputService disposed");
        }

        private void CheckInitialized()
        {
            if (!_isInitialized || _disposed)
                throw new InvalidOperationException("UiInputService is not initialized or has been disposed");
        }

        public void Enable()
        {
            CheckInitialized();
            _inputActions.Ui.Enable();
        }

        public void Disable()
        {
            CheckInitialized();
            _inputActions.Ui.Disable();
        }

        public bool ClosePause() => _inputActions?.ClosePause?.WasPressedThisFrame() ?? false;
    }
}