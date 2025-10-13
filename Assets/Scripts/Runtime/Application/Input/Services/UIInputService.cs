using System;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Maps;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;

namespace ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Services
{
    public class UiInputService : IUiInputService
    {
        private UiMapInputActions _inputActions;

        public void Initialize()
        {
            try
            {
                _inputActions = new UiMapInputActions();
                _inputActions.Initialize();

                ValidateInputActions();

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

        public void Enable() => _inputActions.Ui.Enable();
        public void Disable() => _inputActions.Ui.Disable();

        public bool ClosePause() => _inputActions?.ClosePause?.WasPressedThisFrame() ?? false;
    }
}