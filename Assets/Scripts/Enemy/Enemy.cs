using UnityEngine;
using Weapon;

namespace Enemy
{
    // Enemy.Enemy looks weird
    public class Enemy : MonoBehaviour
    {
        [SerializeField] protected float _movementSpeed;
        [SerializeField] protected int _maxHealth;

        protected int _currentHealth;
        
        void Update()
        {
            Move();
        }

        protected virtual void Move()
        {
            transform.Translate(Vector3.down * (_movementSpeed * Time.deltaTime));
        }
    }
}
