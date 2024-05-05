using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : Menu {
    
    [SerializeField] Button accessibilityButton;
    [SerializeField] TextMeshProUGUI accessibilityText;

    [SerializeField] Button lessMotionButton;
    [SerializeField] TextMeshProUGUI lessMotionText;
    
    [SerializeField] Slider volumeSlider;

    [SerializeField] Button backButton;

    [SerializeField] Texture2D cursorTexture;

    new void Awake() {
        base.Awake();
        
        Util.CheckReference(name, "Accessibility Button", accessibilityButton);
        Util.CheckReference(name, "Accessibility Text", accessibilityText);
        Util.CheckReference(name, "Less Motion Button", lessMotionButton);
        Util.CheckReference(name, "Less Motion Text", lessMotionText);
        Util.CheckReference(name, "Back Button", backButton);
        Util.CheckReference(name, "Volume Slider", volumeSlider);

        Util.CheckReference(name, "Cursor Texture", cursorTexture);

        accessibilityButton.onClick.AddListener(delegate {ToggleAccessibility();});
        lessMotionButton.onClick.AddListener(delegate    {ToggleLessMotion();});
        backButton.onClick.AddListener(delegate          {MenuController.Back();});
        volumeSlider.onValueChanged.AddListener(delegate {SetVolume();});

        accessibilityText.text = Util.BoolToStatus(Util.GetBool("accessibility", true));
        lessMotionText.text =    Util.BoolToStatus(Util.GetBool("lessMotion", false));

        volumeSlider.normalizedValue = Util.GetFloat("volume", 1f);
        AudioListener.volume = volumeSlider.normalizedValue;

        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void SetVolume() {
        Util.SetFloat("volume", volumeSlider.normalizedValue);
        AudioListener.volume = volumeSlider.normalizedValue;
    }

    public void ToggleLessMotion() {
        Util.ToggleBool("lessMotion");
        lessMotionText.text = Util.BoolToStatus(Util.GetBool("lessMotion"));
    }

    public void ToggleAccessibility() {
        Util.ToggleBool("accessibility");
        accessibilityText.text = Util.BoolToStatus(Util.GetBool("accessibility"));

        UAP_AccessibilityManager.EnableAccessibility(Util.GetBool("accessibility"));
    }
}
