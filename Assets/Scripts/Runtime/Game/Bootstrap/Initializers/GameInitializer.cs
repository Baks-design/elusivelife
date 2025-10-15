using System.Threading;
using Cysharp.Threading.Tasks;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Manager;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Spawner;
using VContainer.Unity;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Bootstrap.Initializers
{
    public class GameInitializer : IAsyncStartable
    {
        private readonly PlayerSpawner _playerSpawner;
        private readonly GameManager _gameManager;
        private readonly PlayerControllerFactory _playerControllerFactory;

        public GameInitializer(
            PlayerSpawner playerSpawner,
            GameManager gameManager,
            PlayerControllerFactory playerControllerFactory) 
        {
            _playerSpawner = playerSpawner;
            _gameManager = gameManager;
            _playerControllerFactory = playerControllerFactory;
        }

        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            var playerView = await _playerSpawner.CreatePlayer();
            _playerControllerFactory.SetPlayerView(playerView);
            _gameManager.StartGame();
        }
    }
}