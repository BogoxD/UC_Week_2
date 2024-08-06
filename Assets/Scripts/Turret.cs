using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] Transform gunBarrel;
    public float gunElevation = 10f;

    void Update()
    {
        //create plane
        var plane = new Plane(Vector3.up, transform.position);
        //create Ray from mouse Point
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //plane.Raycast returns distance from ray start to hit point
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);

            transform.LookAt(hitPoint);
        }

    }
}
