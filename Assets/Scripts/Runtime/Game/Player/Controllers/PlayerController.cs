using System;
using ElusiveLife.Application.Assets.Scripts.Runtime.Application.Input.Interfaces;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Camera;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Collision;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Movement;
using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;
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

        private bool _disposed;

        public PlayerController(IPlayerInputService inputService, IPlayerView playerView)
        {
            if (inputService == null) throw new ArgumentNullException(nameof(inputService));
            if (playerView == null) throw new ArgumentNullException(nameof(playerView));

            try
            {
                CameraInitialize(inputService, playerView);
                GroundInitialize(inputService, playerView);
                MovementInitialize(inputService, playerView);

                Logging.Log("PlayerController initialized successfully");
            }
            catch (Exception ex)
            {
                Logging.LogError($"Failed to initialize PlayerController: {ex.Message}");
                Dispose();
                throw;
            }
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
            // Initialize in correct order to avoid circular dependencies
            _headBob = new HeadBobHandler(playerView);
            _directionHandler = new DirectionHandler(inputService, playerView);
            _jumpHandler = new JumpHandler(inputService, playerView);
            _landingHandler = new LandingHandler(playerView);
            _crouchHandler = new CrouchHandler(_roofCheck, inputService, playerView);
            _runningHandler = new RunningHandler(inputService, playerView);

            // Now create handlers that depend on others
            _cameraHandler = new CameraHandler(_headBob, _cameraSwaying, _cameraZoom, inputService, playerView);
            _velocityHandler = new VelocityHandler(_runningHandler, inputService, playerView);
        }

        public void Tick()
        {
            if (_disposed)
                return;

            try
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
            catch (Exception ex)
            {
                Logging.LogError($"Error in PlayerController.Tick: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                _crouchHandler?.Dispose();
                _landingHandler?.Dispose();
                _cameraZoom?.Dispose();
            }
            finally
            {
                _disposed = true;
                Logging.Log("PlayerController disposed");
            }
        }
    }
}