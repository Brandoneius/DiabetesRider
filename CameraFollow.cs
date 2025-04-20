using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform rider;
    public float followSpeed = 0.2f;
    public Vector3 offset = new Vector3(5, 2, -10);
    public float zoomSpeed = 10f; // Increased for snappier zoom

    private Vector3 velocity = Vector3.zero;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (rider != null)
        {
            Vector3 desiredPosition = rider.position + offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, new Vector3(desiredPosition.x, desiredPosition.y, transform.position.z), ref velocity, followSpeed);
            transform.position = smoothedPosition;

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
