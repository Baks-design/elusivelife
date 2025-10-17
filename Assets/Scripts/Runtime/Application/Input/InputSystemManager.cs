using ElusiveLife.Runtime.Application.Input.Interfaces;
using ElusiveLife.Runtime.Utils.Helpers;

namespace ElusiveLife.Runtime.Application.Input
{
    public class InputSystemManager : IInputSystemManager
    {
        private readonly IPlayerInputService _playerInputService;
        private readonly IUiInputService _uiInputService;

        public InputSystemManager(
            IPlayerInputService playerInputService,
            IUiInputService uiInputService)
        {
            _playerInputService = playerInputService;
            _uiInputService = uiInputService;
        }

        public void SwitchToPlayerInput()
        {
            _uiInputService.Disable();
            _playerInputService.Enable();
        }

        public void SwitchToUiInput()
        {
            _playerInputService.Disable();
            _uiInputService.Enable();
        }

        public void DisableAllInput()
        {
            _playerInputService.Disable();
            _uiInputService.Disable();
        }
    }
}