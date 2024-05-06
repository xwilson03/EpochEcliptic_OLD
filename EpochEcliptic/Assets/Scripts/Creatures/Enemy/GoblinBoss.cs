using System.Collections;
using UnityEngine;

public class GoblinBoss : Boss {

    new public static float size    = 1.4f;
    new public static float spacing = 0.2f;

    bool busy = false;

    [SerializeField] float timeBetweenVolleys = 2f;
    [SerializeField] float chaseDuration = 3f;
    [SerializeField] float actionCooldown = 1f;

    int phase = 0;

    void Update() {
        if (busy) return;
        if (Refs.player == null) return;
        
        if (phase++ == 3) {
            Attack();
            phase = 0;
        }
        else ChasePlayer();
    }

    IEnumerator StartCooldown(float duration) {
        yield return new WaitForSeconds(duration);
        busy = false;
    }

    void ChasePlayer() {
        StartCoroutine(ChasePlayer_(chaseDuration));
    }

    IEnumerator ChasePlayer_(float duration) {
        busy = true;

        float timer = 0;
        while (timer < duration) {
            Move(Refs.player.transform.position - transform.position);
            timer += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(StartCooldown(actionCooldown));
    }

    void Attack() {
        StartCoroutine(Attack_(timeBetweenVolleys));
    }

    IEnumerator Attack_(float timeBetweenVolleys) {
        busy = true;

        int numSpiralArms = 8;
        int numVolleys = 6;

        for (float i = 0; i < numVolleys; i++) {
            ShootRing(numSpiralArms, 360f / numVolleys * i, true);
            yield return new WaitForSeconds(timeBetweenVolleys);
        }

        yield return new WaitForSeconds(timeBetweenVolleys * 2);

        for (float i = numVolleys; i >= 0; i--) {
            ShootRing(numSpiralArms, 360f / numVolleys * i, true);
            yield return new WaitForSeconds(timeBetweenVolleys);
        }

        StartCoroutine(StartCooldown(actionCooldown));
    }
}
