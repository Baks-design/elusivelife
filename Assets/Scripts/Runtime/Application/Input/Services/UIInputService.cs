using ElusiveLife.Runtime.Application.Input.Interfaces;
using ElusiveLife.Runtime.Application.Input.Maps;

namespace ElusiveLife.Runtime.Application.Input.Services
{
    public class UiInputService : IUiInputService
    {
        private UiMapInputActions _inputActions;

        public void Initialize()
        {
            _inputActions = new UiMapInputActions();
            _inputActions.Initialize();
        }

        public void Enable() => _inputActions.Ui.Enable();
        public void Disable() => _inputActions.Ui.Disable();

        public bool ClosePause() => _inputActions.ClosePause.WasPressedThisFrame();
    }
}