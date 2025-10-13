namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Sound
{
    public interface ISoundServices
    {
        SoundBuilder CreateSoundBuilder();
        SoundEmitter Get();
        bool CanPlaySound(SoundData data);
        void ReturnToPool(SoundEmitter soundEmitter);
        void StopAll();
    }
}