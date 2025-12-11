using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("BossRoom");
            Destroy(gameObject);
        }
    }
}