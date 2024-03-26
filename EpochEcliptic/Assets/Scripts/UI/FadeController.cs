using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeController : MonoBehaviour
{
    const float inDelay_ = 1.0f;
    const float inDuration_ = 1.5f;
    const float outDelay_ = 0.5f;
    const float outDuration_ = 1.5f;
    [SerializeField] Canvas faderCanvas;
    [SerializeField] GameObject fadeImagePrefab;

    void Start() {
        // Check serial fields
        if (faderCanvas     == null) Debug.LogError("FadeController: Missing reference to Fader Canvas.");
        if (fadeImagePrefab == null) Debug.LogError("FadeController: Missing reference to Image Prefab.");

        FadeIn();
    }

    public void FadeIn(float duration = inDuration_, float delay = inDelay_) {
        StartCoroutine(FadeIn_(delay, duration));
    }

    public void FadeOut(string toScene, float duration = outDuration_, float delay = outDelay_) {
        StartCoroutine(FadeOut_(toScene, delay, duration));
    }

    IEnumerator FadeOut_(string toScene, float delay, float duration)
    {
        Time.timeScale = 0;
        Color fadeColor = Color.black;

        Image fadeImage = Instantiate(fadeImagePrefab.GetComponent<Image>(),
                                      faderCanvas.transform);
        fadeImage.color = Color.clear;

        float timer = 0;
        while (timer <= delay) {
            yield return null;
            timer += Time.unscaledDeltaTime;
        }

        timer = 0;
        while (timer <= duration) {
            yield return null;
            timer += Time.unscaledDeltaTime;

            fadeImage.color = new Color (fadeColor.r, fadeColor.g, fadeColor.b,
                                         timer / duration);
        }

        Time.timeScale = 1;
        if (toScene != "") {
            SceneManager.LoadScene(toScene);
        }
    }

    IEnumerator FadeIn_(float delay, float duration) {
        Time.timeScale = 0;
        Color fadeColor = Color.black;

        Image fadeImage = Instantiate(fadeImagePrefab.GetComponent<Image>(),
                                      faderCanvas.transform);
        fadeImage.color = fadeColor;

        float timer = 0;
        while (timer <= delay) {
            yield return null;
            timer += Time.unscaledDeltaTime;
        }

        timer = 0;
        while (timer <= duration) {
            yield return null;
            timer += Time.unscaledDeltaTime;

            fadeImage.color = new Color (fadeColor.r, fadeColor.g, fadeColor.b,
                                         1f - (timer / duration));
        }

        Destroy(fadeImage.gameObject);
        Time.timeScale = 1;
    }
}
