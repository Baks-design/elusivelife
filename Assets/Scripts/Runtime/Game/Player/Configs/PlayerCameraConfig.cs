using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Configs
{
    [CreateAssetMenu(menuName = "Config/Player/PlayerCameraConfig")]
    public class PlayerCameraConfig : ScriptableObject
    {
        [Header("Factors Settings")] 
        public float DecayFactor = 16f;

        [Header("Look Settings")] 
        public float ScaleSensivity = 0.5f;
        public Vector2 SmoothAmount = new(5f, 5f);
        public Vector2 LookAngleMinMax = new(-45f, 45f);

        [Header("Breathing Settings")] 
        public bool X = true;
        public bool Y = false;
        public bool Z = false;

        [Header("Sway Settings")] 
        public float SwayAmount = 1f;
        public float SwaySpeed = 1f;
        public float ReturnSpeed = 3f;
        public float ChangeDirectionMultiplier = 4f;
        public AnimationCurve SwayCurve;

        [Header("Zoom Settings")] 
        [SerializeField, Range(20f, 60f)]   public float ZoomFov = 40f;
        public float ZoomTransitionDuration = 0.25f;
        public AnimationCurve ZoomCurve;
        [Range(60f, 100f)] public float RunFov = 70f;
        public float RunTransitionDuration = 0.75f;
        public float RunReturnTransitionDuration = 0.5f;
        public AnimationCurve RunCurve;
    }
}
