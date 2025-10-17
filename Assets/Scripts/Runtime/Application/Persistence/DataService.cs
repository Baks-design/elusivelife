using Cysharp.Threading.Tasks;
using ElusiveLife.Runtime.Utils.Helpers;
using System;
using System.IO;
using System.Threading;
using UnityEngine;

namespace ElusiveLife.Runtime.Application.Persistence
{
   public class DataService : IDataService
    {
        private readonly string _savePath;
        private readonly string _backupPath;
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public GameData PlayerData { get; private set; } = new GameData();

        public DataService(string fileName = "savegame.json")
        {
            // Ensure the persistent data directory exists
            if (!Directory.Exists(UnityEngine.Application.persistentDataPath))
                Directory.CreateDirectory(UnityEngine.Application.persistentDataPath);
            
            _savePath = Path.Combine(UnityEngine.Application.persistentDataPath, fileName);
            _backupPath = Path.Combine(
                UnityEngine.Application.persistentDataPath,
                $"{Path.GetFileNameWithoutExtension(fileName)}_backup.json");
            
            Logging.Log($"Save path: {_savePath}");
            Logging.Log($"Backup path: {_backupPath}");
        }

        public async UniTask SaveGameAsync()
        {
            try
            {
                // Create backup before saving
                if (File.Exists(_savePath))
                    File.Copy(_savePath, _backupPath, true);

                var json = JsonUtility.ToJson(PlayerData, true); // pretty print
                await File.WriteAllTextAsync(_savePath, json, _cancellationTokenSource.Token);
                
                Logging.Log($"Game saved successfully to: {_savePath}");
            }
            catch (Exception e)
            {
                Logging.LogError($"Save failed: {e.Message}");
                await RestoreFromBackup();
            }
        }

        public async UniTask LoadGameAsync()
        {
            try
            {
                if (File.Exists(_savePath))
                {
                    var json = await File.ReadAllTextAsync(_savePath, _cancellationTokenSource.Token);
                    PlayerData = JsonUtility.FromJson<GameData>(json);
                    Logging.Log("Game loaded successfully from main file");
                }
                else if (File.Exists(_backupPath))
                {
                    // Try to load from backup if main file doesn't exist
                    var json = await File.ReadAllTextAsync(_backupPath, _cancellationTokenSource.Token);
                    PlayerData = JsonUtility.FromJson<GameData>(json);
                    Logging.Log("Game loaded from backup file");
                    
                    // Restore the main file from backup
                    await SaveGameAsync();
                }
                else
                {
                    PlayerData = new GameData();
                    Logging.Log("No save file found, creating new data");
                    
                    // Save initial data
                    await SaveGameAsync();
                }
            }
            catch (Exception e)
            {
                Logging.LogError($"Load failed: {e.Message}");
                await RestoreFromBackup();
            }
        }

        private async UniTask RestoreFromBackup()
        {
            try
            {
                if (File.Exists(_backupPath))
                {
                    var json = await File.ReadAllTextAsync(_backupPath, _cancellationTokenSource.Token);
                    PlayerData = JsonUtility.FromJson<GameData>(json);
                    await SaveGameAsync(); // Restore main file from backup
                    Logging.Log("Data restored from backup");
                }
                else
                {
                    PlayerData = new GameData();
                    Logging.Log("No backup available, creating new data");
                }
            }
            catch (Exception ex)
            {
                Logging.LogError($"Backup restoration failed: {ex.Message}");
                PlayerData = new GameData();
            }
        }

        public bool SaveFileExists() => File.Exists(_savePath) || File.Exists(_backupPath);

        public void DeleteSaveData()
        {
            try
            {
                if (File.Exists(_savePath))
                    File.Delete(_savePath);
                
                if (File.Exists(_backupPath))
                    File.Delete(_backupPath);
                    
                PlayerData = new GameData();
                Logging.Log("Save data deleted");
            }
            catch (Exception e)
            {
                Logging.LogError($"Failed to delete save data: {e.Message}");
            }
        }
    }
}