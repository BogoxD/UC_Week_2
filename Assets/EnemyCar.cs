using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCar : MonoBehaviour
{
    public int MaxHealth = 100;
    private int _currentHealth;

    private WeaponSystem playerWeaponSystem;

    private void Start()
    {
        _currentHealth = MaxHealth;
        playerWeaponSystem = FindObjectOfType<WeaponSystem>();
    }
    private void FixedUpdate()
    {
        if (_currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    public void TakeDamage(int ammount)
    {
        _currentHealth -= ammount;
    }
}
