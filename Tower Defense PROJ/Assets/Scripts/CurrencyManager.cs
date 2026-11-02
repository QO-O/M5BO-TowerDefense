using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private static CurrencyManager instance;
    public static CurrencyManager Instance { get { return instance; } }

    [SerializeField] private int startingGold = 50;
    [SerializeField] private TMPro.TextMeshProUGUI currencyText;

    private int gold;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            gold = Mathf.Abs(startingGold);
            UpdateCurrencyUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddGold(int amount)
    {
        gold += Mathf.Abs(amount);
        UpdateCurrencyUI();
    }

    public bool TrySpendGold(int amount)
    {
        amount = Mathf.Abs(amount); 
        if (gold < amount) return false;
        gold -= amount;
        UpdateCurrencyUI();
        return true;
    }
    public int Gold { get { return gold; } }

    void UpdateCurrencyUI()
    {
        if (currencyText == null) return;
        currencyText.text = $"{gold}";
    }
}