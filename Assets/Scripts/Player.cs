using UnityEngine;
using Weapon;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;

    private IWeapon[] _weapons;

    public event IWeapon.ShootAction Shoot;
    private PlayerInput playerInput;
    private PlayerInput.PlayerActions playerActions;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerActions = playerInput.Player;
        _weapons = gameObject.GetComponentsInChildren<IWeapon>();
        foreach (var weapon in _weapons)
            Shoot += weapon.Shoot;
        Debug.Log(_weapons.Length);

        //Shoot += GetComponentInChildren<IWeapon>().Shoot;
        //Shoot += _weaponTest.GetComponent<IWeapon>().Shoot;
        if (Shoot == null)
            print("WHYYYY");
    }

    void Update()
    {
        // Movement
        Move(playerActions.Move.ReadValue<Vector2>());

        if (Input.GetKey(KeyCode.Space) && Shoot != null)
            Shoot();
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
        playerActions.Enable();    
    }

    private void OnDisable()
    {
        playerActions.Disable();    
    }

}