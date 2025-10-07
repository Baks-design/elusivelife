using UnityEngine;
using UnityEngine.InputSystem;

namespace ElusiveLife.Application.Input
{
    [CreateAssetMenu(menuName = "Config/Input/GameInputActions")]
    public class GameInputActions : ScriptableObject
    {
        public InputActionMap Player { get; private set; }
        public InputActionMap UI { get; private set; }
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
            UI = InputSystem.actions.FindActionMap("UI");
            
            OpenPause = Player.FindAction("OpenPause");
            Look = Player.FindAction("Look");
            Aim = Player.FindAction("Aim");
            Move = Player.FindAction("Move");
            Run = Player.FindAction("Run");
            Jump = Player.FindAction("Jump");
            Crouch = Player.FindAction("Crouch");

            ClosePause = UI.FindAction("ClosePause");
        }
    }
}