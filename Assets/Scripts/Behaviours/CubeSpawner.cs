using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private CubesCounter _counter;
    [SerializeField] private CubeMovement _cubePrefab;

    [SerializeField] private int _maxCubesOnScene = 3;
    [SerializeField] private int _startCubesCount = 3;

    [SerializeField] private float _minCubeRespawnTime;
    [SerializeField] private float _maxCubeRespawnTime;

    [SerializeField] private float _cubesSpawnHeight = 0.5f; // spawn height
    [SerializeField] private float _cubesSpawnDeltaPos = 0.5f; // edges position correction

    [SerializeField] private Vector3 _upperLeftCorner; // game field
    [SerializeField] private Vector3 _bottomRightCorner; // edges

    private List<CubeMovement> _cubesPool;
    private List<CubeMovement> _cubes;

    private float _spawnTimer;
    public int CurrentSpawned => _cubes.Count;

    private void Start()
    {
        _cubesPool = new List<CubeMovement>();
        _cubes = new List<CubeMovement>();

        for (int i = 0; i < _maxCubesOnScene; i++) 
        {
            CubeMovement cube = Instantiate(_cubePrefab);
            cube.gameObject.SetActive(false);

            _cubesPool.Add(cube);
        }

        for (int i = 0; i < _startCubesCount; i++)
        {
            SpawnCube();
        }

        if (CurrentSpawned < _maxCubesOnScene)
        {
            _spawnTimer = Random.Range(_minCubeRespawnTime, _maxCubeRespawnTime);
        }
    }

    private void FixedUpdate()
    {
        if (CurrentSpawned < _maxCubesOnScene)
        {
            _spawnTimer -= Time.fixedDeltaTime;

            if (_spawnTimer <= 0)
            {
                SpawnCube();
            }
        }

        foreach(CubeMovement cube in _cubes)
        {
            cube.OnFixedUpdate();
        }
    }

    private void SpawnCube()
    {
        Vector3 spawnPos = new Vector3
            (
                Random.Range(_upperLeftCorner.x + _cubesSpawnDeltaPos, _bottomRightCorner.x - _cubesSpawnDeltaPos),
                _cubesSpawnHeight,
                Random.Range(_bottomRightCorner.z + _cubesSpawnDeltaPos, _upperLeftCorner.z - _cubesSpawnDeltaPos)
            );

        CubeMovement cube = _cubesPool[_cubesPool.Count - 1];

        cube.transform.position = spawnPos;
        cube.gameObject.SetActive(true);

        _cubes.Add(cube);
        _cubesPool.Remove(cube);
    }

    public void OnCubeDies(CubeMovement cube)
    {
        cube.gameObject.SetActive(false);

        _cubesPool.Add(cube);
        _cubes.Remove(cube);

        _counter.OnCubeDies();
    }
}
