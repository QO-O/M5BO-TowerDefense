using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;

    private int currentWayPoint = 0;

    public Transform[] waypoints;

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

        if(Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            currentWayPoint++;

            if(currentWayPoint >= waypoints.Length)
            {
                Destroy(gameObject);
            }
        }
    }
}
