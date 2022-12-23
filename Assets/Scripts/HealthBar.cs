using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private float _chipSpeed = 2f; // Speed of health bar fade
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private Image _frontHealthBar;
    [SerializeField] private Image _backHealthBar;
    private int _playerCurrentHP;
    private int _playerMaxHP;
    private float _lerpTimer; // idk maybe it could've been a local variable try check it.    

    private void Awake()
    {
        _hpText.SetText(_playerCurrentHP + "/" + _playerMaxHP);
    }

    private void LateUpdate()
    {
        var fillF = _frontHealthBar.fillAmount; // Saves a fill amount of hp bar
        var fillB = _backHealthBar.fillAmount;
        var hFraction =
            _playerCurrentHP / (float)_playerMaxHP; // Decimal view of cur. Hp to max hp, to compare with fill amount
        float percentComplete;
        if (fillB > hFraction) // If took damage
        {
            _frontHealthBar.fillAmount = hFraction;
            _backHealthBar.color = Color.red;
            _lerpTimer += Time.deltaTime;
            percentComplete = _lerpTimer / _chipSpeed;
            _backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }

        if (fillF < hFraction) // If heals
        {
            _backHealthBar.color = Color.green;
            _backHealthBar.fillAmount = hFraction;
            _lerpTimer += Time.deltaTime;
            percentComplete = _lerpTimer / _chipSpeed;
            _frontHealthBar.fillAmount = Mathf.Lerp(fillF, _backHealthBar.fillAmount, percentComplete);
        }
        
    }

    private void SetPlayerStats(int CurrentHp, int MaxHp)
    {
        _playerCurrentHP = CurrentHp;
        _playerMaxHP = MaxHp;
        UpdateHpBar();
    }

    public void UpdateHpBar()
    {
        _hpText.SetText(_playerCurrentHP + "/" + _playerMaxHP);
        _lerpTimer = 0;
    }

    private void OnEnable()
    {
        Player.HpChanged += SetPlayerStats;
    }

    private void OnDisable()
    {
        Player.HpChanged -= SetPlayerStats;
    }
}