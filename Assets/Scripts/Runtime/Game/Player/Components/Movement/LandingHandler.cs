using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ElusiveLife.Runtime.Game.Player.Interfaces;

namespace ElusiveLife.Runtime.Game.Player.Components.Movement
{
    public class LandingHandler
    {
        private readonly IPlayerView _playerView;
        private CancellationTokenSource _landingCancellationTokenSource;
        private bool _wasGrounded;

        public LandingHandler(IPlayerView playerView)
        {
            _playerView = playerView;
            Initalize(playerView);
        }

        private void Initalize(IPlayerView playerView) =>
            _wasGrounded = playerView.CollisionData.WasGrounded;

        public void HandleLanding()
        {
            var wasInAir = !_wasGrounded;
            var isNowGrounded = _playerView.Controller.isGrounded;

            var justLanded = wasInAir && isNowGrounded;
            if (justLanded)
            {
                _landingCancellationTokenSource?.Cancel();
                _landingCancellationTokenSource = new CancellationTokenSource();
                _ = StartLanding(_landingCancellationTokenSource.Token);
            }

            _wasGrounded = isNowGrounded;
        }

        private async UniTaskVoid StartLanding(CancellationToken cancellationToken = default) =>
            await LandingAsync(cancellationToken);

        private async UniTask LandingAsync(CancellationToken cancellationToken = default)
        {
            var percent = 0f;
            var speed = 1f / _playerView.MovementConfig.LandDuration;
            var initialLocalPos = _playerView.Yaw.localPosition;
            var initialHeight = initialLocalPos.y;
            var landAmount = CalculateLandAmount();
            var originalPosition = _playerView.Yaw.localPosition;

            try
            {
                while (percent < 1f)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    percent += Time.deltaTime * speed;
                    var curveValue = _playerView.MovementConfig.LandCurve.Evaluate(percent);
                    var desiredOffset = curveValue * landAmount;

                    var currentPos = originalPosition;
                    currentPos.y = initialHeight + desiredOffset;
                    _playerView.Yaw.localPosition = currentPos;

                    await UniTask.NextFrame(PlayerLoopTiming.Update,
                        cancellationToken);
                }

                _playerView.Yaw.localPosition = originalPosition;

                if (_playerView.MovementData != null)
                    _playerView.MovementData.InAirTimer = 0f;
            }
            catch (OperationCanceledException)
            {
                _playerView.Yaw.localPosition = originalPosition;
                throw;
            }
        }

        private float CalculateLandAmount()
        {
            var isHighLand = _playerView.MovementData.InAirTimer > _playerView.MovementConfig.LandTimer;
            return isHighLand
                ? _playerView.MovementConfig.HighLandAmount
                : _playerView.MovementConfig.LowLandAmount;
        }

        public void UpdateAirTimer()
        {
            if (_playerView.Controller.isGrounded || _playerView.CollisionData.OnGrounded)
                return;
                
            _playerView.MovementData.InAirTimer += Time.deltaTime;
        }
    }
}
