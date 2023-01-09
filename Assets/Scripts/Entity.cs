using System;
using Projectiles;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Action<int, int> HpChanged;

    [SerializeField] private int _currentHealth;
    [SerializeField] protected int _maxHealth;

    [Space(10)]
    [SerializeField] private int _touchDamage;

    [SerializeField] private float _invulnerabilityTime = 0.25f;

    [SerializeField] private bool _isPlayer;

    [Space(25)]
    private bool _isDead;

    protected float LastTimeDamageTaken;

    public int TouchDamage
    {
        get => _touchDamage;
        protected set => _touchDamage = value;
    }

    public int CurrentHealth
    {
        get => _currentHealth;
        protected set => _currentHealth = value;
    }

    public bool IsDead
    {
        get => _isDead;
        protected set => _isDead = value;
    }

    public bool IsPlayer => _isPlayer;


    protected virtual void Initialize()
    {
        _currentHealth = _maxHealth;
    }

    public virtual void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            IsDead = true;
        }

        LastTimeDamageTaken = Time.time;

        print($"{gameObject.name}: taken damage {amount}");
        HpChanged?.Invoke(CurrentHealth, _maxHealth);
    }

    public virtual void RecoverHealth(int amount)
    {
        _currentHealth += amount;
        if (_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;

        HpChanged?.Invoke(_currentHealth, _maxHealth);
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
        if (entity is not null && !IsInvulnureable())
        {
            TakeDamage(entity._touchDamage);
        }
    }

    public bool IsInvulnureable() => LastTimeDamageTaken + _invulnerabilityTime > Time.time;
}