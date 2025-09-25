using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 100f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal2");
        float vertical = Input.GetAxis("Vertical2");

        Vector3 movement = transform.forward * vertical * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        // Rotate left and right
        float rotation = horizontal * rotationSpeed * Time.deltaTime;
        Quaternion turn = Quaternion.Euler(0f, rotation, 0f);
        rb.MoveRotation(rb.rotation * turn);
    }
}
