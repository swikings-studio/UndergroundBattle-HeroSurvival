using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Linq;
using System;
using Random = UnityEngine.Random;
public class ObjectsPool : MonoBehaviour
{
    [SerializeField] private int _capacity, _offset;
    [SerializeField, Range(0, 20)] private float _minSpeedSpawn;
    [SerializeField, Range(0, 20)] private float _maxSpeedSpawn;
    [SerializeField] private Transform player, upperWall, bottomWall, leftWall, rightWall;
    private float _spawnCooldown;
    private List<GameObject> _pool = new();
    private const string gameObjectKey = "Orc";
    private readonly Vector3[] directions = new[]
    {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right
    };
    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        for (int i = 0; i < _capacity; i++)
        {
            Addressables.InstantiateAsync(gameObjectKey, transform).Completed += handle =>
            {
                GameObject spawned = handle.Result;
                spawned.SetActive(false);
                if (spawned.TryGetComponent(out FollowTarget followTarget)) followTarget.Initialize(player);
                _pool.Add(spawned);
            };
        }
    }
    private void Update()
    {
        _spawnCooldown -= Time.deltaTime;

        if (_spawnCooldown <= 0)
        {
            _spawnCooldown = Random.Range(_minSpeedSpawn, _maxSpeedSpawn);
            if (TryGetObject(out GameObject _object))
            {
                if (_maxSpeedSpawn > 1f) _maxSpeedSpawn -= 0.01f;
                if (_minSpeedSpawn > 0.5f) _minSpeedSpawn -= 0.01f;
                Vector3 spawnObjectPosition = GetSpawnPosition();
                SetGameObject(_object, spawnObjectPosition);
            }
        }
    }
    private Vector3 GetSpawnPosition()
    {
        Vector3 randomDirection = directions[Random.Range(0, directions.Length)];
        Vector3 position = player.position + randomDirection * _offset;

        return position;
    }
    private void SetGameObject(GameObject _object, Vector3 spawnPoint)
    {
        _object.SetActive(true);
        _object.transform.SetPositionAndRotation(spawnPoint, Quaternion.identity);
    }
    private bool TryGetObject(out GameObject result)
    {
        result = _pool.FirstOrDefault(p => !p.activeSelf);

        return result != null;
    }

    private void OnDestroy()
    {
        foreach (GameObject gameObject in _pool)
        {
            Addressables.ReleaseInstance(gameObject);
        }

        _pool.Clear();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(player.position, _offset);
    }
}