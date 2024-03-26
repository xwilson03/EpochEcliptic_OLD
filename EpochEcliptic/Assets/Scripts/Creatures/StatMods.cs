public class StatMods
{
    public enum StatType {
        Health,
        AttackSpeed,
        ReloadSpeed,
        BulletSpeed,
        BulletDuration,
        MovementSpeed,
    };

    public int extraHeartContainers = 0;
    public float reloadSpeedFlat = 0f;
    public float reloadSpeedMult = 1f;
    public float bulletSpeedFlat = 0f;
    public float bulletSpeedMult = 1f;
    public float bulletDurationFlat = 0f;
    public float bulletDurationMult = 1f;
    public int bulletDamageFlat = 0;
    public int bulletDamageMult = 1;
    public float movementSpeedFlat = 0f;
    public float movementSpeedMult = 1f;
}
