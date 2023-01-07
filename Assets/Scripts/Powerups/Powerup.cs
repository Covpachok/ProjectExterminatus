using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private PowerUpEffect _powerUpEffect;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player is not null)
        {
            Destroy(gameObject);
            _powerUpEffect.Apply(other.gameObject);
        }
    }
}
