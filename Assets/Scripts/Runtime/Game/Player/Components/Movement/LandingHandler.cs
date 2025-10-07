using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ElusiveLife.Game.Player;
using UnityEngine;

namespace GameToolkit.Runtime.Game.Behaviours.Player
{
    public class LandingHandler
    {
        readonly IPlayerView playerView;
        CancellationTokenSource landingCancellationTokenSource;

        public LandingHandler(IPlayerView playerView) => this.playerView = playerView;

        public void HandleLanding()
        {
            if (playerView.CollisionData.PreviouslyGrounded)
                return;

            landingCancellationTokenSource?.Cancel();
            landingCancellationTokenSource = new CancellationTokenSource();

            _ = StartLanding(landingCancellationTokenSource.Token);
        }

        async UniTaskVoid StartLanding(CancellationToken cancellationToken = default)
        {
            try
            {
                await LandingAsync(cancellationToken);
            }
            catch (OperationCanceledException) { }
        }

        async UniTask LandingAsync(CancellationToken cancellationToken = default)
        {
            var percent = 0f;
            var speed = 1f / playerView.MovementConfig.LandDuration;
            var localPos = playerView.Yaw.localPosition;
            var initialHeight = localPos.y;

            while (percent < 1f)
            {
                cancellationToken.ThrowIfCancellationRequested();

                percent += Time.deltaTime * speed;
                var curveValue = playerView.MovementConfig.LandCurve.Evaluate(percent);
                var desiredOffset = curveValue * CalculateLandAmount();
                localPos.y = initialHeight + desiredOffset;
                playerView.Yaw.localPosition = localPos;

                await UniTask.NextFrame(PlayerLoopTiming.Update, cancellationToken);
            }
        }

        float CalculateLandAmount() =>
            playerView.MovementData.InAirTimer > playerView.MovementConfig.LandTimer
                ? playerView.MovementConfig.HighLandAmount
                : playerView.MovementConfig.LowLandAmount;
    }
}
