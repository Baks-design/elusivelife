using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using ElusiveLife.Runtime.Game.Spawner;
using ElusiveLife.Runtime.Game.Player.Controllers;

namespace ElusiveLife.Runtime.Application.Bootstrapper.Initializers
{
    public class GameInitializer : IAsyncStartable
    {
        private readonly PlayerSpawner _playerSpawner;
        private readonly PlayerControllerFactory _playerControllerFactory;

        public GameInitializer(
            PlayerSpawner playerSpawner,
            PlayerControllerFactory playerControllerFactory) 
        {
            _playerSpawner = playerSpawner;
            _playerControllerFactory = playerControllerFactory;
        }

        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            var playerView = await _playerSpawner.CreatePlayer();
            _playerControllerFactory.SetPlayerView(playerView);
        }
    }
}