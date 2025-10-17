using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace ElusiveLife.Runtime.Application.Persistence
{
    public interface ISaveManager
    {
        IReadOnlyList<string> SaveSlots { get; }
        string CurrentSlot { get; }
        IDataService CurrentDataService { get; }

        UniTask SaveGameAsync(string slotName = "default");
        UniTask LoadGameAsync(string slotName = "default");
        UniTask DeleteSaveAsync(string slotName);
        bool SaveExists(string slotName);
        UniTask SwitchSlotAsync(string slotName);
    }
}