using UnityEngine;

namespace ElusiveLife.Application.Input
{
    public class PlayerInputService : IPlayerInputService
    {
        readonly GameInputActions inputActions;

        public PlayerInputService(GameInputActions inputActions)
        => this.inputActions = inputActions;

        public void Enable() => inputActions.Player.Enable();
        public void Disable() => inputActions.Player.Disable();

        public Vector2 Look() => inputActions.Look.ReadValue<Vector2>();
        public Vector2 Move() => inputActions.Move.ReadValue<Vector2>();
        public bool OpenPause() => inputActions.OpenPause.WasPressedThisFrame();
        public bool Aim() => inputActions.Aim.IsPressed();
        public bool Fire() => inputActions.Fire.IsPressed();
        public bool Run() => inputActions.Run.IsPressed();
        public bool Jump() => inputActions.Jump.WasPressedThisFrame();
        public bool Crouch() => inputActions.Crouch.WasPressedThisFrame();
    }
}