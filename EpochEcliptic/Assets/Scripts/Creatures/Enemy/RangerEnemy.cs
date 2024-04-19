using UnityEngine;

public class RangerEnemy : Enemy {

    void Update() {
        if (Refs.player == null) {
            return;
        }
        
        Vector3 playerPos = Refs.player.transform.position;
        float distToPlayer = (transform.position - playerPos).magnitude;

        float bulletRange = (baseStats.bulletSpeed.flat + mods.bulletSpeed.flat) * (baseStats.bulletSpeed.multi + mods.bulletSpeed.multi);

        if (distToPlayer > bulletRange) {
            Move(playerPos - transform.position);
        }

        else {
            Shoot(playerPos);
        }
    }
}
