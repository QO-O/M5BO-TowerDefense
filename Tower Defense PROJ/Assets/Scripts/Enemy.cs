using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private int goldReward = 25;

    private int currentWayPoint = 0;
    private Transform[] waypoints;
    private float health;

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
        Debug.Log($"{name}: waypoints count = {(waypoints != null ? waypoints.Length : -1)}");
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentWayPoint];
        Vector3 dir = (target.position - transform.position).normalized;

        transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            currentWayPoint++;

            if (currentWayPoint >= waypoints.Length)
            {
                Destroy(gameObject);
            }
        }

        //     if (Input.GetKeyDown(KeyCode.K))
        // {
        //     foreach (Enemy e in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        //         e.TakeDamage(1f);
        // }
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
}