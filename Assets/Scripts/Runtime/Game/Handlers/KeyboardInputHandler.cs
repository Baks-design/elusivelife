using EngineRoom.Examples.Interfaces;
using UnityEngine.InputSystem;

namespace EngineRoom.Examples.Handlers
{
    public class KeyboardInputHandler : IInputHandler
    {
        readonly IControlsSettings controlsSettings;

        public float AimDirection => GetAimDirection();
        public bool IsFireButtonPressed => Keyboard.current[controlsSettings.FireKey].wasPressedThisFrame;

        public KeyboardInputHandler(IControlsSettings controlsSettings)
        => this.controlsSettings = controlsSettings;

        float GetAimDirection()
        => Keyboard.current.anyKey.isPressed switch
        {
            true when Keyboard.current[controlsSettings.AimLeftKey].isPressed => -1f,
            true when Keyboard.current[controlsSettings.AimRightKey].isPressed => 1f,
            _ => 0f
        };
    }
}