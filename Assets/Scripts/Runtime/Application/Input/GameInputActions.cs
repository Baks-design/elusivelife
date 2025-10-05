using UnityEngine;
using UnityEngine.InputSystem;

namespace ElusiveLife.Application.Input
{
    [CreateAssetMenu(menuName = "Config/Input/GameInputActions")]
    public class GameInputActions : ScriptableObject
    {
        //Player Actions
        public InputActionMap Player { get; private set; }
        public InputAction OpenPause { get; private set; }
        public InputAction Look { get; private set; }
        public InputAction Aim { get; private set; }
        public InputAction Fire { get; private set; }
        public InputAction Move { get; private set; }
        public InputAction Run { get; private set; }
        public InputAction Jump { get; private set; }
        public InputAction Crouch { get; private set; }
        //UI Actions
        public InputActionMap UI { get; private set; }
        public InputAction ClosePause { get; private set; }

        public void Initialize()
        {
            Player = InputSystem.actions.FindActionMap("Player");
            OpenPause = Player.FindAction("OpenPause");
            Look = Player.FindAction("Look");
            Aim = Player.FindAction("Aim");
            Fire = Player.FindAction("Fire");
            Move = Player.FindAction("Move");
            Run = Player.FindAction("Run");
            Jump = Player.FindAction("Jump");
            Crouch = Player.FindAction("Crouch");

            UI = InputSystem.actions.FindActionMap("UI");
            ClosePause = UI.FindAction("ClosePause");
        }
    }
}