using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilityOverlay : MonoBehaviour
{
    [SerializeField] Image fullImage;

    public void SetChargePercent(float percent)
    {
        fullImage.fillAmount = percent;
    }
}
