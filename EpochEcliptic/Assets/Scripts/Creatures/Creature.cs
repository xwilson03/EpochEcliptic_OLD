using System.Collections;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    [Header("References")]
    protected Globals globals;
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected Rigidbody2D rb;

    public static float size;
    public static float spacing;

    [Header("Stats")]
    [SerializeField] protected int   baseHeartContainers = 1;
    [SerializeField] protected float baseReloadTime = 0.5f;
    [SerializeField] protected float baseBulletSpeed = 5f;
    [SerializeField] protected float baseBulletDuration = 1f;
    [SerializeField] protected int   baseBulletDamage = 1;
    [SerializeField] protected float baseMovementSpeed = 20;
    [SerializeField] protected float invincibilityDuration = 0.25f;

    // Modifiers
    protected StatMods mods;

    // Status
    protected int health;
    protected bool isInvincible = false;
    protected bool canFire = true;


    protected virtual void Start()
    {
        if (!GameObject.Find("Controllers").TryGetComponent(out globals)){
            Debug.LogError("Creature: Unable to acquire reference to Globals.");
        }

        // Check serial fields
        if (bullet == null) Debug.LogError("Creature: Missing reference to Bullet Prefab.");
        if (rb     == null) Debug.LogError("Creature: Missing reference to Rigidbody.");
        if (baseMovementSpeed == 0) Debug.LogError("Creature: Warning! Speed is zero.");

        mods = new StatMods();

        // Movement and Physics
        rb.drag         = 8.0f;
        rb.gravityScale = 0.0f;

        // Initialize health
        health = baseHeartContainers * 4;
    }

    public void Move(Vector2 direction)
    {
        // Ensure normalization of direction vector
        direction.Normalize();
        
        float realMovementSpeed = (baseMovementSpeed + mods.movementSpeedFlat) * mods.movementSpeedMult;

        // Smoothly accelerate to max speed
        rb.velocity += direction * (realMovementSpeed * Time.deltaTime);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, realMovementSpeed);
    }
    
    public void Stop()
    {
        rb.velocity = Vector2.zero;
    }
    
    public void Shoot(Vector3 target)
    {
        // Prevent firing if creature is unable to
        if (!canFire) return;

        // Calculate bullet travel direction
        Vector3 trajectory = target - transform.position;
        trajectory.Normalize();

        int realBulletDamage = (baseBulletDamage + mods.bulletDamageFlat) * mods.bulletDamageMult;
        float realBulletSpeed = (baseBulletSpeed + mods.bulletSpeedFlat) * mods.bulletSpeedMult;
        float realBulletDuration = (baseBulletDuration + mods.bulletDurationFlat) * mods.bulletDurationMult;

        // Instantiate bullet and set attributes
        GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation);
        newBullet.GetComponent<Bullet>().Initialize(trajectory * realBulletSpeed, realBulletDuration, realBulletDamage, gameObject.tag, size + spacing);

        // Start reload timer
        StartCoroutine(Reload());
    }

    IEnumerator Reload() {
        canFire = false;

        float timer = 0;
        // Calculates reload speed every time in case modifiers change mid-shot
        while (timer <= (baseReloadTime + mods.reloadSpeedFlat) * mods.reloadSpeedMult) {
            timer += Time.deltaTime;
            yield return null;
        }

        canFire = true;
    }

    public virtual void AddHealth(int amount)
    {
        // Increase health
        health += amount;

        // Limit health to max health
        int realMaxHealth = (baseHeartContainers + mods.extraHeartContainers) * 4;

        if (health > realMaxHealth) {
            health = realMaxHealth;
        }
    }

    public virtual void RemoveHealth(int amount)
    {
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
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    protected virtual void Die() {
        Destroy(gameObject);
    }

    // MODIFIERS

    public virtual void AddHeartContainers(int heartContainers)
    {
        mods.extraHeartContainers += heartContainers;
    }

    public virtual void AddReloadSpeed(float flat, float mult)
    {
        mods.reloadSpeedFlat += flat;
        mods.reloadSpeedMult += mult;
    }

    public virtual void AddBulletSpeed(float flat, float mult)
    {
        mods.bulletSpeedFlat += flat;
        mods.bulletSpeedMult += mult;
    }

    public virtual void AddBulletDuration(float flat, float mult)
    {
        mods.bulletDurationFlat += flat;
        mods.bulletDurationMult += mult;
    }

    public virtual void AddBulletDamage(int flat, int mult)
    {
        mods.bulletDamageFlat += flat;
        mods.bulletDamageMult += mult;
    }

    public virtual void AddMovementSpeed(float flat, float mult)
    {
        mods.movementSpeedFlat += flat;
        mods.movementSpeedMult += mult;
    }
}
