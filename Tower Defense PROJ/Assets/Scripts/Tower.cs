using UnityEngine;

public class Tower : MonoBehaviour
{
   [SerializeField] private float range = 3f;
    [SerializeField] private float fireInterval = 1f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float maxHealth = 20f;
    [SerializeField] private int cost = 50;
    [SerializeField] private int upgradeCost = 75;
    [SerializeField] private int maxLevel = 3;
    [SerializeField] private float upgradeMultiplier = 1.3f;

    private int level = 1;

    protected SpriteRenderer spriteRenderer;
    private Color baseColor = Color.white;  

    public int Cost { get { return cost; } }

    protected Color BaseColor { get { return baseColor; } }

    private float fireTimer;
    private Vector3Int myCell;
    private float health;
    
    public Vector3Int CurrentCell { get { return myCell; } }

    public void Initialize(Vector3Int cell)
    {
        myCell = cell;
    }

    protected virtual void Awake()
    {
        health = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void TakeDamage(float amount)
    {
        health -= Mathf.Abs(amount);
        if (health <= 0f) DestroyTower();
    }

    void DestroyTower()
    {
        if (TowerPlacer.Instance != null)
            TowerPlacer.Instance.FreeTile(myCell);
        Destroy(gameObject);
    }

   protected virtual void Update()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer < fireInterval) return;
        fireTimer = 0f;

        Enemy target = FindNearestEnemy();
        if (target != null)
        {
            GameObject p = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Projectile proj = p.GetComponent<Projectile>();
            if (proj != null) proj.Launch(target, CurrentDamage());
        }
    }

    Enemy FindNearestEnemy()
    {
        Enemy best = null;
        float bestDist = range;

        foreach (Enemy e in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist < bestDist)
            {
                bestDist = dist;
                best = e;
            }
        }
        return best;
    }

    public void TryUpgrade()
    {
        if (level >= maxLevel) return;
        if (CurrencyManager.Instance == null) return;
        if (!CurrencyManager.Instance.TrySpendGold(upgradeCost)) return;

        level++;
        damage *= upgradeMultiplier;
        range *= upgradeMultiplier;
        fireInterval /= upgradeMultiplier;

        transform.localScale *= 1.12f;
        Color gold = new Color(1f, 0.85f, 0.4f); //gold tint
        baseColor = Color.Lerp(Color.white, gold, (level - 1) / 3f); //fallback for Spirit's red tint during rage-state
        spriteRenderer.color = baseColor;     

        Debug.Log($"{name} upgraded to level {level}");
    }

    protected virtual float CurrentDamage()
    {
        return damage;
    }
}