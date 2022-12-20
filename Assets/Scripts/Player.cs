using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Combat")]
    [Tooltip("Combat stats idk")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private int _touchDamage = 50;

    [Header("Health")]
    [Tooltip("Fields for health and healthbar things")]
    // Serialized only for debugging
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _chipSpeed = 2f; // Speed of health bar fade
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private Image _frontHealthBar;
    [SerializeField] private Image _backHealthBar;
    private float _lerpTimer; // idk maybe it could'v been a loacal variable try check it.
    public int TouchDamage => _touchDamage;

    private Weapon.Weapon[] _weapons;

    private PlayerInput _playerInput;
    private PlayerInput.PlayerActions _playerActions;

    private void Awake()
    {
        _playerInput = new PlayerInput(); // Located in Input Actions
        _playerActions = _playerInput.Player;
        _weapons = gameObject.GetComponentsInChildren<Weapon.Weapon>();
        _currentHealth = _maxHealth;
        _hpText.SetText(_currentHealth.ToString());
        Debug.Log("Player weapons amount: " + _weapons.Length);
    }

    void Update()
    {
        // Movement
        Move(_playerActions.Move.ReadValue<Vector2>());

        if (_playerActions.Fire.IsPressed())
            foreach (var weapon in _weapons)
                weapon.Shoot();
        UpdatePlayerHPBar();
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.up) * 10);
    }

    private void Move(Vector2 input)
    {
        // Input system
        var vertical = input.y * Vector3.up; //Input.GetAxis("Vertical") * Vector3.up;
        var horizontal = input.x * Vector3.right; //Input.GetAxis("Horizontal") * Vector3.right;
        transform.Translate((horizontal + vertical) * (Time.deltaTime * _movementSpeed));

        // Bababooye Original
        // var vertical = Input.GetAxis("Vertical") * Vector3.up; //Input.GetAxis("Vertical") * Vector3.up;
        // var horizontal = Input.GetAxis("Horizontal") * Vector3.right; //Input.GetAxis("Horizontal") * Vector3.right;
        // transform.Translate((horizontal + vertical) * (Time.deltaTime * _movementSpeed));
    }

    private void OnEnable()
    {
        _playerActions.Enable();
    }

    private void OnDisable()
    {
        _playerActions.Disable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Player hit {other.name}");
        var projectile = other.GetComponent<Projectile.Projectile>();
        if (projectile is not null)
        {
            if (!projectile.TargetPlayer)
                return;

            TakeDamage(projectile.Damage); // Taking damage is now separate func
            if (_currentHealth <= 0)
                OnPlayerDeath();
            return;
        }

        var enemy = other.GetComponent<Enemy.Enemy>();
        if (enemy is not null)
        {
            TakeDamage(enemy.TouchDamage);
            if (_currentHealth <= 0)
                OnPlayerDeath();
            return;
        }
    }

    private void UpdatePlayerHPBar()
    {
        float fillF = _frontHealthBar.fillAmount; // Saves a fill amount of hp bar
        float fillB = _backHealthBar.fillAmount;
        float hFraction = (float)_currentHealth / (float)_maxHealth; // Decimal view of cur. Hp yo max hp, to compare with fill amount
        if (fillB > hFraction) // If took damage
        {
            _frontHealthBar.fillAmount = hFraction;
            _backHealthBar.color = Color.red;
            _lerpTimer += Time.deltaTime;
            float percentComplete = _lerpTimer / _chipSpeed;
            _backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction) // If heals
        {
            _backHealthBar.color = Color.green;
            _backHealthBar.fillAmount = hFraction;
            _lerpTimer += Time.deltaTime;
            float percentComplete = _lerpTimer / _chipSpeed;
            _frontHealthBar.fillAmount = Mathf.Lerp(fillF, _backHealthBar.fillAmount, percentComplete);
        }
    }

    private void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        _lerpTimer = 0f;
        _hpText.SetText(_currentHealth.ToString());
    }

    private void RestoreHealth(int amount)
    {
        // Need to add Mathf.Clamp somewhere to prevent hp from bypassing max limit
        _currentHealth -= amount;
        _lerpTimer = 0f;
        _hpText.SetText(_currentHealth.ToString());
    }

    private void OnPlayerDeath()
    {
        print("Player is DEAD (X o X)");
        Destroy(gameObject);
    }
}