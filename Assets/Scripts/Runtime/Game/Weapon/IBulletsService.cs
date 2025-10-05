using UnityEngine;

namespace ElusiveLife.Game.Weapon
{
    public interface IBulletsService
    {
        void SpawnBullet(Vector3 direction);
        void DespawnBullet(BulletView bullet);
    }
}