using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform rider; // Drag the Rider here in Inspector
    public float followSpeed = 5f; // Adjust speed for smooth movement
    public Vector3 offset = new Vector3(5, 2, -10); // Camera position relative to Rider

    void LateUpdate()
    {
        if (rider != null)
        {
            // Smoothly follow the rider without rotating
            transform.rotation = Quaternion.Euler(0, 0, 0);

            Vector3 targetPosition = new Vector3(rider.position.x + offset.x, rider.position.y + offset.y, offset.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}
