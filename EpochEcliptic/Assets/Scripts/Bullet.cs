using UnityEngine;

public class Bullet : MonoBehaviour
{
    bool initialized = false;
    float duration;
    int damage;
    string ownerTag;

    [SerializeField] Rigidbody2D rb;

    void Start()
    {
        // Check serial fields
        if (rb == null) Debug.LogError("Bullet: Missing reference to Rigidbody.");
    }

    void Update()
    {
        // Await member initialization
        if (!initialized) return;

        // Decrease lifetime and destroy object once time expires
        duration -= Time.deltaTime;
        if (duration <= 0.0) Destroy(gameObject);
    }

    public void Initialize(Vector3 velocity, float duration, int damage, string ownerTag, float spawnDistance)
    {
        transform.position += velocity.normalized * spawnDistance;
        rb.velocity = velocity;
        rb.gravityScale = 0.0f;
        
        this.duration = duration;
        this.damage = damage;
        this.ownerTag = ownerTag;

        initialized = true;
    }

    void OnTriggerEnter2D(Collider2D other) {

        // Prevent bullet from damaging owner
        if (other.gameObject.CompareTag(ownerTag)) return;

        // If colliding with creature, decrease their health
        if (other.gameObject.TryGetComponent(out Creature creature)) {
            creature.RemoveHealth(damage);
        }

        // Destroy if colliding with non-bullet
        if (!other.gameObject.CompareTag("Bullet")) {
            Destroy(gameObject);
        }
    }
}
