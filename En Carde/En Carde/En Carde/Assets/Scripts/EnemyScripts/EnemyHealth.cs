using System.Threading.Tasks;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    void Awake()
    {
        currentHealth = maxHealth;
        EnemyManager.Instance?.RegisterEnemy();

    }

    public async Task TakeDamage(int amount)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.red;
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
            Die();
        await Task.Delay(1000);
        sr.color = Color.white;
    }

    void Die()
    {
        // tell the main enemy script to die if it exists
        EnemyLineOfSightShooter shooter = GetComponent<EnemyLineOfSightShooter>();
        if (shooter != null)
        {
            EnemyManager.Instance?.UnregisterEnemy(transform.position);
            Destroy(gameObject);
            return;
        }

        // fallback
        Destroy(gameObject);
    }
}
