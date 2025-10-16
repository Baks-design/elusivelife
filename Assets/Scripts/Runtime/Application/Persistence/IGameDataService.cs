using Cysharp.Threading.Tasks;

namespace ElusiveLife.Runtime.Application.Persistence
{
    public interface IGameDataService
    {
        PlayerData PlayerData { get; }

        UniTask SaveGameAsync();
        UniTask LoadGameAsync();
    }
}