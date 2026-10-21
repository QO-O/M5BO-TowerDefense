using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public int gold = 50;

     public TMPro.TextMeshProUGUI currencyText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddGold(int amount) { gold += amount; UpdateCurrencyUI(); }
    public bool TrySpendGold(int amount)
    {
        if (gold < amount) return false;
        gold -= amount; UpdateCurrencyUI(); return true;
    }

    void UpdateCurrencyUI() 
    {  
        if (currencyText == null) return;
       currencyText.text = $"{gold}";
    }
}
