using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;

    private int currentWayPoint = 0;

    public Transform[] waypoints;
    
    public float maxHealth = 3f;
    public float health;
    void Start()
    {
        Debug.Log($"{name}: waypoints count = {(waypoints != null ? waypoints.Length : -1)}");
    }

    void Awake()   // or OnEnable, so it works with pooling later
    {
        health = maxHealth;
    }
       void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentWayPoint];
        Vector3 dir = (target.position - transform.position).normalized;

        transform.position += dir * speed * Time.deltaTime;

        if(Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            currentWayPoint++;

            if(currentWayPoint >= waypoints.Length)
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
        health -= amount;
        if (health <= 0f)
            Die();
    }

    void Die()
    {
        CurrencyManager.Instance.AddGold(25);
        Destroy(gameObject);
    }
}
