using System.Collections;
using UnityEngine;

public class Player : Creature
{
    new public static float size    = 0.9f;
    new public static float spacing = 0.1f;

    // Stats
    [SerializeField] float abilityDuration = 2f;
    [SerializeField] float abilityCooldown = 5f;

    // Status
    bool abilityReady = true;

    protected override void Start()
    {
        base.Start();
        AddHeartContainers(0);
        AddHealth(0);
    }

    public void ActivateAbility()
    {
        // Prevent ability use before cooldown expires
        if (!abilityReady) {
            return;
        }

        // Temporarily activate player ability
        StartCoroutine(ActivateAbility_());
    }

    IEnumerator ActivateAbility_() {
        abilityReady = false;

        AddReloadSpeed(0, -0.5f);
        AddMovementSpeed(0, 1.5f);

        float timer = 0;
        while (timer <= abilityDuration) {
            timer += Time.deltaTime;
            globals.abilityOverlay.SetChargePercent(1 - (timer / abilityDuration));
            yield return null;
        }

        AddReloadSpeed(0, 0.5f);
        AddMovementSpeed(0, -1.5f);

        timer = 0;
        while (timer <= abilityCooldown) {
            timer += Time.deltaTime;
            globals.abilityOverlay.SetChargePercent(timer / abilityCooldown);
            yield return null;
        }

        abilityReady = true;
    }

    protected override void Die()
    {
        globals.fadeController.FadeOut("MainMenu");
        base.Die();
    }

    public override void AddHealth(int amount)
    {
        // Add health and update UI
        base.AddHealth(amount);
        globals.healthOverlay.SetHealth(health);
    }

    public override void RemoveHealth(int amount)
    {
        // Add health and update UI
        base.RemoveHealth(amount);
        globals.healthOverlay.SetHealth(health);
    }
    
    public override void AddHeartContainers(int hearts)
    {
        // Increase max health and update UI
        base.AddHeartContainers(hearts);
        globals.healthOverlay.SetContainers(baseHeartContainers + mods.extraHeartContainers);
    }  
}
