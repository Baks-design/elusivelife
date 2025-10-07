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

        public bool OpenPause() => inputActions.OpenPause.WasPressedThisFrame();
        public Vector2 Look() => inputActions.Look.ReadValue<Vector2>();
        public Vector2 Move() => inputActions.Move.ReadValue<Vector2>();
        public bool AimPress() => inputActions.Aim.WasPressedThisFrame();
        public bool AimRelease() => inputActions.Aim.WasReleasedThisFrame();
        public bool RunPress() => inputActions.Run.WasPressedThisFrame();
        public bool RunRelease() => inputActions.Run.WasReleasedThisFrame();
        public bool Jump() => inputActions.Jump.WasPressedThisFrame();
        public bool Crouch() => inputActions.Crouch.WasPressedThisFrame();
    }
}