using System;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;
using UnityEngine.InputSystem;

namespace ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Maps
{
    public class UiMapInputActions : IDisposable
    {
        private bool _initialized;

        public InputActionMap Ui { get; private set; }
        public InputAction ClosePause { get; private set; }

        public void Initialize()
        {
            if (_initialized) return;

            try
            {
                GetMap();
                GetUiActions();
                ValidateActions();
                _initialized = true;
            }
            catch (Exception ex)
            {
                Logging.LogError($"Failed to initialize UiMapInputActions: {ex.Message}");
                throw;
            }
        }

        private void GetMap()
        {
            Ui = InputSystem.actions?.FindActionMap("UI");
            if (Ui == null)
                throw new InvalidOperationException("UI action map not found in InputSystem");
        }

        private void GetUiActions() => ClosePause = Ui.FindAction("ClosePause");

        private void ValidateActions()
        {
            if (ClosePause == null)
                throw new InvalidOperationException("ClosePause action not found in UI action map");
        }

        public void Dispose()
        {
            Ui?.Disable();
            Ui = null;
            ClosePause = null;
            _initialized = false;
        }
    }
}