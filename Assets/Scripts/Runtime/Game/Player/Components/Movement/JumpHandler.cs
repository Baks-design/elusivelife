using UnityEngine;
using ElusiveLife.Runtime.Application.Input.Interfaces;
using ElusiveLife.Runtime.Game.Player.Interfaces;

namespace ElusiveLife.Runtime.Game.Player.Components.Movement
{
    public class JumpHandler
    {
        private readonly IPlayerInputService _inputService;
        private readonly IPlayerView _playerView;

        public JumpHandler(IPlayerInputService inputService, IPlayerView playerView)
        {
            _inputService = inputService;
            _playerView = playerView;
        }

        public void UpdateTimers()
        {
            if (_playerView.CollisionData.OnGrounded)
                _playerView.MovementData.CoyoteTimeTimer = 0.15f;
            else
                _playerView.MovementData.CoyoteTimeTimer -= Time.deltaTime;

            if (_inputService.Jump())
                _playerView.MovementData.JumpBufferTimer = 0.1f;
            else
                _playerView.MovementData.JumpBufferTimer -= Time.deltaTime;
        }

        public void HandleJump()
        {
            var canJump = CanJump();
            var shouldJump = _playerView.MovementData.JumpBufferTimer > 0f;
            if (!canJump || !shouldJump)
                return;

            PerformJump();
            ResetJumpBuffers();
        }

        private bool CanJump()
        {
            if (_playerView.MovementData.IsCrouching)
                return false;

            var isGroundedOrCoyoteTime = _playerView.CollisionData.OnGrounded ||
                                         _playerView.MovementData.CoyoteTimeTimer > 0f;
            var isJumping = _playerView.MovementData.FinalMoveVelocity.y > 0f;
            return isGroundedOrCoyoteTime && !isJumping;
        }

        private void PerformJump()
        {
            var gravity = Mathf.Abs(UnityEngine.Physics.gravity.y * _playerView.MovementConfig.GravityMultiplier);
            var jumpHeight = _playerView.MovementConfig.JumpHeight;
            var jumpVelocity = Mathf.Sqrt(2f * gravity * jumpHeight);

            _playerView.MovementData.FinalMoveVelocity.y = jumpVelocity;
        }

        private void ResetJumpBuffers()
        {
            _playerView.MovementData.JumpBufferTimer = 0f;
            _playerView.MovementData.CoyoteTimeTimer = 0f;
        }
    }
}
