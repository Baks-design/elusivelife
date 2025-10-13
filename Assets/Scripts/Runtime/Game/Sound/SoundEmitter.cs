using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        private ISoundServices _soundServices;
        private CancellationTokenSource _playbackCancellationTokenSource;

        public SoundData Data { get; private set; }
        public LinkedListNode<SoundEmitter> Node { get; set; }

        public void Initialize(SoundData data)
        {
            Data = data;
            
            _audioSource.clip = data.Clip;
            _audioSource.outputAudioMixerGroup = data.MixerGroup;
            _audioSource.loop = data.Loop;
            _audioSource.playOnAwake = data.PlayOnAwake;
            _audioSource.mute = data.Mute;
            _audioSource.bypassEffects = data.BypassEffects;
            _audioSource.bypassListenerEffects = data.BypassListenerEffects;
            _audioSource.bypassReverbZones = data.BypassReverbZones;
            _audioSource.priority = data.Priority;
            _audioSource.volume = data.Volume;
            _audioSource.pitch = data.Pitch;
            _audioSource.panStereo = data.PanStereo;
            _audioSource.spatialBlend = data.SpatialBlend;
            _audioSource.reverbZoneMix = data.ReverbZoneMix;
            _audioSource.dopplerLevel = data.DopplerLevel;
            _audioSource.spread = data.Spread;
            _audioSource.minDistance = data.MinDistance;
            _audioSource.maxDistance = data.MaxDistance;
            _audioSource.ignoreListenerVolume = data.IgnoreListenerVolume;
            _audioSource.ignoreListenerPause = data.IgnoreListenerPause;
            _audioSource.rolloffMode = data.RolloffMode;
        }

        public void Play()
        {
            if (_playbackCancellationTokenSource is { IsCancellationRequested: false })
            {
                _playbackCancellationTokenSource.Cancel();
                _playbackCancellationTokenSource.Dispose();
            }

            _playbackCancellationTokenSource = new CancellationTokenSource();
            _audioSource.Play();
            WaitForSoundToEndAsync(_playbackCancellationTokenSource.Token).Forget();
        }

        private async UniTask WaitForSoundToEndAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                while (_audioSource.isPlaying && !cancellationToken.IsCancellationRequested)
                    await UniTask.NextFrame(cancellationToken);

                if (!cancellationToken.IsCancellationRequested)
                    Stop();
            }
            catch (OperationCanceledException) { }
        }

        public void Stop()
        {
            if (_playbackCancellationTokenSource is { IsCancellationRequested: false })
            {
                _playbackCancellationTokenSource.Cancel();
                _playbackCancellationTokenSource.Dispose();
                _playbackCancellationTokenSource = null;
            }

            _audioSource.Stop();
            _soundServices.ReturnToPool(this);
        }
        
        public void WithRandomPitch(float min = -0.05f, float max = 0.05f) =>
            _audioSource.pitch += Random.Range(min, max);

        public void WithSetVolume(float volume = 1f) => _audioSource.volume = volume;
    }
}