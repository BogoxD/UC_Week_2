using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [Header("Vehicle Movement")]
    public float Acceleration = 500f;
    public float BreakForce = 300f;
    public float MaxTurnAngle = 15f;

    private float currentAcceleration = 0f;
    private float currentBreakForce = 0f;
    private float currentTurnAngle = 0f;

    [Header("Vehicle Wheels")]
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    [SerializeField] Transform frontRightTransform;
    [SerializeField] Transform frontLeftTransform;
    [SerializeField] Transform backRightTransform;
    [SerializeField] Transform backLeftTransform;

    [Header("Ground Check")]
    [SerializeField] float GroundCheckRadiusSphere = 1f;
    [SerializeField] LayerMask whatIsGround;

    [Header("Audio")]
    [SerializeField] AudioClip _engineAudioClip;
    private bool _grounded;
    private Rigidbody _rb;
    private AudioSource _audioSource;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        if (Physics.CheckSphere(transform.position, GroundCheckRadiusSphere, whatIsGround))
            _grounded = true;
        else
            _grounded = false;

        if (_grounded)
        {
            OnMovement();
        }
    }
    private void OnMovement()
    {
        //Get front/reverse and left/right input axis
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");

        //Apply acceleration
        currentAcceleration = Acceleration * verticalAxis;

        if (Input.GetKeyDown(KeyCode.Space))
            currentBreakForce = BreakForce;
        else
            currentBreakForce = 0f;

        //Apply acceleration to front wheels
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        //Apply break force to all wheels
        frontRight.brakeTorque = currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;

        //Car Steering
        currentTurnAngle = MaxTurnAngle * horizontalAxis;
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;

        //Update Wheel Meshes
        UpdateWheel(frontRight, frontRightTransform);
        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(backRight, backRightTransform);
        UpdateWheel(backLeft, backLeftTransform);

        //Play Sound
        PlayEngineSoundGradually();
    }

    private void UpdateWheel(WheelCollider col, Transform trans)
    {
        //Get Wheel Colider State
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        //Set Wheel Transform State
        trans.position = Vector3.Lerp(trans.position, position, 2f);
        trans.rotation = Quaternion.Lerp(trans.rotation, rotation, 2f);
    }
    private void PlayEngineSoundGradually()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        _audioSource.clip = _engineAudioClip;
        _audioSource.pitch = verticalAxis;
        _audioSource.pitch = Mathf.Clamp(_audioSource.pitch, 0.9f, 1.1f);
        _audioSource.volume = _rb.velocity.magnitude;
        _audioSource.volume = Mathf.Clamp(_audioSource.volume, 0.1f, 0.5f);
    }
}
