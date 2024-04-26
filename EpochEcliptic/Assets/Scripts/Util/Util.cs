using UnityEngine;

public class Util {

    public static void Log(string name, string message){
        Debug.Log($"{name}: {message}");
    }

    public static void Warning(string name, string message){
        Debug.LogWarning($"{name}: {message}");
    }

    public static void Error(string name, string message){
        Debug.LogError($"{name}: {message}");
    }

    public static void CheckReference(string parentName, string objName, object obj) {
        if (obj == null) Error(parentName, $"Missing reference to {objName}.");
    }

    public static void CheckArray(string parentName, string arrayName, object[] array) {
        CheckReference(parentName, arrayName, array);
        if (array.Length < 1) Error(parentName, $"{arrayName} array empty.");
    }

    public static bool TryFindGet<T>(string name, string parentName, string componentName, out T component) {
        GameObject parent = GameObject.Find(parentName);

        if (parent == null) {
            Error(name, $"Could not find GameObject {parentName}.");
            component = default;
            return false;
        }

        if (!parent.TryGetComponent(out component)) {
            Error(name, $"Could not acquire {componentName} from {parentName}.");
            component = default;
            return false;
        }

        return true;
    }

    public static int GetInt(string pref) {
        return PlayerPrefs.GetInt(pref);
    }

    public static int GetInt(string pref, int defaultVal) {
        if (!PlayerPrefs.HasKey(pref)) PlayerPrefs.SetInt(pref, defaultVal);
        return PlayerPrefs.GetInt(pref);
    }

    public static void SetInt(string pref, int val) {
        PlayerPrefs.SetInt(pref, val);
    }

    public static float GetFloat(string pref) {
        return PlayerPrefs.GetFloat(pref);
    }

    public static float GetFloat(string pref, float defaultVal) {
        if (!PlayerPrefs.HasKey(pref)) PlayerPrefs.SetFloat(pref, defaultVal);
        return PlayerPrefs.GetFloat(pref);
    }

    public static void SetFloat(string pref, float val) {
        PlayerPrefs.SetFloat(pref, val);
    }

    public static bool GetBool(string pref) {
        return PlayerPrefs.GetInt(pref) != 0;
    }

    public static bool GetBool(string pref, bool defaultVal) {
        if (!PlayerPrefs.HasKey(pref)) PlayerPrefs.SetInt(pref, defaultVal ? 1 : 0);
        return PlayerPrefs.GetInt(pref) != 0;
    }

    public static void SetBool(string pref, bool val) {
        PlayerPrefs.SetInt(pref, val ? 1 : 0);
    }

    public static void ToggleBool(string pref) {
        SetBool(pref, !GetBool(pref));
    }

    public static string BoolToStatus(bool val) {
        return val ? "Enabled" : "Disabled";
    }
}