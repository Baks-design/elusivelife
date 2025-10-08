using UnityEngine.InputSystem;

namespace ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input
{
    public class GameInputActions
    {
        //Maps
        public InputActionMap Player { get; private set; }
        public InputActionMap Ui { get; private set; }
        //Player Actions
        public InputAction OpenPause { get; private set; }
        public InputAction Look { get; private set; }
        public InputAction Aim { get; private set; }
        public InputAction Move { get; private set; }
        public InputAction Run { get; private set; }
        public InputAction Jump { get; private set; }
        public InputAction Crouch { get; private set; }
        //UI Actions
        public InputAction ClosePause { get; private set; }

        public void Initialize()
        {
            Player = InputSystem.actions.FindActionMap("Player");
            Ui = InputSystem.actions.FindActionMap("UI");

            OpenPause = Player.FindAction("OpenPause");
            Look = Player.FindAction("Look");
            Aim = Player.FindAction("Aim");
            Move = Player.FindAction("Move");
            Run = Player.FindAction("Run");
            Jump = Player.FindAction("Jump");
            Crouch = Player.FindAction("Crouch");

            ClosePause = Ui.FindAction("ClosePause");
        }
    }
}