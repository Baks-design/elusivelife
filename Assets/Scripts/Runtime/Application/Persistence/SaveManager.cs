using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using ElusiveLife.Runtime.Utils.Helpers;
using ZLinq;

namespace ElusiveLife.Runtime.Application.Persistence
{
    public class SaveManager : ISaveManager
    {
        private readonly Dictionary<string, IDataService> _saveServices = new();
    
        public string CurrentSlot { get; private set; } = "default";
        public IReadOnlyList<string> SaveSlots => _saveServices.Keys.ToList();
        public IDataService CurrentDataService => GetOrCreateService(CurrentSlot);

        public SaveManager() => GetOrCreateService("default");   // Initialize default slot

        public async UniTask SaveGameAsync(string slotName = "default")
        {
            var service = GetOrCreateService(slotName);
            await service.SaveGameAsync();
            
            if (slotName != CurrentSlot)
                await SwitchSlotAsync(slotName);
        }

        public async UniTask LoadGameAsync(string slotName = "default")
        {
            var service = GetOrCreateService(slotName);
            await service.LoadGameAsync();
            
            if (slotName != CurrentSlot)
                await SwitchSlotAsync(slotName);
        }

        public async UniTask SwitchSlotAsync(string slotName)
        {
            if (!_saveServices.ContainsKey(slotName))
                GetOrCreateService(slotName);
            
            CurrentSlot = slotName;
            Logging.Log($"Switched to save slot: {CurrentSlot}");
            await UniTask.CompletedTask;
        }

        public async UniTask DeleteSaveAsync(string slotName)
        {
            if (_saveServices.TryGetValue(slotName, out var service) && service is DataService fileService)
            {
                fileService.DeleteSaveData();
                _saveServices.Remove(slotName);
                
                // If we're deleting the current slot, switch to default
                if (slotName == CurrentSlot)
                    await SwitchSlotAsync("default");
            }
            await UniTask.CompletedTask;
        }

        public bool SaveExists(string slotName)
        {
            if (_saveServices.TryGetValue(slotName, out var service) && service is DataService fileService)
                return fileService.SaveFileExists();
            return false;
        }

        private IDataService GetOrCreateService(string slotName)
        {
            if (!_saveServices.ContainsKey(slotName))
            {
                var fileName = $"save_{slotName}.json";
                _saveServices[slotName] = new DataService(fileName);
            }
            return _saveServices[slotName];
        }
    }
}