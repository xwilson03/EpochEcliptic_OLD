using System;
using System.Collections;
using UnityEngine;

public abstract class Creature : MonoBehaviour {
    
    public static float size;
    public static float spacing;

    [Header("References")]
    [SerializeField] protected GameObject bullet;
    protected ObjectPool bulletPool;
    protected Rigidbody2D rb;

    [Header("Stats")]
    [SerializeField] protected StatLine baseStats;
    public StatLine mods;

    // Status
    public int health;
    protected bool isInvincible = false;
    protected bool canFire = true;
    protected bool abilityReady = true;


    protected virtual void Awake() {
        Util.CheckReference(name, "Bullet Prefab", bullet);
        if (baseStats.movementSpeed.flat == 0) Util.Warning(name, "Speed is zero.");

        // Movement and Physics
        if (!TryGetComponent(out rb)) Util.Error(name, "Failed to acquire reference to RigidBody.");
        rb.drag         = 8.0f;
        rb.gravityScale = 0.0f;
    }

    public void Move(Vector2 direction) {
        // Smoothly accelerate to max speed
        float realMovementSpeed = RealMovementSpeed();
        rb.velocity += direction.normalized * (realMovementSpeed * Time.deltaTime);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, realMovementSpeed);
    }
    
    public void Stop() {
        rb.velocity = Vector2.zero;
    }
    
    public void Shoot(Vector3 target) {
        // Prevent firing if creature is unable to
        if (!canFire) return;

        // Calculate bullet travel direction
        Vector3 trajectory = target - transform.position;
        SpawnBullet(trajectory);

        // Start reload timer
        Reload();
    }

    protected void SpawnBullet(Vector3 trajectory) {

        BulletData bulletData = new (
            trajectory.normalized * RealBulletSpeed(),
            size + spacing,
            (int) Math.Ceiling(RealBulletDamage()),
            RealBulletDuration(),
            tag
        );

        bulletPool.Spawn(bulletData);
    }

    protected void Reload() {
        StartCoroutine(Reload_());
    }

    IEnumerator Reload_() {
        canFire = false;
        yield return new WaitForSeconds(1f / Mathf.Exp(0.14f * RealReloadHaste()));
        canFire = true;
    }

    public virtual void Heal(int amount) {
        // Increase health
        health += amount;

        // Limit health to max health
        int realMaxHealth = baseStats.maxHealth + mods.maxHealth;

        if (health > realMaxHealth) {
            health = realMaxHealth;
        }
    }

    public virtual void Damage(int amount) {
        // Prevent damage if creature is invincible
        if (isInvincible) {
            return;
        }

        // Grant temporary invincibility and reduce health
        StartCoroutine(GrantInvincibility());
        health -= amount;
        
        if (health < 1) {
            Die();
        }
    }

    IEnumerator GrantInvincibility() {
        isInvincible = true;
        float realInvincibilityDuration = RealInvincibilityDuration();
        yield return new WaitForSeconds(realInvincibilityDuration);
        isInvincible = false;
    }

    protected virtual void Die() {
        Destroy(gameObject);
    }

    // STATS

    public virtual void AddStats(StatLine stats) {
        mods += stats;
        Heal(stats.maxHealth);
    }

    public int RealMaxHealth() {
        return baseStats.maxHealth + mods.maxHealth;
    }

    public float RealReloadHaste() {
        Stat temp = baseStats.reloadHaste + mods.reloadHaste;
        return temp.flat * temp.multi;
    }

    public float RealBulletSpeed() {
        Stat temp = baseStats.bulletSpeed + mods.bulletSpeed;
        return temp.flat * temp.multi;
    }

    public float RealBulletDuration() {
        Stat temp = baseStats.bulletDuration + mods.bulletDuration;
        return temp.flat * temp.multi;
    }

    public float RealBulletDamage() {
        Stat temp = baseStats.bulletDamage + mods.bulletDamage;
        return temp.flat * temp.multi;
    }

    public float RealMovementSpeed() {
        Stat temp = baseStats.movementSpeed + mods.movementSpeed;
        return temp.flat * temp.multi;
    }

    public float RealAbilityHaste() {
        Stat temp = baseStats.abilityHaste + mods.abilityHaste;
        return temp.flat * temp.multi;
    }

    public float RealAbilityDuration() {
        Stat temp = baseStats.abilityDuration + mods.abilityDuration;
        return temp.flat * temp.multi;
    }

    public float RealInvincibilityDuration() {
        Stat temp = baseStats.invincibilityDuration + mods.invincibilityDuration;
        return temp.flat * temp.multi;
    }
}
