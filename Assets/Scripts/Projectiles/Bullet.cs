using System;
using UnityEngine;

namespace Projectiles
{
    public class Bullet : Projectile
    {
        [Serializable]
        public class BulletData : Projectile.ProjectileData
        {
            [SerializeField] private float _size;

            public float Size => _size;
        }

        private Vector3 _direction;

        public override void Initialize(Projectile.ProjectileData projectileData, Vector3 pos, bool targetPlayer)
        {
            if (projectileData is not BulletData bulletData)
            {
                Debug.LogError("ERROR: Passing invalid Data in Bullet");
                return;
            }
            
            _damage = bulletData.Damage;
            _speed = bulletData.Speed;
            _direction = bulletData.Direction;
            _targetPlayer = targetPlayer;

            var localTransform = transform;
            localTransform.position = pos + bulletData.RelativeSpawnPos;
            localTransform.rotation = Utils.LookAt2D(_direction);
            localTransform.localScale = new Vector3(bulletData.Size, bulletData.Size, bulletData.Size);
        }

        private void Update()
        {
            Move(Time.deltaTime);
        }

        public override void Move(float delta)
        {
            transform.position += _direction * (delta * _speed);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_targetPlayer)
            {
                var player = other.GetComponent<Player>();
                if (player is not null)
                {
                    Debug.Log("HIT PLAYER!!");
                    Destroy(gameObject);
                }
            }
            else
            {
                var enemy = other.GetComponent<Enemies.Enemy>();
                if (enemy is not null)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}