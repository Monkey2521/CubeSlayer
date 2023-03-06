using TMPro;
using UnityEngine;

public class CubesCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _countText;

    private int _count;

    private void Start()
    {
        _count = 0;

        UpdateCounter();
    }

    public void OnCubeDies()
    {
        _count++;

        UpdateCounter();
    }

    private void UpdateCounter()
    {
        _countText.text = _count.ToString();
    }
}