using UnityEngine;

public class SlashEffect : MonoBehaviour
{
    public float lifetime = 5.0f;
    public int damage = 5;
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Slash hit: " + other.name + " | Tag: " + other.tag);
        if (other.CompareTag("Enemy"))
            {
                // Try boss first
                BossHealth boss = other.GetComponent<BossHealth>();
                if (boss != null)
                {
                    boss.TakeDamage(damage);
                    Destroy(gameObject, lifetime);
                    return;
                }

                // Try normal enemy health
                EnemyHealth enemy = other.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Destroy(gameObject, lifetime);
                    return;
                }

                //Destroy(gameObject);
            }
    }
}
