using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Camera
{
    public class CameraRotation
    {
        private readonly IPlayerInputService _inputService;
        private readonly IPlayerView _playerView;
        private float _yaw;
        private float _pitch;
        private float _desiredYaw;
        private float _desiredPitch;

        public CameraRotation(IPlayerInputService inputService, IPlayerView playerView)
        {
            _inputService = inputService;
            _playerView = playerView;

            InitializeRotations();
        }

        public void RotationHandler()
        {
            CalculateRotation();
            SmoothRotation();
            ApplyRotation();
        }

        private void InitializeRotations()
        {
            _yaw = _desiredYaw = _playerView.Yaw.transform.eulerAngles.y;
            _pitch = _desiredPitch = _playerView.Pitch.transform.localEulerAngles.x;
            _pitch = NormalizeAngle(_pitch);
        }

        private static float NormalizeAngle(float angle)
        {
            while (angle > 180f) angle -= 360f;
            while (angle < -180f) angle += 360f;
            return angle;
        }

        private void CalculateRotation()
        {
            _desiredYaw += _inputService.Look().x * _playerView.CameraConfig.Sensitivity.x * Time.deltaTime;
            _desiredPitch -= _inputService.Look().y * _playerView.CameraConfig.Sensitivity.y * Time.deltaTime;
            _desiredPitch = Mathf.Clamp(
                _desiredPitch, _playerView.CameraConfig.LookAngleMinMax.x, _playerView.CameraConfig.LookAngleMinMax.y
            );
        }

        private void SmoothRotation()
        {
            _yaw = Mathf.LerpAngle(_yaw, _desiredYaw, _playerView.CameraConfig.SmoothAmount.x * Time.deltaTime);
            _pitch = Mathf.LerpAngle(_pitch, _desiredPitch, _playerView.CameraConfig.SmoothAmount.y * Time.deltaTime);
        }

        private void ApplyRotation()
        {
            _playerView.Yaw.transform.rotation = Quaternion.Euler(0f, _yaw, 0f);
            _playerView.Pitch.transform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }
    }
}
