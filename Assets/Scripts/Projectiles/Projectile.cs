using System;
using UnityEngine;

namespace Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        [Serializable]
        public abstract class ProjectileData
        {
            [SerializeField] private Vector3 _relativeSpawnPos;
            [SerializeField] private Vector3 _direction;
            [SerializeField] private float _speed;
            [SerializeField] private int _damage;

            public Vector3 RelativeSpawnPos => _relativeSpawnPos;
            public Vector3 Direction => _direction;
            public float Speed => _speed;
            public int Damage => _damage;
        }

        protected int _damage;
        protected float _speed;
        protected bool _targetPlayer;

        public int Damage => _damage;
        public bool TargetPlayer => _targetPlayer;

        public virtual void Initialize(ProjectileData data, Vector3 pos, bool targetPlayer)
        {}

        public virtual void Move(float delta)
        {}
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var entity = other.GetComponent<Entity>();
            if (entity is not null && _targetPlayer == entity.IsPlayer)
            {
                Destroy(gameObject);
            }
        }
    }
}