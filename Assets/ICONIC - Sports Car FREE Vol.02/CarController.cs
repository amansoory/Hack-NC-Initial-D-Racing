using UnityEngine;

public class SamuraiCarControllerV2 : MonoBehaviour
{
    public float motorForce = 1500f;
    public float steeringAngle = 30f;
    public float reverseMultiplier = 0.5f;  // Multiplier to control reverse speed
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

    // Cameras
    public Camera player1Camera;
    public Camera player2Camera;

    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentMotorForce = 0f;

    // Player identifiers
    public bool isPlayer1 = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Lowering the center of mass to improve stability
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        rb.angularDrag = 2f; // Helps resist rolling

        // Set up cameras for split screen
        SetupSplitScreen();
    }

    void Update()
    {
        // Get user input based on player
        if (isPlayer1)
        {
            horizontalInput = UnityEngine.Input.GetAxis("Horizontal");
            verticalInput = UnityEngine.Input.GetAxis("Vertical");
        }
        else
        {
            horizontalInput = UnityEngine.Input.GetAxis("Horizontal_P2");
            verticalInput = UnityEngine.Input.GetAxis("Vertical_P2");
        }
    }

    void FixedUpdate()
    {
        // Apply anti-roll bar force
        AntiRollBar(frontLeftWheelCollider, frontRightWheelCollider);
        AntiRollBar(rearLeftWheelCollider, rearRightWheelCollider);

        // Handle movement and steering
        Steer();
        Accelerate();
        UpdateWheels();
    }

    private void Steer()
    {
        // Calculate steering angle based on input
        currentSteerAngle = steeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void Accelerate()
    {
        float targetMotorForce = verticalInput * motorForce;

        // Smoothly adjust motor force for better control
        if (Mathf.Abs(targetMotorForce) > Mathf.Abs(currentMotorForce))
        {
            currentMotorForce = Mathf.Lerp(currentMotorForce, targetMotorForce, Time.deltaTime * accelerationRate);
        }
        else
        {
            currentMotorForce = targetMotorForce;
        }

        // Apply reverse multiplier if moving in reverse
        float appliedMotorForce = currentMotorForce;
        if (verticalInput < 0)
        {
            appliedMotorForce *= reverseMultiplier;
        }

        // Apply the adjusted motor force to the front wheels
        frontLeftWheelCollider.motorTorque = appliedMotorForce;
        frontRightWheelCollider.motorTorque = appliedMotorForce;
    }

    private void AntiRollBar(WheelCollider leftWheel, WheelCollider rightWheel)
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

    private void UpdateWheels()
    {
        // Update each wheel's position and rotation based on WheelCollider
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        if (wheelCollider == null || wheelTransform == null)
        {
            Debug.LogError("Wheel Collider or Transform is missing!");
            return;
        }

        Vector3 pos;
        Quaternion rot;

        // Get the world position and rotation from the WheelCollider
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    private void SetupSplitScreen()
    {
        if (player1Camera != null && player2Camera != null)
        {
            // Set player 1 camera to the top half of the screen
            player1Camera.rect = new Rect(0, 0.5f, 1, 0.5f);

            // Set player 2 camera to the bottom half of the screen
            player2Camera.rect = new Rect(0, 0, 1, 0.5f);
        }
        else
        {
            Debug.LogError("Both Player 1 and Player 2 cameras need to be assigned.");
        }
    }
}
