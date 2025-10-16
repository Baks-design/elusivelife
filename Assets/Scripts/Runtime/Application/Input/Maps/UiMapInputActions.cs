using UnityEngine.InputSystem;

namespace ElusiveLife.Runtime.Application.Input.Maps
{
    public class UiMapInputActions
    {
        public InputActionMap Ui { get; private set; }
        public InputAction ClosePause { get; private set; }

        public void Initialize()
        {
            GetMap();
            GetUiActions();
        }

        private void GetMap() => Ui = InputSystem.actions.FindActionMap("UI");

        private void GetUiActions() => ClosePause = Ui.FindAction("ClosePause");
    }
}