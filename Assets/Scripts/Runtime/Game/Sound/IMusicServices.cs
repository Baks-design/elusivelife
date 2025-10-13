using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Sound
{
    public interface IMusicServices
    {
        void Initialize();
        void AddToPlaylist(AudioClip clip);
        void Play(AudioClip clip);
        void PlayNextTrack();
        void Clear();
    }
}