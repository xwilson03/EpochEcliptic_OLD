using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Globals globals;

    public void Play()
    {
        globals.fadeController.FadeOut("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
