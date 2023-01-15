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
        UpdateMaterial(CurrentHp, MaxHp);
        
        Player.HpFullyRestored += Revive;
        HpChanged += UpdateMaterial;
    }

    private void Update()
    {
        if (IsDead)
            return;

        if (Time.time > LastTimeDamageTaken + _recoveryDelay && _isDamageTakenRecently)
        {
            _isDamageTakenRecently = false;
            StartCoroutine(HealthRegeneration());
        }
    }

    private IEnumerator HealthRegeneration()
    {
        while (CurrentHp != MaxHp && !_isDamageTakenRecently)
        {
            RestoreHealth();
            yield return new WaitForSeconds(_tick);
        }
    }

    private void RestoreHealth()
    {
        RestoreHp(_recoveryPerTick);
    }
    
    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        _isDamageTakenRecently = true;
    }

    public override void Revive()
    {
        base.Revive();
        // Delay before shield restoration
        LastTimeDamageTaken = Time.time;
    }

    private void UpdateMaterial(int curr, int max)
    {
        var color = _modelMaterial.color;
        print($"{CurrentHp}/{MaxHp} - {CurrentHp / (float)MaxHp * 0.5f}");
        _modelMaterial.color = new Color(color.r, color.g, color.b, CurrentHp / (float)MaxHp * 0.5f);
    }
}
