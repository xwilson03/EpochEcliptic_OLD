using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathMenu : Menu {
    
    [SerializeField] TextMeshProUGUI levelReachedText;
    [SerializeField] Button mainMenuButton;
    [SerializeField] Button desktopButton;

    new void Awake() {
        base.Awake();

        Util.CheckReference(name, "Level Reached Text", levelReachedText);
        Util.CheckReference(name, "Main Menu Button", mainMenuButton);
        Util.CheckReference(name, "Desktop Button", desktopButton);

        mainMenuButton.onClick.AddListener(delegate {MainMenu();});
        desktopButton.onClick.AddListener(delegate {Desktop();});
    }

    protected override void OnOpen() {
        levelReachedText.text = $"{Globals.biome} {Globals.level}";
    }

    public void MainMenu() {
        Refs.fader.FadeTo("MainMenu");
    }

    public void Desktop() {
        Application.Quit();
    }
}
