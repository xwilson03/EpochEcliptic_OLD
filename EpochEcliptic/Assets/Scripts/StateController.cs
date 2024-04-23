using UnityEngine;
using UnityEngine.SceneManagement;

public class StateController : MonoBehaviour {
    
    [Header("Bullet Pools")]
    [SerializeField] GameObject playerBullet;
    [SerializeField] GameObject enemyBullet;
    [SerializeField] int bulletPoolSize;

    void Awake() {
        MenuController.ClearHistory();
        Util.CheckReference(name, "Player Bullet", playerBullet);
        Util.CheckReference(name, "Enemy Bullet", enemyBullet);
        if (bulletPoolSize == 0) Util.Error(name, "Bullet Pool Size not set.");

        if (Refs.playerBulletPool == null) Refs.playerBulletPool = new (playerBullet, bulletPoolSize);
        else Refs.playerBulletPool.Clear();

        if (Refs.enemyBulletPool == null) Refs.enemyBulletPool = new (enemyBullet,  bulletPoolSize);
        else Refs.enemyBulletPool.Clear();

        
        if (SceneManager.GetActiveScene().name == "MainMenu") {
            MenuController.GoTo("MainMenu");
        }
    }

    void Start() {
        Refs.fader.FadeIn();
    }

    public static void NextFloor() {
        Globals.PrepareNextFloor();
        Refs.fader.FadeTo(Globals.nextScene);
    }
}