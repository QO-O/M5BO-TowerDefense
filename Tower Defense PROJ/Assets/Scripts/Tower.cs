using UnityEngine;

public class Tower : MonoBehaviour
{
   [SerializeField] private float range = 3f;
    [SerializeField] private float fireInterval = 1f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float maxHealth = 20f;    // NEW

    private float fireTimer;
    private Vector3Int myCell;
    private float health;

    public void Initialize(Vector3Int cell)
    {
        myCell = cell;
    }

    void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(float amount)
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

    void Update()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer < fireInterval) return;
        fireTimer = 0f;

        Enemy target = FindNearestEnemy();
        if (target != null)
        {
            GameObject p = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Projectile proj = p.GetComponent<Projectile>();
            if (proj != null) proj.Launch(target, damage);
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}