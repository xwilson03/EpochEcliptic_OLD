using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour {
    
    [SerializeField] GameObject cameraObj;

    [SerializeField] Volume volume;
    ColorAdjustments colorAdjustments;
    DepthOfField depthOfField;

    [SerializeField] AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    const float defaultDuration = 1.0f;
    bool moving = false;

    void Awake() {
        Util.CheckReference(name, "Camera Object", cameraObj);
        Util.CheckReference(name, "Global Volume", volume);

        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out depthOfField);
        depthOfField.active = false;

        Refs.cameraController = this;
    }

    public void MoveToXY(Vector2 destination, float duration = defaultDuration) {
        StartCoroutine(MoveToXY_(destination, duration));
    }

    public void MoveToXY(Vector3 destination, float duration = defaultDuration) {
        StartCoroutine(MoveToXY_(new (destination.x, destination.y), duration));
    }

    IEnumerator MoveToXY_(Vector2 destination, float duration) {
        Time.timeScale = 0;
        moving = true;

        Vector3 dest = new (destination.x,
                            destination.y,
                            cameraObj.transform.position.z);
        

        if (PlayerPrefs.GetInt("lessMotion") == 1) {
            cameraObj.transform.position = dest;
        }

        else {
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
        }

        moving = false;
        Time.timeScale = 1;
    }

    public void WaitForCamera(UnityAction after = null) {
        StartCoroutine(WaitForCamera_(after));
    }

    IEnumerator WaitForCamera_(UnityAction after) {
        while (moving) yield return null;
        after?.Invoke();
    }

    public void SetBlur(bool status) {
        colorAdjustments.saturation.value   = status ? -100f : 0f;
        colorAdjustments.postExposure.value = status ? -1f   : 0f;
        depthOfField.active = status;
    }
}
