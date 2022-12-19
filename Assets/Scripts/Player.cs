using UnityEngine;
using Weapon;

public class Player : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;

    private IWeapon[] _weapons;

    public event IWeapon.ShootAction Shoot;

    private void Awake()
    {
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
        var vertical = Input.GetAxis("Vertical") * Vector3.up;
        var horizontal = Input.GetAxis("Horizontal") * Vector3.right;
        transform.Translate((vertical + horizontal) * (Time.deltaTime * _movementSpeed));

        if (Input.GetKey(KeyCode.Space) && Shoot != null)
            Shoot();
    }
}