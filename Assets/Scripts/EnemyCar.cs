using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCar : MonoBehaviour
{
    [Header("EnemyCar")]
    public int MaxHealth = 100;
    public float Speed = 5f;
    public float AttackDistance = 30f;
    public float AttackFov = 45f;

    [Header("Waypoints")]
    [SerializeField] Transform[] waypoints;
    [SerializeField] float wayPointOffset = 1.5f;

    [Header("Effects")]
    [SerializeField] GameObject explosionEffect;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip explosionAudio;

    private int _currentHealth;
    private int _pathIterrator = 0;
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
    private void FixedUpdate()
    {
        //If this GO is a police car, attack the player when in range
        if (AttackDistance >= Vector3.Distance(transform.position, _playerWeaponSystem.gameObject.transform.position)
            && TryGetComponent<Police_Light_Anim>(out Police_Light_Anim policeCar))
            MoveTowards(_playerWeaponSystem.gameObject.transform);
        else
            MoveTowards(waypoints);
    }
    private void MoveTowards(Transform[] waypointsArray)
    {
        if (_pathIterrator == waypointsArray.Length)
        {
            _pathIterrator = 0;
            //look towards new point
            transform.LookAt(Vector3.Slerp(transform.forward, waypointsArray[_pathIterrator].position, 5f));
        }
        else if (_pathIterrator < waypointsArray.Length)
        {
            //move car towards point
            transform.position = Vector3.MoveTowards(transform.position, waypointsArray[_pathIterrator].position, Speed * Time.deltaTime);

            if (wayPointOffset > Vector3.Distance(transform.position, waypointsArray[_pathIterrator].position))
            {
                //increment path iterrator
                _pathIterrator++;

                //look towards new point
                transform.LookAt(Vector3.Slerp(transform.forward, waypointsArray[_pathIterrator].position, 5f));
            }
        }

    }
    private void MoveTowards(Transform pointTrans)
    {
        //move car towards point
        transform.position = Vector3.MoveTowards(transform.position, pointTrans.position, 5f * Time.deltaTime) - new Vector3(0, -pointTrans.position.y, 0);
        //look towards new point
        transform.LookAt(Vector3.Slerp(transform.forward, pointTrans.position, 5f));
    }
    public void TakeDamage(int ammount)
    {
        _currentHealth -= ammount;

        //check if health is bellow or equals to 0
        if (_currentHealth <= 0 && _alive)
        {
            _alive = false;
            //change speed to 0
            Speed = 0f;
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
