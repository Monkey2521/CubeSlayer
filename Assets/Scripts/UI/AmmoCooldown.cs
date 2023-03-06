using UnityEngine;
using UnityEngine.UI;

public class AmmoCooldown : MonoBehaviour
{
    [SerializeField] private Image _cooldownImage;

    private float _cooldown;
    private float _cooldownTimer;

    public bool Ready => _cooldownTimer >= _cooldown;
    public float CurrentCooldown => _cooldownTimer;

    public void Initialize(float cooldown, float currentCooldown = 0)
    {
        _cooldown = cooldown;
        _cooldownTimer = currentCooldown;

        UpdateCD();
    }

    public void ResetCooldown()
    {
        _cooldownTimer = 0;

        UpdateCD();
    }

    public void OnUpdate()
    {
        _cooldownTimer += Time.deltaTime;

        UpdateCD();
    }

    private void UpdateCD()
    {
        _cooldownImage.fillAmount = Mathf.Abs(_cooldownTimer / _cooldown - 1);
    }
}
