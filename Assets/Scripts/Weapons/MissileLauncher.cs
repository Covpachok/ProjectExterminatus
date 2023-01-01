using System;
using System.Collections;
using Projectiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    public class MissileLauncher : Weapon
    {
        [SerializeField] private GameObject _missilePrefab;

        [SerializeField] private Missile.MissileData _missileData;

        [Space(10)]
        [SerializeField] private int _maxChargesAmount;

        [SerializeField] private int _maxChargesPerShot;
        [SerializeField] private int _minChargesPerShot;

        [SerializeField] private int _reloadingAmount;
        [SerializeField] private float _reloadingDelay;

        [SerializeField] private float _shootingDelay;

        [Space(25)]
        // SerializeField FOR DEBUGGING
        [SerializeField] private int _currentCharges;

        private float _lastShotTime;

        private void Start()
        {
            _currentCharges = _maxChargesAmount;
            StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            for (;;)
            {
                if (_currentCharges < _maxChargesAmount)
                    _currentCharges += _reloadingAmount;

                if (_currentCharges > _maxChargesAmount)
                    _currentCharges = _maxChargesAmount;

                yield return new WaitForSeconds(_reloadingDelay);
            }
        }

        public override void Shoot()
        {
            if (_currentCharges <= _maxChargesPerShot || _lastShotTime + _shootingDelay > Time.time)
                return;

            _lastShotTime = Time.time;

            var chargesToShoot = _currentCharges > _minChargesPerShot ? _minChargesPerShot : _currentCharges;
            var pos = transform.position;

            for (int i = 0; i < chargesToShoot; ++i)
            {
                var missile = Instantiate(_missilePrefab).GetComponent<Missile>();
                missile.Initialize(_missileData,
                    new Vector3(pos.x + Random.value - 0.5f, pos.y + Random.value / 2, 0), _targetPlayer);
            }

            _currentCharges -= chargesToShoot;
        }
    }
}