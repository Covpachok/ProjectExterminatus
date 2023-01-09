using System.Collections;
using UnityEngine;

public class Shield : Entity
{
    [SerializeField] private float _tick = 0.5f;
    [SerializeField] private int _recoveryPerTick = 1;
    [SerializeField] private float _recoveryDelay = 5f;
    
    private bool _isDamageTakenRecently;
    
    private Material _modelMaterial;

    void Start()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        base.Initialize();
        _modelMaterial = transform.Find("Model").GetComponent<Renderer>().material;
        UpdateMaterial(CurrentHealth, _maxHealth);
        
        Player.HpFullyRestored += Restore;
        HpChanged += UpdateMaterial;
    }

    private void Update()
    {
        if (IsDead)
            return;

        if (Time.time > LastTimeDamageTaken + _recoveryDelay && _isDamageTakenRecently)
        {
            _isDamageTakenRecently = false;
            StartCoroutine(HealthRecovery());
        }
    }

    private IEnumerator HealthRecovery()
    {
        while (CurrentHealth != _maxHealth && !_isDamageTakenRecently)
        {
            RecoverHealth();
            yield return new WaitForSeconds(_tick);
        }
    }

    private void RecoverHealth()
    {
        RecoverHealth(_recoveryPerTick);
    }
    
    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        _isDamageTakenRecently = true;
    }

    private void Restore()
    {
        IsDead = false;
        // Delay before shield restoration
        LastTimeDamageTaken = Time.time;
    }

    private void UpdateMaterial(int curr, int max)
    {
        var color = _modelMaterial.color;
        _modelMaterial.color = new Color(color.r, color.g, color.b, CurrentHealth / (float)_maxHealth * 0.5f);
    }
}
