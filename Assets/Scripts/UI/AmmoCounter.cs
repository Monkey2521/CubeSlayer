using System.Collections.Generic;
using UnityEngine;

public class AmmoCounter : MonoBehaviour
{
    [SerializeField] private Transform _cooldownsParent;
    [SerializeField] private AmmoCooldown _ammoCooldownPrefab;
    [SerializeField][Range(1,5)] private int _maxAmmo;

    [SerializeField] private float _restoreAmmoTime;

    private List<AmmoCooldown> _cooldowns;

    public bool HaveAmmo => _cooldowns.Find(item => item.Ready) != null;
    public int AmmoAmount => _cooldowns.FindAll(item => item.Ready).Count;

    private void Start()
    {
        _cooldowns = new List<AmmoCooldown>();

        for (int i = 0; i < _maxAmmo; i++)
        {
            AmmoCooldown ammoCooldown = Instantiate(_ammoCooldownPrefab, _cooldownsParent);

            ammoCooldown.Initialize(_restoreAmmoTime);

            _cooldowns.Add(ammoCooldown);
        }
    }

    private void Update()
    {
        if (AmmoAmount < _maxAmmo)
        {
            AmmoCooldown cooldown = _cooldowns.Find(item => !item.Ready);

            cooldown?.OnUpdate(); // restore only one ammo
        }
    }

    public void Throw()
    {
        if (!HaveAmmo) return;

        AmmoCooldown throwedAmmo = _cooldowns.FindLast(item => item.Ready);
        AmmoCooldown lastCooldownAmmo = _cooldowns.Find(item => !item.Ready);

        // replace cooldowns 
        throwedAmmo.Initialize(_restoreAmmoTime, lastCooldownAmmo != null ? lastCooldownAmmo.CurrentCooldown : 0);
        lastCooldownAmmo?.ResetCooldown();
    }
}
