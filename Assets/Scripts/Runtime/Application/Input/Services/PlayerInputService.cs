using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Maps;
using UnityEngine;

namespace ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Services
{
    public class PlayerInputService : IPlayerInputService
    {
        private PlayerMapInputActions _inputActions;

        public void Initialize()
        {
            _inputActions = new PlayerMapInputActions();
            _inputActions.Initialize();
        }

        public void Enable() => _inputActions.Player.Enable();
        public void Disable() => _inputActions.Player.Disable();

        public bool OpenPause() => _inputActions.OpenPause.WasPressedThisFrame();
        public Vector2 Look() => _inputActions.Look.ReadValue<Vector2>();
        public Vector2 Move() => _inputActions.Move.ReadValue<Vector2>();
        public bool AimPress() => _inputActions.Aim.WasPressedThisFrame();
        public bool AimRelease() => _inputActions.Aim.WasReleasedThisFrame();
        public bool RunPress() => _inputActions.Run.WasPressedThisFrame();
        public bool RunHold() => _inputActions.Run.IsPressed();
        public bool RunRelease() => _inputActions.Run.WasReleasedThisFrame();
        public bool Jump() => _inputActions.Jump.WasPressedThisFrame();
        public bool Crouch() => _inputActions.Crouch.WasPressedThisFrame();
    }
}