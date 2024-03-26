using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Globals globals;

    enum Direction {none, top, left, down, right};
    [SerializeField] Direction direction;

    void Start() {
        if (direction == Direction.none) Debug.LogError("Door: Direction uninitialized.");

        GameObject.Find("Controllers").TryGetComponent(out globals);
        if (globals == null) Debug.LogError("Door: Unable to acquire reference to Globals.");
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        transform.parent.parent.TryGetComponent(out Room thisRoom);
        if (thisRoom.enemies > 0) return;

        // Move camera and player to new room
        Vector3 cameraDistToMove = Vector3.zero;
        Vector3 playerDistToMove = Vector3.zero;

        switch (direction) {
            case Direction.top:
                globals.rooms[thisRoom.y + 1, thisRoom.x].SetActive(true);
                cameraDistToMove.y += Room.size.y + Room.spacing.y;
                playerDistToMove.y += Room.spacing.y + Player.size + Player.spacing;
                break;
            case Direction.left:
                globals.rooms[thisRoom.y, thisRoom.x - 1].SetActive(true);
                cameraDistToMove.x -= Room.size.x + Room.spacing.x;
                playerDistToMove.x -= Room.spacing.x + Player.size + Player.spacing;
                break;
            case Direction.down:
                globals.rooms[thisRoom.y - 1, thisRoom.x].SetActive(true);
                cameraDistToMove.y -= Room.size.y + Room.spacing.y;
                playerDistToMove.y -= Room.spacing.y + Player.size + Player.spacing;
                break;
            case Direction.right:
                globals.rooms[thisRoom.y, thisRoom.x + 1].SetActive(true);
                cameraDistToMove.x += Room.size.x + Room.spacing.x;
                playerDistToMove.x += Room.spacing.x + Player.size + Player.spacing;
                break;
        }

        globals.player.transform.position += playerDistToMove;
        globals.player.Stop();

        StartCoroutine(globals.cameraController.MoveByXY(cameraDistToMove, 1f));
    }
}
