using UnityEngine;
using UnityEngine.Audio;

public class BallAudio : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private float _speedAudioThreshold = 1.0f;

    [Header("Roll Audio")]
    [SerializeField] private AudioSource _rollAudio;
    [SerializeField] private AnimationCurve _volumeCurve;
    [SerializeField] private AnimationCurve _pitchCurve;

    private void Awake()
    {
        _rigidbody = GetComponentInParent<Rigidbody>();
    }

    private void Update()
    {
        float speed = _rigidbody.linearVelocity.magnitude;
        float normalizedSpeed = Mathf.Clamp(speed / _speedAudioThreshold, 0f, 1f);

        _rollAudio.pitch = _pitchCurve.Evaluate(normalizedSpeed);
    }
}
