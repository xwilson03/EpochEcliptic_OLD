using System.Collections;
using UnityEngine;

public class Boss : Enemy {

    protected override void Die() {
        transform.parent.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
        base.Die();
    }
    
    protected override IEnumerator Reload() {
        yield return null;
    }

}