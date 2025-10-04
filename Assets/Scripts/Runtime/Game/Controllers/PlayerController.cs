using EngineRoom.Examples.Interfaces;
using UnityEngine;
using VContainer.Unity;

namespace EngineRoom.Examples.Controllers
{
    public class PlayerController : IPlayerController, ITickable
    {
        readonly IInputHandler inputHandler;
        readonly IGameSettings gameSettings;
        readonly IPlayerView playerView;
        readonly IBulletsService bulletsService;

        public PlayerController(
            IInputHandler inputHandler,
            IGameSettings gameSettings, 
            IPlayerView playerView,
            IBulletsService bulletsService)
        {
            this.inputHandler = inputHandler;
            this.gameSettings = gameSettings;
            this.playerView = playerView;
            this.bulletsService = bulletsService;
        }

        public void Tick()
        {
            if (!Mathf.Approximately(inputHandler.AimDirection, 0f))
                playerView.CannonRotation += gameSettings.AimRotationDegreesPerSecond * Time.deltaTime * inputHandler.AimDirection;

            if (inputHandler.IsFireButtonPressed)
            {
                var aimDirection = Quaternion.Euler(0f,  playerView.CannonRotation,0f) * Vector3.forward;
                bulletsService.SpawnBullet(aimDirection);
            }
        }
    }
}