using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float _maxTiltAngle = 30f; // Maximum tilt angle in degrees
    [SerializeField] private float _smoothSpeed = 5f;   // How fast the rotation interpolates

    private Vector2 _screenCenter;
    private Vector3 _targetTilt;

    private void Start()
    {
        _screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
    }

    private void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            HandleAccelerometerInput(); // Use accelerometer on mobile
        }
        else
        {
            HandleMouseInput(); // Use mouse on PC
        }

        // Smoothly interpolate rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_targetTilt), Time.deltaTime * _smoothSpeed);
    }

    private void HandleMouseInput()
    {
        Vector2 mousePos = Input.mousePosition;

        // Normalize position to range [-1, 1] relative to screen center
        float tiltX = Mathf.Clamp((mousePos.y - _screenCenter.y) / _screenCenter.y, -1f, 1f);
        float tiltZ = Mathf.Clamp((_screenCenter.x - mousePos.x) / _screenCenter.x, -1f, 1f);

        // Convert to tilt angles
        _targetTilt.x = tiltX * _maxTiltAngle;
        _targetTilt.z = tiltZ * _maxTiltAngle;
    }

    private void HandleAccelerometerInput()
    {
        Vector3 accel = Input.acceleration;

        // Map acceleration to tilt angles
        float tiltX = Mathf.Clamp(accel.y, -1f, 1f) * _maxTiltAngle;  // Front-back tilt
        float tiltZ = Mathf.Clamp(-accel.x, -1f, 1f) * _maxTiltAngle; // Left-right tilt

        _targetTilt.x = tiltX;
        _targetTilt.z = tiltZ;
    }
}
