using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Sound
{
    [CreateAssetMenu(menuName = "Config/Sound/Library")]
    public class SoundLibraryObject : ScriptableObject
    {
        [field: SerializeField] public SoundData FootstepClip { get; set; }
        [field: SerializeField] public SoundData LandingClip { get; set; }
        [field: SerializeField] public SoundData SwimmingClip { get; set; }
        [field: SerializeField] public SoundData JumpingClip { get; set; }
        [field: SerializeField] public SoundData ClimbingClip { get; set; }
        [field: SerializeField] public SoundData DamagingClip { get; set; }
    }
}