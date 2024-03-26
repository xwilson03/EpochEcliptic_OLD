using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] Globals globals;

    public bool inMenu = false;

    void Update()
    {
        // Toggle Control Scheme
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if      (!inMenu) globals.pauseMenu.Pause();
            else if (inMenu) globals.pauseMenu.Resume();
        }

        // Disable player controls if menu is open
        if (!inMenu) {

            if (globals.player == null) return;

            // Get desired direction from keyboard input
            Vector2 desired = Vector2.zero;

            if (Input.GetKey(KeyCode.W)) desired.y += 1;
            if (Input.GetKey(KeyCode.S)) desired.y -= 1;
            if (Input.GetKey(KeyCode.A)) desired.x -= 1;
            if (Input.GetKey(KeyCode.D)) desired.x += 1;

            // Move player (function normalizes vector)
            if (desired != Vector2.zero) {
                globals.player.Move(desired);
            }

            // Fire if left mouse button is held
            if (Input.GetMouseButton(0)) {
                
                // Get mouse location in game world (adjust Z value to prevent view clipping)
                Vector3 realMousePos = Input.mousePosition;
                realMousePos.z = -Camera.main.transform.position.z;
                realMousePos = Camera.main.ScreenToWorldPoint(realMousePos);
                
                globals.player.Shoot(realMousePos);
            }

            // Activate ability if right mouse button held
            if (Input.GetMouseButton(1)) {
                globals.player.ActivateAbility();
            }
        }
    }
}
