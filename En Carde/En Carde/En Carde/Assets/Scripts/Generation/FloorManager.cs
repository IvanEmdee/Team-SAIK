using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [Header("References")]
    public TilemapPainter painter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FloorGenerator generator = new FloorGenerator(Random.Range(0, int.MaxValue));
        var rooms = generator.GenerateFloor();
        var edges = generator.GetMSTEdges();
        painter.PaintDungeon(rooms, edges);
        Debug.Log($"Dungeon generated with {rooms.Count} rooms and {edges.Count} edges.");
    }
}
