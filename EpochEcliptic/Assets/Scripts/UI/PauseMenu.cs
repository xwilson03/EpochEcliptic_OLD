using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Globals globals;

    [SerializeField] GameObject menuCanvas;
    [SerializeField] Volume volume;
    ColorAdjustments screenColor;
    DepthOfField depthOfField;

    public void Start()
    {
        // Check serial fields
        if (menuCanvas  == null) Debug.LogError("PauseMenu: Missing reference to Menu Canvas.");
        if (volume      == null) Debug.LogError("PauseMenu: Missing reference to Global Volume.");

        volume.profile.TryGet(out screenColor);
        volume.profile.TryGet(out depthOfField);

        depthOfField.active = false;
    }

    public void Pause()
    {
        // Stop time
        Time.timeScale = 0;

        // Disable player controls
        globals.inputController.inMenu = true;

        // Enable game blur
        screenColor.saturation.value = -100f;
        screenColor.postExposure.value = -1f;
        depthOfField.active = true;
        
        // Show menu
        menuCanvas.SetActive(true);
    }

    public void Resume()
    {
        // Hide menu and reset event system target
        menuCanvas.SetActive(false);
        // globals.eventSystem.SetSelectedGameObject(null);

        // Enable player controls
        globals.inputController.inMenu = false;

        // Disable game blur
        screenColor.saturation.value = 0f;
        screenColor.postExposure.value = 0f;
        depthOfField.active = false;

        // Resume time
        Time.timeScale = 1;
    }

    public void Options()
    {
        Debug.Log("options");
    }

    public void SaveAndExit()
    {
        // save ??

        globals.fadeController.FadeOut("MainMenu");
    }
}
