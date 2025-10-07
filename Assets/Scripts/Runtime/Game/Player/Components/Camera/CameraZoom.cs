using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ElusiveLife.Application.Input;
using ElusiveLife.Game.Player;
using GameToolkit.Runtime.Utils.Helpers;
using UnityEngine;

namespace GameToolkit.Runtime.Game.Behaviours.Player
{
    public class CameraZoom //TODO: Zoom inverted
    {
        readonly IPlayerInputService inputService;
        readonly IPlayerView playerView;
        readonly CancellationTokenSource fovCancellationTokenSource;
        readonly CancellationTokenSource runFovCancellationTokenSource;
        readonly float initFOV;
        bool running;

        public CameraZoom(
            IPlayerInputService inputService,
            IPlayerView playerView)
        {
            this.inputService = inputService;
            this.playerView = playerView;

            initFOV = playerView.Cam.Lens.FieldOfView;
        }

        public void HandleAimFOV()
        {
            if (inputService.AimPress() || inputService.AimRelease())
                _ = ChangeFOV();
        }

        public void HandleRunFOV(bool returning) => _ = ChangeRunFOV(returning);

        async UniTaskVoid ChangeFOV()
        {
            if (running)
            {
                playerView.CameraData.IsZooming = !playerView.CameraData.IsZooming;
                return;
            }

            await ExecuteFOVChangeAsync(
                fovCancellationTokenSource,
                () => runFovCancellationTokenSource?.Cancel(),
                ct => ChangeFOVAsync(ct)
            );
        }

        async UniTaskVoid ChangeRunFOV(bool returning) =>
            await ExecuteFOVChangeAsync(
                runFovCancellationTokenSource,
                () => fovCancellationTokenSource?.Cancel(),
                ct => ChangeRunFOVAsync(returning, ct)
            );

        async UniTask ExecuteFOVChangeAsync(
            CancellationTokenSource source, Action cancelOther, Func<CancellationToken,
            UniTask> fovTask)
        {
            source?.Cancel();
            source = new CancellationTokenSource();
            cancelOther();

            try
            {
                await fovTask(source.Token);
            }
            catch (OperationCanceledException) { }
        }

        async UniTask ChangeFOVAsync(CancellationToken cancellationToken)
        {
            playerView.CameraData.IsZooming = !playerView.CameraData.IsZooming;

            var currentFOV = playerView.Cam.Lens.FieldOfView;
            var targetFOV = playerView.CameraData.IsZooming ? initFOV : playerView.CameraConfig.ZoomFOV;
            await AnimateFOV(
                currentFOV,
                targetFOV,
                playerView.CameraConfig.ZoomTransitionDuration,
                playerView.CameraConfig.ZoomCurve,
                cancellationToken
            );
        }

        async UniTask ChangeRunFOVAsync(bool returning, CancellationToken cancellationToken)
        {
            running = !returning;

            var currentFOV = playerView.Cam.Lens.FieldOfView;
            var targetFOV = returning ? initFOV : playerView.CameraConfig.RunFOV;
            var duration = returning
                ? playerView.CameraConfig.RunReturnTransitionDuration
                : playerView.CameraConfig.RunTransitionDuration;
            await AnimateFOV(
                currentFOV,
                targetFOV,
                duration,
                playerView.CameraConfig.RunCurve,
                cancellationToken
            );
        }

        async UniTask AnimateFOV(
            float currentFOV, float targetFOV, float duration, AnimationCurve curve,
            CancellationToken cancellationToken)
        {
            if (duration <= 0f)
            {
                playerView.Cam.Lens.FieldOfView = targetFOV;
                return;
            }

            var percent = 0f;
            var speed = 1f / duration;
            while (percent < 1f)
            {
                cancellationToken.ThrowIfCancellationRequested();

                percent += Time.deltaTime * speed;
                var smoothPercent = curve.Evaluate(percent);
                playerView.Cam.Lens.FieldOfView = Mathfs.Eerp(currentFOV, targetFOV, smoothPercent);

                await UniTask.NextFrame(PlayerLoopTiming.Update, cancellationToken);
            }

            playerView.Cam.Lens.FieldOfView = targetFOV;
        }
    }
}
