using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Camera;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Movement
{
    public class CameraHandler
    {
        private readonly HeadBobHandler _headBobHandler;
        private readonly CameraSwaying _cameraSwaying;
        private readonly CameraZoom _cameraZoom;
        private readonly IPlayerInputService _inputService;
        private readonly IPlayerView _playerView;
        private float _lastFovChangeTime;

        public CameraHandler(
            HeadBobHandler headBobHandler,
            CameraSwaying cameraSwaying,
            CameraZoom cameraZoom,
            IPlayerInputService inputService,
            IPlayerView playerView)
        {
            _headBobHandler = headBobHandler;
            _cameraSwaying = cameraSwaying;
            _cameraZoom = cameraZoom;
            _inputService = inputService;
            _playerView = playerView;
        }

        public void RotateTowardsCamera() =>
            _playerView.Controller.transform.rotation = Quaternion.Slerp(
                _playerView.Controller.transform.rotation,
                _playerView.Yaw.rotation,
                Time.deltaTime * _playerView.MovementConfig.SmoothRotateSpeed);

        public void HandleHeadBob()
        {
            var shouldBob = _inputService.Move() != Vector2.zero &&
                            !_playerView.CollisionData.HasObstructed;

            var canBob = shouldBob &&
                         !_playerView.MovementData.IsDuringCrouchAnimation &&
                         !_playerView.MovementData.IsDuringLandingAnimation;

            if (canBob)
            {
                var canRun = _playerView.MovementData.IsRunning && _playerView.MovementData.IsMoving;
                _headBobHandler.ScrollHeadBob(
                    canRun, _playerView.MovementData.IsCrouching,
                    _inputService.Move(),
                    Time.deltaTime
                );
            }
            else
                _headBobHandler.ResetHeadBob();
        }

        public void HandleCameraSway() => _cameraSwaying.SwayPlayer(_inputService.Move().x);

        public void HandleRunFov()
        {
            if (Time.time < _lastFovChangeTime + 0.2f)
                return;

            var isMoving = _inputService.Move() != Vector2.zero && _playerView.MovementData.IsMoving;

            var canRun = isMoving && !_playerView.CollisionData.HasObstructed;
            var wantsToRun = _inputService.RunPress() || _inputService.RunHold();

            var shouldRun = canRun && (wantsToRun || _playerView.MovementData.IsRunning);
            var shouldStop = !canRun || !wantsToRun || _playerView.CollisionData.HasObstructed;

            var targetRunState = _playerView.MovementData.IsDuringRunAnimation;

            if (shouldRun && !_playerView.MovementData.IsDuringRunAnimation)
                targetRunState = true;
            else if (shouldStop && _playerView.MovementData.IsDuringRunAnimation)
                targetRunState = false;

            SetRun(targetRunState);
        }

        private void SetRun(bool targetRunState)
        {
            if (targetRunState == _playerView.MovementData.IsDuringRunAnimation)
                return;

            _playerView.MovementData.IsDuringRunAnimation = targetRunState;
            _cameraZoom.HandleRunFov(!targetRunState);
            _lastFovChangeTime = Time.time;
        }

        public void UpdateFinalCameraPosition()
        {
            if (_playerView.MovementData.IsDuringCrouchAnimation ||
                _playerView.MovementData.IsDuringLandingAnimation)
                return;

            var hasHeadBobOffset = _playerView.MovementData.FinalOffset.sqrMagnitude > 0.001f;
            var targetPosition = Vector3.up * _playerView.MovementData.CurrentStateHeight;

            if (hasHeadBobOffset)
                targetPosition += _playerView.MovementData.FinalOffset;
            else
            {
                targetPosition.x = Mathfs.ExpDecay(
                    _playerView.Yaw.localPosition.x,
                    0f,
                    Time.deltaTime *
                    _playerView.MovementConfig.SmoothCamera,
                    _playerView.MovementConfig.DecayFactor
                );
                targetPosition.z = Mathfs.ExpDecay(
                    _playerView.Yaw.localPosition.z,
                    0f,
                    Time.deltaTime * _playerView.MovementConfig.SmoothCamera,
                    _playerView.MovementConfig.DecayFactor
                );
            }

            _playerView.Yaw.localPosition = Mathfs.ExpDecay(
                _playerView.Yaw.localPosition,
                targetPosition,
                Time.deltaTime * _playerView.MovementConfig.SmoothHeadBobSpeed);
        }
    }
}
