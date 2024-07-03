using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonSetting
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        if (SingletonManager.Instance == null)
        {
            GameObject managerObj = new GameObject() { name = "SingletonManger" };
            managerObj.AddComponent<SingletonManager>();
        }
    }
}