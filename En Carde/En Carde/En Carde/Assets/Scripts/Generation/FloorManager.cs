using UnityEngine;
using System.Collections.Generic;

public class FloorManager : MonoBehaviour
{
    [Header("References")]
    public TilemapPainter painter;

    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject torchPrefab;
    private System.Random rng;

    void Start()
    {
        rng = new System.Random();
        FloorGenerator generator = new FloorGenerator(Random.Range(0, int.MaxValue));
        var rooms = generator.GenerateFloor();
        var edges = generator.GetMSTEdges();
        painter.PaintDungeon(rooms, edges);
        SpawnEnemies(rooms);
        SpawnTorches(rooms);
        Debug.Log($"Dungeon generated with {rooms.Count} rooms and {edges.Count} edges.");
    }

    private void SpawnEnemies(List<FloorGenerator.Room> rooms)
    {
        if (enemyPrefab == null)
            return;
        
        foreach (var room in rooms)
        {
            int x = rng.Next(room.bounds.xMin + 1, room.bounds.xMax - 1);
            int y = rng.Next(room.bounds.yMin + 1, room.bounds.yMax - 1);
            Vector2Int spawnPoint = new Vector2Int(x, y);
            Instantiate(enemyPrefab, new Vector3(spawnPoint.x + 0.5f, spawnPoint.y + 0.5f, 0), Quaternion.identity);
        }
    }

    private void SpawnTorches(List<FloorGenerator.Room> rooms)
    {
        if (torchPrefab == null)
            return;

        foreach (var room in rooms)
        {
            Instantiate(torchPrefab, new Vector3(room.Center.x + 0.5f, room.Center.y + 0.5f, 0), Quaternion.identity);
        }
    }
}
