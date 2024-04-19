using System.Collections.Generic;
using UnityEngine;

enum Heading {
    None  = 0,
    Top   = 1,
    Left  = 2,
    Down  = 3,
    Right = 4
}

enum RoomType {
    Normal   = 0,
    Start    = 1,
    Boss     = 2,
    Treasure = 3
}

public class FloorPlanner : MonoBehaviour {

    class Cell {
        public Cell(string name) {
            this.name = name;
        }

        public string name;
        public RoomType type = RoomType.Normal;
        public bool[] openDoors = {false, false, false, false};
        public bool isEndRoom;
    }

    [Header("Prefabs")]
    [SerializeField] GameObject roomBase;
    [SerializeField] GameObject[] normalPatterns;
    [SerializeField] GameObject[] bossPatterns;
    [SerializeField] GameObject[] treasurePatterns;

    [Header("Forest Meshes")]
    [SerializeField] GameObject forestCorners;
    [SerializeField] GameObject forestGround;
    [SerializeField] GameObject[] forestBlockers;

    [Header("Temple Meshes")]
    [SerializeField] GameObject templeCorners;
    [SerializeField] GameObject templeGround;
    [SerializeField] GameObject[] templeBlockers;
    [SerializeField] GameObject[] templeDoors;

    [Header("Config")]
    [SerializeField] Vector2Int floorSize;
    [SerializeField] int maxRooms = 20;
    [SerializeField] int maxNeighbors = 1;
    [SerializeField] int maxStraights = 4;
    [SerializeField] AnimationCurve roomFailChance;

    Cell[,] map;
    Queue<Vector2Int> queue;
    Vector2Int startCell;
    int roomCount;

    bool complete = false;

    void Awake() {
        // Prefabs
        Util.CheckReference(name, "Room Base", roomBase);
        Util.CheckArray(name, "Normal Patterns", normalPatterns);
        Util.CheckArray(name, "Boss Patterns", bossPatterns);
        Util.CheckArray(name, "Treasure Patterns", treasurePatterns);

        // Forest Meshes
        Util.CheckReference(name, "Forest Corners", forestCorners);
        Util.CheckReference(name, "Forest Ground", forestGround);
        Util.CheckArray(name, "Forest Blockers", forestBlockers);

        // Temple Meshes
        Util.CheckReference(name, "Temple Corners", templeCorners);
        Util.CheckReference(name, "Temple Ground", templeGround);
        Util.CheckArray(name, "Temple Blockers", templeBlockers);
        Util.CheckArray(name, "Temple Doors", templeDoors);

        if (floorSize  == Vector2.zero) Util.Error(name, "Floor Size uninitialized.");
    }

    void Start() {   

        int attempts = 10;
        while (!complete && attempts-- > 0) {

            // Initialize data structures
            map = new Cell[floorSize.y, floorSize.x];
            Refs.rooms = new Room[floorSize.y, floorSize.x];
            roomCount = 0;
            queue = new Queue<Vector2Int>();

            // Generate and instantiate map
            GenerateMap();
            AddSpecialRooms();
        }

        InstantiateMap();
    }

    void GenerateMap() {
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
                    // Debug.Log($"FloorPlanner: Reduced strictness to {strictness}.");
                }

                // Move on if attempt failed with lowest strictness
                if (strictness < 0) {
                    Debug.Log($"FloorPlanner: Unable to create children for room {map[y,x].name}.");
                    break;
                }
            }
        }

        // Debug.Log($"FloorPlanner: Generated {roomCount} rooms.");
    }

    bool TryCreateCell(int x, int y, Heading heading, int strictness) {
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

    void AddSpecialRooms() {
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

        complete = true;
    }

    void InstantiateMap() {
        for (int cellY = 0; cellY < floorSize.y; cellY++) {
            for (int cellX = 0; cellX < floorSize.x; cellX++) {

                // Skip unused cells
                if (map[cellY,cellX] == null) continue;

                Cell cell = map[cellY,cellX];
                Vector3 position = new ((Room.size.x + Room.spacing.x) * (cellX - startCell.x),
                                        (Room.size.y + Room.spacing.y) * (cellY - startCell.y),
                                        0);

                GameObject roomObj = Instantiate(roomBase, position, transform.rotation);
                roomObj.name = cell.name;

                Room room = roomObj.GetComponent<Room>();
                room.x = cellX;
                room.y = cellY;
                room.SetDoors(cell.openDoors);

                Transform root = roomObj.transform.Find("Root");
                AddRoomMeshes(root, cell.openDoors, Globals.biome);
                InstantiatePattern(root, cell.type, cell.openDoors);

                Refs.rooms[cellY,cellX] = room;
            }
        }
        
        Refs.rooms[startCell.y, startCell.x].Enter(Direction.None);
    }

    void InstantiatePattern(Transform parent, RoomType type, bool[] doors) {
        GameObject[] patterns =
            type switch {
                RoomType.Normal   => normalPatterns,
                RoomType.Treasure => treasurePatterns,
                RoomType.Boss     => bossPatterns,
                _                 => null
            };

        if (patterns == null) return;
        if (patterns.Length < 1) return;
        
        string name;
        int layoutIdx = -1;
        int attempts = 1000;

        while (layoutIdx == -1 && attempts-- > 0) {
            layoutIdx = Random.Range(0, patterns.Length);
            name = patterns[layoutIdx].name;
            if (doors[0] && !name.Contains("T")
             || doors[1] && !name.Contains("L")
             || doors[2] && !name.Contains("D")
             || doors[3] && !name.Contains("R"))
            {
                layoutIdx = -1;
            }
        }

        if (layoutIdx == -1) {
            Debug.LogError($"FloorPlanner: Unable to find valid {type} pattern.");
            return;
        }

        Instantiate(patterns[layoutIdx], parent);
    }

    void AddRoomMeshes(Transform room, bool[] doors, Biome biome) {
        // Corners
        Instantiate(biome switch {
                        Biome.Forest => forestCorners,
                        Biome.Temple => templeCorners,
                        _ => null
                    }, room);
        
        // Ground
        Instantiate(biome switch {
                        Biome.Forest => forestGround,
                        Biome.Temple => templeGround,
                        _ => null
                    }, room);

        // Blockers
        GameObject[] blockerMeshes = biome switch {
            Biome.Forest => forestBlockers,
            Biome.Temple => templeBlockers,
            _ => null
        };

        // Doors
        GameObject[] doorMeshes = biome switch {
            Biome.Forest => null,
            Biome.Temple => templeDoors,
            _ => null
        };

        // Add Doors/Blockers
        if (!doors[0]) Instantiate(blockerMeshes[0], room);
        else if (doorMeshes != null) Instantiate(doorMeshes[0], room);

        if (!doors[1]) Instantiate(blockerMeshes[1], room);
        else if (doorMeshes != null) Instantiate(doorMeshes[1], room);

        if (!doors[2]) Instantiate(blockerMeshes[2], room);
        else if (doorMeshes != null) Instantiate(doorMeshes[2], room);

        if (!doors[3]) Instantiate(blockerMeshes[3], room);
        else if (doorMeshes != null) Instantiate(doorMeshes[3], room);
    }
}