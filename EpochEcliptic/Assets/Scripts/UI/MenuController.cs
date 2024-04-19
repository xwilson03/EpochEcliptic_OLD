using System.Collections.Generic;
using UnityEngine;

public class MenuController {
    
    static readonly Dictionary<string, Menu> menus = new();
    static readonly Stack<Menu> history = new();
    static bool inMenu;

    static readonly string name = "MenuController";

    public static void RegisterMenu(string menuName, Menu menu) {
        menus[menuName] = menu;
    }

    public static bool InMenu() {
        return inMenu;
    }

    static void Pause() {
        inMenu = true;
        Time.timeScale = 0;
        if (Refs.cameraController != null) Refs.cameraController.SetBlur(true);
    }

    static void Unpause() {
        if (Refs.cameraController != null) Refs.cameraController.SetBlur(false);
        Time.timeScale = 1;
        inMenu = false;
    }

    public static void GoTo(string menuName) {

        if (!menus.ContainsKey(menuName)) {
            Util.Error(name, $"Missing menu {menuName}.");
            return;
        }

        if (!inMenu) Pause();

        Menu newMenu = menus[menuName];

        if (history.Count > 0) history.Peek().Close();
        newMenu.Open();
        history.Push(newMenu);
    }

    public static void Back() {
        Menu currentMenu = history.Pop();
        if (currentMenu != null) currentMenu.Close();

        if (history.Count > 0) history.Peek().Open();
        else Unpause();
    }

    public static void Exit() {
        while (history.Count > 0) Back();
    }

    public static void ClearHistory() {
        history.Clear();
        inMenu = false;
    }
}
