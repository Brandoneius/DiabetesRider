using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform rider;
    public float followSpeed = 0.2f;
    public Vector3 offset = new Vector3(5, 2, -10);
    public float zoomSpeed = 10f; // Increased for snappier zoom

    private Vector3 velocity = Vector3.zero;
    private Camera cam;
    private float smoothedY;

    void Start()
    {
        cam = GetComponent<Camera>();
        smoothedY = rider.position.y + offset.y;
    }

    void LateUpdate()
    {
    if (rider != null)
    {
        // Initialize smoothedY once, in case it hasn't been set yet
        if (smoothedY == 0f)
            smoothedY = rider.position.y + offset.y;

        // Smooth only the Y movement (dampen up/down bounce)
        smoothedY = Mathf.Lerp(smoothedY, rider.position.y + offset.y, Time.deltaTime * 200f); // Feel free to tweak 1.5f

        // Desired camera position with softened Y
        Vector3 desiredPosition = new Vector3(rider.position.x + offset.x, smoothedY, transform.position.z);

        // Smooth move camera
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, followSpeed);

        // Lock rotation
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

        if (cam != null && cam.orthographic)
        {
            float scroll = 0f;
            if (Input.GetKey(KeyCode.Q)) scroll = -20f;
            else if (Input.GetKey(KeyCode.E)) scroll = 20f;

            if (scroll != 0f)
            {
                cam.orthographicSize += scroll * zoomSpeed * Time.deltaTime;
            }
        }
    }
}
