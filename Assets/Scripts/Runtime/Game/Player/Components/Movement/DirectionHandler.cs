using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;
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
            => _playerView.MovementData.SmoothInputVector = Mathfs.ExpDecay(
                _playerView.MovementData.SmoothInputVector,
                _inputService.Move(),
                Time.deltaTime * _playerView.MovementConfig.SmoothInputSpeed,
                _playerView.MovementConfig.DecayFactor
            );

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

        public void SmoothDirection() =>
            _playerView.MovementData.SmoothFinalMoveDir = Mathfs.ExpDecay(
                _playerView.MovementData.SmoothFinalMoveDir,
                _playerView.MovementData.FinalMoveDirection,
                Time.deltaTime * _playerView.MovementConfig.SmoothFinalDirectionSpeed,
                _playerView.MovementConfig.DecayFactor
            );
    }
}
