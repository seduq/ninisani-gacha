using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerData playerData;
    void Awake()
    {
        string playerJson = JavascriptPlugin.Get("player");

        if (string.IsNullOrEmpty(playerJson)) {
            playerData = new PlayerData {
                id = Guid.NewGuid().ToString()[..8].ToUpperInvariant(),
                lastPull = "0",
                collection = ""
            };
            JavascriptPlugin.Set("player", JsonUtility.ToJson(playerData));
        } else {
            try {
                playerData = JsonUtility.FromJson<PlayerData>(playerJson);
            } catch (Exception e) {
                JavascriptPlugin.Log(e.Message);
            }
        }

        JavascriptPlugin.Log(JsonUtility.ToJson(playerData));
    }
}
