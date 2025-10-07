using ElusiveLife.Application.Input;
using ElusiveLife.Game.Player;

namespace GameToolkit.Runtime.Game.Behaviours.Player
{
    public class JumpHandler
    {
        readonly IPlayerInputService inputService;
        readonly IPlayerView playerView;

        public JumpHandler(
            IPlayerInputService inputService,
            IPlayerView playerView)
        {
            this.inputService = inputService;
            this.playerView = playerView;
        }

        public void HandleJump()
        {
            if (!inputService.Jump() ||
                !playerView.Controller.isGrounded ||
                playerView.MovementData.IsCrouching)
                return;

            playerView.MovementData.FinalMoveVelocity.y = playerView.MovementConfig.JumpHeight;
            playerView.CollisionData.PreviouslyGrounded = true;
            playerView.CollisionData.OnGrounded = false;
        }
    }
}
