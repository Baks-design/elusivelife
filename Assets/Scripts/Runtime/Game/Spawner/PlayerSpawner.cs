using Cysharp.Threading.Tasks;
using UnityEngine;
using ElusiveLife.Runtime.Game.Player.Views;

namespace ElusiveLife.Runtime.Game.Spawner
{
    public class PlayerSpawner
    {
        private readonly PlayerFactory _playerFactory;
        private readonly Transform _spawnPoint;
        private PlayerView _currentPlayer;

        public PlayerSpawner(PlayerFactory playerFactory, Transform spawnPoint)
        {
            _playerFactory = playerFactory;
            _spawnPoint = spawnPoint;
        }

        public async UniTask<PlayerView> CreatePlayer()
        {
            _currentPlayer = await _playerFactory.CreatePlayer(_spawnPoint);
            if (_currentPlayer != null)
                Object.DontDestroyOnLoad(_currentPlayer.gameObject);
            return _currentPlayer;
        }
    }
}