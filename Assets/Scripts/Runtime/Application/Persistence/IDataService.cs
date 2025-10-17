using Cysharp.Threading.Tasks;

namespace ElusiveLife.Runtime.Application.Persistence
{
    public interface IDataService
    {
        GameData PlayerData { get; }
        
        UniTask SaveGameAsync();
        UniTask LoadGameAsync();
    }
}