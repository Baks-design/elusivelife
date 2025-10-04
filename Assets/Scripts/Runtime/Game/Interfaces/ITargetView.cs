using System;

namespace EngineRoom.Examples.Interfaces
{
    public interface ITargetView
    {
        bool HasTarget { get; }
        
        event Action TargetGotHit;
        
        void SpawnTarget();
        void DespawnTarget();
    }
}