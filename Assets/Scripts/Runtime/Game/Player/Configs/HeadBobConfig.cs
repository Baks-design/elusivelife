using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Configs
{
    [CreateAssetMenu(menuName = "Config/Camera/HeadBobConfig")]
    public class HeadBobConfig : ScriptableObject
    {
        [Header("Curves")] 
        public AnimationCurve XCurve;
        public AnimationCurve YCurve;

        [Header("Amplitude")] 
        public float XAmplitude = 0.05f;
        public float YAmplitude = 0.1f;

        [Header("Frequency")] 
        public float XFrequency = 2f;
        public float YFrequency = 0.5f;

        [Header("Run Multipliers")] 
        public float RunAmplitudeMultiplier = 1.5f;
        public float RunFrequencyMultiplier = 1.5f;

        [Header("Crouch Multipliers")] 
        public float CrouchAmplitudeMultiplier = 0.2f;
        public float CrouchFrequencyMultiplier = 1f;

        public float MoveBackwardsFrequencyMultiplier { get; set; }
        public float MoveSideFrequencyMultiplier { get; set; }
    }
}
