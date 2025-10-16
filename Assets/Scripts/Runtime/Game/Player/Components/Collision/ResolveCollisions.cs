using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using UnityEngine;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Extensions;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Collision
{
    public class ResolveCollisions
    {
        private readonly IPlayerView _playerView;

        public ResolveCollisions(IPlayerView playerView) => _playerView = playerView;

        public void ResolveComplexCollisions()
        {
            // Use our extension for complex penetration resolution
            if (!_playerView.Controller.GetPenetrationInLayer(
                    _playerView.CollisionConfig.GroundLayer, out var correction))
                return;

            // Apply penetration correction
            _playerView.Controller.transform.position += correction;

            // Handle different collision types
            if (Mathf.Abs(correction.y) > 0.01f)
            {
                // Vertical collision (ground/ceiling)
                if (correction.y > 0f) // Ground
                {
                    _playerView.CollisionData.OnGrounded = true;
                    _playerView.MovementData.FinalMoveVelocity.y = 0f;
                }
                else // Ceiling
                    _playerView.MovementData.FinalMoveVelocity.y =
                        Mathf.Min(0f, _playerView.MovementData.FinalMoveVelocity.y);
            }
            else
                // Horizontal collision - slide along surface
                SlideAlongSurface(correction.normalized);
        }

        private void SlideAlongSurface(Vector3 surfaceNormal)
        {
            // Remove vertical component for wall sliding
            surfaceNormal.y = 0f;
            surfaceNormal.Normalize();

            // Project velocity onto surface plane
            _playerView.MovementData.FinalMoveVelocity = Vector3.ProjectOnPlane(
                _playerView.MovementData.FinalMoveVelocity, surfaceNormal);
        }
    }
}