using UnityEngine;
using UnityEngine.SceneManagement;

public class StateController : MonoBehaviour {
    
    [Header("Bullet Pools")]
    [SerializeField] GameObject playerBullet;
    [SerializeField] GameObject enemyBullet;
    [SerializeField] int playerBulletPoolSize;
    [SerializeField] int enemyBulletPoolSize;

    void Awake() {
        MenuController.ClearHistory();
        
        if (SceneManager.GetActiveScene().name == "MainMenu") {
            MenuController.GoTo("MainMenu");
        }

        if (SceneManager.GetActiveScene().name == "Game") {

            Util.CheckReference(name, "Player Bullet", playerBullet);
            Util.CheckReference(name, "Enemy Bullet", enemyBullet);
            if (playerBulletPoolSize == 0) Util.Error(name, "Player Bullet Pool Size not set.");
            if (enemyBulletPoolSize == 0) Util.Error(name, "Enemy Bullet Pool Size not set.");

            if (Refs.playerBulletPool == null) Refs.playerBulletPool = new (playerBullet, playerBulletPoolSize);
            else Refs.playerBulletPool.Clear();

            if (Refs.enemyBulletPool == null) Refs.enemyBulletPool = new (enemyBullet, enemyBulletPoolSize);
            else Refs.enemyBulletPool.Clear();
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