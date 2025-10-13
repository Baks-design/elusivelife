namespace ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces
{
    public interface IUiInputService 
    {
        void Initialize();
        void Enable();
        void Disable();
        bool ClosePause();
    }
}