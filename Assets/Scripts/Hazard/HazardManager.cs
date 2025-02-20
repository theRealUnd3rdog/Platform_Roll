using UnityEngine;
using System;

public class HazardManager : MonoBehaviour
{
    [SerializeField] private GameObject _hazardPrefab;
    [SerializeField] private GameObject _hazardIndicator;
    [SerializeField] private LayerMask _layerMask;

    [Header("Platform")]
    [SerializeField] private Transform _platformObj;
    [SerializeField] private Transform _platform;

    [Header("Spawning")]
    [SerializeField] private float _spawnRate = 2f;
    [SerializeField] private float _spawnHeight = 10f;
    [SerializeField] private float _targetPlayerChance = 0.5f;

    private Transform _player;
    private Vector2 _spawnArea;

    // Event to update the player reference
    public static event Action<Transform> OnPlayerSpawned;

    private void Awake()
    {
        OnPlayerSpawned += SetPlayer; // Subscribe to event
    }

    private void OnDestroy()
    {
        OnPlayerSpawned -= SetPlayer; // Unsubscribe to avoid memory leaks
    }

    private void Start()
    {
        if (_platformObj != null)
        {
            Collider platformCollider = _platformObj.GetComponent<Collider>();
            if (platformCollider != null)
            {
                _spawnArea = new Vector2(platformCollider.bounds.size.x / 2, platformCollider.bounds.size.z / 2);
            }
        }

        InvokeRepeating(nameof(SpawnHazard), 1f, _spawnRate);
    }

    private void SpawnHazard()
    {
        if (_hazardPrefab == null || _platformObj == null || _player == null) return;

        Vector3 spawnPos;

        if (UnityEngine.Random.value < _targetPlayerChance)
        {
            spawnPos = new Vector3(_player.position.x, _spawnHeight, _player.position.z);
        }
        else
        {
            spawnPos = new Vector3(
                UnityEngine.Random.Range(-_spawnArea.x, _spawnArea.x) + _platformObj.position.x,
                _spawnHeight,
                UnityEngine.Random.Range(-_spawnArea.y, _spawnArea.y) + _platformObj.position.z
            );
        }

        Instantiate(_hazardPrefab, spawnPos, Quaternion.identity);

        if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, _layerMask)) 
        {
            Vector3 hazardPosition = hitInfo.point;
            Vector3 newPosition = new Vector3(spawnPos.x, hazardPosition.y + (hitInfo.normal.y * 0.05f), spawnPos.z);

            GameObject hazardIndicator = Instantiate(_hazardIndicator, newPosition, Quaternion.LookRotation(hitInfo.normal));
            hazardIndicator.transform.SetParent(_platform);

            Destroy(hazardIndicator, 2f);
        }
    }

    private void SetPlayer(Transform newPlayer)
    {
        _player = newPlayer;
    }

    // Call this method from player spawning script when a new player spawns
    public static void NotifyPlayerSpawned(Transform playerTransform)
    {
        OnPlayerSpawned?.Invoke(playerTransform);
    }
}
