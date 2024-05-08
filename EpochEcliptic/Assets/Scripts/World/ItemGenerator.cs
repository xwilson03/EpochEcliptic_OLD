using UnityEngine;

public class ItemGenerator : MonoBehaviour {
    
    [SerializeField] StatLine min;
    [SerializeField] StatLine max;

    void Awake () {
        Refs.itemGenerator = this;
    }

    public StatLine CreateItem(float baseStrength, float extraStrength, bool defensive = false) {
        StatLine item = new (0, 0);

        int offStatIdx, defStatIdx;
        float offStrength, defStrength;

        // Pick one offensive and defensive stat
        offStatIdx = Random.Range(0,5);
        defStatIdx = Random.Range(0,3);

        offStrength = !defensive ? (baseStrength + extraStrength) : -extraStrength;
        defStrength =  defensive ? (baseStrength + extraStrength) : -extraStrength;

        switch (offStatIdx) {
            case 0:  item.reloadHaste.flat    = Mathf.Lerp(min.reloadHaste.flat,    max.reloadHaste.flat,    Random.value) * offStrength; break;
            case 1:  item.bulletSpeed.flat    = Mathf.Lerp(min.bulletSpeed.flat,    max.bulletSpeed.flat,    Random.value) * offStrength; break;
            case 2:  item.bulletDuration.flat = Mathf.Lerp(min.bulletDuration.flat, max.bulletDuration.flat, Random.value) * offStrength; break;
            case 3:  item.bulletDamage.flat   = Mathf.Lerp(min.bulletDamage.flat,   max.bulletDamage.flat,   Random.value) * offStrength; break;
            default: item.abilityHaste.flat   = Mathf.Lerp(min.abilityHaste.flat,   max.abilityHaste.flat,   Random.value) * offStrength; break;
        }

        switch (defStatIdx) {
            case 0:  item.invincibilityDuration.flat = Mathf.Lerp(min.invincibilityDuration.flat, max.invincibilityDuration.flat, Random.value) * defStrength; break;
            case 1:  item.maxHealth = (defStrength > 0f) ? 4 : -4; break;
            default: item.movementSpeed.flat = Mathf.Lerp(min.movementSpeed.flat, max.movementSpeed.flat, Random.value) * defStrength; break;
        }

        return item;
    }
}
