using UnityEngine;

public class HealthOverlay : MonoBehaviour {
    
    [SerializeField] MeshFilter[] heartObjs;
    [SerializeField] GameObject[] containerObjs;
    [SerializeField] Mesh emptyHeart;
    [SerializeField] Mesh oneHeart;
    [SerializeField] Mesh twoHeart;
    [SerializeField] Mesh threeHeart;
    [SerializeField] Mesh fourHeart;

    void Awake() {
        Util.CheckArray(name, "Heart GameObjects", heartObjs);
        Refs.healthOverlay = this;
    }

    void Start() {
        Refresh();
    }

    public void Refresh() {
        RefreshContainers();
        RefreshHealth();
    }

    void RefreshContainers() {
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

    void RefreshHealth() {
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
}
