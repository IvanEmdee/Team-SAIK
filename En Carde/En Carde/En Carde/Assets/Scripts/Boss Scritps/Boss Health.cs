using UnityEngine;
using System;
using System.Threading.Tasks;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private Enemy enemyScript;
    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public event Action<float, float> OnHealthChanged; // (current, max)

    void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth); // Initialize UI
    }

    public async void TakeDamage(int damage)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.red;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        await Task.Delay(1000);
        sr.color = Color.white;
    }

    void Die()
    {
    if (enemyScript != null)
        enemyScript.Die();

    Destroy(gameObject);
    }
}
