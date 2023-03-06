using UnityEngine;

public class AmmoTrajectory : MonoBehaviour
{
    [SerializeField] private float _throwTime = 2f;

    private const float g = 9.81f;

    private float _timer;
    private float _speed;
    private float _startPosY;

    public void Initialize(Vector3 point)
    {
        _timer = 0;
        _startPosY = transform.position.y; // throw height

        Vector3 pos = transform.position;
        Vector3 zeroPos = new Vector3(pos.x, 0f, pos.z);

        float distance = (point - zeroPos).magnitude; // calculate throw distance

        _speed = distance / _throwTime; // calculate speed for parabolic trajectory

        transform.LookAt(new Vector3(point.x, pos.y, point.z)); // rotate 
    }

    private void FixedUpdate()
    {
        Vector3 pos = transform.position;
        Vector3 newPos = Vector3.MoveTowards // moving in x & z axis
            (
                pos, 
                pos + transform.TransformDirection(Vector3.forward) * _speed, 
                _speed * Time.fixedDeltaTime
            );

        newPos.y = _startPosY - g * _timer * _timer * 0.5f; // parabolic trajectory correction

        transform.position = newPos;

        _timer += Time.fixedDeltaTime;
       
        if (_timer >= _throwTime)
        {
            Destroy(gameObject);
        }
    }
}
