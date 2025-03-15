using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform rider; // Assign in Inspector
    public float followSpeed = 0.2f; // Adjust for smoothness
    public Vector3 offset = new Vector3(5, 2, -10); // Keep -10 Z for proper depth

    private Vector3 velocity = Vector3.zero; // Used for smoothing

    void LateUpdate()
    {
        if (rider != null)
        {
            // Target position but maintain fixed Z position
            Vector3 targetPosition = new Vector3(rider.position.x + offset.x, rider.position.y + offset.y, transform.position.z);

            // Smoothly move camera toward target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followSpeed);

            // ✅ Prevent rotation from changing
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
