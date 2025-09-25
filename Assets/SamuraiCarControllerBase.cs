using UnityEngine;

public class SamuraiCarControllerBase : MonoBehaviour
{
    public float motorForce = 1500f;
    public float steeringAngle = 30f;
    public float reverseMultiplier = 0.5f;
    public float antiRollForce = 5000f;
    public float accelerationRate = 10f;

    // Wheel Colliders
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    // Wheel Transforms (for visuals)
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    protected Rigidbody rb;
    protected float horizontalInput;
    protected float verticalInput;
    protected float currentSteerAngle;
    protected float currentMotorForce = 0f;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        rb.angularDrag = 2f; // Helps resist rolling
    }

    protected void Steer()
    {
        currentSteerAngle = steeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    protected void Accelerate()
    {
        float targetMotorForce = verticalInput * motorForce;

        if (Mathf.Abs(targetMotorForce) > Mathf.Abs(currentMotorForce))
        {
            currentMotorForce = Mathf.Lerp(currentMotorForce, targetMotorForce, Time.deltaTime * accelerationRate);
        }
        else
        {
            currentMotorForce = targetMotorForce;
        }

        float appliedMotorForce = currentMotorForce;
        if (verticalInput < 0)
        {
            appliedMotorForce *= reverseMultiplier;
        }

        frontLeftWheelCollider.motorTorque = appliedMotorForce;
        frontRightWheelCollider.motorTorque = appliedMotorForce;
    }

    protected void AntiRollBar(WheelCollider leftWheel, WheelCollider rightWheel)
    {
        WheelHit hit;
        float travelLeft = 1.0f;
        float travelRight = 1.0f;

        bool groundedLeft = leftWheel.GetGroundHit(out hit);
        if (groundedLeft)
            travelLeft = (-leftWheel.transform.InverseTransformPoint(hit.point).y - leftWheel.radius) / leftWheel.suspensionDistance;

        bool groundedRight = rightWheel.GetGroundHit(out hit);
        if (groundedRight)
            travelRight = (-rightWheel.transform.InverseTransformPoint(hit.point).y - rightWheel.radius) / rightWheel.suspensionDistance;

        float antiRollForceApplied = (travelLeft - travelRight) * antiRollForce;

        if (groundedLeft)
            rb.AddForceAtPosition(leftWheel.transform.up * -antiRollForceApplied, leftWheel.transform.position);
        if (groundedRight)
            rb.AddForceAtPosition(rightWheel.transform.up * antiRollForceApplied, rightWheel.transform.position);
    }

    protected void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    protected void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        if (wheelCollider == null || wheelTransform == null)
        {
            Debug.LogError("Wheel Collider or Transform is missing!");
            return;
        }

        Vector3 pos;
        Quaternion rot;

        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
}
