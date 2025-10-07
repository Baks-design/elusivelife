namespace ElusiveLife.Application.Input
{
    public class UIInputService : IUIInputService
    {
        readonly GameInputActions inputActions;

        public UIInputService(GameInputActions inputActions)
        => this.inputActions = inputActions;

        public void Enable() => inputActions.UI.Enable();
        public void Disable() => inputActions.UI.Disable();

        public bool ClosePause() => inputActions.ClosePause.WasPressedThisFrame();
    }
}