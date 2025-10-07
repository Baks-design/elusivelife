using ElusiveLife.Game.Player;
using UnityEngine;

namespace GameToolkit.Runtime.Game.Behaviours.Player
{
    public class HeadBobHandler
    {
        readonly IPlayerView playerView;
        float xScroll;
        float yScroll;

        public HeadBobHandler(IPlayerView playerView)
        {
            this.playerView = playerView;

            playerView.HeadBobConfig.MoveBackwardsFrequencyMultiplier = playerView.MovementConfig.MoveBackwardsSpeedPercent;
            playerView.HeadBobConfig.MoveSideFrequencyMultiplier = playerView.MovementConfig.MoveSideSpeedPercent;
            playerView.MovementData.Resetted = false;
            playerView.MovementData.FinalOffset = Vector3.zero;
            xScroll = yScroll = 0f;
        }

        public void ScrollHeadBob(bool running, bool crouching, Vector2 input, float deltaTime)
        {
            playerView.MovementData.Resetted = false;

            (var amplitudeMultiplier, var frequencyMultiplier) = CalculateMovementMultipliers(running, crouching);
            var additionalMultiplier = CalculateDirectionMultiplier(input);
            xScroll += deltaTime * playerView.HeadBobConfig.xFrequency * frequencyMultiplier;
            CalculateHeadBobOffset(amplitudeMultiplier, additionalMultiplier);
        }

        public void ResetHeadBob()
        {
            playerView.MovementData.Resetted = true;
            xScroll = yScroll = 0f;
            playerView.MovementData.FinalOffset = Vector3.zero;
        }

        (float amplitude, float frequency) CalculateMovementMultipliers(bool running, bool crouching)
        {
            var amplitude = 1f;
            var frequency = 1f;
            
            if (running)
            {
                amplitude = playerView.HeadBobConfig.runAmplitudeMultiplier;
                frequency = playerView.HeadBobConfig.runFrequencyMultiplier;
            }

            if (crouching)
            {
                amplitude = playerView.HeadBobConfig.crouchAmplitudeMultiplier;
                frequency = playerView.HeadBobConfig.crouchFrequencyMultiplier;
            }

            return (amplitude, frequency);
        }

        float CalculateDirectionMultiplier(Vector2 input)
        {
            if (input.y < -0.1f)
                return playerView.HeadBobConfig.MoveBackwardsFrequencyMultiplier;

            if (Mathf.Abs(input.x) > 0.1f && Mathf.Abs(input.y) < 0.1f)
                return playerView.HeadBobConfig.MoveSideFrequencyMultiplier;

            return 1f;
        }

        void CalculateHeadBobOffset(float amplitudeMultiplier, float additionalMultiplier)
        {
            var xValue = playerView.HeadBobConfig.xCurve.Evaluate(xScroll);
            var yValue = playerView.HeadBobConfig.yCurve.Evaluate(yScroll);
            playerView.MovementData.FinalOffset = new Vector3(
                xValue * playerView.HeadBobConfig.xAmplitude * amplitudeMultiplier * additionalMultiplier,
                yValue * playerView.HeadBobConfig.yAmplitude * amplitudeMultiplier * additionalMultiplier,
                0f
            );
        }
    }
}
