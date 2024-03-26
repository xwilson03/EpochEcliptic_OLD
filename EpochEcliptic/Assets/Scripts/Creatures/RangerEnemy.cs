using UnityEngine;

public class RangerEnemy : Enemy
{
    void Update()
    {
        if (globals.player == null) {
            return;
        }
        
        Vector3 playerPos = globals.player.transform.position;
        float distToPlayer = (transform.position - playerPos).magnitude;

        if (distToPlayer > (baseBulletSpeed * baseBulletDuration)) {
            Move(playerPos - transform.position);
        }

        else {
            Shoot(playerPos);
        }
    }
}
