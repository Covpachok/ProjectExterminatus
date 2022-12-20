using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private float _chipSpeed = 2f; // Speed of health bar fade
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private Image _frontHealthBar;
    [SerializeField] private Image _backHealthBar;
    private float _lerpTimer; // idk maybe it could've been a local variable try check it.    

    private Player _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        if(_player is null)
            Debug.Log("ERROR: HealthBar didn't find Player");

        _player.HpChanged += UpdateHpBar;
    }

    private void LateUpdate()
    {
        var fillF = _frontHealthBar.fillAmount; // Saves a fill amount of hp bar
        var fillB = _backHealthBar.fillAmount;
        var hFraction =
            _player.CurrentHp / (float)_player.MaxHp; // Decimal view of cur. Hp yo max hp, to compare with fill amount
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

    public void UpdateHpBar()
    {
        _hpText.SetText(_player.CurrentHp + "/" + _player.MaxHp);
        _lerpTimer = 0;
    }
}