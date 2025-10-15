using System.Collections.Generic;
using Alchemy.Inspector;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;
using UnityEngine;
using UnityEngine.Pool;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Sound
{
    public class SoundManager : MonoBehaviour, ISoundServices
    {
        [SerializeField, AssetsOnly, Required] private SoundEmitter _soundEmitterPrefab;
        private const bool CollectionCheck = true;
        private const int DefaultCapacity = 10;
        private const int MaxPoolSize = 100;
        private const int MaxSoundInstances = 30;
        private IObjectPool<SoundEmitter> _soundEmitterPool;
        private List<SoundEmitter> _activeSoundEmitters;
        public LinkedList<SoundEmitter> FrequentSoundEmitters;

        private void Start()
        {
            _activeSoundEmitters = new List<SoundEmitter>();
            FrequentSoundEmitters = new LinkedList<SoundEmitter>();

            _soundEmitterPool = new ObjectPool<SoundEmitter>(
                CreateSoundEmitter,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                CollectionCheck,
                DefaultCapacity,
                MaxPoolSize
            );
        }

        public SoundBuilder CreateSoundBuilder() => new(this);

        public bool CanPlaySound(SoundData data)
        {
            if (!data.FrequentSound || FrequentSoundEmitters.Count < MaxSoundInstances)
                return true;

            try
            {
                FrequentSoundEmitters.First.Value.Stop();
                return true;
            }
            catch
            {
                Logging.Log("SoundEmitter is already released");
            }

            return false;
        }

        public SoundEmitter Get() => _soundEmitterPool.Get();

        public void ReturnToPool(SoundEmitter soundEmitter) =>
            _soundEmitterPool.Release(soundEmitter);

        public void StopAll()
        {
            foreach (var soundEmitter in _activeSoundEmitters)
                soundEmitter.Stop();

            FrequentSoundEmitters.Clear();
        }

        private SoundEmitter CreateSoundEmitter()
        {
            var soundEmitter = Instantiate(_soundEmitterPrefab);
            soundEmitter.gameObject.SetActive(false);
            return soundEmitter;
        }

        private void OnTakeFromPool(SoundEmitter soundEmitter)
        {
            soundEmitter.gameObject.SetActive(true);
            _activeSoundEmitters.Add(soundEmitter);
        }

        private void OnReturnedToPool(SoundEmitter soundEmitter)
        {
            if (soundEmitter.Node != null)
            {
                FrequentSoundEmitters.Remove(soundEmitter.Node);
                soundEmitter.Node = null;
            }

            soundEmitter.gameObject.SetActive(false);
            _activeSoundEmitters.Remove(soundEmitter);
        }

        private static void OnDestroyPoolObject(SoundEmitter soundEmitter)
        {
            if (soundEmitter == null)
                return;

            Destroy(soundEmitter.gameObject);
        }
    }
}