using UnityEngine;

public class EnemyLineOfSightShooter : MonoBehaviour
{
    [Header("Player Detection")]
    public Transform player;
    public float maxSightDistance = 10f;
    public LayerMask obstructionLayers; 
    public LayerMask playerLayer;

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public float shootInterval = 1.5f;
    private float shootTimer = 0f;

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        shootTimer += Time.deltaTime;

        if (HasLineOfSight() && shootTimer >= shootInterval)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    public void TakeDamage(int amount)
{
    EnemyHealth health = GetComponent<EnemyHealth>();
    if (health != null)
        health.TakeDamage(amount);
}

    bool HasLineOfSight()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

        // raycast for anything except obstacles
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            distance,
            obstructionLayers | playerLayer
        );

        if (hit.collider == null)
            return false;

        // true only if the ray directly hits player
        return hit.collider.CompareTag("Player");
    }

    void Shoot()
    {
        if (projectilePrefab == null) return;

        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // aim projectile toward player
        Vector2 direction = (player.position - transform.position).normalized;
        proj.GetComponent<Rigidbody2D>().linearVelocity = direction * 5f; // speed adjustable
    }
}
