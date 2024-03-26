using UnityEngine;

public class HealthOverlay : MonoBehaviour
{
    [SerializeField] MeshFilter[] heartObjs;
    [SerializeField] GameObject[] containerObjs;
    [SerializeField] Mesh emptyHeart;
    [SerializeField] Mesh oneHeart;
    [SerializeField] Mesh twoHeart;
    [SerializeField] Mesh threeHeart;
    [SerializeField] Mesh fourHeart;

    void Start()
    {
        if (heartObjs.Length == 0) Debug.LogError("HealthOverlay: Missing reference to Heart GameObjects");
    }

    public void SetContainers(int containers)
    {
        if (containers < 1) {
            Debug.LogError($"HealthOverlay: SetContainers given container count of {containers}.");
        }

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

    public void SetHealth(int health)
    {
        if (health < 0) {
            health = 0;
        }

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
