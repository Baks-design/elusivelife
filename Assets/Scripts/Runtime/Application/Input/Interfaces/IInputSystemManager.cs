namespace ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces
{
    public interface IInputSystemManager
    {
        void SwitchToPlayerInput();
        void SwitchToUiInput();
        void DisableAllInput();
    }
}