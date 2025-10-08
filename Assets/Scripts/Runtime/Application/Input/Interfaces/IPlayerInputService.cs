using UnityEngine;

namespace ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces
{
    public interface IPlayerInputService : IInputService
    {
        bool OpenPause();
        Vector2 Look();
        Vector2 Move();
        bool AimPress();
        bool AimRelease();
        bool RunPress();
        bool RunHold();
        bool RunRelease();
        bool Jump();
        bool Crouch();
    }
}