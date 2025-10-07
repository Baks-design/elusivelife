using ElusiveLife.Game.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameToolkit.Runtime.Game.Behaviours.Player
{
    public class CameraScroller
    {
        readonly IPlayerView playerView;
        Vector3 noiseOffset;

        public CameraScroller(IPlayerView playerView)
        {
            this.playerView = playerView;

            var rand = 32f;
            noiseOffset = new Vector3(
                Random.Range(0f, rand),
                Random.Range(0f, rand),
                Random.Range(0f, rand)
            );
        }

        public void UpdateNoise()
        {
            var scrollOffset = Time.deltaTime * playerView.PerlinNoiseConfig.Frequency;
            noiseOffset.x += scrollOffset;

            var noise = new Vector3(
                Mathf.PerlinNoise(noiseOffset.x, 0f),
                Mathf.PerlinNoise(noiseOffset.x, 1f),
                Mathf.PerlinNoise(noiseOffset.x, 2f)
            );
            noise = (noise - Vector3.one * 0.5f) * playerView.PerlinNoiseConfig.Amplitude;
            
            playerView.CameraData.Noise = noise;
        }
    }
}
