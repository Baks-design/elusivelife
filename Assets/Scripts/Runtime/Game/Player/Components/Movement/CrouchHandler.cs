using System.Threading;
using Cysharp.Threading.Tasks;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Collision;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Movement
{
    public class CrouchHandler
    {
        private readonly RoofCheck _roofCheck;
        private readonly IPlayerInputService _movementInput;
        private readonly IPlayerView _playerView;
        private CancellationTokenSource _crouchCancellationTokenSource;
        private Vector3 _initCenter;
        private Vector3 _crouchCenter;
        private float _initHeight;
        private float _crouchHeight;
        private float _crouchCamHeight;
        private float _crouchStandHeightDifference;
        private bool _isDisposed;

        public CrouchHandler(
            RoofCheck roofCheck, IPlayerInputService movementInput, IPlayerView playerView)
        {
            _roofCheck = roofCheck;
            _movementInput = movementInput;
            _playerView = playerView;

            Initalize(playerView);
        }

        private void Initalize(IPlayerView playerView)
        {
            _initHeight = playerView.CollisionData.InitHeight;
            _initCenter = playerView.CollisionData.InitCenter;
            _crouchHeight = _initHeight * playerView.MovementConfig.CrouchPercent;
            _crouchCenter = (_crouchHeight / 2f + playerView.Controller.skinWidth) * Vector3.up;
            _crouchStandHeightDifference = _initHeight - _crouchHeight;
            _crouchCamHeight = playerView.MovementData.InitCamHeight - _crouchStandHeightDifference;
        }

        public void HandleCrouch()
        {
            if (_isDisposed || !_movementInput.Crouch())
                return;

            _crouchCancellationTokenSource?.Cancel();
            _crouchCancellationTokenSource = new CancellationTokenSource();

            _ = _playerView.MovementData.IsCrouching
                ? TryStandUpAsync(_crouchCancellationTokenSource.Token)
                : StartCrouchAsync(_crouchCancellationTokenSource.Token);
        }

        private async UniTaskVoid StartCrouchAsync(CancellationToken cancellationToken = default) =>
            await CrouchDownAsync(cancellationToken);

        private async UniTaskVoid TryStandUpAsync(CancellationToken cancellationToken = default)
        {
            if (_roofCheck.CheckRoof())
                return;

            await StandUpAsync(cancellationToken);
        }

        private async UniTask CrouchDownAsync(CancellationToken cancellationToken = default)
        {
            if (_playerView.MovementData.IsDuringCrouchAnimation || _playerView.MovementData.IsCrouching)
                return;

            _playerView.MovementData.IsDuringCrouchAnimation = true;

            try
            {
                var currentHeight = _playerView.Controller.height;
                var currentCenter = _playerView.Controller.center;
                var camPos = _playerView.Yaw.localPosition;
                var camCurrentHeight = camPos.y;

                var speed = 1f / _playerView.MovementConfig.CrouchTransitionDuration;
                var percent = 0f;

                while (percent < 1f)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    percent += Time.deltaTime * speed;
                    var smoothPercent = _playerView.MovementConfig.CrouchTransitionCurve.Evaluate(percent);

                    _playerView.Controller.height = Mathfs.ExpDecay(currentHeight, _crouchHeight, smoothPercent);
                    _playerView.Controller.center = Mathfs.ExpDecay(currentCenter, _crouchCenter, smoothPercent);

                    camPos.y = Mathfs.ExpDecay(camCurrentHeight, _crouchCamHeight, smoothPercent);
                    _playerView.Yaw.localPosition = camPos;

                    await UniTask.NextFrame(PlayerLoopTiming.Update, cancellationToken);
                }

                _playerView.MovementData.IsCrouching = true;
                _playerView.MovementData.CurrentStateHeight = _crouchCamHeight;
            }
            finally
            {
                _playerView.MovementData.IsDuringCrouchAnimation = false;
            }
        }

        private async UniTask StandUpAsync(CancellationToken cancellationToken = default)
        {
            if (_playerView.MovementData.IsDuringCrouchAnimation || !_playerView.MovementData.IsCrouching)
                return;

            _playerView.MovementData.IsDuringCrouchAnimation = true;

            try
            {
                var currentHeight = _playerView.Controller.height;
                var currentCenter = _playerView.Controller.center;
                var camPos = _playerView.Yaw.localPosition;
                var camCurrentHeight = camPos.y;

                var speed = 1f / _playerView.MovementConfig.CrouchTransitionDuration;
                var percent = 0f;

                while (percent < 1f)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (_roofCheck.CheckRoof())
                        break;

                    percent += Time.deltaTime * speed;
                    var smoothPercent = _playerView.MovementConfig.CrouchTransitionCurve.Evaluate(percent);

                    _playerView.Controller.height = Mathfs.ExpDecay(currentHeight, _initHeight, smoothPercent);
                    _playerView.Controller.center = Mathfs.ExpDecay(currentCenter, _initCenter, smoothPercent);

                    camPos.y = Mathfs.ExpDecay(camCurrentHeight, _playerView.MovementData.InitCamHeight, smoothPercent);
                    _playerView.Yaw.localPosition = camPos;

                    await UniTask.NextFrame(PlayerLoopTiming.Update, cancellationToken);
                }

                if (percent >= 1f)
                {
                    _playerView.MovementData.IsCrouching = false;
                    _playerView.MovementData.CurrentStateHeight = _playerView.MovementData.InitCamHeight;
                }
            }
            finally
            {
                _playerView.MovementData.IsDuringCrouchAnimation = false;
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;

            _crouchCancellationTokenSource?.Cancel();
            _crouchCancellationTokenSource?.Dispose();
        }
    }
}
