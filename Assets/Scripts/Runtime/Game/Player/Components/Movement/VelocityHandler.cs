using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Movement
{
    public class VelocityHandler
    {
        private readonly RunningHandler _runningHandler;
        private readonly IPlayerInputService _inputService;
        private readonly IPlayerView _playerView;

        public VelocityHandler(
            RunningHandler runningHandler,
            IPlayerInputService inputService,
            IPlayerView playerView)
        {
            _runningHandler = runningHandler;
            _inputService = inputService;
            _playerView = playerView;
        }

        public void CalculateSpeed()
        {
            var moveInput = _inputService.Move();
            if (moveInput == Vector2.zero)
            {
                _playerView.MovementData.CurrentSpeed = 0f;
                return;
            }

            var baseSpeed = CalculateBaseSpeed();
            _playerView.MovementData.CurrentSpeed = ApplyDirectionModifiers(baseSpeed);
        }

        private float CalculateBaseSpeed()
        {
            if (_playerView.MovementData.IsCrouching)
                return _playerView.MovementConfig.CrouchSpeed;

            if (_playerView.MovementData.IsRunning && _runningHandler.CanRun())
                return _playerView.MovementConfig.RunSpeed;

            return _playerView.MovementConfig.WalkSpeed;
        }

        private float ApplyDirectionModifiers(float baseSpeed)
        {
            var input = _inputService.Move();

            if (input.y < -0.1f)
                return baseSpeed * _playerView.MovementConfig.MoveBackwardsSpeedPercent;

            if (Mathf.Abs(input.x) > 0.1f && Mathf.Abs(input.y) < 0.1f)
                return baseSpeed * _playerView.MovementConfig.MoveSideSpeedPercent;

            return baseSpeed;
        }

        public void SmoothSpeed()
        {
            _playerView.MovementData.SmoothCurrentSpeed = Mathfs.ExpDecay(
                _playerView.MovementData.SmoothCurrentSpeed, _playerView.MovementData.CurrentSpeed,
                Time.deltaTime * _playerView.MovementConfig.SmoothVelocitySpeed,
                _playerView.MovementConfig.DecayFactor);

            if (_playerView.MovementData.IsRunning && _runningHandler.CanRun())
            {
                var walkRunPercent =
                    Mathf.InverseLerp(
                        _playerView.MovementConfig.WalkSpeed,
                        _playerView.MovementConfig.RunSpeed,
                        _playerView.MovementData.SmoothCurrentSpeed);

                var walkRunSpeedDifference = _playerView.MovementConfig.RunSpeed - _playerView.MovementConfig.WalkSpeed;

                _playerView.MovementData.FinalSmoothCurrentSpeed =
                    _playerView.MovementConfig.RunTransitionCurve.Evaluate(walkRunPercent)
                    * walkRunSpeedDifference + _playerView.MovementConfig.WalkSpeed;
            }
            else
                _playerView.MovementData.FinalSmoothCurrentSpeed = _playerView.MovementData.SmoothCurrentSpeed;
        }

        public void ApplyGravity()
        {
            var isGrounded = _playerView.Controller.isGrounded;

            _playerView.CollisionData.PreviouslyGrounded = isGrounded;
            _playerView.CollisionData.OnGrounded = isGrounded;

            if (isGrounded)
            {
                _playerView.MovementData.InAirTimer = 0f;

                if (_playerView.MovementData.FinalMoveVelocity.y < 0f)
                    _playerView.MovementData.FinalMoveVelocity.y = -_playerView.MovementConfig.StickToGroundForce;
            }
            else
            {
                _playerView.MovementData.InAirTimer += Time.deltaTime;

                var gravity = UnityEngine.Physics.gravity.y * _playerView.MovementConfig.GravityMultiplier;
                _playerView.MovementData.FinalMoveVelocity.y += gravity * Time.deltaTime;
                _playerView.MovementData.FinalMoveVelocity.y = Mathf.Max(
                    _playerView.MovementData.FinalMoveVelocity.y,
                    _playerView.MovementConfig.MaxFallSpeed
                );
            }
        }

        public void CalculateFinalVelocity()
        {
            var targetVelocity =
                _playerView.MovementData.SmoothCurrentSpeed *
                _playerView.MovementData.SmoothFinalMoveDir;

            _playerView.MovementData.FinalMoveVelocity.x = targetVelocity.x;
            _playerView.MovementData.FinalMoveVelocity.z = targetVelocity.z;
        }

        public void ApplyMove()
        {
            _playerView.Controller.Move(_playerView.MovementData.FinalMoveVelocity * Time.deltaTime);

            var velocity = _playerView.Controller.velocity;
            _playerView.MovementData.CurrentVelocity = velocity;
            _playerView.MovementData.VerticalVelocity = velocity.y;

            var horizontalSpeed = new Vector3(velocity.x, 0f, velocity.z).magnitude;
            _playerView.MovementData.IsMoving = horizontalSpeed > 0.1f;
            _playerView.MovementData.IsWalking =
                _playerView.MovementData.IsMoving &&
                !_playerView.MovementData.IsRunning &&
                !_playerView.MovementData.IsCrouching;
        }
    }
}
