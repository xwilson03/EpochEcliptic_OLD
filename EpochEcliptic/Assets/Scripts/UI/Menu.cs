using UnityEngine;

public class Menu : MonoBehaviour {

    [SerializeField] protected GameObject root;

    protected void Awake() {
        Util.CheckReference(name, "Menu Root", root);
        MenuController.RegisterMenu(name, this);
    }

    public void Open() {
        OnOpen();
        root.SetActive(true);
    }

    public void Close() {
        OnClose();
        root.SetActive(false);
    }

    protected virtual void OnOpen() {}
    protected virtual void OnClose() {}
}
