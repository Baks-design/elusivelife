using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Collision
{
    public class GroundCheck
    {
        private readonly IPlayerView _playerView;
        private bool _previouslyGrounded;
        private bool _isGrounded;
        private RaycastHit _hitInfo;

        public GroundCheck(IPlayerView playerView) => _playerView = playerView;

        public void CheckGround()
        {
            _previouslyGrounded = _isGrounded;
            _isGrounded = _playerView.Controller.isGrounded;

            if (!_isGrounded)
            {
                var check = UnityEngine.Physics.SphereCast(
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
    }
}