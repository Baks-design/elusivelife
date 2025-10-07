using ElusiveLife.Application.Input;
using ElusiveLife.Game.Player;
using UnityEngine;

namespace GameToolkit.Runtime.Game.Behaviours.Player
{
    public class VelocityHandler
    {
        readonly RunnningHandler runningHandler;
        readonly IPlayerInputService inputService;
        readonly IPlayerView playerView;
        float smoothCurrentSpeed;
        float finalSmoothCurrentSpeed;

        public VelocityHandler(
            RunnningHandler runningHandler,
            IPlayerInputService inputService,
            IPlayerView playerView)
        {
            this.runningHandler = runningHandler;
            this.inputService = inputService;
            this.playerView = playerView;

            playerView.MovementData.InAirTimer = 0f;
        }

        public void CalculateSpeed()
        {
            playerView.MovementData.CurrentSpeed = ApplyDirectionModifiers(CalculateBaseSpeed());

            if (inputService.Move() == Vector2.zero)
                playerView.MovementData.CurrentSpeed = 0f;
        }

        float CalculateBaseSpeed()
        {
            if (playerView.MovementData.IsCrouching)
                return playerView.MovementConfig.CrouchSpeed;

            if (playerView.MovementData.IsRunning && runningHandler.CanRun())
                return playerView.MovementConfig.RunSpeed;

            return playerView.MovementConfig.WalkSpeed;
        }

        float ApplyDirectionModifiers(float baseSpeed)
        {
            var input = inputService.Move();

            if (input.y < -0.1f)
                return baseSpeed * playerView.MovementConfig.MoveBackwardsSpeedPercent;

            if (Mathf.Abs(input.x) > 0.1f && Mathf.Abs(input.y) < 0.1f)
                return baseSpeed * playerView.MovementConfig.MoveSideSpeedPercent;

            return baseSpeed;
        }

        public void SmoothSpeed()
        {
            smoothCurrentSpeed = Mathf.Lerp(
                smoothCurrentSpeed,
                playerView.MovementData.CurrentSpeed,
                Time.deltaTime * playerView.MovementConfig.SmoothVelocitySpeed);

            finalSmoothCurrentSpeed =
                playerView.MovementData.IsRunning && runningHandler.CanRun()
                    ? CalculateRunTransitionSpeed()
                    : smoothCurrentSpeed;
        }

        float CalculateRunTransitionSpeed()
        {
            var walkRunPercent = Mathf.InverseLerp(
                playerView.MovementConfig.WalkSpeed,
                playerView.MovementConfig.RunSpeed,
                smoothCurrentSpeed);

            return playerView.MovementConfig.RunTransitionCurve.Evaluate(walkRunPercent)
                    * (playerView.MovementConfig.RunSpeed - playerView.MovementConfig.WalkSpeed)
                    + playerView.MovementConfig.WalkSpeed;
        }

        public void ApplyGravity()
        {
            if (playerView.Controller.isGrounded)
            {
                playerView.MovementData.InAirTimer = 0f;
                playerView.MovementData.FinalMoveVelocity.y = -playerView.MovementConfig.StickToGroundForce;
            }
            else
            {
                playerView.MovementData.InAirTimer += Time.deltaTime;
                playerView.MovementData.FinalMoveVelocity +=
                    Time.deltaTime * playerView.MovementConfig.GravityMultiplier * Physics.gravity;
            }
        }

        public void CalculateFinalAcceleration()
        {
            var targetVelocity = finalSmoothCurrentSpeed * playerView.MovementData.SmoothFinalMoveDir;

            playerView.MovementData.FinalMoveVelocity.x = targetVelocity.x;
            playerView.MovementData.FinalMoveVelocity.z = targetVelocity.z;
            if (playerView.Controller.isGrounded)
                playerView.MovementData.FinalMoveVelocity.y += targetVelocity.y;
        }

        public void ApplyMove()
        {
            playerView.Controller.Move(playerView.MovementData.FinalMoveVelocity * Time.deltaTime);

            UpdateMovementState();
        }

        void UpdateMovementState()
        {
            var velocity = playerView.Controller.velocity;
            playerView.MovementData.CurrentVelocity = velocity;
            playerView.MovementData.VerticalVelocity = velocity.y;

            var horizontalSpeed = new Vector3(velocity.x, 0f, velocity.z).magnitude;
            playerView.MovementData.IsMoving = horizontalSpeed > 0.1f;
            playerView.MovementData.IsWalking = playerView.MovementData.IsMoving && !playerView.MovementData.IsRunning;
        }
    }
}
