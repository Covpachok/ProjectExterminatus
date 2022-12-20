using Projectiles;
using UnityEngine;
using UnityEngine.Events;
using Weapons;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private int _touchDamage = 50;
    // Serialized only for debugging
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxHealth;
    
    [Space(25)]
    
    [SerializeField] private UnityEvent _onHpChange;
    
    public int TouchDamage => _touchDamage;
    public int MaxHp => _maxHealth;
    public int CurrentHp => _currentHealth;

    private Weapon[] _weapons;

    private PlayerInput _playerInput;
    private PlayerInput.PlayerActions _playerActions;

    private void Awake()
    {
        _playerInput = new PlayerInput(); // Located in Input Actions
        _playerActions = _playerInput.Player;
        _weapons = gameObject.GetComponentsInChildren<Weapon>();
        _currentHealth = _maxHealth;
        Debug.Log("Player weapons amount: " + _weapons.Length);
    }

    void Update()
    {
        // Movement
        Move(_playerActions.Move.ReadValue<Vector2>());

        if (_playerActions.Fire.IsPressed())
            foreach (var weapon in _weapons)
                weapon.Shoot();
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
        var projectile = other.GetComponent<Projectile>();
        if (projectile is not null)
        {
            if (!projectile.TargetPlayer)
                return;

            TakeDamage(projectile.Damage); // Taking damage is now separate func
            if (_currentHealth <= 0)
                OnPlayerDeath();
            return;
        }

        var enemy = other.GetComponent<Enemies.Enemy>();
        if (enemy is not null)
        {
            TakeDamage(enemy.TouchDamage);
            if (_currentHealth <= 0)
                OnPlayerDeath();
            return;
        }
    }


    private void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        
        _onHpChange.Invoke();
        
        if(_currentHealth <= 0)
            OnPlayerDeath();
    }

    private void RestoreHealth(int amount)
    {
        _currentHealth += amount;
        if (_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;
        
        _onHpChange.Invoke();
    }

    private void OnPlayerDeath()
    {
        print("Player is DEAD (X o X)");
        Destroy(gameObject);
    }
}