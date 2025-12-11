using UnityEngine;

public class BasicEnemyProjectile : MonoBehaviour
{
    public float lifeTime = 3f;
    public float damage = 10f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth hp = other.GetComponent<PlayerHealth>();
            if (hp != null)
                hp.TryTakeDamage(damage);

            Destroy(gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            Destroy(gameObject);
        }
    }
}
