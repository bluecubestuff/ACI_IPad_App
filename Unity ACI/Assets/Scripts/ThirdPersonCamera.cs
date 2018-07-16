using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {

    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    public Transform target;
    public Transform camTransform;

    private Camera cam;

    private float distance = 10.0f;
    private float currX = 0.0f;
    private float currY = 0.0f;
    private float sensivityX = 4.0f;
    private float sensivityY = 1.0f;

    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;

    }

    private void Update()
    {
        currX += Input.GetAxis("Mouse X") * sensivityX;
        currY -= Input.GetAxis("Mouse Y") * sensivityY;

        currY = Mathf.Clamp(currY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);

        Quaternion rotation = Quaternion.Euler(currY, currX, 0);
        camTransform.position = target.position + rotation * dir;
        camTransform.LookAt(target.position);
    }
}
