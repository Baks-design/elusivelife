using ElusiveLife.Application.Input;
using ElusiveLife.Game.Player;
using UnityEngine;

namespace GameToolkit.Runtime.Game.Behaviours.Player
{
    public class ObstacleCheck
    {
        readonly IPlayerInputService inputService;
        readonly IPlayerView playerView;

        public ObstacleCheck(
            IPlayerInputService inputService,
            IPlayerView playerView)
        {
            this.inputService = inputService;
            this.playerView = playerView;
        }

        public void CheckObstacle()
        {
            var hitWall = false;
            if (inputService.Move() != Vector2.zero && playerView.MovementData.FinalMoveDirection.sqrMagnitude > 0f)
                hitWall = Physics.SphereCast(
                    playerView.Controller.transform.position + playerView.Controller.center,
                    playerView.CollisionConfig.RayObstacleSphereRadius,
                    playerView.MovementData.FinalMoveDirection,
                    out _,
                    playerView.CollisionConfig.RayObstacleLength,
                    playerView.CollisionConfig.ObstacleLayers,
                    QueryTriggerInteraction.Ignore);

            playerView.CollisionData.HasObstructed = hitWall;
        }
    }
}
