using UnityEngine;
using ElusiveLife.Runtime.Game.Player.Interfaces;

namespace ElusiveLife.Runtime.Game.Player.Components.Collision
{
    public class CharacterPush
    {
        private readonly IPlayerView _playerView;

        public CharacterPush(IPlayerView playerView) => _playerView = playerView;

        public void AddImpact(Vector3 direction, float force)
        {
            _playerView.MovementData.FinalMoveVelocity.y = 0f;
            _playerView.MovementData.FinalMoveVelocity += direction * force;
        }

        public void PushBody(ControllerColliderHit hit)
        {
            var body = hit.collider.attachedRigidbody;
            var pushStrength = CalculatePushStrength();
            if (!CanPush(hit) || !IsValidRigidbody(body) || pushStrength < Mathf.Epsilon)
                return;

            var pushDirection = CalculatePushDirection(hit.moveDirection);
            ApplyPushForce(body, pushDirection, pushStrength);
        }

        private bool CanPush(ControllerColliderHit hit)
        {
            if (hit == null)
                return false;

            UpdateCollisionState(hit);

            return _playerView.CollisionConfig.IsPushEnabled && hit.moveDirection.y >= -0.3f;
        }

        private void UpdateCollisionState(ControllerColliderHit hit)
            => _playerView.CollisionData.HasObjectColliding = !hit.collider.isTrigger;

        private static bool IsValidRigidbody(Rigidbody body) => body != null && !body.isKinematic;

        private static Vector3 CalculatePushDirection(Vector3 moveDirection)
        {
            var pushDir = new Vector3(moveDirection.x, 0f, moveDirection.z);
            return pushDir.sqrMagnitude < Mathf.Epsilon ? Vector3.zero : pushDir.normalized;
        }

        private float CalculatePushStrength()
        {
            var controllerSpeed = _playerView.Controller.velocity.magnitude;
            return Mathf.Clamp(
                _playerView.CollisionConfig.PushPower * controllerSpeed,
                0f,
                _playerView.CollisionConfig.MaxPushForce
            );
        }

        private void ApplyPushForce(Rigidbody body, Vector3 direction, float strength)
        {
            if (direction == Vector3.zero)
                return;

            if (_playerView.CollisionConfig.UseForceInsteadOfVelocity)
                body.AddForce(direction * strength, _playerView.CollisionConfig.ForceMode);
            else
                body.linearVelocity = direction * strength;
        }
    }
}
