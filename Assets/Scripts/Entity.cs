using System;
using Projectiles;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Action<int, int> HpChanged;

    [SerializeField] private int _currentHp;
    [SerializeField] private int _maxHp;

    [SerializeField] private int _touchDamage;
    [SerializeField] private float _invulnerabilityTime = 0.25f;

    [SerializeField] private bool _isPlayer;

    public int TouchDamage => _touchDamage;

    public int CurrentHp => _currentHp;

    public int MaxHp => _maxHp;

    public bool IsPlayer => _isPlayer;
    public bool IsDead { get; private set; }

    protected float LastTimeDamageTaken { get; set; }


    protected virtual void Initialize()
    {
        _currentHp = _maxHp;
    }

    public virtual void TakeDamage(int amount)
    {
        _currentHp -= amount;
        if (_currentHp <= 0)
        {
            _currentHp = 0;
            IsDead = true;
        }

        LastTimeDamageTaken = Time.time;

        print($"{gameObject.name}: taken damage {amount}");
        HpChanged?.Invoke(CurrentHp, _maxHp);
    }

    public virtual void RestoreHp(int amount)
    {
        _currentHp += amount;
        if (_currentHp > _maxHp)
            _currentHp = _maxHp;

        HpChanged?.Invoke(_currentHp, _maxHp);
    }

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        print($"{gameObject.name}: trigger enter! {IsDead}");
        if (IsDead)
            return;

        print($"trigger is {col.gameObject.name}");

        var projectile = col.GetComponent<Projectile>();
        if (projectile is not null)
        {
            if (projectile.TargetPlayer != IsPlayer)
                return;

            TakeDamage(projectile.Damage);
            return;
        }

        var entity = col.GetComponent<Entity>();
        if (entity is not null && !IsInvulnerable())
        {
            TakeDamage(entity._touchDamage);
        }
    }

    public virtual void Revive()
    {
        IsDead = false;
    }
    
    public bool IsInvulnerable() => LastTimeDamageTaken + _invulnerabilityTime > Time.time;
    public bool IsHpFull() => _currentHp == _maxHp;
}