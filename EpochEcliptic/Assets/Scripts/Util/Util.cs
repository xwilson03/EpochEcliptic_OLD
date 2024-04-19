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
}