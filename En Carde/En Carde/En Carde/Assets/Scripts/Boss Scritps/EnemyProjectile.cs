using UnityEngine;
using System.Collections.Generic;
public class EnemyProjectile : MonoBehaviour
{
    public float speed = 5f;                 // Projectile speed
    public float homingStrength = 2f;        // How fast it adjusts toward the player
    public float lifetime = 5f;              // Automatically destroy after some time
    public int damage = 20;
    private Transform player;

    //(Steven) adding sprites for animation on projectile
    [SerializeField] List<Sprite> sprites;
    SpriteAnimator spriteAnimator;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        //(Steven) starting animation
        spriteAnimator = new SpriteAnimator(sprites, GetComponent<SpriteRenderer>());
        spriteAnimator.Start();
        if (playerObj != null)
            player = playerObj.transform;

        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 moveDir = Vector2.Lerp(transform.up, direction, homingStrength * Time.fixedDeltaTime);
            transform.up = moveDir;
            transform.position += (Vector3)moveDir * speed * Time.fixedDeltaTime;
        }
        else
        {
            transform.position += transform.up * speed * Time.fixedDeltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Damage the player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                //Testing iFrame (Steven)
                playerHealth.TryTakeDamage(damage);
            }

            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    //(Steven) handling changing sprite on update
    private void Update()
    {
        spriteAnimator.HandleUpdate();
    }
}
