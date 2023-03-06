using System.Collections.Generic;
using UnityEngine;

public class AmmoCounter : MonoBehaviour
{
    [SerializeField] private Transform _cooldownsParent;
    [SerializeField] private AmmoCooldown _ammoCooldownPrefab;
    [SerializeField][Range(1,5)] private int _maxAmmo;

    [SerializeField] private float _restoreAmmoTime;

    private float _timer;
    private List<AmmoCooldown> _cooldowns;

    public bool HaveAmmo => true; // TODO ammo cooldown

    private void Start()
    {
        _timer = 0;

        _cooldowns = new List<AmmoCooldown>();

        for (int i = 0; i < _maxAmmo; i++)
        {
            AmmoCooldown ammoCooldown = Instantiate(_ammoCooldownPrefab, _cooldownsParent);

            _cooldowns.Add(ammoCooldown);
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _restoreAmmoTime)
        {
            // TODO restore ammo

            _timer -= _restoreAmmoTime;
        }
    }
}
