using System.Collections.Generic;
using UnityEngine;

public class FloorPlanner : MonoBehaviour
{
    class Cell {
        public Cell(string name) {
            this.name = name;
        }

        public string name;
        public RoomType type = RoomType.Normal;
        public bool[] openDoors = {false, false, false, false};
        public bool isEndRoom;
    }

    enum Heading {Top, Left, Down, Right, None}
    enum RoomType {Normal, Start, Boss, Treasure}

    [SerializeField] Globals globals;

    [Header("Prefabs")]
    [SerializeField] GameObject startRoom;
    [SerializeField] GameObject bossRoom;
    [SerializeField] GameObject treasureRoom;
    [SerializeField] GameObject normalRoom;
    [SerializeField] GameObject[] roomLayouts;

    [Header("Config")]
    [SerializeField] Vector2Int floorSize;
    [SerializeField] int maxRooms = 20;
    [SerializeField] int maxNeighbors = 1;
    [SerializeField] int maxStraights = 4;
    [SerializeField] AnimationCurve roomFailChance = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    Cell[,] map;
    Queue<Vector2Int> queue;
    Vector2Int startCell;
    int roomCount;


    void Start()
    {
        // Check serial fields
        if (startRoom == null)    Debug.LogError("FloorPlanner: Missing reference to Start Room Prefab");
        if (bossRoom == null)     Debug.LogError("FloorPlanner: Missing reference to Boss Room Prefab");
        if (treasureRoom == null) Debug.LogError("FloorPlanner: Missing reference to Treasure Room Prefab");
        if (normalRoom == null)   Debug.LogError("FloorPlanner: Missing reference to Normal Room Prefab");
        if (roomLayouts == null) Debug.LogError("FloorPlanner: Missing reference to Room Pattern Prefabs");
        if (floorSize  == Vector2.zero) Debug.LogError("FloorPlanner: Floor Size uninitialized.");

        // Initialize data structures
        map = new Cell[floorSize.y, floorSize.x];
        globals.rooms = new GameObject[floorSize.y, floorSize.x];
        queue = new Queue<Vector2Int>();

        // Generate and instantiate map
        GenerateMap();
        AddSpecialRooms();
        InstantiateMap();
    }

    void GenerateMap()
    {
        // Ensure queue is clear before beginning
        queue.Clear();

        // Create start room
        startCell = new (floorSize.x / 2, floorSize.y / 2);
        while (!TryCreateCell(startCell.x, startCell.y, Heading.None, 4));
        map[startCell.y, startCell.x].type = RoomType.Start;

        // Generate map breadth-first
        while (queue.Count > 0 && roomCount < maxRooms) {

            Vector2Int curCell = queue.Dequeue();
            int x = curCell.x;
            int y = curCell.y;

            int strictness = 4;
            bool createdRoom = false;

            while (!createdRoom) {

                // Create neighbor cells and open doors
                if (TryCreateCell(x, y+1, Heading.Top, strictness)) {    // Top
                    map[y,x].openDoors[0] = true;
                    map[y+1, x].openDoors[2] = true;
                    createdRoom = true;
                }
                if (TryCreateCell(x-1 , y, Heading.Left, strictness)) {  // Left
                    map[y,x].openDoors[1] = true;
                    map[y, x-1].openDoors[3] = true;
                    createdRoom = true;
                }
                if (TryCreateCell(x, y-1, Heading.Down, strictness)) {   // Down
                    map[y,x].openDoors[2] = true;
                    map[y-1, x].openDoors[0] = true;
                    createdRoom = true;
                }
                if (TryCreateCell(x+1 , y, Heading.Right, strictness)) { // Right
                    map[y,x].openDoors[3] = true;
                    map[y, x+1].openDoors[1] = true;
                    createdRoom = true;
                }
                
                // If creation attempt failed, reduce strictness
                if (!createdRoom) {
                    strictness--;
                    Debug.Log($"FloorPlanner: Reduced strictness to {strictness}.");
                }

                // Move on if attempt failed with lowest strictness
                if (strictness < 0) {
                    Debug.Log($"FloorPlanner: Unable to create children for room {map[y,x].name}.");
                    break;
                }
            }
        }

        Debug.Log($"FloorPlanner: Generated {roomCount} rooms.");
    }

