using UnityEngine;
using System;
using System.Threading.Tasks;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float invincibilityDuration = 1f; // <-- 1 second i-frames
    private float currentHealth;
    public bool isInvincible = false;
    public float invincibilityTimer = 0f;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public event Action<float, float> OnHealthChanged;
    [SerializeField] playerAudio audio;
    void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Update()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        // TEST KEY
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }

        // Count down i-frames
        if (isInvincible)
        {
            sr.color = Color.blue;
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
                sr.color = Color.white;
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

    public async void  TryTakeDamage(float damage)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (isInvincible) return;  // Ignore damage during i-frames
        sr.color = Color.red;
        TakeDamage(damage);

        // Start i-frame
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
        await Task.Delay(1000);
        sr.color = Color.white;
    }

    public async void TakeDamage(float damage)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.red;
        audio.playTakeDamage();
        currentHealth -= damage;
        Debug.Log(currentHealth);
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
        Debug.Log("Player Died!");
        DeathScreen deathScreen = FindFirstObjectByType<DeathScreen>();
        if (deathScreen != null)
            deathScreen.ShowDeathScreen();
    }
}
