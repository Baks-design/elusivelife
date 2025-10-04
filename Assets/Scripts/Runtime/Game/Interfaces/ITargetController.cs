using System;

namespace EngineRoom.Examples.Interfaces
{
    public interface ITargetController
    {
        bool CanSpawnTarget { get; }

        event Action TargetGotHit;
        event Action TargetDespawned;

        void SetView(ITargetView view);
        void SpawnTarget();
    }
}