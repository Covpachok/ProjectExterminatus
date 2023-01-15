using UnityEngine;
using System;
using ObjectPools;

namespace Enemies
{
    public class Enemy : Entity
    {
        public static Action Died;
        
        [SerializeField] protected float _movementSpeed;

        public GameObjectPool TEMPPOOL;

        void Start()
        {
            Initialize();
        }

        void Update()
        {
            Move();
        }

        protected virtual void Move()
        {
            transform.Translate(Vector3.down * (_movementSpeed * Time.deltaTime));
        }

        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);
            if (IsDead)
                Die();
        }

        private void Die()
        {
            Died?.Invoke();
            TEMPPOOL.Pool.Release(gameObject);
            //Destroy(gameObject);
        }
    }
}