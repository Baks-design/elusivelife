using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Configs
{
    public enum TransformTarget
    {
        Position,
        Rotation,
        Both
    }

    [CreateAssetMenu(menuName = "Config/Camera/PerlinNoiseData")]
    public class PerlinNoiseConfig : ScriptableObject
    {
        public TransformTarget TransformTarget = TransformTarget.Rotation;
        public float Amplitude = 1f;
        public float Frequency = 0.5f;
        public float PositionScale = 1f;
        public float RotationScale = 1f;
    }
}
