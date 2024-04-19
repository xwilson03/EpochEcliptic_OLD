using UnityEngine;

public class InputController : MonoBehaviour {
    
    void Awake() {
        MenuController.Exit();
    }

    void Update() {
        // Open Pause Menu
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!MenuController.InMenu()) MenuController.GoTo("Pause");
            else MenuController.Exit();
        }

        // Get desired direction from keyboard input
        Vector2 desired = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) desired.y += 1;
        if (Input.GetKey(KeyCode.S)) desired.y -= 1;
        if (Input.GetKey(KeyCode.A)) desired.x -= 1;
        if (Input.GetKey(KeyCode.D)) desired.x += 1;

        // Move player (function normalizes vector)
        if (desired != Vector2.zero) {
            Refs.player.Move(desired);
        }

        // Fire if left mouse button is held
        if (Input.GetMouseButton(0)) {
            
            // Get mouse location in game world (adjust Z value to prevent view clipping)
            Vector3 realMousePos = Input.mousePosition;
            realMousePos.z = -Camera.main.transform.position.z;
            realMousePos = Camera.main.ScreenToWorldPoint(realMousePos);
            
            Refs.player.Shoot(realMousePos);
        }

        // Activate ability if right mouse button or space held
        if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.Space)) {
            Refs.player.ActivateAbility();
        }
    }
}
