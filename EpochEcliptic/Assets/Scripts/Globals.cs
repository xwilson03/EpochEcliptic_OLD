using UnityEngine;
using UnityEngine.EventSystems;

public class Globals : MonoBehaviour
{
    public InputController inputController;
    public CameraController cameraController;
    public FadeController fadeController;

    public PauseMenu pauseMenu;
    public HealthOverlay healthOverlay;
    public AbilityOverlay abilityOverlay;
    public EventSystem eventSystem;

    public Player player;

    public GameObject[,] rooms;
}
