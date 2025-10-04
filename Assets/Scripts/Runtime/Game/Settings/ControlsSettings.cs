using EngineRoom.Examples.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EngineRoom.Examples.Settings
{
    [CreateAssetMenu(fileName = "Controls Settings", menuName = "Engine Room/Controls Settings")]
    public class ControlsSettings : ScriptableObject, IControlsSettings
    {
        [field: SerializeField] public Key AimLeftKey { get; set; } = Key.A;
        [field:SerializeField] public Key AimRightKey { get; set; } = Key.D;
        [field:SerializeField] public Key FireKey { get; set; } = Key.Space;
    }
}