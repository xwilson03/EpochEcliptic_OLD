using System.Collections;
using UnityEngine;

public class Boss : Enemy {

    protected override IEnumerator Reload() {
        yield return null;
    }

}