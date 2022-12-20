using Projectiles;
using UnityEngine;

namespace Enemies
{
    // Enemy.Enemy looks weird
    public class Enemy : MonoBehaviour
    {
        [SerializeField] protected float _movementSpeed;
        [SerializeField] protected int _maxHealth;
        [SerializeField] protected int _touchDamage;
        
        protected int _currentHealth;

        public int TouchDamage => _touchDamage;


        private void Start()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            _currentHealth = _maxHealth;
        }

        void Update()
        {
            Move();
        }

        protected virtual void Move()
        {
            transform.Translate(Vector3.down * (_movementSpeed * Time.deltaTime));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var projectile = other.GetComponent<Projectile>();
            if (projectile is not null)
            {
                if (projectile.TargetPlayer)
                    return;
                
                _currentHealth -= projectile.Damage;
                print($"Taken {projectile.Damage} damage. HP: {_currentHealth}");
                if(_currentHealth <= 0)
                    Destroy(gameObject);
                
                return;
            }

            var player = other.GetComponent<Player>();
            if (player is not null)
            {
                _currentHealth -= player.TouchDamage;
                print($"Taken {player.TouchDamage} damage. HP: {_currentHealth}");
                if(_currentHealth <= 0)
                    Destroy(gameObject);
                
                return;
            }
        }
    }
}