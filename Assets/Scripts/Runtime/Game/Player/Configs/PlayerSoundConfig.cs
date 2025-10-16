using UnityEngine;

namespace ElusiveLife.Runtime.Game.Player.Configs
{
    [CreateAssetMenu(menuName = "Config/Player/PlayerSoundConfig")]
    public class PlayerSoundConfig : ScriptableObject
    {
        [Header("Footstep Sounds")] 
        public float FootstepIntervalWalk = 0.5f;
        public float FootstepIntervalRun = 0.3f;
        public float FootstepVolume = 0.7f;

        [Header("Landing Sounds")] 
        public float ScaleFactor = 0.2f;
        public float ImpactLandingMin = 0.3f;
        public float InmpactLandingMax = 1f;
        public float LandingVolume = 0.8f;
        public float LandingMinPitch = 0.8f;
        public float LandingMaxPitch = 1.2f;

        [Header("Swimming Sounds")] 
        public float SwimmingInterval = 0.4f;
        public float SwimmingVolume = 0.6f;
        public float SwimmingMinPitch = 0.8f;
        public float SwimmingMaxPitch = 1.2f;

        [Header("Climbing Sounds")] 
        public float ClimbingInterval = 0.6f;
        public float ClimbingVolume = 0.5f;
        public float ClimbingMinPitch = 0.9f;
        public float ClimbingMaxPitch = 1.1f;
    }
}