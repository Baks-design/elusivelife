using System.Threading;
using Cysharp.Threading.Tasks;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Bootstrap
{
    public interface ISceneLoader
    {
        UniTask LoadSceneAsync(string sceneName, CancellationToken cancellation = default);
        UniTask UnloadSceneAsync(string sceneName, CancellationToken cancellation = default);
    }
}