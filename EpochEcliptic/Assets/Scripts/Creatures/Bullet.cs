using System.Collections;
using UnityEngine;

public class BulletData {

    public BulletData(
        Vector3 velocity,
        Vector3 startPos,
        float offset,
        int damage,
        float duration,
        string ownerTag)
    {
        this.velocity = velocity;
        this.startPos = startPos;
        this.offset   = offset;
        
        this.damage   = damage;
        this.duration = duration;
        this.ownerTag = ownerTag;
    }

    public readonly Vector3 velocity;
    public readonly Vector3 startPos;
    public readonly float offset;

    public readonly int damage;
    public readonly float duration;
    public readonly string ownerTag;
}

public class Bullet : Poolable {

    int damage;
    string ownerTag;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] AudioSource fireNoise;

    void Awake() {
        Util.CheckReference(name, "RigidBody", rb);
        Util.CheckReference(name, "Fire Noise", fireNoise);
        rb.gravityScale = 0.0f;
    }

    public override void Init<T>(int id, T t_data){
        if (t_data is not BulletData data) {
            Util.Error(name, "Invalid spawn data.");
            return;
        }

        base.Init(id, t_data);

        this.id = id;

        transform.position = data.startPos + (data.velocity.normalized * data.offset);
        rb.velocity = data.velocity;
        
        damage = data.damage;
        ownerTag = data.ownerTag;

        fireNoise.Play();
        StartCoroutine(LiveFor_(data.duration));
    }

    IEnumerator LiveFor_(float duration){
        yield return new WaitForSeconds(duration);
        Die();
    }

    void OnTriggerEnter2D(Collider2D other) {

        // Prevent bullet from damaging owner
        if (other.CompareTag(ownerTag)) return;

        // If colliding with creature, decrease their health
        if (other.TryGetComponent(out Creature creature)) {
            creature.Damage(damage);
        }

        // Destroy if colliding with non-bullet
        if (other.CompareTag("Player") || other.CompareTag("Enemy") || other.CompareTag("Wall")) {
            Die();
        }
    }
}
