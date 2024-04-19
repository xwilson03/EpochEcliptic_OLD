using UnityEngine;

public class Hatch : MonoBehaviour {

    void Start() {
        UAP_AccessibilityManager.Say("Hatch unlocked.");
    }

    public void OnTriggerStay2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;
        Refs.player.Heal(1);
        StateController.NextFloor();
    }
}
