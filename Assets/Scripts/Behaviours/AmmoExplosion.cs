using UnityEngine;

public class AmmoExplosion : MonoBehaviour
{
    [SerializeField] private float _duration;

    private float _timer;

    private void OnEnable()
    {
        _timer = 0;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _duration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CubeMovement cube))
        {
            cube.Die();
        }
    }
}
