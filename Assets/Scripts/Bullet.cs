using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] LayerMask impactLayer;
    [SerializeField] GameObject impactPrefab;
    void Start()
    {
        StartCoroutine(DestroyBullet(5f));
    }

    IEnumerator DestroyBullet(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
