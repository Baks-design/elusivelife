using UnityEngine;

namespace ElusiveLife.Application.Input
{
    public interface IPlayerInputService : IInputService
    {
        bool OpenPause();
        Vector2 Look();
        Vector2 Move();
        bool AimPress();
        bool AimRelease();
        bool RunPress();
        bool RunRelease();
        bool Jump();
        bool Crouch();
    }
}