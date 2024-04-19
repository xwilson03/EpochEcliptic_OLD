using System.Collections;
using UnityEngine;

public class GoblinBoss : Boss {

    new public static float size    = 1.4f;
    new public static float spacing = 0.2f;

    bool busy = false;

    [SerializeField] float attackChance = 0.25f;
    [SerializeField] float timeBetweenVolleys = 2f;
    [SerializeField] float chaseChance = 0.5f;
    [SerializeField] float chaseDuration = 3f;
    [SerializeField] float actionCooldown = 1f;

    void Update() {
        if (busy) return;
        if (Refs.player == null) return;
        
        if (Random.value <= attackChance) StartCoroutine(Attack(timeBetweenVolleys));
        else if (Random.value <= chaseChance) StartCoroutine(ChasePlayer(chaseDuration));
    }

    IEnumerator StartCooldown(float duration) {
        yield return new WaitForSeconds(duration);
        busy = false;
    }

    IEnumerator ChasePlayer(float duration) {
        busy = true;

        float timer = 0;
        while (timer < duration) {
            Move(Refs.player.transform.position - transform.position);
            timer += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(StartCooldown(actionCooldown));
    }

    IEnumerator Attack(float timeBetweenVolleys) {
        busy = true;

        for (int i = 0; i < 4; i++) {
            ShootRing(8, 0, true);
            yield return new WaitForSeconds(timeBetweenVolleys);
            ShootRing(8, 27.5f, true);
            yield return new WaitForSeconds(timeBetweenVolleys);
        }

        StartCoroutine(StartCooldown(actionCooldown));
    }
}
