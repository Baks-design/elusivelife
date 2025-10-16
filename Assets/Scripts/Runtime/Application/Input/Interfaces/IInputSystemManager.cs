namespace ElusiveLife.Runtime.Application.Input.Interfaces
{
    public interface IInputSystemManager
    {
        void SwitchToPlayerInput();
        void SwitchToUiInput();
        void DisableAllInput();
    }
}