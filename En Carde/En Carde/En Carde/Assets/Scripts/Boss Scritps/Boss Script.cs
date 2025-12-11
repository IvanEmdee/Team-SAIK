using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    private Transform player;

    [Header("Death Settings")]
    public GameObject spawnOnDeathPrefab;

    [Header("Attack Settings")]
    public GameObject projectilePrefab;
    public float shootInterval = 2f;
    private float shootTimer = 0f;

    private Rigidbody2D rb;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //This makes the boss clip through obsticles
        //rb.bodyType = RigidbodyType2D.Kinematic;
        rb.freezeRotation = true;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player != null)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                ShootProjectile();
                shootTimer = 0f;
            }
        }

        // test kill 
        /**
        if (Input.GetKeyDown(KeyCode.K))
        {
            BossHealth bossHealth = GetComponent<BossHealth>();
            if (bossHealth != null)
                bossHealth.TakeDamage(5);
        }
        **/
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 newPos = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && player != null)
        {
            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Vector2 dir = (player.position - transform.position).normalized;
            proj.transform.up = dir;
        }
    }

    // called from BossHealth when boss dies
    public void Die()
    {
        if (spawnOnDeathPrefab != null)
            Instantiate(spawnOnDeathPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
    public async Task TakeDamage(int amount)
    {

        Debug.Log("Checking for boss");
        BossHealth bossHealth = GetComponent<BossHealth>();
            if (bossHealth != null)
                bossHealth.TakeDamage(5);


    }
}
