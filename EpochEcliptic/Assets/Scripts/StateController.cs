using UnityEngine;
using UnityEngine.SceneManagement;

public class StateController : MonoBehaviour {
    
    void Awake() {
        MenuController.ClearHistory();

        if (SceneManager.GetActiveScene().name == "MainMenu") {
            MenuController.GoTo("MainMenu");
        }
    }

    void Start() {
        Refs.fader.FadeIn();
    }

    public static void NextFloor() {
        Globals.PrepareNextFloor();
        Refs.fader.FadeTo(Globals.nextScene);
    }
}