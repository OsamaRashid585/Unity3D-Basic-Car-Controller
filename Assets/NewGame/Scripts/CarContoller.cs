using System.Collections;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public VehicleSound _vehicleSound;
    public VehicleInputs _vehicleInputs;
    public GameObject BrakeLight;
    public Transform CernterofMass;
    public Transform FrontLeftWheelMesh;
    public Transform FrontRightWheelMesh;
    public Transform RearLeftWheelMesh;
    public Transform RearRightWheelMesh;

    public WheelCollider FrontLeftWheelCollider;
    public WheelCollider FrontRightWheelCollider;
    public WheelCollider RearLeftWheelCollider;
    public WheelCollider RearRightWheelCollider;

    public float maxSpeed = 50000f;
    public float brakeForce = 8000f;
    public float maxSteerAngle = 30f;

    private Rigidbody rb;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _vehicleSound = GetComponent<VehicleSound>();
        _vehicleInputs = GetComponent<VehicleInputs>();
    }
    private void Start() {
       rb.centerOfMass = CernterofMass.localPosition;
    }

    private void Update()
    {
        HandleEngineSound();
        HandleBreakLight(_vehicleInputs.BrakeValue);
        HandleBreakSound(_vehicleInputs.BrakeValue);
    }

    private void FixedUpdate() {
        ApplyMotorTorque(_vehicleInputs.moveValue);
        ApplyBrake(_vehicleInputs.BrakeValue);
        UpdateAllWheelPositions();
    }

    public void ApplyMotorTorque(float input)
    {
        RearLeftWheelCollider.motorTorque = input * maxSpeed * Time.fixedDeltaTime;
        RearRightWheelCollider.motorTorque = input * maxSpeed * Time.fixedDeltaTime;
        FrontLeftWheelCollider.motorTorque = input * maxSpeed * Time.fixedDeltaTime;
        FrontRightWheelCollider.motorTorque = input * maxSpeed * Time.fixedDeltaTime;
    }


    public void ApplyBrake(float input)
    {        
        RearLeftWheelCollider.brakeTorque = input * brakeForce;
        RearRightWheelCollider.brakeTorque = input * brakeForce;
        FrontLeftWheelCollider.brakeTorque = input * brakeForce;
        FrontRightWheelCollider.brakeTorque = input * brakeForce;
    }

    public void ApplySteering(float input)
    {
        FrontLeftWheelCollider.steerAngle = input * maxSteerAngle;
        FrontRightWheelCollider.steerAngle = input * maxSteerAngle;
    }

    private void UpdateAllWheelPositions()
    {
        UpdateWheelPosition(FrontLeftWheelCollider, FrontLeftWheelMesh);
        UpdateWheelPosition(FrontRightWheelCollider, FrontRightWheelMesh);
        UpdateWheelPosition(RearLeftWheelCollider, RearLeftWheelMesh);
        UpdateWheelPosition(RearRightWheelCollider, RearRightWheelMesh);
        
    }

    private void UpdateWheelPosition(WheelCollider collider, Transform mesh)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        mesh.position = position;
        mesh.rotation = rotation;
    }

    private void HandleBreakLight(int input){
        BrakeLight.SetActive(input == 1);
    }
    private void HandleBreakSound(int input){
        if (input == 1)
        {
            _vehicleSound.PlayBrakeSound();
        }
    }
    private void HandleEngineSound()
    {
        float desiredPitch = rb.velocity.magnitude / 2;

        // Apply constraints
        if (desiredPitch < 1f)
        {
            desiredPitch = 1f;
        }
        else if (desiredPitch > 3f)
        {
            desiredPitch = 3f;
        }

        // Set the pitch
        _vehicleSound.EngineSound.pitch = desiredPitch;
    }

    public void ResetVehicle()
    {
        // Reset the position to a new Vector3 with the same x and z, and a higher y value.
        transform.position = new Vector3(-6.6f, 0.71f, 47.5f);
        
        // Reset the rotation to identity (no rotation).
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

}
