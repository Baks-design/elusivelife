using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Sound
{
    public class SoundBuilder
    {
        private readonly SoundManager _soundManager;
        private Vector3 _position = Vector3.zero;
        private Transform _transform;
        private bool _randomPitch;
        private bool _setVolume;

        public SoundBuilder(SoundManager soundManager) => _soundManager = soundManager;

        public SoundBuilder WithPosition(Vector3 position)
        {
            _position = position;
            return this;
        }

        public SoundBuilder WithRandomPitch(float min = -0.05f, float max = 0.05f)
        {
            _randomPitch = true;
            return this;
        }

        public SoundBuilder WithSetVolume(float volume = 1f)
        {
            _setVolume = true;
            return this;
        }

        public SoundBuilder WithGameObjectAsParent(Transform transform)
        {
            _transform = transform;
            return this;
        }

        public void Play(SoundData soundData)
        {
            if (soundData == null || !_soundManager.CanPlaySound(soundData))
            {
                Logging.LogError("SoundData is null");
                return;
            }

            var soundEmitter = _soundManager.Get();
            soundEmitter.Initialize(soundData);
            if (_transform == null)
            {
                soundEmitter.transform.parent = _soundManager.transform;
                soundEmitter.transform.position = _position;
            }
            else
            {
                soundEmitter.transform.parent = _transform;
                soundEmitter.transform.localPosition = Vector3.zero;
            }

            if (_randomPitch)
                soundEmitter.WithRandomPitch();

            if (_setVolume)
                soundEmitter.WithSetVolume();

            if (soundData.FrequentSound)
                soundEmitter.Node = _soundManager.FrequentSoundEmitters.AddLast(soundEmitter);

            soundEmitter.Play();
        }
    }
}