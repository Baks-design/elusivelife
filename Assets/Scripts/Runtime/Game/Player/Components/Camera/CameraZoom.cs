using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Camera
{
    public class CameraZoom
    {
        private readonly IPlayerInputService _inputService;
        private readonly IPlayerView _playerView;
        private CancellationTokenSource _aimFovCts;
        private CancellationTokenSource _runFovCts;
        private bool _isAimZoomActive;
        private bool _isRunZoomActive;
        private bool _isDisposed;
        private float _initFov;

        public CameraZoom(IPlayerInputService inputService, IPlayerView playerView)
        {
            _inputService = inputService;
            _playerView = playerView;
            Initalize(playerView);
        }

        private void Initalize(IPlayerView playerView)
        {
            _aimFovCts = new CancellationTokenSource();
            _runFovCts = new CancellationTokenSource();
            _initFov = playerView.Cam.Lens.FieldOfView;
        }

        public void HandleAimFov()
        {
            if (_isDisposed)
                return;

            if (_inputService.AimPress())
                _ = StartAimZoomAsync();
            else if (_inputService.AimRelease())
                _ = StopAimZoomAsync();
        }

        public void HandleRunFov(bool isRunning)
        {
            if (_isDisposed)
                return;

            _ = HandleRunZoomAsync(isRunning);
        }

        private async UniTaskVoid StartAimZoomAsync()
        {
            if (_isAimZoomActive)
                return;

            await ExecuteFovChangeAsync(_aimFovCts,
                () => _runFovCts?.Cancel(),
                ct => ChangeAimFovAsync(true, ct));
        }

        private async UniTaskVoid StopAimZoomAsync()
        {
            if (!_isAimZoomActive)
                return;

            await ExecuteFovChangeAsync(_aimFovCts,
                () => { },
                ct => ChangeAimFovAsync(false, ct));
        }

        private async UniTaskVoid HandleRunZoomAsync(bool isRunning)
        {
            if (isRunning == _isRunZoomActive)
                return;

            await ExecuteFovChangeAsync(_runFovCts,
                () => _aimFovCts?.Cancel(),
                ct => ChangeRunFovAsync(isRunning, ct));
        }

        private static async UniTask ExecuteFovChangeAsync(
            CancellationTokenSource source, Action cancelOtherActions, Func<CancellationToken, UniTask> fovChangeTask)
        {
            source.Cancel();
            source = new CancellationTokenSource();
            cancelOtherActions();

            await fovChangeTask(source.Token);
        }

        private async UniTask ChangeAimFovAsync(bool zoomIn, CancellationToken cancellationToken)
        {
            _isAimZoomActive = zoomIn;
            _playerView.CameraData.IsZooming = zoomIn;

            var currentFov = _playerView.Cam.Lens.FieldOfView;
            var targetFov = zoomIn ? _playerView.CameraConfig.ZoomFov : _initFov;

            await AnimateFov(currentFov, targetFov, _playerView.CameraConfig.ZoomTransitionDuration,
                _playerView.CameraConfig.ZoomCurve, cancellationToken);
        }

        private async UniTask ChangeRunFovAsync(bool isRunning, CancellationToken cancellationToken)
        {
            _isRunZoomActive = isRunning;

            var currentFov = _playerView.Cam.Lens.FieldOfView;
            var targetFov = isRunning ? _playerView.CameraConfig.RunFov : _initFov;
            var duration = isRunning
                ? _playerView.CameraConfig.RunTransitionDuration
                : _playerView.CameraConfig.RunReturnTransitionDuration;

            await AnimateFov(currentFov, targetFov, duration, _playerView.CameraConfig.RunCurve, cancellationToken);
        }

        private async UniTask AnimateFov(
            float currentFov, float targetFov, float duration, AnimationCurve curve,
            CancellationToken cancellationToken)
        {
            if (duration <= 0f || Mathf.Approximately(currentFov, targetFov))
            {
                _playerView.Cam.Lens.FieldOfView = targetFov;
                return;
            }

            var elapsed = 0f;
            while (elapsed < duration)
            {
                cancellationToken.ThrowIfCancellationRequested();
                elapsed += Time.deltaTime;

                var percent = Mathf.Clamp01(elapsed / duration);
                var smoothPercent = curve.Evaluate(percent);

                _playerView.Cam.Lens.FieldOfView = Mathfs.Eerp(currentFov, targetFov, smoothPercent);

                await UniTask.NextFrame(PlayerLoopTiming.Update, cancellationToken);
            }

            _playerView.Cam.Lens.FieldOfView = targetFov;
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;

            _aimFovCts?.Cancel();
            _aimFovCts?.Dispose();
            _runFovCts?.Cancel();
            _runFovCts?.Dispose();
        }
    }
}
