namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Status
{
    public interface IHealthChangeable
    {
        void TakeDamage(int damage);
        void TakeHeal(int healAmount);
        void Die();
        void FullHeal();
    }
}