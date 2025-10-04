using EngineRoom.Examples.Views;
using UnityEngine;

namespace EngineRoom.Examples.Interfaces
{
    public interface IBulletsService
    {
        void SpawnBullet(Vector3 direction);
        void DespawnBullet(BulletView bullet);
    }
}