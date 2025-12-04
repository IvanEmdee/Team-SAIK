using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator
{
    // spawning bounds for the floor
    public const int GRID_SIZE = 50;
    // room size constraints
    public const int MIN_ROOM_SIZE = 6;
    public const int MAX_ROOM_SIZE = 12;
    // room number constraint
    public const int MAX_ROOMS = 8;

    private System.Random rng;

    public FloorGenerator(int seed)
    {
        rng = new System.Random(seed);
    }

    #region --- Data Models ---
    public class Room
    {
        public int id;
        public RectInt bounds;
        public List<Vector2Int> doors = new();
        public bool isStart, isExit, isTeleporter;

        public Vector2Int Center => new Vector2Int(bounds.x + bounds.width / 2, bounds.y + bounds.height / 2);
    }

    public class Edge
    {
        public Room a, b;
        public float weight;
    }

    public class Corridor
    {
        public List<Vector2Int> tiles = new();
    }
    #endregion

    // Store the MST edges so they can be accessed externally
    private List<Edge> mstEdges;
    public List<Edge> GetMSTEdges() => mstEdges;

    // ------------------------------------------------------------
    //  MAIN ENTRY POINT
    // ------------------------------------------------------------
    public List<Room> GenerateFloor()
    {
        List<Room> rooms = PlaceRooms();

        List<Edge> edges = ComputeGraphEdges(rooms);
        mstEdges = ComputeMST(rooms, edges);

        // (Optionally) add loops
        AddExtraEdges(edges, mstEdges, 0.2f);

        // Place doors based on actual connections
        PlaceDoors(rooms, mstEdges);

        // Special rooms
        AssignSpecialRooms(rooms);

        return rooms; // You will later pass this to a Tilemap painter.
    }

    #region --- ROOM PLACEMENT ---
    private List<Room> PlaceRooms()
    {
        List<Room> rooms = new();
        int attempts = 0;

        // max 2000 attempts to place rooms
        while (rooms.Count < MAX_ROOMS && attempts < 2000)
        {
            attempts++;

            int w = rng.Next(MIN_ROOM_SIZE, MAX_ROOM_SIZE + 1);
            int h = rng.Next(MIN_ROOM_SIZE, MAX_ROOM_SIZE + 1);

            int x = rng.Next(0, GRID_SIZE - w);
            int y = rng.Next(0, GRID_SIZE - h);

            RectInt candidate = new RectInt(x, y, w, h);

            if (OverlapsExisting(candidate, rooms)) continue;

            rooms.Add(new Room {id = rooms.Count, bounds = candidate});
        }
        
        return rooms;
    }

    private bool OverlapsExisting(RectInt rect, List<Room> rooms)
    {
        // 1-tile padding to prevent walls touching
        RectInt padded = new RectInt(rect.xMin - 1, rect.yMin - 1, rect.width + 2, rect.height + 2);

        foreach (var r in rooms)
            if (padded.Overlaps(r.bounds)) return true;

        return false;
    }
    #endregion

    #region --- DOOR PLACEMENT ---
    private void PlaceDoors(List<Room> rooms, List<Edge> edges)
    {
        // Place a door for each connection between rooms
        foreach (var edge in edges)
        {
            // Add door to room A facing room B
            Vector2Int doorA = PickDoorTowardsRoom(edge.a, edge.b);
            if (!edge.a.doors.Contains(doorA))
            {
                edge.a.doors.Add(doorA);
            }

            // Add door to room B facing room A
            Vector2Int doorB = PickDoorTowardsRoom(edge.b, edge.a);
            if (!edge.b.doors.Contains(doorB))
            {
                edge.b.doors.Add(doorB);
            }
        }
    }

    private Vector2Int PickDoorTowardsRoom(Room from, Room target)
    {
        // Determine which side of 'from' is closest to 'target'
        Vector2Int center = from.Center;
        Vector2Int targetCenter = target.Center;

        int dx = targetCenter.x - center.x;
        int dy = targetCenter.y - center.y;

        // Pick the dominant direction
        if (Mathf.Abs(dx) > Mathf.Abs(dy))
        {
            // Horizontal connection is stronger
            if (dx > 0)
            {
                // Target is to the right
                return new Vector2Int(
                    from.bounds.xMax,
                    rng.Next(from.bounds.yMin + 1, from.bounds.yMax - 1)
                );
            }
            else
            {
                // Target is to the left
                return new Vector2Int(
                    from.bounds.xMin - 1,
                    rng.Next(from.bounds.yMin + 1, from.bounds.yMax - 1)
                );
            }
        }
        else
        {
            // Vertical connection is stronger
            if (dy > 0)
            {
                // Target is above
                return new Vector2Int(
                    rng.Next(from.bounds.xMin + 1, from.bounds.xMax - 1),
                    from.bounds.yMax
                );
            }
            else
            {
                // Target is below
                return new Vector2Int(
                    rng.Next(from.bounds.xMin + 1, from.bounds.xMax - 1),
                    from.bounds.yMin - 1
                );
            }
        }
    }

    private Vector2Int PickDoorPosition(Room room)
    {
        int edge = rng.Next(4);

        switch (edge)
        {
            case 0: // Top
                return new Vector2Int(
                    rng.Next(room.bounds.xMin + 1, room.bounds.xMax - 1),
                    room.bounds.yMax
                );
            case 1: // Bottom
                return new Vector2Int(
                    rng.Next(room.bounds.xMin + 1, room.bounds.xMax - 1),
                    room.bounds.yMin - 1
                );
            case 2: // Left
                return new Vector2Int(
                    room.bounds.xMin - 1,
                    rng.Next(room.bounds.yMin + 1, room.bounds.yMax - 1)
                );
            default: // Right
                return new Vector2Int(
                    room.bounds.xMax,
                    rng.Next(room.bounds.yMin + 1, room.bounds.yMax - 1)
                );
        }
    }
    #endregion

    #region --- GRAPH + MST ---
    public List<Edge> ComputeGraphEdges(List<Room> rooms)
    {
        List<Edge> edges = new();

        for (int i = 0; i < rooms.Count; i++)
        {
            for (int j = i + 1; j < rooms.Count; j++)
            {
                float dist = Vector2Int.Distance(rooms[i].Center, rooms[j].Center);

                edges.Add(new Edge
                {
                    a = rooms[i],
                    b = rooms[j],
                    weight = dist
                });
            }
        }

        return edges;
    }

    public List<Edge> ComputeMST(List<Room> rooms, List<Edge> edges)
    {
        // Kruskal's algorithm
        List<Edge> result = new();

        var sorted = new List<Edge>(edges);
        sorted.Sort((x, y) => x.weight.CompareTo(y.weight));

        var parent = new Dictionary<Room, Room>();
        foreach (var r in rooms) parent[r] = r;

        Room Find(Room r)
        {
            if (parent[r] == r) return r;
            parent[r] = Find(parent[r]);
            return parent[r];
        }

        void Union(Room a, Room b) => parent[Find(a)] = Find(b);

        foreach (var e in sorted)
        {
            if (Find(e.a) != Find(e.b))
            {
                result.Add(e);
                Union(e.a, e.b);

                if (result.Count == rooms.Count - 1) break;
            }
        }

        return result;
    }

    private void AddExtraEdges(List<Edge> all, List<Edge> mst, float chance)
    {
        foreach (var e in all)
        {
            if (mst.Contains(e)) continue;
            if (rng.NextDouble() < chance)
                mst.Add(e);
        }
    }
    #endregion

    #region --- SPECIAL ROOMS ---
    private void AssignSpecialRooms(List<Room> rooms)
    {
        Room start = rooms[rng.Next(rooms.Count)];
        start.isStart = true;

        Room exit = PickFarthest(rooms, start);
        exit.isExit = true;

        Room teleporter;
        do { teleporter = rooms[rng.Next(rooms.Count)]; }
        while (teleporter == start || teleporter == exit);

        teleporter.isTeleporter = true;
    }

    private Room PickFarthest(List<Room> rooms, Room from)
    {
        Room far = null;
        float dist = -1;

        foreach (var r in rooms)
        {
            if (r == from) continue;

            float d = Vector2.Distance(r.Center, from.Center);

            if (d > dist)
            {
                dist = d;
                far = r;
            }
        }

        return far;
    }
    #endregion
}
