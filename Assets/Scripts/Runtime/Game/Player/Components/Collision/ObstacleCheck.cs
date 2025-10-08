using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Collision
{
    public class ObstacleCheck
    {
        private readonly IPlayerInputService _inputService;
        private readonly IPlayerView _playerView;

        public ObstacleCheck(IPlayerInputService inputService, IPlayerView playerView)
        {
            _inputService = inputService;
            _playerView = playerView;
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
    }
}
