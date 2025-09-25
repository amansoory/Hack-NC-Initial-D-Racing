using UnityEngine;

public class SamuraiCarController : MonoBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 100f;

    void Update()
    {
        // Get user input
        float moveInput = Input.GetAxis("Vertical");  // Forward/Backward
        float turnInput = Input.GetAxis("Horizontal"); // Left/Right

        // Move the car forward/backward
        transform.Translate(Vector3.forward * moveInput * speed * Time.deltaTime);

        // Rotate the car
        transform.Rotate(Vector3.up, turnInput * turnSpeed * Time.deltaTime);
    }
}


