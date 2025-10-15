using System.Collections.Generic;
using Alchemy.Inspector;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Extensions;
using UnityEngine;
using UnityEngine.Audio;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Sound
{
    public class MusicManager : MonoBehaviour, IMusicServices
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField, Required] private AudioMixerGroup _musicMixerGroup;
        [SerializeField, Required] private List<AudioClip> _initialPlaylist;
        private AudioSource _current;
        private AudioSource _previous;
        private readonly Queue<AudioClip> _playlist = new();
        private float _fading;
        private const float CrossFadeTime = 1f;

        public bool IsFadingActive => _fading > 0f;

        private void Start()
        {
            if (_initialPlaylist == null)
                return;

            foreach (var clip in _initialPlaylist)
                AddToPlaylist(clip);
        }

        public void AddToPlaylist(AudioClip clip)
        {
            _playlist.Enqueue(clip);
            if (_current == null && _previous == null)
                PlayNextTrack();
        }

        public void Clear() => _playlist.Clear();

        public void PlayNextTrack()
        {
            if (_playlist.TryDequeue(out var nextTrack))
                Play(nextTrack);
        }

        public void Play(AudioClip clip)
        {
            if (_current && _current.clip == clip)
                return;

            if (_previous)
            {
                Destroy(_previous);
                _previous = null;
            }

            _previous = _current;

            _current = _audioSource;
            _current.clip = clip;
            _current.outputAudioMixerGroup = _musicMixerGroup; // Set mixer group
            _current.loop = false; // For playlist functionality, we want tracks to play once
            _current.volume = 0f;
            _current.bypassListenerEffects = true;
            _current.Play();

            _fading = 0.001f;
        }

        public void ProcessUpdate()
        {
            HandleCrossFade();

            if (_current && !_current.isPlaying && _playlist.Count > 0)
                PlayNextTrack();
        }

        private void HandleCrossFade()
        {
            if (!IsFadingActive)
                return;

            _fading += Time.deltaTime;
            UpdateVolumes();
            CompleteFadeIfNeeded();
        }

        private void UpdateVolumes()
        {
            var fraction = CalculateFadeFraction();
            var logFraction = fraction.ToLogarithmicFraction();
            _previous.volume = 1f - logFraction;
            _current.volume = logFraction;
        }

        private void CompleteFadeIfNeeded()
        {
            var fraction = CalculateFadeFraction();
            if (fraction < 1f)
                return;

            _fading = 0f;
            CleanupPreviousTrack();
        }

        private float CalculateFadeFraction() =>
            CrossFadeTime > Mathf.Epsilon ? Mathf.Clamp01(_fading / CrossFadeTime) : 1f;

        private void CleanupPreviousTrack()
        {
            if (_previous == null)
                return;

            Destroy(_previous);
            _previous = null;
        }
    }
}