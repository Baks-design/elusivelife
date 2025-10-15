using UnityEngine.InputSystem;

namespace ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Maps
{
    public class PlayerMapInputActions
    {
        public InputActionMap Player { get; private set; }
        public InputAction OpenPause { get; private set; }
        public InputAction Look { get; private set; }
        public InputAction Aim { get; private set; }
        public InputAction Move { get; private set; }
        public InputAction Run { get; private set; }
        public InputAction Jump { get; private set; }
        public InputAction Crouch { get; private set; }

        public void Initialize()
        {
            GetMaps();
            GetPlayerActions();
        }

        private void GetMaps() => Player = InputSystem.actions.FindActionMap("Player");

        private void GetPlayerActions()
        {
            OpenPause = Player.FindAction("OpenPause");
            Look = Player.FindAction("Look");
            Aim = Player.FindAction("Aim");
            Move = Player.FindAction("Move");
            Run = Player.FindAction("Run");
            Jump = Player.FindAction("Jump");
            Crouch = Player.FindAction("Crouch");
        }
    }
}