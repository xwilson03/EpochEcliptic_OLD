using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : Menu {
    
    [SerializeField] Button resumeButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button quitButton;

    new void Awake() {
        base.Awake();

        Util.CheckReference(name, "Resume Button", resumeButton);
        Util.CheckReference(name, "Options Button", optionsButton);
        Util.CheckReference(name, "Quit Button", quitButton);

        resumeButton.onClick.AddListener(delegate  {MenuController.Exit();});
        optionsButton.onClick.AddListener(delegate {MenuController.GoTo("Options");});
        quitButton.onClick.AddListener(delegate    {Refs.fader.FadeTo("MainMenu");});
    }
}
