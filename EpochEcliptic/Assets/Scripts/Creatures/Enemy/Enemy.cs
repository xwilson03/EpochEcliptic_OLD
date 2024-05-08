using UnityEngine;

public class Enemy : Creature {
    
    Room parentRoom;

    new protected float size = 0.9f;
    new protected float spacing = 0.1f;

    protected override void Awake() {
        base.Awake();
        
        transform.parent.parent.parent.TryGetComponent(out parentRoom);
        Util.CheckReference(name, "Parent Room", parentRoom);
        parentRoom.RegisterEnemy(this);
    }

    void Start() {
        bulletPool = Refs.enemyBulletPool;
    }

    public override void Move(Vector2 direction) {
        base.Move(direction);
        Face(direction);
    }

    protected void ShootRing(int count, float angle, bool force = false) {
        if (!force && !canFire) return;

        Quaternion spacing = Quaternion.Euler(0, 0, 360f / count);
        Vector3 trajectory = Quaternion.Euler(0, 0, angle) * Vector3.up;

        for (int i = 0; i < count; i++) {
            SpawnBullet(trajectory);
            trajectory = spacing * trajectory;
        }

        Reload();
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
