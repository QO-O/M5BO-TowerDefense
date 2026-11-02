using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private int goldReward = 25;
    [SerializeField] private float baseDamage = 5f;
    [SerializeField] private float attackInterval = 1f;

    [SerializeField] private float towerDetectRange = 1.2f;
    [SerializeField] private float towerDamage = 2f;

    private Tower targetTower;

    private int currentWayPoint = 0;
    private Transform[] waypoints;
    private float health;
     private float attackTimer = 0f;
    private bool attackingBase = false;

    public void SetRoute(Transform[] path)
    {
        waypoints = path;
        currentWayPoint = 0;
    }


    void Awake()
    {
        health = maxHealth;
    }

    void Start()
    {
    
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (attackingBase)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                attackTimer = 0f;
                if (HomeBase.Instance != null)
                    HomeBase.Instance.TakeDamage(baseDamage);
                else
                    Debug.Log($"{name}: reached base but HomeBase.Instance is NULL!");
            }
            return;
        }

         if (targetTower != null)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                attackTimer = 0f;
                targetTower.TakeDamage(towerDamage);
                if (targetTower == null) attackTimer = 0f;
            }
            return;
        }

    Collider2D hit = Physics2D.OverlapCircle(transform.position, towerDetectRange, LayerMask.GetMask("Tower"));
    if (hit != null)
    {
        Tower t = hit.GetComponent<Tower>();
        if (t != null)
        {
            targetTower = t;
            attackTimer = 0f;
            return;
        }
    }

        Transform target = waypoints[currentWayPoint];
        Vector3 dir = (target.position - transform.position).normalized;

        transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            currentWayPoint++;

            if (currentWayPoint >= waypoints.Length)
            {
                attackingBase = true;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        amount = Mathf.Abs(amount);  
        health -= amount;
        if (health <= 0f)
            Die();
    }

    void Die()
    {
        CurrencyManager.Instance.AddGold(goldReward);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, towerDetectRange);
    }
}