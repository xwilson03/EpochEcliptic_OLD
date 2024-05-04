using UnityEngine;

public class Refs {
    
    public static CameraController cameraController;

    public static Fader fader;
    public static HealthOverlay healthOverlay;
    public static AbilityOverlay abilityOverlay;
    public static BossOverlay bossOverlay;

    public static ItemGenerator itemGenerator;

    public static Player player;
    public static Creature boss;
    public static Room[,] rooms;

    public static ObjectPool playerBulletPool;
    public static GameObject playerBullet;
    public static ObjectPool enemyBulletPool;
    public static GameObject enemyBullet;
}
