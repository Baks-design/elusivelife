using ElusiveLife.Application.Input;
using ElusiveLife.Game.Player;
using UnityEngine;

namespace GameToolkit.Runtime.Game.Behaviours.Player
{
    public class RunnningHandler
    {
        readonly IPlayerInputService inputService;
        readonly IPlayerView playerView;

        public RunnningHandler(IPlayerInputService inputService, IPlayerView playerView)
        {
            this.inputService = inputService;
            this.playerView = playerView;
        }

        public void HandleRun()
        {
            if (inputService.RunPress())
                playerView.MovementData.IsRunning = true;
            if (inputService.RunRelease())
                playerView.MovementData.IsRunning = false;
        }

        public bool CanRun()
        {
            var normalizedDir = Vector3.zero;

            if (playerView.MovementData.SmoothFinalMoveDir != Vector3.zero)
                normalizedDir = playerView.MovementData.SmoothFinalMoveDir.normalized;

            var dot = Vector3.Dot(playerView.Controller.transform.forward, normalizedDir);
            
            return dot >= playerView.MovementConfig.CanRunThreshold && !playerView.MovementData.IsCrouching;
        }
    }
}
