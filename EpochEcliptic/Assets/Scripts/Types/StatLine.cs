using System;

[Serializable]
public class StatLine {

    public static StatLine zero = new (0,0);

    public int maxHealth = 0;

    // "Haste":
    // 5 haste -> 50%, 10 haste -> 75%, ...
    // 1 / e^(.14x)

    public Stat reloadHaste;
    public Stat bulletSpeed;
    public Stat bulletDuration;
    public Stat bulletDamage;
    public Stat movementSpeed;
    public Stat abilityHaste;
    public Stat abilityDuration;
    public Stat invincibilityDuration;

    public StatLine(float flat = 0, float multi = 1) {
        reloadHaste           = new (flat, multi);
        bulletSpeed           = new (flat, multi);
        bulletDuration        = new (flat, multi);
        bulletDamage          = new (flat, multi);
        movementSpeed         = new (flat, multi);
        abilityHaste          = new (flat, multi);
        abilityDuration       = new (flat, multi);
        invincibilityDuration = new (flat, multi);
    }

    public override string ToString() {
        string outString = "";

        if (maxHealth             != 0)         outString += $"maxHealth:\t\t{maxHealth}\n";
        if (reloadHaste           != Stat.zero) outString += $"reloadHaste:\t\t{reloadHaste}\n";
        if (bulletSpeed           != Stat.zero) outString += $"bulletSpeed:\t\t{bulletSpeed}\n";
        if (bulletDuration        != Stat.zero) outString += $"bulletDuration:\t\t{bulletDuration}\n";
        if (bulletDamage          != Stat.zero) outString += $"bulletDamage:\t\t{bulletDamage}\n";
        if (movementSpeed         != Stat.zero) outString += $"movementSpeed:\t{movementSpeed}\n";
        if (abilityHaste          != Stat.zero) outString += $"abilityHaste:\t{abilityHaste}\n";
        if (abilityDuration       != Stat.zero) outString += $"abilityDuration:\t\t{abilityDuration}\n";
        if (invincibilityDuration != Stat.zero) outString += $"invincibilityDuration:\t{invincibilityDuration}\n";

        return outString;
    }

    public string GetActiveStats() {
        string outString = "";

        if (maxHealth != 0)
            { outString += ((maxHealth > 0) ? "+" : "-") + " Vigor\n"; }

        if (reloadHaste != Stat.zero)
            { outString += ((reloadHaste.flat > 0) ? "+" : "-") + " Reload\n"; }

        if (bulletSpeed != Stat.zero)
            { outString += ((bulletSpeed.flat > 0) ? "+" : "-") + " Velocity\n"; }

        if (bulletDuration != Stat.zero)
            { outString += ((bulletDuration.flat > 0) ? "+" : "-") + " Range\n"; }

        if (bulletDamage != Stat.zero)
            { outString += ((bulletDamage.flat > 0) ? "+" : "-") + " Damage\n"; }

        if (movementSpeed != Stat.zero)
            { outString += ((movementSpeed.flat > 0) ? "+" : "-") + " Speed\n"; }

        if (abilityHaste != Stat.zero)
            { outString += ((abilityHaste.flat > 0) ? "+" : "-") + " Recovery\n"; }

        if (abilityDuration != Stat.zero)
            { outString += ((abilityDuration.flat > 0) ? "+" : "-") + " Stamina\n"; }

        if (invincibilityDuration != Stat.zero)
            { outString += ((invincibilityDuration.flat > 0) ? "+" : "-") + " Guts\n"; }

        return outString;
    }

    public static StatLine operator +(StatLine lhs, StatLine rhs) {
        return new StatLine {
            maxHealth             = lhs.maxHealth             + rhs.maxHealth,
            reloadHaste           = lhs.reloadHaste           + rhs.reloadHaste,
            bulletSpeed           = lhs.bulletSpeed           + rhs.bulletSpeed,
            bulletDuration        = lhs.bulletDuration        + rhs.bulletDuration,
            bulletDamage          = lhs.bulletDamage          + rhs.bulletDamage,
            movementSpeed         = lhs.movementSpeed         + rhs.movementSpeed,
            abilityHaste          = lhs.abilityHaste          + rhs.abilityHaste,
            abilityDuration       = lhs.abilityDuration       + rhs.abilityDuration,
            invincibilityDuration = lhs.invincibilityDuration + rhs.invincibilityDuration
        };
    }

    public static StatLine operator -(StatLine lhs, StatLine rhs) {
        return new StatLine {
            maxHealth             = lhs.maxHealth             - rhs.maxHealth,
            reloadHaste           = lhs.reloadHaste           - rhs.reloadHaste,
            bulletSpeed           = lhs.bulletSpeed           - rhs.bulletSpeed,
            bulletDuration        = lhs.bulletDuration        - rhs.bulletDuration,
            bulletDamage          = lhs.bulletDamage          - rhs.bulletDamage,
            movementSpeed         = lhs.movementSpeed         - rhs.movementSpeed,
            abilityHaste          = lhs.abilityHaste          - rhs.abilityHaste,
            abilityDuration       = lhs.abilityDuration       - rhs.abilityDuration,
            invincibilityDuration = lhs.invincibilityDuration - rhs.invincibilityDuration
        };
    }

    public static StatLine operator -(StatLine lhs) {
        return new StatLine {
            maxHealth             = -lhs.maxHealth,
            reloadHaste           = -lhs.reloadHaste,
            bulletSpeed           = -lhs.bulletSpeed,
            bulletDuration        = -lhs.bulletDuration,
            bulletDamage          = -lhs.bulletDamage,
            movementSpeed         = -lhs.movementSpeed,
            abilityHaste          = -lhs.abilityHaste,
            abilityDuration       = -lhs.abilityDuration,
            invincibilityDuration = -lhs.invincibilityDuration
        };
    }

    public static bool operator == (StatLine lhs, StatLine rhs) {
        if (lhs is null ^ rhs is null) return false;
        if (lhs is null) return true;

        return lhs.maxHealth             == rhs.maxHealth
            && lhs.reloadHaste           == rhs.reloadHaste
            && lhs.bulletSpeed           == rhs.bulletSpeed
            && lhs.bulletDuration        == rhs.bulletDuration
            && lhs.bulletDamage          == rhs.bulletDamage
            && lhs.movementSpeed         == rhs.movementSpeed
            && lhs.abilityHaste          == rhs.abilityHaste
            && lhs.abilityDuration       == rhs.abilityDuration
            && lhs.invincibilityDuration == rhs.invincibilityDuration;
    }

    public static bool operator != (StatLine lhs, StatLine rhs) {
        if (lhs is null ^ rhs is null) return true;
        if (lhs is null) return false;

        return lhs.maxHealth             != rhs.maxHealth
            || lhs.reloadHaste           != rhs.reloadHaste
            || lhs.bulletSpeed           != rhs.bulletSpeed
            || lhs.bulletDuration        != rhs.bulletDuration
            || lhs.bulletDamage          != rhs.bulletDamage
            || lhs.movementSpeed         != rhs.movementSpeed
            || lhs.abilityHaste          != rhs.abilityHaste
            || lhs.abilityDuration       != rhs.abilityDuration
            || lhs.invincibilityDuration != rhs.invincibilityDuration;
    }

    public override bool Equals(object obj) {
        StatLine rhs = obj as StatLine;
        return (rhs is not null) && (this == rhs);
    }
}
