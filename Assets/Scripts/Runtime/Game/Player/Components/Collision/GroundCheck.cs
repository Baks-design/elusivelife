using ElusiveLife.Game.Player;
using UnityEngine;

namespace GameToolkit.Runtime.Game.Behaviours.Player
{
    public class GroundCheck
    {
        readonly IPlayerView playerView;

        public GroundCheck(IPlayerView playerView) => this.playerView = playerView;

        public void CheckGround()
        {
            var hitGround = Physics.SphereCast(
                playerView.Controller.transform.position + playerView.Controller.center,
                playerView.CollisionConfig.RaySphereRadius,
                Vector3.down,
                out var hitInfo,
                playerView.CollisionData.FinalRayLength,
                playerView.CollisionConfig.GroundLayer,
                QueryTriggerInteraction.Ignore);

            playerView.CollisionData.OnGrounded = hitGround;
            playerView.CollisionData.GroundedNormal = hitInfo.normal;
        }
    }
}
