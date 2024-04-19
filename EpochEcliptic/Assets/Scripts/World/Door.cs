using UnityEngine;

public class Door : MonoBehaviour {

    [SerializeField] Direction direction;
    [SerializeField] Room parent;

    void Awake() {
        Util.CheckReference(name, "Parent Room", parent);
        if (direction == Direction.None) Util.Error(name, "Direction uninitialized.");
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (!other.gameObject.CompareTag("Player")) return;
        parent.Exit(direction);
    }
}
