using ElusiveLife.Application.Input;
using ElusiveLife.Game.Player;
using UnityEngine;

namespace GameToolkit.Runtime.Game.Behaviours.Player
{
    public class DirectionHandler
    {
        readonly IPlayerInputService inputService;
        readonly IPlayerView playerView;

        public DirectionHandler(
            IPlayerInputService inputService,
            IPlayerView playerView)
        {
            this.inputService = inputService;
            this.playerView = playerView;
        }

        public void SmoothInput() =>
            playerView.MovementData.SmoothInputVector = Vector2.Lerp(
                playerView.MovementData.SmoothInputVector,
                inputService.Move().normalized,
                Time.deltaTime * playerView.MovementConfig.SmoothInputSpeed);

        public void CalculateMovementDirection()
        {
            var zDir = playerView.Controller.transform.forward * playerView.MovementData.SmoothInputVector.y;
            var xDir = playerView.Controller.transform.right * playerView.MovementData.SmoothInputVector.x;
            var desiredDir = xDir + zDir;
            var flattenDir = FlattenVectorOnSlopes(desiredDir);
            playerView.MovementData.FinalMoveDirection = flattenDir;
        }

        Vector3 FlattenVectorOnSlopes(Vector3 vectorToFlat)
        {
            if (playerView.CollisionData.OnGrounded)
                vectorToFlat = Vector3.ProjectOnPlane(vectorToFlat, playerView.CollisionData.GroundedNormal);
            return vectorToFlat;
        }

        public void SmoothDirection() =>
            playerView.MovementData.SmoothFinalMoveDir = Vector3.Lerp(
                playerView.MovementData.SmoothFinalMoveDir,
                playerView.MovementData.FinalMoveDirection,
                Time.deltaTime * playerView.MovementConfig.SmoothFinalDirectionSpeed);
    }
}
