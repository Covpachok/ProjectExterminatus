using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Projectiles
{
    public class Missile : Projectile
    {
        [Serializable]
        public class MissileData : Projectile.ProjectileData
        {
            [SerializeField] private float _minSize;
            [SerializeField] private float _maxSize;
            [SerializeField] private float _rotationSpeed;
            [SerializeField] private float _detectionRange;
            [SerializeField] private float _targetPursuitAcceleration;
            [SerializeField] private float _wanderStrength;

            public float MinSize => _minSize;
            public float MaxSize => _maxSize;
            public float RotationSpeed => _rotationSpeed;
            public float DetectionRange => _detectionRange;
            public float TargetPursuitAcceleration => _targetPursuitAcceleration;
            public float WanderStrength => _wanderStrength;
        }

        private Vector3 _currentDirection;
        private float _targetPursuitAcceleration;
        private float _rotationSpeed;
        private float _detectionRange;
        private float _wanderStrength;

        private GameObject _target;

        public override void Initialize(Projectile.ProjectileData projectileData, Vector3 pos, bool targetPlayer)
        {
            if (projectileData is not MissileData missileData)
            {
                Debug.LogError("ERROR: Passing invalid Data in Missile");
                return;
            }

            _damage = missileData.Damage;
            _speed = missileData.Speed;
            _rotationSpeed = missileData.RotationSpeed;
            _currentDirection = missileData.Direction;
            _targetPlayer = targetPlayer;
            _detectionRange = missileData.DetectionRange;
            _targetPursuitAcceleration = missileData.TargetPursuitAcceleration;
            _wanderStrength = missileData.WanderStrength;

            var localTransform = transform;
            localTransform.position = pos + missileData.RelativeSpawnPos;
            localTransform.rotation = Utils.LookAt2D(_currentDirection);
            var scale = Mathf.Lerp(missileData.MinSize, missileData.MaxSize, Random.value);
            localTransform.localScale = Vector3.one * scale;
        }

        private void Update()
        {
            if (_target is not null)
            {
                if (_target.IsDestroyed())
                    _target = null;
                else
                    MoveTowardsTarget(Time.deltaTime);
            }
            else
            {
                Move(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            if (_target is not null)
                return;

            // 1 << 7 layer ENEMY
            // 1 << 6 layer PLAYER
            int layer = _targetPlayer ? 1 << 6 : 1 << 7;

            var circle = Physics2D.OverlapCircle(transform.position, _detectionRange, layer);
            if (circle is null)
                return;
            _target = circle.gameObject;
            print(_target.name);

            /*
            var hit = Physics2D.Raycast(transform.position, _currentDirection,
                _detectionRange, layer);

            if (hit.collider is null)
            {
                Debug.DrawRay(transform.position, _currentDirection * _detectionRange,
                    Color.yellow);
                return;
            }

            Debug.DrawRay(transform.position, _currentDirection * hit.distance,
                Color.red);
                
            _target = hit.collider.gameObject;
            */
        }

        //private Vector3 _velocity;
        //private float _steerStrenght = 2;

        public override void Move(float delta)
        {
            _currentDirection = (_currentDirection + (Vector3)(Random.insideUnitCircle * _wanderStrength)).normalized;

            /*
            var desiredVelocity = _currentDirection * _speed;
            var desiredSteeringForce = (desiredVelocity - _velocity) * _steerStrenght;
            var acceleration = Vector3.ClampMagnitude(desiredSteeringForce, _steerStrenght) / 1;

            _velocity = Vector3.ClampMagnitude(_velocity + acceleration * delta, _speed);
            transform.Translate(_velocity * delta);
            */
            transform.position += _currentDirection * (delta * _speed);
            transform.rotation = Utils.LookAt2D(_currentDirection);

            Debug.DrawRay(transform.position, _currentDirection * _detectionRange, Color.yellow);
        }

        private void MoveTowardsTarget(float delta)
        {
            var speed = _speed + _targetPursuitAcceleration;
            var heading = _target.transform.position - transform.position;
            var distance = heading.magnitude;
            var _desiredDirection = heading / distance;
            _currentDirection = Utils.Vector3Lerp(_currentDirection, _desiredDirection, _rotationSpeed / 200)
                .normalized;

            Debug.DrawRay(transform.position, _currentDirection * _detectionRange, Color.red);
            transform.rotation = Utils.LookAt2D(_currentDirection);
            transform.position += _currentDirection * (delta * speed);
        }

        private void RotateDirection(float angle)
        {
            transform.Rotate(Vector3.forward, angle);
            _currentDirection = Utils.Rotate2D(_currentDirection, angle);
        }
    }
}