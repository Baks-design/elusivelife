using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Controllers;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using VContainer;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player
{
    public class PlayerControllerFactory
    {
        private readonly IObjectResolver _resolver;
        private readonly IPlayerInputService _playerInputService;
        private PlayerController _playerController;
        private IPlayerView _currentPlayerView;

        public PlayerControllerFactory(
            IObjectResolver resolver,
            IPlayerInputService playerInputService)
        {
            _resolver = resolver;
            _playerInputService = playerInputService;
        }

        public void SetPlayerView(IPlayerView playerView)
        {
            _currentPlayerView = playerView;
            _playerController = null;
        }

        public IPlayerController GetOrCreatePlayerController()
        {
            if (_currentPlayerView == null)
                return null;

            if (_playerController != null)
                return _playerController;

            _playerController = new PlayerController(_currentPlayerView, _playerInputService);
            _resolver.Inject(_playerController);

            return _playerController;
        }
    }
}