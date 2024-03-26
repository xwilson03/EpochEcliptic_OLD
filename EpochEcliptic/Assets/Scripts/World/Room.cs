using UnityEngine;

public class Room : MonoBehaviour
{

    public static Vector2 size    = new (15, 9);
    public static Vector2 spacing = new (4f, 2f);

    public int x, y;
    public int enemies = 0;

    [SerializeField] private GameObject[] doors;
    [SerializeField] private GameObject[] partWalls;
    [SerializeField] private GameObject[] fullWalls;
    public bool[] doorStatuses;

    void Start()
    {
        // Check serial fields
        if (doors     == null) Debug.LogError("Room: Missing reference to Doors.");
        if (partWalls == null) Debug.LogError("Room: Missing reference to Partial Walls.");
        if (fullWalls == null) Debug.LogError("Room: Missing reference to Full Walls.");
    }

    public Room EnableDoors(bool[] statuses)
    {
        for (int segment = 0; segment < 4; segment++) {

            // Get segment status
            bool status = statuses[segment];

            // Enable segment's doors and partial walls
            doors     [segment]        .SetActive(status);
            partWalls [segment * 2]    .SetActive(status);
            partWalls [segment * 2 + 1].SetActive(status);

            // Disable segment's full wall
            fullWalls[segment].SetActive(!status);
        }

        return this;
    }

    public Room SetXY(int x, int y)
    {
        this.x = x;
        this.y = y;

        return this;
    }
}
