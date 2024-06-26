using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SingletonManager : MonoBehaviour
{
    private static SingletonManager instance; 
    private static CaptureManager captureManager;
    
    public static SingletonManager Instance => instance;
    public static CaptureManager CaptureManager => captureManager;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this); 
            return;
        }

        DontDestroyOnLoad(this); 
        instance = this;
        InitManagers();
    }

    private void InitManagers()
    {
        GameObject captureObj = new GameObject() { name = "CaptureManager" };
        captureObj.transform.SetParent(transform);
        captureManager = captureObj.AddComponent<CaptureManager>();
    }
}