using System;

namespace ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces
{
    public interface IInputSystemManager : IDisposable
    {
        void SwitchToPlayerInput();
        void SwitchToUiInput();
        void DisableAllInput();
    }
}