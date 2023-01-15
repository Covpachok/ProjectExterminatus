using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerup effects/Heal")]
public class PowerUpHeal : PowerUpEffect
{
    [SerializeField] private int _amount;
    public override void Apply(GameObject target)
    {
       target.GetComponent<Player>().RestoreHp(_amount);
    }
}
