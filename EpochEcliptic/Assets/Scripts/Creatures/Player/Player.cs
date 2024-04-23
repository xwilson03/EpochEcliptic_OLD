using System.Collections;
using UnityEngine;

public class Player : Creature {

    new public static float size    = 0.9f;
    new public static float spacing = 0.1f;
    [SerializeField] StatLine abilityMods;

    protected override void Awake() {
        base.Awake();

        if (Globals.playerMods != null) mods += Globals.playerMods;
        if (Globals.playerHealth != 0) health = Globals.playerHealth;

        Refs.player = this;
    }

    void Start() {
        Refs.healthOverlay.Refresh();
        bulletPool = Refs.playerBulletPool;

        UAP_AccessibilityManager.Say($"{health} of " + RealMaxHealth() + "health left.");
        UAP_AccessibilityManager.SaySkippable("Ability ready.");
    }

    public void ActivateAbility() {
        // Prevent ability use before cooldown expires
        if (!abilityReady) {
            return;
        }

        // Temporarily activate player ability
        StartCoroutine(ActivateAbility_());
    }

    IEnumerator ActivateAbility_() {
        abilityReady = false;

        float realAbilityDuration = RealAbilityDuration();
        float realAbilityHaste = RealAbilityHaste();
        float realAbilityCooldown = 2.5f / Mathf.Exp(0.14f * realAbilityHaste);

        AddStats(abilityMods);
        UAP_AccessibilityManager.SaySkippable("Ability used.");

        float timer = 0;
        while (timer <= realAbilityDuration) {
            timer += Time.deltaTime;
            Refs.abilityOverlay.SetChargePercent(1 - (timer / realAbilityDuration));
            yield return null;
        }

        AddStats(-abilityMods);

        timer = 0;
        while (timer <= realAbilityCooldown) {
            timer += Time.deltaTime;
            Refs.abilityOverlay.SetChargePercent(timer / realAbilityCooldown);
            yield return null;
        }

        abilityReady = true;
        UAP_AccessibilityManager.SaySkippable("Ability ready.");
    }

    protected override void Die() {
        MenuController.GoTo("GameOver");
        base.Die();
    }

    public override void Heal(int amount) {
        int oldHealth = health;

        // Add health and update UI
        base.Heal(amount);
        Refs.healthOverlay.Refresh();
        
        if (oldHealth != health)
            UAP_AccessibilityManager.Say($"{health} health left.");
    }

    public override void Damage(int amount) {
        int oldHealth = health;

        // Remove health if not invulnerable and update UI
        base.Damage(amount);
        Refs.healthOverlay.Refresh();

        if (oldHealth != health)
            UAP_AccessibilityManager.Say($"{health} health left.");
    }
    
    public override void AddStats(StatLine stats) {
        // Apply mods and refresh health UI if needed
        base.AddStats(stats);
        if (stats.maxHealth != 0) {
            Refs.healthOverlay.Refresh();
        }
    }  
}
