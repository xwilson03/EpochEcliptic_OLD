using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : Menu {
    
    [SerializeField] Button accessibilityButton;
    [SerializeField] Button lessMotionButton;
    [SerializeField] Button backButton;

    [SerializeField] TextMeshProUGUI accessibilityText;
    [SerializeField] TextMeshProUGUI lessMotionText;
    [SerializeField] Slider volumeSlider;

    [SerializeField] Texture2D cursorTexture;

    new void Awake() {
        base.Awake();
        
        Util.CheckReference(name, "Accessibility Button", accessibilityButton);
        Util.CheckReference(name, "Less Motion Button", lessMotionButton);
        Util.CheckReference(name, "Back Button", backButton);
        Util.CheckReference(name, "Accessibility Text", accessibilityText);
        Util.CheckReference(name, "Less Motion Text", lessMotionText);
        Util.CheckReference(name, "Volume Slider", volumeSlider);

        Util.CheckReference(name, "Cursor Texture", cursorTexture);

        accessibilityButton.onClick.AddListener(delegate {ToggleAccessibility();});
        lessMotionButton.onClick.AddListener(delegate    {ToggleLessMotion();});
        backButton.onClick.AddListener(delegate          {MenuController.Back();});

        accessibilityText.text =
            "Accessibility:     " + (PlayerPrefs.GetInt("accessibility") == 1 ? "Enabled" : "Disabled");
        lessMotionText.text =
            "Less Motion:        " + (PlayerPrefs.GetInt("lessMotion") == 1 ? "Enabled" : "Disabled");

        volumeSlider.normalizedValue = PlayerPrefs.HasKey("volume") ? PlayerPrefs.GetFloat("volume") : 1f;
        AudioListener.volume = volumeSlider.normalizedValue;

        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void SetVolume() {
        PlayerPrefs.SetFloat("volume", volumeSlider.normalizedValue);
        AudioListener.volume = volumeSlider.normalizedValue;
    }

    public void ToggleLessMotion() {
        PlayerPrefs.SetInt("lessMotion",
            PlayerPrefs.GetInt("lessMotion") == 1 ? 0 : 1);
        lessMotionText.text = "Less Motion:        "
            + (PlayerPrefs.GetInt("lessMotion") == 1 ? "Enabled" : "Disabled");
    }

    public void ToggleAccessibility() {
        PlayerPrefs.SetInt("accessibility",
            PlayerPrefs.GetInt("accessibility") == 1 ? 0 : 1);
        accessibilityText.text =
            "Accessibility:     " + (PlayerPrefs.GetInt("accessibility") == 1 ? "Enabled" : "Disabled");

        UAP_AccessibilityManager.EnableAccessibility(PlayerPrefs.GetInt("accessibility") == 1);
    }
}
