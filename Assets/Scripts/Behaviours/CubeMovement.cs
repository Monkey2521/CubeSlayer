using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private float _leapSpeed;
    [SerializeField] private float _minLeapCooldown;
    [SerializeField] private float _maxLeapCooldown;

    private CubeSpawner _spawner;

    public void Initialize(CubeSpawner spawner)
    {
        _spawner = spawner;
    }

    public void OnFixedUpdate()
    {

    }

    public void Die()
    {
        _spawner.OnCubeDies(this);
    }
}
