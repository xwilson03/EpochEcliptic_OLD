using UnityEngine;
using UnityEngine.UI;

public class ControlsMenu : Menu {
    
    [SerializeField] Button backButton;

    new void Awake() {
        base.Awake();
        
        Util.CheckReference(name, "Back Button", backButton);

        backButton.onClick.AddListener(delegate {MenuController.Back();});
    }
}
