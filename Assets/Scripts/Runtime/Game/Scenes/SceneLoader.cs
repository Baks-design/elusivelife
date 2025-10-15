using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Scenes
{
    public class SceneLoader : ISceneLoader
    {
        public async UniTask LoadSceneAsync(string sceneName, CancellationToken cancellation = default)
        {
            cancellation.ThrowIfCancellationRequested();

            if (SceneManager.GetSceneByName(sceneName).IsValid())
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                return;
            }

            var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            await operation.ToUniTask(cancellationToken: cancellation);

            await UniTask.WaitUntil(
                () => SceneManager.GetSceneByName(sceneName).isLoaded, cancellationToken: cancellation);

            var scene = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(scene);

            await UniTask.NextFrame(cancellation);
        }

        public async UniTask UnloadSceneAsync(string sceneName, CancellationToken cancellation = default)
        {
            cancellation.ThrowIfCancellationRequested();

            var scene = SceneManager.GetSceneByName(sceneName);
            if (!scene.IsValid())
                return;

            var operation = SceneManager.UnloadSceneAsync(scene);
            await operation.ToUniTask(cancellationToken: cancellation);
        }
    }
}