    bool TryCreateCell(int x, int y, Heading heading, int strictness)
    {
        /* Strictness:
         * 0: All non-critical checks ignored.
         * 1: Straightness check ignored.
         * 2: Neighbor check ignored.
         * 3: Randomness check ignored.
        **/

        // Fail if location out of bounds
        if (x < 0 || x >= floorSize.x
         || y < 0 || y >= floorSize.y) {
            return false;
        }

        // Fail if cell already exists
        if (map[y,x] != null) {
            return false;
        }

        // Fail if max room count will be exceeded
        if (roomCount + 1 > maxRooms) {
            return false;
        }

        // Prevent long straight paths by looking backwards (i.e., if heading down, look up)
        if (strictness > 1) {

            int i;
            for (i = 1; i < maxStraights; i++) {
                if      (heading == Heading.Down  && !map[y+i, x].openDoors[0]) break;
                else if (heading == Heading.Right && !map[y, x-i].openDoors[1]) break;
                else if (heading == Heading.Top   && !map[y-i, x].openDoors[2]) break;
                else if (heading == Heading.Left  && !map[y, x+i].openDoors[3]) break;

                else if (heading == Heading.None) break;
            }

            if (i >= maxStraights) return false;
        }

        // Fail if too many neighboring squares
        if (strictness > 2) {
            int neighborCount = 0;
            if (y < floorSize.y - 1 && map[y+1, x] != null) neighborCount++; // Top
            if (x > 0               && map[y, x-1] != null) neighborCount++; // Left
            if (y > 0               && map[y-1, x] != null) neighborCount++; // Down
            if (x < floorSize.x - 1 && map[y, x+1] != null) neighborCount++; // Right

            if (neighborCount > maxNeighbors) {
                return false;
            }
        }

        // Randomly omit non-vital rooms to spice up floor shape
        if (strictness > 3 && Random.value < roomFailChance.Evaluate((roomCount + 1) / maxRooms)) {
            return false;
        }

        // Create cell in map and add to generation queue
        map[y,x] = new Cell($"Room{roomCount + 1}");
        queue.Enqueue(new Vector2Int(x,y));
        roomCount++;

        return true;
    }

    void AddSpecialRooms()
    {
        if (queue.Count < 2) {
            Debug.LogError("FloorPlanner: Not enough end rooms in map; can't place Boss and Treasure room.");
            return;
        }

        Vector2Int curCell;
        int x, y;
        bool hasTreasure = false;

        // Randomly place treasure room using ramping probability
        while (!hasTreasure) {
            curCell = queue.Dequeue();
            x = curCell.x;
            y = curCell.y;

            if (Random.value <= (1 / queue.Count)) {
                map[y,x].type = RoomType.Treasure;
                hasTreasure = true;
            }
        }

        // Discard rooms until end of queue
        while (queue.Count > 1) {
            queue.Dequeue();
        }

        // Place boss room in last cell
        curCell = queue.Dequeue();
        x = curCell.x;
        y = curCell.y;
        map[y,x].type = RoomType.Boss;
    }

    void InstantiateMap()
    {
        for (int cellY = 0; cellY < floorSize.y; cellY++) {
            for (int cellX = 0; cellX < floorSize.x; cellX++) {

                // Skip unused cells
                if (map[cellY,cellX] == null) continue;

                Cell cell = map[cellY,cellX];
                Vector3 position = new ((Room.size.x + Room.spacing.x) * (cellX - startCell.x),
                                        (Room.size.y + Room.spacing.y) * (cellY - startCell.y),
                                        0);

                GameObject roomObj = cell.type switch
                {
                    RoomType.Start    => Instantiate(startRoom,    position, transform.rotation),
                    RoomType.Boss     => Instantiate(bossRoom,     position, transform.rotation),
                    RoomType.Treasure => Instantiate(treasureRoom, position, transform.rotation),
                    _                 => Instantiate(normalRoom,   position, transform.rotation),
                };

                roomObj.name = cell.name;
                roomObj.GetComponent<Room>()
                    .EnableDoors(cell.openDoors)
                    .SetXY(cellX, cellY);

                // Generate tilemap for Normal rooms
                if (cell.type == RoomType.Normal) {
                    int layoutIdx = GetValidLayout(cell.openDoors);
                    Instantiate(roomLayouts[layoutIdx], roomObj.transform);
                }

                globals.rooms[cellY,cellX] = roomObj;
                roomObj.SetActive(false);
            }
        }
        
        globals.rooms[startCell.y, startCell.x].SetActive(true);
    }

    int GetValidLayout(bool[] doors)
    {
        int layoutIdx = -1;

        while (layoutIdx == -1) {

            // Randomly choose pattern prefab
            layoutIdx = Random.Range(0, roomLayouts.Length);

            // Ensure doors are open
            string tag = roomLayouts[layoutIdx].tag;

            if (doors[0] && !tag.Contains("T")
             || doors[1] && !tag.Contains("L")
             || doors[2] && !tag.Contains("D")
             || doors[3] && !tag.Contains("R"))
            {
                layoutIdx = -1;
            }

        }

        return layoutIdx;
    }
}