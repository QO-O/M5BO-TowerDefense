using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeBaseHealthUI : MonoBehaviour
{
    [SerializeField] private HomeBase homeBase;
    [SerializeField] private TextMeshProUGUI healthText;

    void Update()
    {
        if (homeBase == null) return;

        if (healthText != null)
            healthText.text = $"{homeBase.Health:0} / {homeBase.MaxHealth:0}";
    }
}