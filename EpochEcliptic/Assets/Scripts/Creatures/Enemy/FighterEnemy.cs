public class FighterEnemy : Enemy {

    void Update() {
        if (Refs.player == null) {
            return;
        }
        
        Move(Refs.player.transform.position - transform.position);
    }
}
