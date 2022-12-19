using UnityEngine;
using Weapon;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        void Start()
        {
            var test = GetComponentsInChildren<IWeapon>();
            if (test is null)
                print("wtf");

        }

        void Update()
        {
        
        }
    }
}
