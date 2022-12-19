using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Projectile
{
    public class Missile : MonoBehaviour, IProjectile
    {
        [Serializable]
        public class MissileData : IProjectile.ProjectileData
        {
            [SerializeField] private float _minSize;
            [SerializeField] private float _maxSize;
            [SerializeField] private float _rotationSpeed;
            [SerializeField] private float _detectionRange;

            public float MinSize => _minSize;
            public float MaxSize => _maxSize;
            public float RotationSpeed => _rotationSpeed;
            public float DetectionRange => _detectionRange;
        }

        private Vector3 _currentDirection;
        private int _damage;
        private float _speed;
        private float _rotationSpeed;

        public void Initialize(IProjectile.ProjectileData projectileData, Vector3 pos)
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

            var localTransform = transform;
            localTransform.position = pos + missileData.RelativeSpawnPos;
            localTransform.rotation = Utils.LookAt2D(_currentDirection);
            var scale = Mathf.Lerp(missileData.MinSize, missileData.MaxSize, Random.value);
            localTransform.localScale = Vector3.one * scale;
        }

        public void Move(float delta)
        {
            
        }
    }
}