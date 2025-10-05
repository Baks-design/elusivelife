using ElusiveLife.Application.Input;
using ElusiveLife.Game.Settings;
using ElusiveLife.Game.Weapon;
using UnityEngine;
using VContainer.Unity;

namespace ElusiveLife.Game.Player
{
    public class PlayerController : IPlayerController, ITickable
    {
        readonly IPlayerInputService inputService;
        readonly IGameSettings gameSettings;
        readonly IPlayerView playerView;
        readonly IBulletsService bulletsService;

        public PlayerController(
            IPlayerInputService inputService,
            IGameSettings gameSettings,
            IPlayerView playerView,
            IBulletsService bulletsService)
        {
            this.inputService = inputService;
            this.gameSettings = gameSettings;
            this.playerView = playerView;
            this.bulletsService = bulletsService;
        }

        public void Tick()
        {
            var direction = Mathf.Sign(inputService.Look().x); 
            playerView.CannonRotation +=
                gameSettings.AimRotationDegreesPerSecond * Time.deltaTime * direction;

            if (inputService.Fire())
            {
                var aimDirection = Quaternion.Euler(0f, playerView.CannonRotation, 0f) * Vector3.forward;
                bulletsService.SpawnBullet(aimDirection);
            }
        }
    }
}