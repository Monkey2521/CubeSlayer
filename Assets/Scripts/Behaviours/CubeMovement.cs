using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    [Header("Moving")]
    [SerializeField] private float _minMovingTime = 1f;
    [SerializeField] private float _maxMovingTime = 3f;

    [Header("Rotation")]
    [SerializeField] private float _minRotationTime = 0.5f;
    [SerializeField] private float _maxRotationTime = 1.5f;

    [Header("Leap")]
    [SerializeField] private float _leapSpeed;
    [SerializeField] private float _minLeapCooldown = 3f;
    [SerializeField] private float _maxLeapCooldown = 6f;
    [SerializeField] private float _minLeapTime = 1f;
    [SerializeField] private float _maxLeapTime = 2f;

    private CubeSpawner _spawner;
    private MoveState _currentState;
    private RotationSide _currentRotationSide;

    private float _moveTimer;
    private float _leapTimer;

    private enum MoveState
    {
        Move,
        Rotate,
        Leap
    }

    private enum RotationSide
    {
        Right,
        Left
    }

    public void Initialize(CubeSpawner spawner)
    {
        _spawner = spawner;

        _currentState = (MoveState)Random.Range(0, 2);

        switch (_currentState)
        {
            case MoveState.Move:
                _moveTimer = Random.Range(_minMovingTime, _maxMovingTime);
                break;
            case MoveState.Rotate:
                _moveTimer = Random.Range(_minRotationTime, _maxRotationTime);
                _currentRotationSide = (RotationSide)Random.Range(0, 2);
                break;
            default:
                _currentState = MoveState.Move;
                _moveTimer = Random.Range(_minMovingTime, _maxMovingTime);
                break;
        }

        _leapTimer = Random.Range(_minLeapCooldown, _maxLeapCooldown);
    }

    public void ResetCube()
    {
        _spawner = null;
    }

    public void OnFixedUpdate()
    {
        _moveTimer -= Time.fixedDeltaTime;

        bool onEdge = _spawner.CheckEdgeReaching(transform.position, transform.TransformDirection(Vector3.forward));

        if (_leapTimer <= 0 && !onEdge)
        {
            _currentState = MoveState.Leap;
            _moveTimer = Random.Range(_minLeapTime, _maxLeapTime);
        }

        switch (_currentState)
        {
            case MoveState.Move:
                if (onEdge || _moveTimer <= 0)
                {
                    _moveTimer = Random.Range(_minRotationTime, _maxRotationTime);

                    _currentState = MoveState.Rotate;
                    _currentRotationSide = (RotationSide)Random.Range(0, 2);

                    break;
                }

                Move(_speed);

                break;

            case MoveState.Rotate:
                if (_moveTimer <= 0)
                {
                    _currentState = MoveState.Move;
                    _moveTimer = Random.Range(_minMovingTime, _maxMovingTime);

                    break;
                }

                transform.Rotate
                        (
                            _currentRotationSide == RotationSide.Left ? Vector3.up : Vector3.down,
                            _rotationSpeed * Time.fixedDeltaTime
                        );

                break;

            case MoveState.Leap:
                if (onEdge || _moveTimer <= 0)
                {
                    _moveTimer = Random.Range(_minRotationTime, _maxRotationTime);
                    _leapTimer = Random.Range(_minLeapCooldown, _maxLeapCooldown);

                    _currentState = MoveState.Rotate;
                    _currentRotationSide = (RotationSide)Random.Range(0, 2);

                    break;
                }

                Move(_leapSpeed);

                break;
            default:
                _currentState = MoveState.Move;
                break;
        }

        if (_currentState != MoveState.Leap)
        {
            _leapTimer -= Time.fixedDeltaTime;
        }
    }

    private void Move(float speed)
    {
        Vector3 pos = transform.position;

        transform.position = Vector3.MoveTowards
            (
                pos,
                pos + transform.TransformDirection(Vector3.forward) * speed,
                speed * Time.fixedDeltaTime
            );
    }

    [ContextMenu("Die")]
    public void Die()
    {
        _spawner.OnCubeDies(this);
    }
}
