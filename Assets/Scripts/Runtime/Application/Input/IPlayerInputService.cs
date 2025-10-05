using UnityEngine;

namespace ElusiveLife.Application.Input
{
    public interface IPlayerInputService : IInputService
    {
        bool OpenPause();
        Vector2 Look();
        bool Aim();
        bool Fire();
        Vector2 Move();
        bool Run();
        bool Jump();
        bool Crouch();
    }
}