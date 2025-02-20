using Unity.Cinemachine;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private HazardAudio _hazardAudio;
    private CinemachineImpulseSource _hazardShake;

    [SerializeField] private GameObject _hazardMesh;

    [Header("Properties")]
    [SerializeField] private float _hazardSpeed = 10f; // in metres per second
    [SerializeField] private GameObject _hazardFractured;

    private bool _isCollided = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _hazardAudio= GetComponentInChildren<HazardAudio>();
        _hazardShake = GetComponent<CinemachineImpulseSource>();
    }

    private void FixedUpdate()
    {
        if (!_isCollided)
            _rigidbody.linearVelocity = Vector3.down * _hazardSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _isCollided = true;
        GameObject fractured = Instantiate(_hazardFractured, transform.position, transform.rotation);

        Destroy(fractured, 3f);
        Destroy(_hazardMesh);

        _hazardAudio.PlayImpactAudio();
        _hazardAudio.ReduceFallPitch(0.4f);

        _hazardShake.GenerateImpulse(0.2f);

        Destroy(this.gameObject, 4f);
    }
}
