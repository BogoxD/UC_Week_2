using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Police_Light_Anim : MonoBehaviour
{
    public bool LightFlicker2 = false;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        animator.SetBool("Light_2", LightFlicker2);
    }
}
