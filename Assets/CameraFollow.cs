/*using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player the camera will follow
    public Vector3 offset = new Vector3(0, 5, -5); // Position of the camera relative to the player
    public float smoothSpeed = 0.125f; // Smoothness factor for the camera movement

    void LateUpdate()
    {
        // Desired position is target's position plus the offset, relative to the target's rotation
        Vector3 desiredPosition = target.position + target.rotation * offset;

        // Smoothly move the camera to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Make the camera look in the same direction as the target
        transform.rotation = target.rotation;
    }
}
*/




/*using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player the camera will follow
    public Vector3 offset = new Vector3(0, 5, -10); // Offset from the player's position

    void LateUpdate()
    {
        // Set the camera's position directly behind the player with the offset
        transform.position = target.position + target.rotation * offset;

        // Make the camera look forward in the direction the player is facing
        transform.rotation = target.rotation;
    }
}
*/

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player the camera will follow
    public Vector3 offset = new Vector3(0, 5, -10); // Position of the camera relative to the player

    void LateUpdate()
    {
        // Set the camera's position directly behind the player with the offset
        transform.position = target.position + target.rotation * offset;

        // Make the camera look in the direction the player is facing
        transform.LookAt(target.position + target.forward * 2); // Focus a bit ahead of the player
    }
}
