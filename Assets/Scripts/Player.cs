using System;
using Projectiles;
using Enemies;
using UnityEngine;
using Weapons;

public class Player : Entity
{
    //public delegate void HpChangedEventHandler(int CurrentHealth, int _maxHealth);
    public static Action HpFullyRestored;
    
    [Header("Stats")]
    [SerializeField] private float _movementSpeed;
    // Serialized only for debugging
    
    private Weapon[] _weapons;
    private Shield _shield;

    private PlayerInput _playerInput;
    private PlayerInput.PlayerActions _playerActions;
    

    public bool _TEMPTRIGGER;

    private void Awake()
    {
        _playerInput = new PlayerInput(); // Located in Input Actions
        _playerActions = _playerInput.Player;
        _weapons = gameObject.GetComponentsInChildren<Weapon>();
        CurrentHealth = _maxHealth;
        Debug.Log("Player weapons amount: " + _weapons.Length);
    }

    private void Start()
    {
        _shield = transform.Find("Shield").GetComponent<Shield>();
        
        if(_shield is null)
            Debug.LogError("ERROR: Shield not found.");
        
        HpChanged?.Invoke(CurrentHealth, _maxHealth);
    }

    private void Update()
    {
        // Movement
        Move(_playerActions.Move.ReadValue<Vector2>());

        if (_playerActions.Fire.IsPressed())
            foreach (var weapon in _weapons)
                weapon.Shoot();

        if (_TEMPTRIGGER)
        {
            _TEMPTRIGGER = false;
            HpFullyRestored.Invoke();
        }
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

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (!_shield.IsDead)
            return;
        
        base.OnTriggerEnter2D(other);
    }


    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if(IsDead)
            Die();
    }

    private void Die()
    {
        print("Player is DEAD (X o X)");
        //Destroy(gameObject);
    }
}