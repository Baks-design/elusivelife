using System;
using UnityEngine;

namespace GameToolkit.Runtime.Game.Behaviours.Player
{
    [Serializable]
    public class PlayerCollisionConfig
    {
        [Header("Ground Settings")]
        public LayerMask GroundLayer;
        [Range(0.1f, 1f)] public float RayLength = 0.1f;
        [Range(0.1f, 1f)] public float RaySphereRadius = 0.2f;

        [Header("Obstacles Settings")]
        public LayerMask ObstacleLayers;
        [Range(0.1f, 1f)] public float RayObstacleLength = 0.4f;
        [Range(0.1f, 1f)] public float RayObstacleSphereRadius = 0.2f;

        [Header("Push Settings")]
        public bool IsPushEnabled = true;
        public float PushPower = 2f;
        public float MaxPushForce = 10f;
        public bool UseForceInsteadOfVelocity = true;
        public ForceMode forceMode = ForceMode.Impulse;

        [Header("Roof Settings")]
        [Range(0.1f, 1f)] public float RoofRadius = 0.2f;
    }
}
