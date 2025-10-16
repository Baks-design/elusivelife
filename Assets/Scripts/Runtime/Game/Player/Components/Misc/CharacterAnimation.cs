using ElusiveLife.Runtime.Utils.Helpers;
using UnityEngine;
using ElusiveLife.Runtime.Game.Player.Interfaces;

namespace ElusiveLife.Runtime.Game.Player.Components.Misc
{
    public class CharacterAnimation
    {
        private readonly IPlayerView _playerView;
        private float _currentSpeed;
        private bool _wasGrounded;
        private bool _wasSwimming;
        private bool _wasClimbing;
        private int _speedId;
        private int _isGroundedId;
        private int _isJumpingId;
        private int _isFallingId;
        private int _isCrouchingId;
        private int _isSwimmingId;
        private int _isClimbingId;
        private int _climbSpeedId;
        private int _swimSpeedId;
        private int _verticalVelocityId;
        private int _landId;
        private int _enterWaterId;
        private int _exitWaterId;
        private int _startClimbingId;
        private int _stopClimbingId;

        public CharacterAnimation(IPlayerView playerView)
        {
            _playerView = playerView;
            InitHashes();
        }

        private void InitHashes()
        {
            _speedId = Animator.StringToHash("_Speed");
            _isGroundedId = Animator.StringToHash("_IsGrounded");
            _isJumpingId = Animator.StringToHash("_IsJumping");
            _isFallingId = Animator.StringToHash("_IsFalling");
            _isCrouchingId = Animator.StringToHash("_IsCrouching");
            _isSwimmingId = Animator.StringToHash("_IsSwimming");
            _isClimbingId = Animator.StringToHash("_IsClimbing");
            _climbSpeedId = Animator.StringToHash("_ClimbSpeed");
            _swimSpeedId = Animator.StringToHash("_SwimSpeed");
            _verticalVelocityId = Animator.StringToHash("_VerticalVelocity");
            _landId = Animator.StringToHash("_Land");
            _enterWaterId = Animator.StringToHash("_EnterWater");
            _exitWaterId = Animator.StringToHash("_ExitWater");
            _startClimbingId = Animator.StringToHash("_StartClimbing");
            _stopClimbingId = Animator.StringToHash("_StopClimbing");
        }

        public void UpdateMoving()
        {
            var targetSpeed = _playerView.MovementData.IsMoving ? 
                _playerView.MovementData.CurrentSpeed : 0f;
            _currentSpeed = Mathfs.ExpDecay(_currentSpeed, targetSpeed, Time.deltaTime * 100f);
            _playerView.Animator.SetFloat(_speedId, _currentSpeed);
        }

        public void UpdateJump()
        {
            var isGrounded = _playerView.CollisionData.OnGrounded;
            var verticalVelocity = _playerView.MovementData.VerticalVelocity;

            _playerView.Animator.SetBool(_isGroundedId, isGrounded);

            if (isGrounded && !_wasGrounded)
                _playerView.Animator.SetTrigger(_landId);

            var isJumping = !isGrounded && verticalVelocity > 0f;
            var isFalling = !isGrounded && verticalVelocity < 0f;

            _playerView.Animator.SetBool(_isJumpingId, isJumping);
            _playerView.Animator.SetBool(_isFallingId, isFalling);
            _playerView.Animator.SetFloat(_verticalVelocityId, verticalVelocity);

            _wasGrounded = isGrounded;
        }

        public void UpdateCrouch() => 
            _playerView.Animator.SetBool(_isCrouchingId, _playerView.MovementData.IsCrouching);

        public void UpdateSwimming()
        {
            var isSwimming = _playerView.MovementData.IsSwimming;

            _playerView.Animator.SetBool(_isSwimmingId, isSwimming);

            if (isSwimming)
            {
                var swimSpeed = _playerView.MovementData.IsMoving ? 
                    Mathf.Abs(_playerView.MovementData.CurrentSpeed) : 0f;
                _playerView.Animator.SetFloat(_swimSpeedId, swimSpeed);
            }

            switch (isSwimming)
            {
                case true when !_wasSwimming:
                    _playerView.Animator.SetTrigger(_enterWaterId);
                    break;
                case false when _wasSwimming:
                    _playerView.Animator.SetTrigger(_exitWaterId);
                    break;
            }

            _wasSwimming = isSwimming;
        }

        public void UpdateClimbing()
        {
            var isClimbing = _playerView.MovementData.IsClimbing;

            _playerView.Animator.SetBool(_isClimbingId, isClimbing);

            if (isClimbing)
            {
                var climbSpeed = Mathf.Abs(_playerView.MovementData.VerticalVelocity);
                _playerView.Animator.SetFloat(_climbSpeedId, climbSpeed);
            }

            switch (isClimbing)
            {
                case true when !_wasClimbing:
                    _playerView.Animator.SetTrigger(_startClimbingId);
                    break;
                case false when _wasClimbing:
                    _playerView.Animator.SetTrigger(_stopClimbingId);
                    break;
            }

            _wasClimbing = isClimbing;
        }
    }
}