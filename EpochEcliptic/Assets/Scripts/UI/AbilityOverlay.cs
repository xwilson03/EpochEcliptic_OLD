using UnityEngine;
using UnityEngine.UI;

public class AbilityOverlay : MonoBehaviour {
    
    [SerializeField] Image fullImage;

    void Awake() {
        Util.CheckReference(name, "Overlay Image", fullImage);
        Refs.abilityOverlay = this;
    }

    public void SetChargePercent(float percent) {
        fullImage.fillAmount = percent;
    }
}
