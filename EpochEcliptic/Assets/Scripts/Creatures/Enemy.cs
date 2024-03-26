using UnityEngine;

public class Enemy : Creature
{
    Room thisRoom;

    new protected float size = 0.9f;
    new protected float spacing = 0.1f;

    protected override void Start()
    {
        base.Start();
        transform.parent.TryGetComponent(out thisRoom);
        thisRoom.enemies++;
    }

    protected override void Die()
    {
        thisRoom.enemies--;
        base.Die();
    }

    void OnCollisionStay2D (Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<Creature>().RemoveHealth(1);
        }
    }
}
