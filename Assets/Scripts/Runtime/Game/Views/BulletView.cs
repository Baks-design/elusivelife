using EngineRoom.Examples.Interfaces;
using UnityEngine;
using VContainer;

namespace EngineRoom.Examples.Views
{
    public class BulletView : MonoBehaviour
    {
        [SerializeField]float speed = 10f;
        [SerializeField] Rigidbody body;
        IBulletsService bulletsService;
        Vector3 moveDirection = Vector3.forward;

        [Inject]
        public void Construct(IBulletsService bulletsService)
        => this.bulletsService = bulletsService;

        void OnEnable() => StartMovement();

        void OnTriggerEnter(Collider other) => bulletsService.DespawnBullet(this);

        public void SetMovementDirection(Vector3 direction)
        {
            moveDirection = direction;
            StartMovement();
        }

        void StartMovement()
        {
             body.linearVelocity = moveDirection.normalized * speed;
            var angle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
}
