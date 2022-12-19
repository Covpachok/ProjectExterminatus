using Projectile;
using UnityEngine;

namespace Weapon
{
    public class Gun : MonoBehaviour, IWeapon
    {
        [SerializeField] private GameObject _bulletPrefab;

        [SerializeField]
        private Bullet.BulletData[] _bulletsData;

        [SerializeField] private float _shootingDelay;

        private float _lastShootTime;

        private float _timeout;

        public void Shoot()
        {
            //if (_lastShootTime + _shootingDelay > Time.time) return;
            //float lag = Time.time - _lastShootTime + _shootingDelay;
            //_lastShootTime = Time.time;

            if (_timeout > 0)
            {
                _timeout -= Time.deltaTime;
                return;
            }

            Debug.Log(_timeout);

            foreach (var bulletData in _bulletsData)
            {
                var bullet = Instantiate(_bulletPrefab).GetComponent<Bullet>();
                bullet.Initialize(bulletData, transform.position);

                bullet.Move(-_timeout);
                //bullet.Move(lag);
            }

            _timeout = _shootingDelay;
        }
    }
}