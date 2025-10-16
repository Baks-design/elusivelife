using System.Threading;
using Cysharp.Threading.Tasks;

namespace ElusiveLife.Runtime.Application.Scene_Management
{
   public interface ISceneLoader
    {
        UniTask LoadSceneAsync(string sceneName, CancellationToken cancellation = default);
        UniTask UnloadSceneAsync(string sceneName, CancellationToken cancellation = default);
    }
}