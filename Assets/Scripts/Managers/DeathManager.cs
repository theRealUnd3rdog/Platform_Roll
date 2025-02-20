using UnityEngine;

public class DeathManager : MonoBehaviour
{
    [SerializeField] private GameObject _deathParticles;
    [SerializeField] private AudioSource _deathAudio;

    private void Awake()
    {
        BallController.OnDeathBegan += OnDeath;
    }

    private void OnDestroy()
    {
        BallController.OnDeathBegan -= OnDeath;
    }

    private void OnDeath(Transform transform) 
    {
        Instantiate(_deathParticles, transform.position, Quaternion.identity);

        _deathAudio.transform.position = transform.position;
        _deathAudio.Play();

        RestartScreen.Instance.ShowRestart();
    }
}
