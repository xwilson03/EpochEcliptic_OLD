using UnityEngine;

public class PlayerDetector : MonoBehaviour {
    
    Pedestal pedestal;

    void Awake() {
        transform.parent.TryGetComponent(out pedestal);
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;
        pedestal.SetOverlayActive(true);
    }

    public void OnTriggerExit2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;
        pedestal.SetOverlayActive(false);
    }
}
