using UnityEngine;

public class GuardEffect : MonoBehaviour
{
    public float lifetime = 5.0f;
    public int HP = 50;
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
    void OnTriggerEnter2D(Collider2D other) {

        EnemyProjectile enemyProj = other.GetComponent<EnemyProjectile>();
        if (enemyProj != null)
        {
            HP -= enemyProj.damage;
            Destroy(other.gameObject);
            return;
        }

        // Try BasicEnemyProjectile
        BasicEnemyProjectile basicProj = other.GetComponent<BasicEnemyProjectile>();
        if (basicProj != null)
        {
            HP -= (int)basicProj.damage;
            Debug.Log(HP);
            Destroy(other.gameObject);
            return;
        }
    }
    void Update()
    {
        if(HP <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void DestroyShield()
    {
        Destroy(gameObject);
    }
}
