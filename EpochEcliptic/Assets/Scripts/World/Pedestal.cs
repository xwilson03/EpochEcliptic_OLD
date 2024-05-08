using TMPro;
using UnityEngine;

public class Pedestal : MonoBehaviour {

    [SerializeField] bool isDefensive = false;
    [SerializeField] bool isChaotic = false;
    public StatLine item;

    GameObject overlay;
    GameObject overlayImageObj;
    GameObject overlayTextObj;
    TextMeshProUGUI overlayTextComp;

    void Awake() {
        item = isChaotic
            ? Refs.itemGenerator.CreateItem(0.5f, Globals.difficulty, isDefensive)
            : Refs.itemGenerator.CreateItem(0.5f, 0f, isDefensive);

        overlay = GameObject.Find($"{gameObject.name}_Overlay");
        overlayImageObj = overlay.transform.GetChild(0).gameObject;
        overlayTextObj = overlay.transform.GetChild(1).gameObject;
        overlayTextComp = overlayTextObj.GetComponent<TextMeshProUGUI>();

        overlayTextComp.text = item.GetActiveStats();
    }

    public void OnTriggerStay2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;

        other.gameObject.GetComponent<Player>().AddStats(item);
        DisableAllPedestals();
    }

    public void SetOverlayActive(bool active) {
        overlayImageObj.SetActive(active);
        overlayTextObj.SetActive(active);

        if (active) {
            string spokenText = overlayTextComp.text
                .Replace("\n",", ")
                .Replace("+", "plus")
                .Replace("-", "minus");
            UAP_AccessibilityManager.SaySkippable(spokenText);
        }

        else {
            UAP_AccessibilityManager.StopSpeaking();
        }
    }

    public void DisableAllPedestals() {
        Transform parent = transform.parent;
        if (!(parent.GetChild(0) == transform))
            parent.GetChild(0).gameObject.SetActive(false);
        if (!(parent.GetChild(1) == transform))
            parent.GetChild(1).gameObject.SetActive(false);
        if (!(parent.GetChild(2) == transform))
            parent.GetChild(2).gameObject.SetActive(false);
        if (!(parent.GetChild(3) == transform))
            parent.GetChild(3).gameObject.SetActive(false);

        gameObject.SetActive(false);
    }
}
