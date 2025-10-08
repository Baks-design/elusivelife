using System;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Camera;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Collision;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Movement;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using VContainer.Unity;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Controllers
{
    public class PlayerController : IPlayerController, ITickable, IDisposable
    {
        //Checks
        private GroundCheck _groundCheck;
        private ObstacleCheck _obstacleCheck;

        private RoofCheck _roofCheck;

        //Movement
        private VelocityHandler _velocityHandler;
        private CrouchHandler _crouchHandler;
        private CameraHandler _cameraHandler;
        private DirectionHandler _directionHandler;
        private JumpHandler _jumpHandler;
        private LandingHandler _landingHandler;
        private HeadBobHandler _headBob;

        private RunningHandler _runningHandler;

        //Camera
        private CameraScroller _cameraScroller;
        private CameraRotation _cameraRotation;
        private CameraBreathing _cameraBreathing;
        private CameraSwaying _cameraSwaying;
        private CameraZoom _cameraZoom;

        public PlayerController(IPlayerInputService inputService, IPlayerView playerView)
        {
            CameraInitialize(inputService, playerView);
            GroundInitialize(inputService, playerView);
            MovementInitialize(inputService, playerView);
        }

        private void CameraInitialize(IPlayerInputService inputService, IPlayerView playerView)
        {
            _cameraRotation = new CameraRotation(inputService, playerView);
            _cameraScroller = new CameraScroller(playerView);
            _cameraBreathing = new CameraBreathing(playerView, _cameraScroller);
            _cameraSwaying = new CameraSwaying(playerView);
            _cameraZoom = new CameraZoom(inputService, playerView);
        }

        private void GroundInitialize(IPlayerInputService inputService, IPlayerView playerView)
        {
            _groundCheck = new GroundCheck(playerView);
            _obstacleCheck = new ObstacleCheck(inputService, playerView);
            _roofCheck = new RoofCheck(playerView);
        }

        private void MovementInitialize(IPlayerInputService inputService, IPlayerView playerView)
        {
            _directionHandler = new DirectionHandler(inputService, playerView);
            _jumpHandler = new JumpHandler(inputService, playerView);
            _landingHandler = new LandingHandler(playerView);
            _headBob = new HeadBobHandler(playerView);
            _crouchHandler = new CrouchHandler(_roofCheck, inputService, playerView);
            _runningHandler = new RunningHandler(inputService, playerView);
            _cameraHandler = new CameraHandler(_headBob, _cameraSwaying, _cameraZoom, inputService, playerView);
            _velocityHandler = new VelocityHandler(_runningHandler, inputService, playerView);
        }

        public void Tick()
        {
            //CHECKS
            _groundCheck.CheckGround();
            _obstacleCheck.CheckObstacle();

            //MOVEMENT
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

            //CAMERA
            _cameraRotation.RotationHandler();
            _cameraBreathing.UpdateBreathing();
            _cameraZoom.HandleAimFov();
        }

        public void Dispose()
        {
            _crouchHandler?.Dispose();
            _landingHandler?.Dispose();
            _cameraZoom?.Dispose();
        }
    }
}