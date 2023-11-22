using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class JavascriptPlugin 
{ 

    [DllImport("__Internal")]
    private static extern void ConsoleLog(string str);

    [DllImport("__Internal")]
    private static extern string GetStorage(string id);
    [DllImport("__Internal")]
    private static extern void SetStorage(string id, string value);

    private static Dictionary<string, string> _storage = new();

    public static void Log(string str) {
#if UNITY_WEBGL
    ConsoleLog(str);
#else
    Debug.Log(str);
#endif
    }
    public static string Get(string id) {
#if UNITY_WEBGL
        return GetStorage(id);
#else
    return _storage.GetValueOrDefault(id, "");
#endif
    }
    public static void Set(string id, string value) {
#if UNITY_WEBGL
        SetStorage(id, value);
#else
        _storage[id] = value;
#endif
    }
}
