using ElusiveLife.Application.Input;
using ElusiveLife.Game.Player;
using UnityEngine;

namespace GameToolkit.Runtime.Game.Behaviours.Player
{
    public class CameraHandler
    {
        readonly HeadBobHandler headBobHandler;
        readonly RunnningHandler runnningHandler;
        readonly CameraSwaying cameraSwaying;
        readonly CameraZoom cameraZoom;
        readonly IPlayerInputService inputService;
        readonly IPlayerView playerView;

        public CameraHandler(
            HeadBobHandler headBobHandler,
            RunnningHandler runnningHandler,
            CameraSwaying cameraSwaying,
            CameraZoom cameraZoom,
            IPlayerInputService inputService,
            IPlayerView playerView)
        {
            this.headBobHandler = headBobHandler;
            this.runnningHandler = runnningHandler;
            this.cameraSwaying = cameraSwaying;
            this.cameraZoom = cameraZoom;
            this.inputService = inputService;
            this.playerView = playerView;

            playerView.MovementData.InitCamHeight = playerView.Yaw.localPosition.y;
            playerView.MovementData.CurrentStateHeight = playerView.MovementData.InitCamHeight;
        }

        public void RotateTowardsCamera()
            => playerView.Controller.transform.rotation = Quaternion.Slerp(
                playerView.Controller.transform.rotation,
                playerView.Yaw.rotation,
                Time.deltaTime * playerView.MovementConfig.SmoothRotateSpeed);

        public void HandleHeadBob()
        {
            var shouldBob = inputService.Move() != Vector2.zero && !playerView.CollisionData.HasObstructed;
            var canBob = shouldBob && !playerView.MovementData.IsDuringCrouchAnimation;

            if (canBob)
            {
                var canRun = playerView.MovementData.IsRunning && runnningHandler.CanRun();
                headBobHandler.ScrollHeadBob(
                    canRun, playerView.MovementData.IsCrouching, inputService.Move(), Time.deltaTime);
                UpdateHeadPosition(
                    Time.deltaTime,
                    (Vector3.up * playerView.MovementData.CurrentStateHeight) + playerView.MovementData.FinalOffset);
            }
            else
            {
                ResetHeadBobState();
                UpdateHeadPosition(Time.deltaTime, new Vector3(0f, playerView.MovementData.CurrentStateHeight, 0f));
            }
        }

        public void HandleCameraSway()
            => cameraSwaying.SwayPlayer(
                playerView.MovementData.SmoothInputVector, inputService.Move().x, Time.deltaTime);

        public void HandleRunFOV()
        {
            var canStartRun =
                inputService.Move() != Vector2.zero &&
                !playerView.CollisionData.HasObstructed && runnningHandler.CanRun();

            var shouldStartRun =
                canStartRun &&
                (inputService.RunPress() ||
                (playerView.MovementData.IsRunning && !playerView.MovementData.IsDuringRunAnimation));

            var shouldStopRun =
                !inputService.RunRelease() ||
                inputService.Move() == Vector2.zero ||
                playerView.CollisionData.HasObstructed;

            if (shouldStartRun && !playerView.MovementData.IsDuringRunAnimation)
            {
                playerView.MovementData.IsDuringRunAnimation = true;
                cameraZoom.HandleRunFOV(false);
            }
            else if (shouldStopRun && playerView.MovementData.IsDuringRunAnimation)
            {
                playerView.MovementData.IsDuringRunAnimation = false;
                cameraZoom.HandleRunFOV(true);
            }
        }

        void UpdateHeadPosition(float deltaTime, Vector3 targetPosition)
        {
            if (playerView.MovementData.IsDuringCrouchAnimation)
                return;

            playerView.Yaw.localPosition = Vector3.Lerp(
                playerView.Yaw.localPosition,
                targetPosition,
                deltaTime * playerView.MovementConfig.SmoothHeadBobSpeed
            );
        }

        void ResetHeadBobState()
        {
            if (playerView.MovementData.Resetted)
                return;

            headBobHandler.ResetHeadBob();
        }
    }
}
