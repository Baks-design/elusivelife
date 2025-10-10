using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;
using UnityEngine.SceneManagement;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Bootstrap
{
    public class SceneLoader : ISceneLoader
    {
        public async UniTask LoadSceneAsync(string sceneName, CancellationToken cancellation = default)
        {
            if (string.IsNullOrEmpty(sceneName))
                throw new ArgumentException("Scene name cannot be null or empty", nameof(sceneName));

            cancellation.ThrowIfCancellationRequested();

            try
            {
                // Check if scene is already loaded
                if (SceneManager.GetSceneByName(sceneName).IsValid())
                {
                    Logging.LogWarning($"Scene {sceneName} is already loaded");
                    return;
                }

                var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive) ??
                                throw new InvalidOperationException($"Failed to start loading scene: {sceneName}");

                await operation.ToUniTask(cancellationToken: cancellation);

                var scene = SceneManager.GetSceneByName(sceneName);
                if (!scene.IsValid())
                    throw new InvalidOperationException($"Scene {sceneName} failed to load properly");

                SceneManager.SetActiveScene(scene);
                Logging.Log($"Scene {sceneName} loaded and set as active");
            }
            catch (OperationCanceledException)
            {
                Logging.Log($"Scene {sceneName} loading cancelled");
                throw;
            }
            catch (Exception ex)
            {
                Logging.LogError($"Failed to load scene {sceneName}: {ex.Message}");
                throw;
            }
        }

        public async UniTask UnloadSceneAsync(string sceneName, CancellationToken cancellation = default)
        {
            if (string.IsNullOrEmpty(sceneName))
                throw new ArgumentException("Scene name cannot be null or empty", nameof(sceneName));

            cancellation.ThrowIfCancellationRequested();

            try
            {
                var scene = SceneManager.GetSceneByName(sceneName);
                if (!scene.IsValid())
                {
                    Logging.LogWarning($"Scene {sceneName} is not loaded");
                    return;
                }

                var operation = SceneManager.UnloadSceneAsync(scene) ??
                                throw new InvalidOperationException($"Failed to start unloading scene: {sceneName}");

                await operation.ToUniTask(cancellationToken: cancellation);
                Logging.Log($"Scene {sceneName} unloaded");
            }
            catch (OperationCanceledException)
            {
                Logging.Log($"Scene {sceneName} unloading cancelled");
                throw;
            }
            catch (Exception ex)
            {
                Logging.LogError($"Failed to unload scene {sceneName}: {ex.Message}");
                throw;
            }
        }
    }
}