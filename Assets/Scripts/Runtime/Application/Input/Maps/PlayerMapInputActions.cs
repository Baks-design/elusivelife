using System;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;
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
            try
            {
                GetMaps();
                GetPlayerActions();
                ValidateActions();
            }
            catch (Exception ex)
            {
                Logging.LogError($"Failed to initialize PlayerMapInputActions: {ex.Message}");
                throw;
            }
        }

        private void GetMaps()
        {
            Player = InputSystem.actions?.FindActionMap("Player");
            if (Player == null)
                throw new InvalidOperationException("Player action map not found in InputSystem");
        }

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

        private void ValidateActions()
        {
            var actions = new (InputAction action, string name)[]
            {
                (OpenPause, nameof(OpenPause)),
                (Look, nameof(Look)),
                (Aim, nameof(Aim)),
                (Move, nameof(Move)),
                (Run, nameof(Run)),
                (Jump, nameof(Jump)),
                (Crouch, nameof(Crouch))
            };
            foreach (var (action, name) in actions)
                if (action == null)
                    throw new InvalidOperationException($"Action {name} not found in Player action map");
        }
    }
}