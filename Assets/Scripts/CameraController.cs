using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VehicleController))]
public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject reverseCamera;

    private VehicleController vehicleController;

    void Start()
    {
        vehicleController = GetComponent<VehicleController>();
    }

    void FixedUpdate()
    {
        if(vehicleController.isReversing)
        {
            mainCamera.gameObject.SetActive(false);
            reverseCamera.gameObject.SetActive(true);
        }
        else
        {
            mainCamera.gameObject.SetActive(true);
            reverseCamera.gameObject.SetActive(false);
        }
    }
}
