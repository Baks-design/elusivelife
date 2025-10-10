using System;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Maps;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;
using UnityEngine;

namespace ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Services
{
    public class PlayerInputService : IPlayerInputService
    {
        private PlayerMapInputActions _inputActions;
        private bool _isInitialized;
        private bool _disposed;

        public void Initialize()
        {
            if (_isInitialized)
                return;

            try
            {
                _inputActions = new PlayerMapInputActions();
                _inputActions.Initialize();

                ValidateInputActions();
                _isInitialized = true;

                Logging.Log("PlayerInputService initialized successfully");
            }
            catch (Exception ex)
            {
                Logging.LogError($"Failed to initialize input system: {ex.Message}");
                throw;
            }
        }

        private void ValidateInputActions()
        {
            if (_inputActions.Player == null)
                throw new InvalidOperationException("Player action map not found");

            if (_inputActions.OpenPause == null)
                throw new InvalidOperationException("OpenPause action not found");

            if (_inputActions.Look == null)
                throw new InvalidOperationException("Look action not found");

            if (_inputActions.Aim == null)
                throw new InvalidOperationException("Aim action not found");

            if (_inputActions.Move == null)
                throw new InvalidOperationException("Move action not found");

            if (_inputActions.Jump == null)
                throw new InvalidOperationException("Jump action not found");

            if (_inputActions.Run == null)
                throw new InvalidOperationException("Run action not found");

            if (_inputActions.Crouch == null)
                throw new InvalidOperationException("Crouch action not found");
        }

        public void Dispose()
        {
            if (_disposed) return;

            _inputActions?.Player?.Disable();
            _inputActions = null;
            _isInitialized = false;
            _disposed = true;

            Logging.Log("PlayerInputService disposed");
        }

        private void CheckInitialized()
        {
            if (!_isInitialized || _disposed)
                throw new InvalidOperationException("PlayerInputService is not initialized or has been disposed");
        }

        public void Enable()
        {
            CheckInitialized();
            _inputActions.Player.Enable();
        }

        public void Disable()
        {
            CheckInitialized();
            _inputActions.Player.Disable();
        }

        public bool OpenPause() => _inputActions?.OpenPause?.WasPressedThisFrame() ?? false;
        public Vector2 Look() => _inputActions?.Look?.ReadValue<Vector2>() ?? Vector2.zero;
        public Vector2 Move() => _inputActions?.Move?.ReadValue<Vector2>() ?? Vector2.zero;
        public bool AimPress() => _inputActions?.Aim?.WasPressedThisFrame() ?? false;
        public bool AimRelease() => _inputActions?.Aim?.WasReleasedThisFrame() ?? false;
        public bool RunPress() => _inputActions?.Run?.WasPressedThisFrame() ?? false;
        public bool RunHold() => _inputActions?.Run?.IsPressed() ?? false;
        public bool RunRelease() => _inputActions?.Run?.WasReleasedThisFrame() ?? false;
        public bool Jump() => _inputActions?.Jump?.WasPressedThisFrame() ?? false;
        public bool Crouch() => _inputActions?.Crouch?.WasPressedThisFrame() ?? false;
    }
}