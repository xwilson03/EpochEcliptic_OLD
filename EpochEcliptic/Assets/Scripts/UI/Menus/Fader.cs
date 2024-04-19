using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Fader : MonoBehaviour {
    
    const float inDelay = 1.0f;
    const float inDuration = 1.0f;
    const float outDelay = 0.5f;
    const float outDuration = 1.0f;

    [SerializeField] GameObject root;
    [SerializeField] Image faderImage;

    void Awake() {
        Util.CheckReference(name, "Fader Image", faderImage);
        Refs.fader = this;
    }

    public void FadeIn(float duration = inDuration, float delay = inDelay) {
        StartCoroutine(FadeIn_(delay, duration));
    }

    public void FadeTo(string toScene, float duration = outDuration, float delay = outDelay) {
        StartCoroutine(FadeTo_(toScene, delay, duration));
    }

    IEnumerator FadeTo_(string toScene, float delay, float duration) {
        Time.timeScale = 0;

        Color fadeColor = Color.black;
        faderImage.color = Color.clear;

        root.SetActive(true);
        yield return new WaitForSecondsRealtime(delay);

        float timer = 0;
        while (timer <= duration) {
            yield return null;
            timer += Time.unscaledDeltaTime;

            faderImage.color = new Color (fadeColor.r, fadeColor.g, fadeColor.b, timer / duration);
        }

        SceneManager.LoadScene(toScene);
    }

    IEnumerator FadeIn_(float delay, float duration) {
        Time.timeScale = 0;

        Color fadeColor = Color.black;
        faderImage.color = fadeColor;
        root.SetActive(true);

        yield return new WaitForSecondsRealtime(delay);

        float timer = 0;
        while (timer <= duration) {
            yield return null;
            timer += Time.unscaledDeltaTime;

            faderImage.color = new Color (fadeColor.r, fadeColor.g, fadeColor.b,
                                         1f - (timer / duration));
        }

        if (!MenuController.InMenu()) Time.timeScale = 1;
        root.SetActive(false);
    }
}
