namespace Weapon
{
    public interface IWeapon
    {
        public delegate void ShootAction();
        public void Shoot();
    }
}
