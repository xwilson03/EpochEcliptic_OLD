using System.Collections;
using UnityEngine;

public class GoblinBoss : Enemy
{
    new public static float size    = 1.4f;
    new public static float spacing = 0.2f;

    bool busy = false;

    [SerializeField] float attack1Chance = 0.25f;
    [SerializeField] float timeBetweenVolleys = 2f;
    [SerializeField] float chaseChance = 0.5f;
    [SerializeField] float chaseDuration = 3f;
    [SerializeField] float actionCooldown = 1f;

    void Update()
    {
        if (busy) return;
        if (globals.player == null) return;
        
        if (Random.value <= attack1Chance) StartCoroutine(Attack1(timeBetweenVolleys));
        else if (Random.value <= chaseChance) StartCoroutine(ChasePlayer(chaseDuration));
    }

    protected override void Die()
    {
        globals.fadeController.FadeOut("MainMenu");
        base.Die();
    }

    IEnumerator StartCooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        busy = false;
    }

    IEnumerator ChasePlayer(float duration)
    {
        busy = true;

        float timer = 0;
        while (timer < duration) {
            Move(globals.player.transform.position - transform.position);
            timer += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(StartCooldown(actionCooldown));
    }

    IEnumerator Attack1(float timeBetweenVolleys)
    {
        busy = true;

        for (int i = 0; i < 4; i++) {
            ShootPlus();
            yield return new WaitForSeconds(timeBetweenVolleys);
            ShootX();
            yield return new WaitForSeconds(timeBetweenVolleys);
        }

        StartCoroutine(StartCooldown(actionCooldown));
    }

    void ShootPlus()
    {
        Shoot(transform.position + Vector3.up);    canFire = true;
        Shoot(transform.position + Vector3.left);  canFire = true;
        Shoot(transform.position + Vector3.down);  canFire = true;
        Shoot(transform.position + Vector3.right); canFire = true;
    }

    void ShootX()
    {
        Shoot(transform.position + (Vector3.up   + Vector3.left ).normalized); canFire = true;
        Shoot(transform.position + (Vector3.up   + Vector3.right).normalized); canFire = true;
        Shoot(transform.position + (Vector3.down + Vector3.left ).normalized); canFire = true;
        Shoot(transform.position + (Vector3.down + Vector3.right).normalized); canFire = true;
    }
}
