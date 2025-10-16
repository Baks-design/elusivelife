using VContainer;
using ElusiveLife.Runtime.Game.Player.Interfaces;
using ElusiveLife.Runtime.Application.Input.Interfaces;

namespace ElusiveLife.Runtime.Game.Player.Controllers
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