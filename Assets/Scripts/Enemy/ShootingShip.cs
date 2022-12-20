using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class ShootingShip : Enemy
    {
        [SerializeField] private float _delayBetweenShots;
            
        private Weapon.Weapon _weapon;

        private void Start()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();
            _weapon = GetComponentInChildren<Weapon.Weapon>();
            if(_weapon is null)
                Debug.LogError("ERROR: ShootingShip have no attached weapon");
            
        }

        void Update()
        {
            Move();
            _weapon.Shoot();
        }

        private IEnumerator Shoot()
        {
            _weapon.Shoot();
            yield return new WaitForSeconds(_delayBetweenShots);
            StartCoroutine(Shoot());
        }
    }
}
