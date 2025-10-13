using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Status
{
    public class CharacterHealth : MonoBehaviour, IHealthChangeable, IDeathable
    {
        [SerializeField] private int _initialMaxHealth = 100;
        private PlayerStatus _status;

        private void Awake()
        {
            var initialStatus = new PlayerStatusData
            {
                MaxHealth = _initialMaxHealth,
                CurrentHealth = _initialMaxHealth
            };
            _status = new PlayerStatus(initialStatus);
            _status.UpdateStatus(_ => initialStatus);
        }

        private void OnEnable()  => _status.OnStatusChanged += HandleStatusChanged;

        private void OnDisable() => _status.OnStatusChanged -= HandleStatusChanged;

        private void HandleStatusChanged(PlayerStatusData newStatus, PlayerStatusData previousStatus)
        {
            if (newStatus.CurrentHealth == previousStatus.CurrentHealth)
                return;

            if (!newStatus.IsAlive && previousStatus.IsAlive)
                Die();
        }

        public void TakeDamage(int damage)
        {
            if (!_status.CurrentStatus.IsAlive)
                return;

            _status.UpdateStatus(status => status.WithDamage(damage));
        }

        public void TakeHeal(int healAmount)
        {
            if (!_status.CurrentStatus.IsAlive)
                return;

            _status.UpdateStatus(status => status.WithHealing(healAmount));
        }

        public void Die()
        {
            gameObject.SetActive(false);
            _status.UpdateStatus(status => status with { CurrentHealth = 0 });
        }

        public void FullHeal() => _status.UpdateStatus(status => status.WithFullHeal());
    }
}