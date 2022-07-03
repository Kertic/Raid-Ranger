namespace Interfaces
{
    public interface IEntity
    {
        public int GetCurrentHealth();
        public int GetMaxHealth();
        public void TakeDamage(int damage);
    }
}
