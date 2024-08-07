using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [Header("Vehicle Movement")]
    public float Acceleration = 500f;
    public float BreakForce = 300f;
    public float MaxTurnAngle = 15f;
    public float ReverseSpeedTreshold = 0.2f;

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
    private bool once = false;

    //Is reversing property
    public bool isReversing;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        //Ground Check
        if (Physics.CheckSphere(transform.position, GroundCheckRadiusSphere, whatIsGround))
            _grounded = true;
        else
            _grounded = false;

        //Reversing
        if (Input.GetKeyDown(KeyCode.R))
            isReversing = !isReversing;

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

        //Check if reversing and reverse acceleration
        if (isReversing && once)
        {
            currentAcceleration = Acceleration * verticalAxis * -1f;
            once = false;
        }
        else
            once = true;

        if (Input.GetKey(KeyCode.Space) || verticalAxis < 0)
            currentBreakForce = BreakForce;
        else
            currentBreakForce = 0f;

        //Apply acceleration to front wheels
        ApplyAcceleration();

        //Apply break force to all wheels
        ApplyBreak();

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
    private void ApplyAcceleration()
    {
        //Apply acceleration to front wheels
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;
    }
    private void ApplyBreak()
    {
        //Apply break force to all wheels
        frontRight.brakeTorque = currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;
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
        _audioSource.pitch = Mathf.Clamp(_audioSource.pitch, 0.6f, 0.9f);

        //get local forward velocity
        var locVel = transform.InverseTransformDirection(_rb.velocity);
        _audioSource.volume = locVel.z * (2 * Time.deltaTime);
        _audioSource.volume = Mathf.Clamp(_audioSource.volume, 0.2f, 0.3f);
    }
}
