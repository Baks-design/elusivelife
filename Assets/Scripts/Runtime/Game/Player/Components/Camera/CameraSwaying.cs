using ElusiveLife.Runtime.Game.Player.Interfaces;
using UnityEngine;
using ElusiveLife.Runtime.Utils.Helpers;

namespace ElusiveLife.Runtime.Game.Player.Components.Camera
{
    public class CameraSwaying
    {
        private readonly IPlayerView _playerView;
        private float _currentTiltAmount;
        private float _previousXInput;

        public CameraSwaying(IPlayerView playerView)
        {
            _playerView = playerView;
            Initialize();
        }

        private void Initialize()
        {
            _currentTiltAmount = 0f;
            _previousXInput = 0f;
        }

        public void SwayPlayer(float rawXInput)
        {
            if (Mathf.Abs(rawXInput) > 0.01f)
                ApplySway(rawXInput, IsChangingDirection(rawXInput, _previousXInput));
            else
                ReturnToCenter();

            ApplyTiltToCamera();
            _previousXInput = rawXInput;
        }

        private static bool IsChangingDirection(float currentInput, float previousInput) =>
            (currentInput > 0f && previousInput < 0f) || (currentInput < 0f && previousInput > 0f);

        private void ApplySway(float rawXInput, bool isChangingDirection)
        {
            var speedMultiplier = isChangingDirection
                ? _playerView.CameraConfig.ChangeDirectionMultiplier
                : 1f;
            var swayAcceleration = _playerView.CameraConfig.SwaySpeed * Time.deltaTime * speedMultiplier;

            _currentTiltAmount = Mathfs.ExpDecay(_currentTiltAmount, Mathf.Sign(rawXInput), swayAcceleration,
                _playerView.CameraConfig.DecayFactor);
            _currentTiltAmount = Mathf.Clamp(_currentTiltAmount, -1f, 1f);
        }

        private void ReturnToCenter()
        {
            var returnSpeed = _playerView.CameraConfig.ReturnSpeed * Time.deltaTime;
            _currentTiltAmount = Mathfs.ExpDecay(_currentTiltAmount, 0f, returnSpeed, _playerView.CameraConfig.DecayFactor);
        }

        private void ApplyTiltToCamera()
        {
            var curveValue = _playerView.CameraConfig.SwayCurve.Evaluate(Mathf.Abs(_currentTiltAmount));
            var finalTiltAmount = curveValue * _playerView.CameraConfig.SwayAmount * Mathf.Sign(_currentTiltAmount);

            var currentRotation = _playerView.Cam.transform.localRotation;
            var targetRotation = Quaternion.Euler(
                currentRotation.eulerAngles.x, currentRotation.eulerAngles.y, finalTiltAmount);

            _playerView.Cam.transform.localRotation = targetRotation;
        }
    }
}
