using UnityEngine;

public class HomeBase : MonoBehaviour
{
    private static HomeBase instance;
    public static HomeBase Instance { get { return instance; } }

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private ScreenManager screenManager;
    [SerializeField] private UnityEngine.Events.UnityEvent onBaseDestroyed;

    private float health;

    private float lastAttackedTime = -999f;
    public bool RecentlyAttacked { get { return Time.time - lastAttackedTime < 3f; } }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            health = Mathf.Abs(maxHealth);
        }
        else Destroy(gameObject);
    }

    public void TakeDamage(float amount)
    {
        lastAttackedTime = Time.time;
        health -= Mathf.Abs(amount);
        if (health <= 0f)
        {
            health = 0f;
            GameOver();
        }
    }

    void GameOver()
    {
        if (screenManager != null)
            screenManager.ShowLose();
    }

    public float Health => health;
    public float MaxHealth => maxHealth;
}