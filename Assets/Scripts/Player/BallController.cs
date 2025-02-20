using System;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public static Action<Transform> OnDeathBegan;

    private Rigidbody _rb;
    [SerializeField] private Transform _platform;

    [Header("Properties")]
    [SerializeField] private float _gravityForce = 20f; // Strength of the gravity effect

    private void Start()
    {
        if (_rb == null) 
            _rb = GetComponent<Rigidbody>();

        _platform = transform.parent; // Assuming the ball is a child of the platform
        HazardManager.NotifyPlayerSpawned(this.transform);
    }

    private void FixedUpdate()
    {
        if (_platform == null) return;

        // Get platform's tilt direction (local up vector)
        Vector3 customGravity = -_platform.up * _gravityForce;

        // Apply gravity manually
        _rb.AddForce(customGravity, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DeathZone") || collision.gameObject.CompareTag("Hazard")) 
        {
            Destroy(gameObject);

            OnDeathBegan?.Invoke(this.transform);
        }
    }
}
