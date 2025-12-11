using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [Header("Spawn when last enemy dies")]
    public GameObject finalSpawnPrefab;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private int enemyCount = 0;

    public void RegisterEnemy()
    {
        enemyCount++;
        //Debug.Log("Enemies Alive: " + enemyCount);
    }

    public void UnregisterEnemy(Vector3 deathPosition)
    {
        enemyCount--;
        //Debug.Log("Enemy Died. Remaining: " + enemyCount);

        if (enemyCount <= 0)
        {
            SpawnFinalObject(deathPosition);
        }
    }

    void SpawnFinalObject(Vector3 pos)
    {
        if (finalSpawnPrefab != null)
            Instantiate(finalSpawnPrefab, pos, Quaternion.identity);
    }
}
