using Cysharp.Threading.Tasks;
using ElusiveLife.Runtime.Utils.Helpers;
using System;
using UnityEngine;

namespace ElusiveLife.Runtime.Application.Persistence
{
    public class GameDataService : IGameDataService
    {
        private const string SAVE_KEY = "PlayerData";
        
        public PlayerData PlayerData { get; private set; } = new PlayerData();

        public async UniTask SaveGameAsync()
        {
            try
            {
                var json = JsonUtility.ToJson(PlayerData);
                PlayerPrefs.SetString(SAVE_KEY, json);
                //await PlayerPrefs.Save();
                Logging.Log("Game saved successfully");
            }
            catch (Exception e)
            {
                Logging.LogError($"Save failed: {e.Message}");
            }
        }

        public async UniTask LoadGameAsync()
        {
            try
            {
                if (PlayerPrefs.HasKey(SAVE_KEY))
                {
                    var json = PlayerPrefs.GetString(SAVE_KEY);
                    PlayerData = JsonUtility.FromJson<PlayerData>(json);
                }
                else
                    PlayerData = new PlayerData();
                    
                Logging.Log("Game loaded successfully");
            }
            catch (Exception e)
            {
                Logging.LogError($"Load failed: {e.Message}");
                PlayerData = new PlayerData();
            }
        }
    }
}