using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private BossHealth bossHealth; 
    [SerializeField] private Image fillImage;        

    void Start()
    {
        if (bossHealth != null)
        {
            bossHealth.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(bossHealth.CurrentHealth, bossHealth.MaxHealth);
        }
        else
        {
            Debug.LogError("BossHealthBar: BossHealth reference not set!");
        }
    }

    void UpdateHealthBar(float current, float max)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = current / max;
        }
    }
}
