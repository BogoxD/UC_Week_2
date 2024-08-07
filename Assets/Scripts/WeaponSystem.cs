using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [Header("Shooting")]
    public int Damage = 25;
    public float AttackRange = 200f;
    public float upwardForce = 2f;
    public float reloadingTime = 2f;
    public float timeBetweenShots = 0.1f;
    public bool fullAuto = false;
    public float recoilForce = 2f;

    [Header("References")]
    [SerializeField] LayerMask whatIsImpact;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject muzzleFlashPrefab;
    [SerializeField] GameObject impactPrefab;

    [Header("Audio")]
    [SerializeField] AudioSource gunAudioSource;
    [SerializeField] AudioClip gunShot, gunReload;

    private bool isReadyToShoot = true, shooting;
    private float currentReloadingTime;
    private GameObject muzzleFlashEffect;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        //If gun is full auto 
        if (fullAuto)
        {
            shooting = Input.GetMouseButton(0);
            currentReloadingTime = 0;
        }
        else
        {
            shooting = Input.GetMouseButtonDown(0);
            currentReloadingTime = reloadingTime;
        }

        if (shooting && isReadyToShoot)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        //Play Audio
        gunAudioSource.PlayOneShot(gunShot);

        isReadyToShoot = false;

        //Instantiate bullet and effects
        muzzleFlashEffect = Instantiate(muzzleFlashPrefab, firePoint.position, Quaternion.identity);

        //Cast ray to aim and play impact effect
        if (Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, AttackRange, whatIsImpact))
        {
            Instantiate(impactPrefab, hit.point, Quaternion.identity);

            //damage enemies
            if (hit.collider.TryGetComponent(out EnemyCar car))
            {
                car.TakeDamage(Damage);
            }

        }

        //Change direction and add force to the bullet
        muzzleFlashEffect.transform.forward = firePoint.forward;

        //Add force to tank/vehicle
        rb.AddForce(-firePoint.forward * recoilForce, ForceMode.Impulse);

        //Reload Gun
        StartCoroutine(Reloading());
    }
    private IEnumerator Reloading()
    {
        //Reload based if the gun is full auto or not
        if (!fullAuto)
        {
            yield return new WaitForSeconds(gunShot.length);

            gunAudioSource.PlayOneShot(gunReload);

            yield return new WaitForSeconds(currentReloadingTime - gunShot.length);
        }

        yield return new WaitForSeconds(timeBetweenShots);

        isReadyToShoot = true;
    }
}
