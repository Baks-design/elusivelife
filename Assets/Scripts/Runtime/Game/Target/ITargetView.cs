using System;

namespace ElusiveLife.Game.Target
{
    public interface ITargetView
    {
        bool HasTarget { get; }

        event Action TargetGotHit;

        void SpawnTarget();
        void DespawnTarget();
    }
}