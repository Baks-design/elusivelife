using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Movement
{
    public class RunningHandler
    {
        private readonly IPlayerInputService _inputService;
        private readonly IPlayerView _playerView;

        public RunningHandler(IPlayerInputService inputService, IPlayerView playerView)
        {
            _inputService = inputService;
            _playerView = playerView;
        }

        public void HandleRun()
        {
            if (_inputService.RunPress() && CanRun())
                _playerView.MovementData.IsRunning = true;
            if (_inputService.RunRelease() || !CanRun())
                _playerView.MovementData.IsRunning = false;
        }

        public bool CanRun()
        {
            if (_playerView.MovementData.IsCrouching ||
                !_playerView.Controller.isGrounded ||
                _playerView.CollisionData.HasObstructed ||
                _playerView.MovementData.SmoothFinalMoveDir.sqrMagnitude < 0.01f)
                return false;

            var normalizedDir = _playerView.MovementData.SmoothFinalMoveDir.normalized;
            var dot = Vector3.Dot(_playerView.Controller.transform.forward, normalizedDir);
            return dot >= (_playerView.MovementConfig?.CanRunThreshold ?? 0.7f);
        }
    }
}
