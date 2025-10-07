using ElusiveLife.Game.Player;
using UnityEngine;

namespace GameToolkit.Runtime.Game.Behaviours.Player
{
    public class RoofCheck //TODO: Roof check
    {
        readonly IPlayerView playerView;

        public RoofCheck(IPlayerView playerView) => this.playerView = playerView;

        public bool CheckRoof()
        {
            var hitRoof = Physics.SphereCast(
                playerView.Controller.transform.position,
                playerView.CollisionConfig.RoofRadius,
                Vector3.up,
                out _,
                playerView.CollisionData.InitHeight,
                Physics.AllLayers,
                QueryTriggerInteraction.Ignore
            );

            playerView.CollisionData.HasRoofed = hitRoof;
            return hitRoof;
        }
    }
}
