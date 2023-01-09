using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _powerups;
    private void OnEnable()
    {
        Enemies.Enemy.Died += SpawnPowerup;
    }

    private void OnDisable()
    {
        Enemies.Enemy.Died -= SpawnPowerup;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void SpawnPowerup(Vector3 position)
    {
        Instantiate(_powerups[Random.Range(0, _powerups.Length)], position, Quaternion.identity);
    }
}
