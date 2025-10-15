using Cysharp.Threading.Tasks;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Views;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Spawner
{
    public class PlayerSpawner
    {
        private readonly PlayerFactory _playerFactory;
        private readonly Transform _spawnPoint;
        private PlayerView _currentPlayer;

        public PlayerView GetCurrentPlayer => _currentPlayer;

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

        // public async UniTask<PlayerView> MovePlayer()
        // {
        //     if (_currentPlayer != null)
        //     {
        //         //Move Player;
        //     }
        //     return _currentPlayer;
        // }
    }
}