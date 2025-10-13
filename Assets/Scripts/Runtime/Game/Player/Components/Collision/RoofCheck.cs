using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Collision
{
    public class RoofCheck
    {
        private readonly IPlayerView _playerView;

        public RoofCheck(IPlayerView playerView) => _playerView = playerView;

        public bool CheckRoof()
        {
            var hitRoof = UnityEngine.Physics.SphereCast(
                _playerView.Controller.transform.position,
                _playerView.CollisionConfig.RoofRadius,
                Vector3.up,
                out _,
                _playerView.CollisionData.InitHeight,
                UnityEngine.Physics.AllLayers,
                QueryTriggerInteraction.Ignore
            );

            _playerView.CollisionData.HasRoofed = hitRoof;
            
            return hitRoof;
        }
    }
}
