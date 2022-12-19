using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Projectile
{
    public class Bullet : MonoBehaviour, IProjectile
    {
        [Serializable]
        public class BulletData : IProjectile.ProjectileData
        {
            [SerializeField] private float _size;

            public float Size => _size;
        }

        private Vector3 _direction;
        private float _speed;
        private int _damage;

        public void Initialize(IProjectile.ProjectileData projectileData, Vector3 pos)
        {
            if (projectileData is not BulletData bulletData)
            {
                Debug.LogError("ERROR: Passing invalid Data in Bullet");
                return;
            }
            
            _damage = bulletData.Damage;
            _speed = bulletData.Speed;
            _direction = bulletData.Direction;

            var localTransform = transform;
            localTransform.position = pos + bulletData.RelativeSpawnPos;
            localTransform.rotation = Utils.LookAt2D(_direction);
            localTransform.localScale = new Vector3(bulletData.Size, bulletData.Size, bulletData.Size);
        }

        private void Update()
        {
            Move(Time.deltaTime);
            if (transform.position.x is > 100 or < -100 || transform.position.y is > 100 or < -100)
                Destroy(gameObject);
        }

        public void Move(float delta)
        {
            transform.position += _direction * (delta * _speed);
        }
    }
}