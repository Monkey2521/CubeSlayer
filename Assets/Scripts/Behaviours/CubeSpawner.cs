using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private TMP_Text _diedCountText;
    [SerializeField] private CubeMovement _cubePrefab;

    [SerializeField] private int _maxCubesOnScene = 3;
    [SerializeField] private int _startCubesCount = 3;

    [Header("Spawn")]
    [SerializeField] private float _minCubeRespawnTime;
    [SerializeField] private float _maxCubeRespawnTime;

    [SerializeField] private float _cubesSpawnHeight = 0.5f; // spawn height
    [SerializeField] private float _edgeDeltaPos = 0.5f; // edges position correction

    [Header("Field edges")]
    [SerializeField] private Vector3 _upperLeftCorner; // game field
    [SerializeField] private Vector3 _bottomRightCorner; // edges

    private List<CubeMovement> _cubesPool;
    private List<CubeMovement> _cubes;

    private float _spawnTimer;
    private int _count;

    public int CurrentSpawned => _cubes.Count;

    private void Start()
    {
        _count = 0;

        UpdateCounter();

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

                _spawnTimer = Random.Range(_minCubeRespawnTime, _maxCubeRespawnTime);
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
                Random.Range(_upperLeftCorner.x + _edgeDeltaPos, _bottomRightCorner.x - _edgeDeltaPos),
                _cubesSpawnHeight,
                Random.Range(_bottomRightCorner.z + _edgeDeltaPos, _upperLeftCorner.z - _edgeDeltaPos)
            );

        CubeMovement cube = _cubesPool[_cubesPool.Count - 1];

        cube.transform.position = spawnPos;
        cube.gameObject.SetActive(true);

        cube.Initialize(this);

        _cubes.Add(cube);
        _cubesPool.Remove(cube);
    }

    public bool CheckEdgeReaching(Vector3 position, Vector3 viewSide)
    {
        Vector3 viewPos = position + viewSide;

        return viewPos.x <= _upperLeftCorner.x + _edgeDeltaPos ||
               viewPos.z >= _upperLeftCorner.z - _edgeDeltaPos ||
               viewPos.x >= _bottomRightCorner.x - _edgeDeltaPos ||
               viewPos.z <= _bottomRightCorner.z + _edgeDeltaPos;
    }

    public void OnCubeDies(CubeMovement cube)
    {
        cube.gameObject.SetActive(false);
        cube.ResetCube();

        _cubesPool.Add(cube);
        _cubes.Remove(cube);

        _count++;

        UpdateCounter();
    }

    private void UpdateCounter()
    {
        _diedCountText.text = _count.ToString();
    }
}
