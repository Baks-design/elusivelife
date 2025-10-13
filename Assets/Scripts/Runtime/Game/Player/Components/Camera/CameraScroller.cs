using ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Player.Components.Camera
{
    public class CameraScroller
    {
        private readonly IPlayerView _playerView;
        private Vector3 _noiseOffset;

        public CameraScroller(IPlayerView playerView)
        {
            _playerView = playerView;
            InitializeNoise();
        }

        private void InitializeNoise() =>
            _noiseOffset = new Vector3(
                Random.Range(0f, 1000f),
                Random.Range(0f, 1000f),
                Random.Range(0f, 1000f)
            );

        public void UpdateNoise()
        {
            var deltaTime = Time.deltaTime;
            if (Time.timeScale < 0.01f)
                return;

            var scrollOffset = deltaTime * _playerView.PerlinNoiseConfig.Frequency;
            _noiseOffset.x += scrollOffset;

            var baseNoise = Mathf.PerlinNoise(_noiseOffset.x, 0f);
            var quarterPhase = Mathf.PerlinNoise(_noiseOffset.x + 0.25f, 0f);
            var halfPhase = Mathf.PerlinNoise(_noiseOffset.x + 0.5f, 0f);

            var noise = new Vector3(baseNoise, quarterPhase, halfPhase);
            noise = 2f * _playerView.PerlinNoiseConfig.Amplitude * (noise - Vector3.one * 0.5f);
            _playerView.CameraData.Noise = noise;
        }
    }
}
