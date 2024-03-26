using UnityEngine;

public class GroundHazard : MonoBehaviour
{
    public void OnTriggerStay2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) return;

        other.gameObject.GetComponent<Player>().RemoveHealth(1);
    }
}
