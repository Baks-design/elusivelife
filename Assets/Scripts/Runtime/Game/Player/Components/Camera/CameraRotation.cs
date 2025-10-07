using ElusiveLife.Application.Input;
using ElusiveLife.Game.Player;
using UnityEngine;

namespace GameToolkit.Runtime.Game.Behaviours.Player
{
    public class CameraRotation
    {
        readonly IPlayerInputService inputService;
        readonly IPlayerView playerView;
        float yaw;
        float pitch;
        float desiredYaw;
        float desiredPitch;

        public CameraRotation(
            IPlayerInputService inputService,
            IPlayerView playerView)
        {
            this.inputService = inputService;
            this.playerView = playerView;
        }

        public void RotationHandler()
        {
            CalculateRotation();
            SmoothRotation();
            ApplyRotation();
        }

        void CalculateRotation()
        {
            desiredYaw += inputService.Look().x * playerView.CameraConfig.Sensitivity.x * Time.deltaTime;
            desiredPitch -= inputService.Look().y * playerView.CameraConfig.Sensitivity.y * Time.deltaTime;
            desiredPitch = Mathf.Clamp(
                desiredPitch, playerView.CameraConfig.LookAngleMinMax.x, playerView.CameraConfig.LookAngleMinMax.y);
        }

        void SmoothRotation()
        {
            yaw = Mathf.Lerp(yaw, desiredYaw, playerView.CameraConfig.SmoothAmount.x * Time.deltaTime);
            pitch = Mathf.Lerp(pitch, desiredPitch, playerView.CameraConfig.SmoothAmount.y * Time.deltaTime);
        }

        void ApplyRotation()
        {
            playerView.Yaw.transform.eulerAngles = new Vector3(0f, yaw, 0f);
            playerView.Pitch.transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
        }
    }
}
