using UnityEngine;

public class HealthOverlay : MonoBehaviour {
    
    [Header("Player Health")]
    [SerializeField] MeshFilter[] heartObjs;
    [SerializeField] GameObject[] containerObjs;
    [SerializeField] Mesh emptyHeart;
    [SerializeField] Mesh oneHeart;
    [SerializeField] Mesh twoHeart;
    [SerializeField] Mesh threeHeart;
    [SerializeField] Mesh fourHeart;

    [Header("Boss Health")]
    [SerializeField] GameObject bossContainer;
    [SerializeField] GameObject left;
    [SerializeField] GameObject[] middles;
    [SerializeField] GameObject right;

    void Awake() {
        Util.CheckArray(name, "Player Hearts", heartObjs);
        Util.CheckArray(name, "Player Containers", containerObjs);
        Util.CheckReference(name, "Player OneQ Mesh", oneHeart);
        Util.CheckReference(name, "Player TwoQ Mesh", twoHeart);
        Util.CheckReference(name, "Player ThreeQ Mesh", threeHeart);
        Util.CheckReference(name, "Player Full Mesh", fourHeart);

        Util.CheckReference(name, "Boss Container", bossContainer);
        Util.CheckReference(name, "Boss Left", left);
        Util.CheckArray(name, "Boss Middles", middles);
        Util.CheckReference(name, "Boss Right", right);

        Refs.healthOverlay = this;
    }

    void Start() {
        RefreshPlayer();
    }

    public void RefreshPlayer() {
        RefreshPlayerContainers();
        RefreshPlayerHealth();
    }

    public void RefreshBoss() {
        RefreshBossHealth();
    }

    void RefreshPlayerContainers() {
        int containers = Refs.player.RealMaxHealth() / 4;

        foreach (var container in containerObjs) {
            if (containers > 0) {
                container.SetActive(true);
                containers--;
            }

            else {
                container.SetActive(false);
            }
        }
    }

    void RefreshPlayerHealth() {
        int health = Refs.player.health;

        foreach (var heart in heartObjs) {
            switch (health) {
                case 0:
                    heart.sharedMesh = null;
                    continue;

                case 1:
                    heart.sharedMesh = oneHeart;
                    health -= 1;
                    continue;

                case 2:
                    heart.sharedMesh = twoHeart;
                    health -= 2;
                    continue;

                case 3:
                    heart.sharedMesh = threeHeart;
                    health -= 3;
                    continue;

                default:
                    heart.sharedMesh = fourHeart;
                    health -= 4;
                    continue;
            }
        }
    }

    public void ShowBossHealth() {
        bossContainer.SetActive(true);
    }

    public void HideBossHealth() {
        bossContainer.SetActive(false);
    }

    void RefreshBossHealth() {
        float bossHealth = ((float) Refs.boss.health) / Refs.boss.RealMaxHealth();
        float sectionPct = 1f / (middles.Length + 2);
        int numMiddles = Mathf.CeilToInt(bossHealth / sectionPct) - 1;
        if (numMiddles < 0) numMiddles = 0;

        for (int i = numMiddles; i < middles.Length; i++) {
            if (!middles[i].activeInHierarchy) break;
            middles[i].SetActive(false);
        }
    }
}
