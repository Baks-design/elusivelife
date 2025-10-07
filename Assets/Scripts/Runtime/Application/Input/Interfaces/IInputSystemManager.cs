namespace ElusiveLife.Application.Input
{
    public interface IInputSystemManager
    {
        void SwitchToPlayerInput();
        void SwitchToUIInput();
        void DisableAllInput();
    }
}