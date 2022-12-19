using System;
using System.Collections;
using UnityEngine;
using Weapon;

namespace Enemy
{
    public class ShootingShip : Enemy
    {
        [SerializeField] private float _delayBetweenShots;
            
        private IWeapon _weapon;

        private void Start()
        {
            _weapon = GetComponentInChildren<IWeapon>();
            if(_weapon is null)
                Debug.LogError("ERROR: ShootingShip have no attached weapon");
            
            //StartCoroutine(Shoot());
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
