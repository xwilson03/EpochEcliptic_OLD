using TMPro;
using UnityEngine;

public class BossOverlay : MonoBehaviour {

    [SerializeField] TextMeshProUGUI title;

    [Header("Health")]
    [SerializeField] GameObject bossContainer;
    [SerializeField] GameObject left;
    [SerializeField] GameObject[] middles;
    [SerializeField] GameObject right;

    void Awake() {
        Util.CheckReference(name, "Boss Container", bossContainer);
        Util.CheckReference(name, "Boss Left", left);
        Util.CheckArray(name, "Boss Middles", middles);
        Util.CheckReference(name, "Boss Right", right);

        Refs.bossOverlay = this;
    }

    public void SetTitle(string title) {
        this.title.text = title;
    }


    public void Show() {
        title.gameObject.SetActive(true);
        bossContainer.SetActive(true);
    }

    public void Hide() {
        title.gameObject.SetActive(false);
        bossContainer.SetActive(false);
    }

    public void RefreshHealth() {
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
