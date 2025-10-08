using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Movement
{
    public class DirectionHandler
    {
        private readonly IPlayerInputService _inputService;
        private readonly IPlayerView _playerView;

        public DirectionHandler(IPlayerInputService inputService, IPlayerView playerView)
        {
            _inputService = inputService;
            _playerView = playerView;
        }

        public void SmoothInput()
        {
            var moveInput = _inputService.Move();
            var targetInput = moveInput.sqrMagnitude > 0.01f ? moveInput.normalized : Vector2.zero;

            _playerView.MovementData.SmoothInputVector = Vector2.Lerp(
                _playerView.MovementData.SmoothInputVector,
                targetInput,
                Time.deltaTime * (_playerView.MovementConfig?.SmoothInputSpeed ?? 8f)
            );
        }

        public void CalculateMovementDirection()
        {
            var smoothInput = _playerView.MovementData.SmoothInputVector;
            if (smoothInput.sqrMagnitude < 0.01f)
            {
                _playerView.MovementData.FinalMoveDirection = Vector3.zero;
                return;
            }

            var zDir = _playerView.Controller.transform.forward * smoothInput.y;
            var xDir = _playerView.Controller.transform.right * smoothInput.x;
            var desiredDir = xDir + zDir;

            var flattenDir = FlattenVectorOnSlopes(desiredDir);
            _playerView.MovementData.FinalMoveDirection = flattenDir.normalized * smoothInput.magnitude;
        }

        private Vector3 FlattenVectorOnSlopes(Vector3 vectorToFlat)
        {
            if (_playerView.CollisionData.OnGrounded && _playerView.CollisionData.GroundedNormal != Vector3.zero)
                vectorToFlat = Vector3.ProjectOnPlane(vectorToFlat, _playerView.CollisionData.GroundedNormal);
            return vectorToFlat;
        }

        public void SmoothDirection() => _playerView.MovementData.SmoothFinalMoveDir = Vector3.Lerp(
            _playerView.MovementData.SmoothFinalMoveDir,
            _playerView.MovementData.FinalMoveDirection,
            Time.deltaTime * (_playerView.MovementConfig?.SmoothFinalDirectionSpeed ?? 8f)
        );
    }
}
