using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float invincibilityDuration = 1f; // <-- 1 second i-frames

    private float currentHealth;
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public event Action<float, float> OnHealthChanged;

    void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Update()
    {
        // TEST KEY
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }

        // Count down i-frames
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            TryTakeDamage(10);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // So you can take damage again after cooldown even if still touching
        if (other.CompareTag("Enemy"))
        {
            TryTakeDamage(10);
        }
    }

    public void TryTakeDamage(float damage)
    {
        if (isInvincible) return;  // Ignore damage during i-frames

        TakeDamage(damage);

        // Start i-frame
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        DeathScreen deathScreen = FindFirstObjectByType<DeathScreen>();
        if (deathScreen != null)
            deathScreen.ShowDeathScreen();
    }
}
