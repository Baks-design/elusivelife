using System;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Configs;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Camera
{
    public class CameraBreathing
    {
        private readonly IPlayerView _playerView;
        private readonly CameraScroller _cameraScroller;
        private Vector3 _originalLocalPosition;
        private Vector3 _originalLocalRotation;

        public CameraBreathing(IPlayerView playerView, CameraScroller cameraScroller)
        {
            _playerView = playerView;
            _cameraScroller = cameraScroller;

            InitializeOriginalTransform();
        }

        public void UpdateBreathing()
        {
            if (_playerView.MovementData.IsMoving)
            {
                ResetToOriginalTransform();
                return;
            }

            _cameraScroller.UpdateNoise();
            ApplyBreathingEffect();
        }

        private void InitializeOriginalTransform()
        {
            _originalLocalPosition = _playerView.Cam.transform.localPosition;
            _originalLocalRotation = _playerView.Cam.transform.localEulerAngles;
        }

        private void ResetToOriginalTransform()
        {
            _playerView.Cam.transform.localPosition = _originalLocalPosition;
            _playerView.Cam.transform.localEulerAngles = _originalLocalRotation;
        }

        private void ApplyBreathingEffect()
        {
            var noise = _playerView.CameraData.Noise;
            var config = _playerView.CameraConfig;
            var noiseConfig = _playerView.PerlinNoiseConfig;
            var scaledNoise = noise * noiseConfig.Amplitude;

            switch (noiseConfig.TransformTarget)
            {
                case TransformTarget.Position:
                    ApplyPositionNoise(scaledNoise, config);
                    break;
                case TransformTarget.Rotation:
                    ApplyRotationNoise(scaledNoise, config);
                    break;
                case TransformTarget.Both:
                    ApplyPositionNoise(scaledNoise * noiseConfig.PositionScale, config);
                    ApplyRotationNoise(scaledNoise * noiseConfig.RotationScale, config);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ApplyPositionNoise(Vector3 noise, PlayerCameraConfig config) =>
            _playerView.Cam.transform.localPosition =
                _originalLocalPosition
                + new Vector3(
                    config.X ? noise.x : 0f,
                    config.Y ? noise.y : 0f,
                    config.Z ? noise.z : 0f
                );

        private void ApplyRotationNoise(Vector3 noise, PlayerCameraConfig config) =>
            _playerView.Cam.transform.localEulerAngles =
                _originalLocalRotation
                + new Vector3(
                    config.X ? noise.x : 0f,
                    config.Y ? noise.y : 0f,
                    config.Z ? noise.z : 0f
                );
    }
}
