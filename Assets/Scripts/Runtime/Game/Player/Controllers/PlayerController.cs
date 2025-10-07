using ElusiveLife.Application.Input;
using GameToolkit.Runtime.Game.Behaviours.Player;
using GameToolkit.Runtime.Utils.Helpers;
using VContainer.Unity;

namespace ElusiveLife.Game.Player
{
    public class PlayerController : IPlayerController, ITickable
    {
        readonly IPlayerView playerView;

        CameraScroller cameraScroller;
        CameraRotation cameraRotation;
        CameraBreathing cameraBreathing;
        CameraSwaying cameraSwaying;
        CameraZoom cameraZoom;

        VelocityHandler velocity;
        CrouchHandler crouch;
        CameraHandler camera;
        DirectionHandler direction;
        JumpHandler jump;
        LandingHandler land;
        HeadBobHandler headBob;
        RunnningHandler run;

        GroundCheck groundCheck;
        ObstacleCheck obstacleCheck;
        RoofCheck roofCheck;

        public PlayerController(
            IPlayerInputService inputService,
            IPlayerView playerView)
        {
            this.playerView = playerView;

            GameSystem.SetCursor(true);

            CameraInitialize(inputService, playerView);
            GroundInitialize(inputService, playerView);
            MovementInitialize(inputService, playerView);
        }

        void CameraInitialize(IPlayerInputService inputService, IPlayerView playerView)
        {
            cameraRotation = new CameraRotation(inputService, playerView);
            cameraScroller = new CameraScroller(playerView);
            cameraBreathing = new CameraBreathing(playerView, cameraScroller);
            cameraSwaying = new CameraSwaying(playerView);
            cameraZoom = new CameraZoom(inputService, playerView);
        }

        void GroundInitialize(IPlayerInputService inputService, IPlayerView playerView)
        {
            groundCheck = new GroundCheck(playerView);
            obstacleCheck = new ObstacleCheck(inputService, playerView);
            roofCheck = new RoofCheck(playerView);
        }

        void MovementInitialize(IPlayerInputService inputService, IPlayerView playerView)
        {
            direction = new DirectionHandler(inputService, playerView);
            jump = new JumpHandler(inputService, playerView);
            land = new LandingHandler(playerView);
            headBob = new HeadBobHandler(playerView);
            crouch = new CrouchHandler(roofCheck, inputService, playerView);
            run = new RunnningHandler(inputService, playerView);
            camera = new CameraHandler(headBob, run, cameraSwaying, cameraZoom, inputService, playerView);
            velocity = new VelocityHandler(run, inputService, playerView);
        }

        public void Tick()
        {
            cameraRotation.RotationHandler();
            cameraBreathing.UpdateBreathing();
            cameraZoom.HandleAimFOV();

            camera.RotateTowardsCamera();
            direction.SmoothInput();

            groundCheck.CheckGround();
            obstacleCheck.CheckObstacle();

            direction.CalculateMovementDirection();
            direction.SmoothDirection();
            velocity.CalculateSpeed();
            velocity.SmoothSpeed();
            velocity.CalculateFinalAcceleration();

            run.HandleRun();
            crouch.HandleCrouch();
            jump.HandleJump();

            velocity.ApplyGravity();
            velocity.ApplyMove();

            playerView.CollisionData.PreviouslyGrounded = playerView.CollisionData.OnGrounded;

            camera.HandleHeadBob();
            camera.HandleRunFOV();
            camera.HandleCameraSway();
            land.HandleLanding();
        }
    }
}