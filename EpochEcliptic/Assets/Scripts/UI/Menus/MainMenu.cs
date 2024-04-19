using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu {
    
    [SerializeField] Button playButton;
    [SerializeField] Button controlsButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button quitButton;

    new void Awake() {
        base.Awake();

        Util.CheckReference(name, "Play Button", playButton);
        Util.CheckReference(name, "Controls Button", controlsButton);
        Util.CheckReference(name, "Options Button", optionsButton);
        Util.CheckReference(name, "Quit Button", quitButton);

        playButton.onClick.AddListener(delegate     {Play();});
        controlsButton.onClick.AddListener(delegate {MenuController.GoTo("Controls");});
        optionsButton.onClick.AddListener(delegate  {MenuController.GoTo("Options");});
        quitButton.onClick.AddListener(delegate     {Quit();});
    }

    public void Play() {
        Refs.fader.FadeTo("Game");
    }

    public void Quit() {
        Application.Quit();
    }
}
