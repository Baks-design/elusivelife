using ElusiveLife.Game.Player;
using UnityEngine;

namespace GameToolkit.Runtime.Game.Behaviours.Player
{
    public class CameraSwaying
    {
        readonly IPlayerView playerView;
        bool differentDirection;
        float scrollSpeed;
        float xAmountThisFrame;
        float xAmountPreviousFrame;

        public CameraSwaying(IPlayerView playerView) => this.playerView = playerView;

        public void SwayPlayer(Vector3 inputVector, float rawXInput, float deltaTime)
        {
            xAmountThisFrame = rawXInput;

            if (rawXInput != 0f)
            {
                differentDirection = xAmountThisFrame != xAmountPreviousFrame && xAmountPreviousFrame != 0f;

                var speedMultiplier = differentDirection
                    ? playerView.CameraConfig.ChangeDirectionMultiplier
                    : 1f;
                scrollSpeed += inputVector.x * playerView.CameraConfig.SwaySpeed * deltaTime * speedMultiplier;
            }
            else
            {
                if (xAmountThisFrame == xAmountPreviousFrame)
                    differentDirection = false;

                scrollSpeed = Mathf.Lerp(scrollSpeed, 0f, deltaTime * playerView.CameraConfig.ReturnSpeed);
            }

            scrollSpeed = Mathf.Clamp(scrollSpeed, -1f, 1f);

            var curveValue = playerView.CameraConfig.SwayCurve.Evaluate(Mathf.Abs(scrollSpeed));
            var swayFinalAmount = curveValue * -playerView.CameraConfig.SwayAmount;
            if (scrollSpeed < 0f)
                swayFinalAmount = -swayFinalAmount;

            var currentEuler = playerView.Cam.transform.localEulerAngles;
            playerView.Cam.transform.localEulerAngles = new Vector3(
                currentEuler.x, currentEuler.y, swayFinalAmount);

            xAmountPreviousFrame = xAmountThisFrame;
        }
    }
}
