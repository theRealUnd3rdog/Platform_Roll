using System.Collections;
using UnityEngine;

public class HazardAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _fallAudio;

    [SerializeField] private AudioSource _impactAudio;
    [SerializeField] private AudioClip[] _impactClips;

    private void Start()
    {
        _fallAudio.pitch = Random.Range(0.75f, 0.8f);
        _fallAudio.Play();
    }

    public void PlayImpactAudio() 
    {
        _impactAudio.clip = _impactClips[Random.Range(0, _impactClips.Length)];

        _impactAudio.pitch = Random.Range(0.9f, 1.1f);
        _impactAudio.Play();
    }

    private IEnumerator ReducePitchCoroutine(float duration)
    {
        float startPitch = _fallAudio.pitch;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            _fallAudio.pitch = Mathf.Lerp(startPitch, 0f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _fallAudio.pitch = 0f; // Ensure it reaches exactly 0
        _fallAudio.Stop();
    }

    public void ReduceFallPitch(float duration)
    {
        StartCoroutine(ReducePitchCoroutine(duration));
    }
}
