using UnityEngine.InputSystem;

namespace EngineRoom.Examples.Interfaces
{
    public interface IControlsSettings
    {
        Key AimLeftKey { get; }
        Key AimRightKey { get; }
        Key FireKey { get; }
    }
}