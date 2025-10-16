using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Camera;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Collision;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Misc;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Movement;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Sound;
using VContainer.Unity;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Controllers
{
    public class PlayerController : IPlayerController, ITickable, ILateTickable
    {
        private GroundCheck _groundCheck;
        private ObstacleCheck _obstacleCheck;
        private RoofCheck _roofCheck;
        private ResolveCollisions _resolveCollisions;
        private VelocityHandler _velocityHandler;
        private CrouchHandler _crouchHandler;
        private CameraHandler _cameraHandler;
        private DirectionHandler _directionHandler;
        private JumpHandler _jumpHandler;
        private LandingHandler _landingHandler;
        private HeadBobHandler _headBob;
        private RunningHandler _runningHandler;
        private CameraScroller _cameraScroller;
        private CameraRotation _cameraRotation;
        private CameraBreathing _cameraBreathing;
        private CameraSwaying _cameraSwaying;
        private CameraZoom _cameraZoom;
        private CharacterAnimation _animation;
        private CharacterSound _sound;

        public PlayerController(
            IPlayerView playerView,
            IPlayerInputService inputService)
        {
            CollisionInitialize(playerView, inputService);
            CameraInitialize(playerView, inputService);
            MovementInitialize(playerView, inputService);
            //MiscInitialize(playerView, soundServices);
        }

        private void CollisionInitialize(IPlayerView playerView, IPlayerInputService inputService)
        {
            _groundCheck = new GroundCheck(playerView);
            _obstacleCheck = new ObstacleCheck(inputService, playerView);
            _roofCheck = new RoofCheck(playerView);
            _resolveCollisions = new ResolveCollisions(playerView);
        }

        private void CameraInitialize(IPlayerView playerView, IPlayerInputService inputService)
        {
            _cameraRotation = new CameraRotation(inputService, playerView);
            _cameraScroller = new CameraScroller(playerView);
            _cameraBreathing = new CameraBreathing(playerView, _cameraScroller);
            _cameraSwaying = new CameraSwaying(playerView);
            _cameraZoom = new CameraZoom(inputService, playerView);
        }

        private void MovementInitialize(IPlayerView playerView, IPlayerInputService inputService)
        {
            _headBob = new HeadBobHandler(playerView);
            _directionHandler = new DirectionHandler(inputService, playerView);
            _jumpHandler = new JumpHandler(inputService, playerView);
            _landingHandler = new LandingHandler(playerView);
            _crouchHandler = new CrouchHandler(_roofCheck, inputService, playerView);
            _runningHandler = new RunningHandler(inputService, playerView);
            _cameraHandler = new CameraHandler(_headBob, _cameraSwaying, _cameraZoom, inputService, playerView);
            _velocityHandler = new VelocityHandler(_runningHandler, inputService, playerView);
        }

        private void MiscInitialize(IPlayerView playerView, ISoundServices soundServices)
        {
            _animation = new CharacterAnimation(playerView);
            _sound = new CharacterSound(playerView, soundServices);
        }

        public void Tick()
        {
            Collision();
            Movement();
            //Animation();
            //Sound();
            ResolveCollisions();
        }

        private void Collision()
        {
            _groundCheck.CheckGround();
            _obstacleCheck.CheckObstacle();
        }

        private void Movement()
        {
            _jumpHandler.UpdateTimers();
            _runningHandler.HandleRun();
            _crouchHandler.HandleCrouch();

            _directionHandler.SmoothInput();
            _directionHandler.CalculateMovementDirection();
            _directionHandler.SmoothDirection();

            _velocityHandler.CalculateSpeed();
            _velocityHandler.SmoothSpeed();

            _jumpHandler.HandleJump();
            _velocityHandler.ApplyGravity();
            _velocityHandler.CalculateFinalVelocity();
            _velocityHandler.ApplyMove();

            _landingHandler.UpdateAirTimer();
            _landingHandler.HandleLanding();

            _cameraHandler.RotateTowardsCamera();
            _cameraHandler.HandleHeadBob();
            _cameraHandler.HandleCameraSway();
            _cameraHandler.HandleRunFov();
            _cameraHandler.UpdateFinalCameraPosition();
        }

        private void Animation()
        {
            _animation.UpdateMoving();
            _animation.UpdateJump();
            _animation.UpdateCrouch();
            _animation.UpdateSwimming();
            _animation.UpdateClimbing();
        }

        private void Sound()
        {
            _sound.UpdateFootsteps();
            _sound.UpdateLanding();
            _sound.UpdateSwimming();
            _sound.UpdateClimbing();
            _sound.UpdateJumping();
            _sound.UpdateDamaging();
        }

        private void ResolveCollisions() => _resolveCollisions.ResolveComplexCollisions();

        public void LateTick() => Camera();

        private void Camera()
        {
            _cameraRotation.RotationHandler();
            _cameraBreathing.UpdateBreathing();
            _cameraZoom.HandleAimFov();
        }
    }
}