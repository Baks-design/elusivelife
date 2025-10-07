using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ElusiveLife.Application.Input;
using ElusiveLife.Game.Player;
using UnityEngine;

namespace GameToolkit.Runtime.Game.Behaviours.Player
{
    public class CrouchHandler
    {
        readonly RoofCheck roofCheck;
        readonly IPlayerInputService movementInput;
        readonly IPlayerView playerView;
        readonly Vector3 initCenter;
        readonly float initHeight;
        readonly float crouchCamHeight;
        readonly float crouchHeight;
        readonly float crouchStandHeightDifference;
        Vector3 crouchCenter;
        CancellationTokenSource crouchCancellationTokenSource;

        public CrouchHandler(RoofCheck roofCheck, IPlayerInputService movementInput, IPlayerView playerView)
        {
            this.roofCheck = roofCheck;
            this.movementInput = movementInput;
            this.playerView = playerView;

            crouchHeight = playerView.CollisionData.InitHeight * playerView.MovementConfig.CrouchPercent;
            crouchCenter = (crouchHeight / 2f + playerView.Controller.skinWidth) * Vector3.up;
            crouchStandHeightDifference = playerView.CollisionData.InitHeight - crouchHeight;
            crouchCamHeight = playerView.MovementData.InitCamHeight - crouchStandHeightDifference;
        }

        public void HandleCrouch()
        {
            var canCrouch =
                movementInput.Crouch() &&
                !playerView.MovementData.IsCrouching &&
                !roofCheck.CheckRoof();
            if (!canCrouch)
                return;

            crouchCancellationTokenSource?.Cancel();
            crouchCancellationTokenSource = new CancellationTokenSource();

            _ = StartCrouch(crouchCancellationTokenSource.Token);
        }

        async UniTaskVoid StartCrouch(CancellationToken cancellationToken = default)
        {
            try
            {
                await CrouchAsync(cancellationToken);
            }
            catch (OperationCanceledException) { }
        }

        async UniTask CrouchAsync(CancellationToken cancellationToken = default)
        {
            playerView.MovementData.IsDuringCrouchAnimation = true;

            var wasCrouching = playerView.MovementData.IsCrouching;

            var targetHeight = wasCrouching ? playerView.CollisionData.InitHeight : crouchHeight;
            var targetCenter = wasCrouching ? playerView.CollisionData.InitCenter : crouchCenter;
            var camPos = playerView.Yaw.localPosition;
            var camCurrentHeight = camPos.y;
            var targetCamHeight = wasCrouching ? playerView.MovementData.InitCamHeight : crouchCamHeight;

            playerView.MovementData.IsCrouching = !wasCrouching;
            playerView.MovementData.CurrentStateHeight = wasCrouching ? playerView.MovementData.InitCamHeight : crouchCamHeight;

            var speed = 1f / playerView.MovementConfig.CrouchTransitionDuration;
            var percent = 0f;

            while (percent < 1f)
            {
                cancellationToken.ThrowIfCancellationRequested();

                percent += Time.deltaTime * speed;
                var smoothPercent = playerView.MovementConfig.CrouchTransitionCurve.Evaluate(percent);
                playerView.Controller.height = Mathf.Lerp(initHeight, targetHeight, smoothPercent);
                playerView.Controller.center = Vector3.Lerp(initCenter, targetCenter, smoothPercent);
                camPos.y = Mathf.Lerp(camCurrentHeight, targetCamHeight, smoothPercent);
                playerView.Yaw.localPosition = camPos;

                await UniTask.NextFrame(PlayerLoopTiming.Update, cancellationToken);
            }

            playerView.MovementData.IsDuringCrouchAnimation = false;
        }
    }
}
