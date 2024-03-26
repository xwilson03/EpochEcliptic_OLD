using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] GameObject cameraObj;

    [SerializeField] AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    void Start()
    {
        if (cameraObj == null) Debug.LogError("CameraController: Missing reference to Camera Object.");
    }

    public IEnumerator MoveByXY(Vector2 distance, float duration)
    {
        Time.timeScale = 0;

        Vector3 dest = new (cameraObj.transform.position.x + distance.x,
                            cameraObj.transform.position.y + distance.y,
                            cameraObj.transform.position.z);
        
        float timer = 0;

        while (timer <= duration){
            timer += Time.unscaledDeltaTime;
            cameraObj.transform.position =
                Vector3.Lerp(
                    cameraObj.transform.position,
                    dest,
                    Mathf.SmoothStep(0, 1, curve.Evaluate(timer / duration))
                );
            
            if (cameraObj.transform.position == dest) break;
            yield return null;
        }

        Time.timeScale = 1;
    }
}
