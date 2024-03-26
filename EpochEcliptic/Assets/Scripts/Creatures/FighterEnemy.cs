using UnityEngine;

public class FighterEnemy : Enemy
{
    void Update()
    {
        if (globals.player == null) {
            return;
        }
        
        Move(globals.player.transform.position - transform.position);
    }
}
