using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;

namespace ElusiveLife.Game.Weapon
{
    public class BulletsService : IBulletsService
    {
        readonly BulletView bulletPrefab;
        readonly IObjectResolver resolver;
        readonly Transform bulletSpawnPoint;
        readonly ObjectPool<BulletView> pool;

        public BulletsService(
            BulletView bulletPrefab,
            IObjectResolver resolver,
            [Key(TransformKey.BulletSpawn)] Transform bulletSpawnPoint)
        {
            this.bulletPrefab = bulletPrefab;
            this.resolver = resolver;
            this.bulletSpawnPoint = bulletSpawnPoint;

            pool = new ObjectPool<BulletView>(CreateBullet, OnSpawned, OnReleased);
        }

        public void SpawnBullet(Vector3 direction)
        {
            var bullet = pool.Get();
            bullet.SetMovementDirection(direction);
        }

        public void DespawnBullet(BulletView bullet) => pool.Release(bullet);

        void OnReleased(BulletView bullet) => bullet.gameObject.SetActive(false);

        void OnSpawned(BulletView bullet)
        {
            bullet.gameObject.SetActive(true);
            bullet.transform.position = bulletSpawnPoint.position;
        }

        BulletView CreateBullet() => resolver.Instantiate(bulletPrefab);
    }
}