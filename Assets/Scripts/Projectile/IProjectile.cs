using System;
using UnityEngine;

namespace Projectile
{
    public interface IProjectile
    {
        [Serializable]
        public abstract class ProjectileData
        {
            [SerializeField] private Vector3 _relativeSpawnPos;
            [SerializeField] private Vector3 _direction;
            [SerializeField] private float _speed;
            [SerializeField] private int _damage;
            [SerializeField] private bool _targetPlayer;

            public Vector3 RelativeSpawnPos => _relativeSpawnPos;
            public Vector3 Direction => _direction;
            public float Speed => _speed;
            public int Damage => _damage;
            public bool TargetPlayer => _targetPlayer;
        }

        public void Initialize(ProjectileData data, Vector3 pos);
        public void Move(float delta);
    }
}