using UnityEngine;

public class Tower : MonoBehaviour
{
    public float range = 3f;
    public float fireInterval = 1f;   // seconds between shots
    public float damage = 1f;
    public GameObject projectilePrefab;

    private float fireTimer;

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
}