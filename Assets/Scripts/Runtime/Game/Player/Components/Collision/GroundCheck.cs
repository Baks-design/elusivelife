using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Collision
{
    public class GroundCheck
    {
        private readonly IPlayerView _playerView;

        public GroundCheck(IPlayerView playerView)=> _playerView = playerView;

        public void CheckGround()
        {
            var hitGround = Physics.SphereCast(
                _playerView.Controller.transform.position + _playerView.Controller.center,
                _playerView.CollisionConfig.RaySphereRadius,
                Vector3.down,
                out var hitInfo,
                _playerView.CollisionData.FinalRayLength,
                _playerView.CollisionConfig.GroundLayer,
                QueryTriggerInteraction.Ignore);

            _playerView.CollisionData.OnGrounded = hitGround;
            _playerView.CollisionData.GroundedNormal = hitInfo.normal;
            _playerView.CollisionData.WasGrounded = hitGround;
        }
    }
}
