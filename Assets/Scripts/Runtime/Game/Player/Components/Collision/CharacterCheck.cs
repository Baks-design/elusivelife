using UnityEngine;
using ElusiveLife.Runtime.Game.Player.Interfaces;
using ElusiveLife.Runtime.Application.Input.Interfaces;

namespace ElusiveLife.Runtime.Game.Player.Components.Collision
{
    public class CharacterCheck
    {
        private readonly IPlayerInputService _inputService;
        private readonly IPlayerView _playerView;
        private bool _previouslyGrounded;
        private bool _isGrounded;
        private RaycastHit _hitInfo;

        public CharacterCheck(IPlayerInputService inputService, IPlayerView playerView)
        {
            _inputService = inputService;
            _playerView = playerView;
        }

        public void CheckGround()
        {
            _previouslyGrounded = _isGrounded;
            _isGrounded = _playerView.Controller.isGrounded;

            if (!_isGrounded)
            {
                var check = Physics.SphereCast(
                    _playerView.Controller.transform.position + _playerView.Controller.center,
                    _playerView.CollisionConfig.RaySphereRadius,
                    Vector3.down,
                    out _hitInfo,
                    _playerView.CollisionData.FinalRayLength,
                    _playerView.CollisionConfig.GroundLayer,
                    QueryTriggerInteraction.Ignore);
                if (check)
                    _isGrounded = true;
            }

            _playerView.CollisionData.WasGrounded = _previouslyGrounded;
            _playerView.CollisionData.OnGrounded = _isGrounded;
            _playerView.CollisionData.GroundedNormal = _hitInfo.normal;
        }

        public void CheckObstacle()
        {
            if (_inputService.Move() == Vector2.zero ||
                !(_playerView.MovementData.FinalMoveDirection.sqrMagnitude > 0f))
                return;

            var hitWall = Physics.SphereCast(
                _playerView.Controller.transform.position + _playerView.Controller.center,
                _playerView.CollisionConfig.RayObstacleSphereRadius,
                _playerView.MovementData.FinalMoveDirection,
                out _,
                _playerView.CollisionConfig.RayObstacleLength,
                _playerView.CollisionConfig.ObstacleLayers,
                QueryTriggerInteraction.Ignore);

            _playerView.CollisionData.HasObstructed = hitWall;
        }

        public bool CheckRoof()
        {
            var hitRoof = Physics.SphereCast(
                _playerView.Controller.transform.position,
                _playerView.CollisionConfig.RoofRadius,
                Vector3.up,
                out _,
                _playerView.CollisionData.InitHeight,
                Physics.AllLayers,
                QueryTriggerInteraction.Ignore
            );

            _playerView.CollisionData.HasRoofed = hitRoof;

            return hitRoof;
        }
    }
}