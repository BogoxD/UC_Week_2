using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform gunBarrel;
    [SerializeField] LayerMask hitMask;

    [Header("Gun Parameters")]
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

        AimCannon(gunBarrel);
    }
    private void AimCannon(Transform cannonTrans)
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (mouseX != 0 || mouseY != 0)
        {
            //cast ray from camera onto game world
            Vector3 mousePointInWorldSpace = Vector3.one;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, hitMask))
            {
                //get point on the hit collider
                mousePointInWorldSpace = hit.point;

                //move cannon look towards hit point
                cannonTrans.forward = hit.point - cannonTrans.position;

                //limit gun rotation
                LimitXRotation(cannonTrans, -gunElevation, gunElevation);

            }
        }
    }
    private void LimitXRotation(Transform transform, float minRot, float maxRot)
    {
        Vector3 eulerAnglesLocal = transform.localEulerAngles;

        eulerAnglesLocal.x = (eulerAnglesLocal.x > 180) ? eulerAnglesLocal.x - 360 : eulerAnglesLocal.x;
        eulerAnglesLocal.x = Mathf.Clamp(eulerAnglesLocal.x, minRot, maxRot);

        transform.localRotation = Quaternion.Euler(new Vector3(eulerAnglesLocal.x, 0, 0));
    }

}
