using System;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Movement
{
    public class HeadBobHandler
    {
        private readonly IPlayerView _playerView;
        private float _xScroll;
        private float _yScroll;
        private bool _isResetting;

        public HeadBobHandler(IPlayerView playerView)
        {
            _playerView = playerView;
            Initialize();
        }

        private void Initialize()
        {
            if (_playerView.HeadBobConfig.MoveBackwardsFrequencyMultiplier == 0f)
                _playerView.HeadBobConfig.MoveBackwardsFrequencyMultiplier =
                    _playerView.MovementConfig.MoveBackwardsSpeedPercent;
            if (_playerView.HeadBobConfig.MoveSideFrequencyMultiplier == 0f)
                _playerView.HeadBobConfig.MoveSideFrequencyMultiplier = _playerView.MovementConfig.MoveSideSpeedPercent;

            ResetHeadBob();
        }

        public void ScrollHeadBob(bool running, bool crouching, Vector2 input, float deltaTime)
        {
            _playerView.MovementData.Resetted = false;
            _isResetting = false;

            var (amplitudeMultiplier, frequencyMultiplier) = 
                CalculateMovementMultipliers(running, crouching);
            var additionalMultiplier = CalculateDirectionMultiplier(input);

            _xScroll += deltaTime * _playerView.HeadBobConfig.XFrequency * frequencyMultiplier * additionalMultiplier;
            _yScroll += deltaTime * _playerView.HeadBobConfig.YFrequency * frequencyMultiplier * additionalMultiplier;

            if (_xScroll > 1000f)
                _xScroll = 0f;
            if (_yScroll > 1000f)
                _yScroll = 0f;

            CalculateHeadBobOffset(amplitudeMultiplier);
        }

        public void ResetHeadBob()
        {
            if (_isResetting)
                return;

            _playerView.MovementData.Resetted = true;
            _isResetting = true;

            _playerView.MovementData.FinalOffset = Vector3.Lerp(
                _playerView.MovementData.FinalOffset,
                Vector3.zero,
                Time.deltaTime * 8f
            );

            if (!(_playerView.MovementData.FinalOffset.sqrMagnitude < 0.001f)) return;
            _xScroll = _yScroll = 0f;
            _playerView.MovementData.FinalOffset = Vector3.zero;
            _isResetting = false;
        }

        private (float amplitude, float frequency) CalculateMovementMultipliers(
            bool running, bool crouching)
        {
            var amplitude = 1f;
            var frequency = 1f;

            if (crouching)
            {
                amplitude = _playerView.HeadBobConfig.CrouchAmplitudeMultiplier;
                frequency = _playerView.HeadBobConfig.CrouchFrequencyMultiplier;
            }
            else if (running)
            {
                amplitude = _playerView.HeadBobConfig.RunAmplitudeMultiplier;
                frequency = _playerView.HeadBobConfig.RunFrequencyMultiplier;
            }

            return (amplitude, frequency);
        }

        private float CalculateDirectionMultiplier(Vector2 input)
        {
            if (input.y < -0.1f)
                return _playerView.HeadBobConfig.MoveBackwardsFrequencyMultiplier;

            if (Mathf.Abs(input.x) > 0.1f && Mathf.Abs(input.y) < 0.1f)
                return _playerView.HeadBobConfig.MoveSideFrequencyMultiplier;

            return 1f;
        }

        private void CalculateHeadBobOffset(float amplitudeMultiplier)
        {
            if (_isResetting)
                return;

            try
            {
                var xValue = _playerView.HeadBobConfig.XCurve.Evaluate(_xScroll);
                var yValue = _playerView.HeadBobConfig.YCurve.Evaluate(_yScroll);
                _playerView.MovementData.FinalOffset = new Vector3(
                    xValue * _playerView.HeadBobConfig.XAmplitude * amplitudeMultiplier,
                    yValue * _playerView.HeadBobConfig.YAmplitude * amplitudeMultiplier,
                    0f
                );
            }
            catch (Exception)
            {
                _playerView.MovementData.FinalOffset = Vector3.zero;
            }
        }
    }
}
