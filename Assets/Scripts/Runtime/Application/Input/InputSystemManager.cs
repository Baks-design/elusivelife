using VContainer.Unity;

namespace ElusiveLife.Application.Input
{
    public class InputSystemManager : IInputSystemManager, IStartable
    {
        readonly IPlayerInputService playerInputService;
        readonly IUIInputService uiInputService;

        public InputSystemManager(
            IPlayerInputService playerInputService,
            IUIInputService uiInputService)
        {
            this.playerInputService = playerInputService;
            this.uiInputService = uiInputService;
        }

        public void Start() => SwitchToPlayerInput();

        public void SwitchToPlayerInput()
        {
            uiInputService.Disable();
            playerInputService.Enable();
        }

        public void SwitchToUIInput()
        {
            playerInputService.Disable();
            uiInputService.Enable();
        }

        public void DisableAllInput()
        {
            playerInputService.Disable();
            uiInputService.Disable();
        }
    }
}