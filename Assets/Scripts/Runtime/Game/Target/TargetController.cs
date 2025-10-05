using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ElusiveLife.Game.Settings;
using UnityEngine;

namespace ElusiveLife.Game.Target
{
    public class TargetController : ITargetController, IDisposable
    {
        readonly IGameSettings gameSettings;
        ITargetView view;
        CancellationTokenSource despawnCancellationSource;
        float lastTimeDespawned;

        public bool CanSpawnTarget =>
            !view.HasTarget && Time.time - lastTimeDespawned >=
            gameSettings.TargetSpawnCooldownInSeconds;

        public event Action TargetGotHit;
        public event Action TargetDespawned;

        public TargetController(IGameSettings gameSettings) => this.gameSettings = gameSettings;

        public void SetView(ITargetView view)
        {
            if (view != null)
                view.TargetGotHit -= OnTargetGotHit;

            this.view = view;
            view.TargetGotHit += OnTargetGotHit;
            lastTimeDespawned = Time.time - gameSettings.TargetSpawnCooldownInSeconds;
        }

        public void SpawnTarget()
        {
            if (!CanSpawnTarget)
                return;

            CancelDespawnTimer();
            view.SpawnTarget();

            despawnCancellationSource = new CancellationTokenSource();
            DespawnAfterDelay(
                gameSettings.DespawnTargetAfterSeconds,
                despawnCancellationSource.Token).Forget();
        }

        void OnTargetGotHit()
        {
            Despawn();
            TargetGotHit?.Invoke();
        }

        void Despawn()
        {
            lastTimeDespawned = Time.time;
            view.DespawnTarget();
            CancelDespawnTimer();
        }

        async UniTaskVoid DespawnAfterDelay(float seconds, CancellationToken token)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(seconds), cancellationToken: token);
                if (token.IsCancellationRequested || !view.HasTarget)
                    return;

                Despawn();
                TargetDespawned?.Invoke();
            }
            catch (OperationCanceledException)
            {
                // ignored: countdown canceled (e.g., target got hit)
            }
        }

        void CancelDespawnTimer()
        {
            despawnCancellationSource?.Cancel();
            despawnCancellationSource?.Dispose();
            despawnCancellationSource = null;
        }

        public void Dispose()
        {
            view.TargetGotHit -= OnTargetGotHit;
            CancelDespawnTimer();
        }
    }
}