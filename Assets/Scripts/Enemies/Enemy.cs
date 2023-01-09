using UnityEngine;
using System;

namespace Enemies
{
    public class Enemy : Entity
    {
        public static Action<Vector3> Died;
        
        [SerializeField] protected float _movementSpeed;

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
            Died?.Invoke(transform.position);
            Destroy(gameObject);
        }
    }
}