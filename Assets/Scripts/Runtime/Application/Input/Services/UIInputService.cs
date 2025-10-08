using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;

namespace ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Services
{
    public class UiInputService : IUiInputService
    {
        private readonly GameInputActions _inputActions;

        public UiInputService(GameInputActions inputActions) => _inputActions = inputActions;

        public void Enable() => _inputActions.Ui.Enable();
        public void Disable() => _inputActions.Ui.Disable();

        public bool ClosePause() => _inputActions.ClosePause.WasPressedThisFrame();
    }
}