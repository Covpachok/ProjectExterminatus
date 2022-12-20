using System.Collections;
using Projectile;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapon
{
    public class MissileLauncher : Weapon
    {
        [SerializeField] private GameObject _missilePrefab;

        [SerializeField] private Missile.MissileData _missileData;

        [SerializeField] private int _maxCharges;
        [SerializeField] private int _chargesReloadingAmount;
        [SerializeField] private int _minChargesShoot;
        [SerializeField] private int _maxChagresShoot;
        [SerializeField] private float _chargesReloadingDelay;
        [SerializeField] private float _shootingDelay;

        private int _currentCharges;
        private float _lastShootTime;

        private void Start()
        {
            _currentCharges = _maxCharges;
            StartCoroutine(UpdateCharges());
        }

        private IEnumerator UpdateCharges()
        {
            if (_currentCharges < _maxCharges)
                _currentCharges += _chargesReloadingAmount;

            if (_currentCharges > _maxCharges)
                _currentCharges = _maxCharges;

            yield return new WaitForSeconds(_chargesReloadingDelay);
            if (_currentCharges != _maxCharges)
                StartCoroutine(UpdateCharges());
        }

        public override void Shoot()
        {
            if (_currentCharges <= _minChargesShoot || _lastShootTime + _shootingDelay > Time.time)
                return;

            _lastShootTime = Time.time;

            var chargesToShoot = _currentCharges > _maxChagresShoot ? _maxChagresShoot : _currentCharges;
            var pos = transform.position;

            for (int i = 0; i < chargesToShoot; ++i)
            {
                var missile = Instantiate(_missilePrefab).GetComponent<Missile>();
                missile.Initialize(_missileData,
                    new Vector3(pos.x + Random.value - 0.5f, pos.y + Random.value / 2, 0), _targetPlayer);
            }

            _currentCharges -= chargesToShoot;
            StartCoroutine(UpdateCharges());
        }
    }
}