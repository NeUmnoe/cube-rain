using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private int _poolMaxSize = 50;
    [SerializeField] private float _spawnAreaPadding = 2f;
    [SerializeField] private Vector3 _spawnAreaSize;

    private ObjectPool<Cube> _cubePool;

    private void Start()
    {
        _cubePool = new ObjectPool<Cube>(CreateCube, OnGetCube, OnReleaseCube, OnDestroyCube, maxSize: _poolMaxSize);
        InvokeRepeating(nameof(SpawnCube), _spawnInterval, _spawnInterval);
    }

    private Cube CreateCube()
    {
        Cube cube = Instantiate(_cubePrefab);
        cube.SetPool(_cubePool);
        return cube;
    }

    private void OnGetCube(Cube cube)
    {
        cube.gameObject.SetActive(true);
    }

    private void OnReleaseCube(Cube cube)
    {
        cube.gameObject.SetActive(false);
    }

    private void OnDestroyCube(Cube cube)
    {
        Destroy(cube.gameObject);
    }

    private void SpawnCube()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(transform.position.x - _spawnAreaSize.x / _spawnAreaPadding, transform.position.x + _spawnAreaSize.x / _spawnAreaPadding),
            transform.position.y + _spawnAreaSize.y / _spawnAreaPadding,
            Random.Range(transform.position.z - _spawnAreaSize.z / _spawnAreaPadding, transform.position.z + _spawnAreaSize.z / _spawnAreaPadding)
        );

        Cube cube = _cubePool.Get();
        cube.transform.position = spawnPosition;
    }
}