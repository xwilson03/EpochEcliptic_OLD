using UnityEngine;

public class Enemy : Creature {
    
    Room parentRoom;

    new protected float size = 0.9f;
    new protected float spacing = 0.1f;

    protected override void Awake() {
        base.Awake();
        transform.parent.parent.parent.TryGetComponent(out parentRoom);
        Util.CheckReference(name, "Parent Room", parentRoom);
    }

    void Start() {
        parentRoom.RegisterEnemy(this);
    }

    protected override void Die() {
        parentRoom.RemoveEnemy(this);
        base.Die();
    }

    void OnCollisionStay2D (Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<Creature>().Damage(1);
        }
    }
}
