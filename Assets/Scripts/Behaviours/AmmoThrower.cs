using System.Threading;
using UnityEngine;

public class AmmoThrower : MonoBehaviour
{
    [SerializeField] private AmmoTrajectory _ammoPrefab;
    [SerializeField] private AmmoCounter _counter;
    [SerializeField] private float _reloadTime;

    private bool _isMobile;
    private float _timer;

    private void OnEnable()
    {
        _isMobile = Application.isMobilePlatform;
        _timer = 0;
    }

    public void Update()
    {
        if (_timer <= 0)
        {
            Vector2 touchPosition = Vector2.zero;
            bool onInput = false;

            if (_isMobile) // mobile control
            {
                if (Input.touchCount > 0)
                {
                    touchPosition = Input.GetTouch(0).position;

                    onInput = true;
                }
            }
            else // mouse control
            {
                if (Input.GetMouseButton(0))
                {
                    touchPosition = Input.GetTouch(0).position;

                    onInput = true;
                }
            }

            if (!onInput || !_counter.HaveAmmo) return;

            _timer = _reloadTime;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(touchPosition), out RaycastHit hit))
            {
                // TODO throw ammo
            }
        }
        else
        {
            _timer -= Time.deltaTime;
        }
    }
}
