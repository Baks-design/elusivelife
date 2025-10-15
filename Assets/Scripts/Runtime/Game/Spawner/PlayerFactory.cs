using Cysharp.Threading.Tasks;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Views;
using UnityEngine;
using VContainer;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Spawner
{
    public class PlayerFactory
    {
        private readonly IObjectResolver _resolver;
        private readonly PlayerView _playerPrefab;

        public PlayerFactory(IObjectResolver resolver, PlayerView playerPrefab)
        {
            _resolver = resolver;
            _playerPrefab = playerPrefab;
        }

        public async UniTask<PlayerView> CreatePlayer(Transform spawnPoint)
        {
            var playerInstance = Object.Instantiate(
                _playerPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

            _resolver.Inject(playerInstance);
            
            await UniTask.NextFrame();

            return playerInstance;
        }
    }
}