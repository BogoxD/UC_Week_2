using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCar : MonoBehaviour
{
    [Header("Health")]
    public int MaxHealth = 100;

    [Header("Effects")]
    [SerializeField] GameObject explosionEffect;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip explosionAudio;

    private int _currentHealth;
    private bool _alive = true;
    private Material _material;
    private WeaponSystem _playerWeaponSystem;
    private Rigidbody _rb;

    private void Start()
    {
        //set current health
        _currentHealth = MaxHealth;

        //get references
        _rb = GetComponent<Rigidbody>();
        _playerWeaponSystem = FindObjectOfType<WeaponSystem>();
        _material = GetComponent<Material>();
    }
    public void TakeDamage(int ammount)
    {
        _currentHealth -= ammount;

        //check if health is bellow or equals to 0
        if (_currentHealth <= 0 && _alive)
        {
            _alive = false;

            //add upward force to rigidbody
            _rb.AddForce(transform.up * 2f, ForceMode.Impulse);

            //apply effects with delay
            StartCoroutine(ApplyEffectsWithDelay());
        }
    }

    IEnumerator ApplyEffectsWithDelay()
    {
        //instantiate explosion effect
        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);

        //play audio
        audioSource.PlayOneShot(explosionAudio);
    }
}
