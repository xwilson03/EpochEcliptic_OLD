using UnityEngine;

public class GroundHazard : MonoBehaviour {

    public void OnTriggerStay2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;
        other.gameObject.GetComponent<Player>().Damage(1);
    }
}
