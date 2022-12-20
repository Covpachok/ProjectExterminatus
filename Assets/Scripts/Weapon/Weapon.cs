using UnityEngine;

namespace Weapon
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected bool _targetPlayer;
        
        public delegate void ShootAction();
        public virtual void Shoot()
        {}
    }
}
