using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour {

    public static Vector2 size    = new (15, 9);
    public static Vector2 spacing = new (6f, 6f);

    public int x, y;
    int enemies = 0;

    [SerializeField] GameObject root;
    [SerializeField] GameObject[] blockers;
    readonly bool[] doorsExist = new bool[4];

    void Awake() {
        Util.CheckReference(name, "Room Root", root);
        Util.CheckArray(name, "Blockers", blockers);

        root.SetActive(false);
    }

    public void Enter(Direction from) {
        root.SetActive(true);
        Refs.cameraController.MoveToXY(transform.position);

        float playerYDist = (size.y - Player.size - Player.spacing) / 2;
        float playerXDist = (size.x - Player.size - Player.spacing) / 2;

        Vector2 playerOffset = from switch {
            Direction.Top   => new (0, -playerYDist),
            Direction.Left  => new ( playerXDist, 0),
            Direction.Down  => new (0,  playerYDist),
            Direction.Right => new (-playerXDist, 0),
            _ => Vector2.zero
        };

        Refs.player.transform.position = new (
            transform.position.x + playerOffset.x,
            transform.position.y + playerOffset.y,
            Refs.player.transform.position.z
        );

        UAP_AccessibilityManager.Say(((enemies == 0) ? "No" : $"{enemies}") + " enemies.");
        if (IsOpen()) SayDoors();
    }

    public void Exit(Direction to) {
        if (!IsOpen()) return;

        Room next = to switch {
            Direction.Top   => Refs.rooms[y+1,x],
            Direction.Left  => Refs.rooms[y,x-1],
            Direction.Down  => Refs.rooms[y-1,x],
            Direction.Right => Refs.rooms[y,x+1],
            _ => null
        };

        if (next != null) {
            UAP_AccessibilityManager.Say(to switch {
                Direction.Top   => "Went Up",
                Direction.Left  => "Went Left",
                Direction.Down  => "Went Down",
                Direction.Right => "Went Right",
                _ => ""
            });
            next.Enter(to);
            Refs.cameraController.WaitForCamera(delegate {
                root.SetActive(false);
            });
        }
    }

    public void RegisterEnemy(Enemy enemy) {
        enemies++;
        Close();
    }

    public void RemoveEnemy(Enemy enemy) {
        enemies--;
        UAP_AccessibilityManager.Say($"{enemies} left.");
        if (enemies < 1) Open();
    }

    bool IsOpen() {
        return enemies < 1;
    }

    void Open() {
        SayDoors();
    }

    void Close() {
    }

    public void SetDoors(bool[] statuses) {
        for (int i = 0; i < 4; i++) {
            doorsExist[i] = statuses[i];
            blockers[i].SetActive(!doorsExist[i]);
        }
    }

    void SayDoors() {
        List<string> doorStrings = new ();
        if (doorsExist[0]) doorStrings.Add("Up");
        if (doorsExist[1]) doorStrings.Add("Left");
        if (doorsExist[2]) doorStrings.Add("Down");
        if (doorsExist[3]) doorStrings.Add("Right");
        string suffix = " open.";

        UAP_AccessibilityManager.Say(string.Join(", ", doorStrings) + suffix);
    }
}
