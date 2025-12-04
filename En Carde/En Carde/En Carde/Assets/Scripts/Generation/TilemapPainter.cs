using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPainter : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    public Tilemap doorTilemap;

    [Header("Tiles")]
    public TileBase roomFloorTile;
    public TileBase corridorTile;
    public TileBase wallTile;
    public TileBase doorTile;

    // Dungeon data produced by FloorGenerator:
    public List<FloorGenerator.Room> rooms;
    public List<FloorGenerator.Edge> edges;

    // A 2D grid used while painting
    private int gridSize = FloorGenerator.GRID_SIZE;
    private TileType[,] tileBuffer;

    private enum TileType
    {
        Empty,
        RoomFloor,
        Corridor,
        Door,
        Wall
    }

    // Call this from your dungeon manager:
    //   painter.PaintDungeon(generatorResultRooms, generatorEdges)
    public void PaintDungeon(List<FloorGenerator.Room> rooms, List<FloorGenerator.Edge> edges)
    {
        this.rooms = rooms;
        this.edges = edges;

        tileBuffer = new TileType[gridSize, gridSize];

        ClearTilemaps();
        PaintRooms();
        PaintCorridors();
        PaintDoors();
        PaintWalls();
        ApplyToTilemaps();
    }

    // ------------------------------------------------------------
    #region CLEARING
    // ------------------------------------------------------------
    private void ClearTilemaps()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
        doorTilemap.ClearAllTiles();
    }
    #endregion

    // ------------------------------------------------------------
    #region ROOM PAINTING
    // ------------------------------------------------------------
    private void PaintRooms()
    {
        foreach (var r in rooms)
        {
            for (int x = r.bounds.xMin; x < r.bounds.xMax; x++)
                for (int y = r.bounds.yMin; y < r.bounds.yMax; y++)
                    tileBuffer[x, y] = TileType.RoomFloor;
        }
    }
    #endregion

    // ------------------------------------------------------------
    #region CORRIDOR PAINTING
    // ------------------------------------------------------------
    private void PaintCorridors()
    {
        // For each edge, carve a corridor (simple L-shape)
        foreach (var e in edges)
        {
            Vector2Int start = PickClosestDoorTile(e.a, e.b);
            Vector2Int end = PickClosestDoorTile(e.b, e.a);

            CarveCorridorPath(start, end);
        }
    }

    // Choose door that is nearest to other room's center
    private Vector2Int PickClosestDoorTile(FloorGenerator.Room r, FloorGenerator.Room target)
    {
        Vector2Int best = r.doors[0];
        float bestDist = Vector2Int.Distance(best, target.Center);

        foreach (var d in r.doors)
        {
            float dist = Vector2Int.Distance(d, target.Center);
            if (dist < bestDist)
            {
                bestDist = dist;
                best = d;
            }
        }

        return best;
    }

    private void CarveCorridorPath(Vector2Int a, Vector2Int b)
    {
        // 50% horizontal-first, 50% vertical-first
        bool horizontalFirst = Random.value > 0.5f;

        if (horizontalFirst)
        {
            CarveHorizontal(a.x, b.x, a.y);
            CarveVertical(a.y, b.y, b.x);
        }
        else
        {
            CarveVertical(a.y, b.y, a.x);
            CarveHorizontal(a.x, b.x, b.y);
        }
    }

    private void CarveHorizontal(int x0, int x1, int y)
    {
        if (x1 < x0) (x0, x1) = (x1, x0);

        for (int x = x0; x <= x1; x++)
        {
            MarkCorridor(x, y);
        }
    }

    private void CarveVertical(int y0, int y1, int x)
    {
        if (y1 < y0) (y0, y1) = (y1, y0);

        for (int y = y0; y <= y1; y++)
        {
            MarkCorridor(x, y);
        }
    }

    private void MarkCorridor(int x, int y)
    {
        if (!InBounds(x, y)) return;

        // Don't overwrite room floors or doors
        if (tileBuffer[x, y] == TileType.RoomFloor ||
            tileBuffer[x, y] == TileType.Door)
            return;

        tileBuffer[x, y] = TileType.Corridor;
    }
    #endregion

    // ------------------------------------------------------------
    #region DOORS
    // ------------------------------------------------------------
    private void PaintDoors()
    {
        foreach (var r in rooms)
        {
            foreach (var d in r.doors)
            {
                if (!InBounds(d.x, d.y)) continue;

                // Corridor on one side + room on the other → door
                tileBuffer[d.x, d.y] = TileType.Door;
            }
        }
    }
    #endregion

    // ------------------------------------------------------------
    #region WALL PAINTING
    // ------------------------------------------------------------
    private void PaintWalls()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (tileBuffer[x, y] == TileType.Empty)
                {
                    if (HasAdjacentFloor(x, y))
                        tileBuffer[x, y] = TileType.Wall;
                }
            }
        }
    }

    private bool HasAdjacentFloor(int x, int y)
    {
        foreach (var n in Neighbors(x, y))
        {
            var t = tileBuffer[n.x, n.y];
            if (t == TileType.RoomFloor || t == TileType.Corridor || t == TileType.Door)
                return true;
        }
        return false;
    }

    private IEnumerable<Vector2Int> Neighbors(int x, int y)
    {
        for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                int nx = x + dx, ny = y + dy;
                if (InBounds(nx, ny)) yield return new(nx, ny);
            }
    }
    #endregion

    // ------------------------------------------------------------
    #region APPLY TO TILEMAPS
    // ------------------------------------------------------------
    private void ApplyToTilemaps()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);

                switch (tileBuffer[x, y])
                {
                    case TileType.RoomFloor:
                        floorTilemap.SetTile(pos, roomFloorTile);
                        break;

                    case TileType.Corridor:
                        floorTilemap.SetTile(pos, corridorTile);
                        break;

                    case TileType.Door:
                        doorTilemap.SetTile(pos, doorTile);
                        break;

                    case TileType.Wall:
                        wallTilemap.SetTile(pos, wallTile);
                        break;

                    default:
                        break;
                }
            }
        }
    }
    #endregion

    private bool InBounds(int x, int y)
    {
        return x >= 0 && x < gridSize && y >= 0 && y < gridSize;
    }
}
