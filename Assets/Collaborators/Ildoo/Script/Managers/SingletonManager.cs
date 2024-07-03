using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SingletonManager : MonoBehaviour
{
    private static SingletonManager _instance; 
    private static CaptureManager _captureManager;
    //private static SceneManager _sceneManager;
    
    public static SingletonManager Instance => _instance;
    public static CaptureManager CaptureManager => _captureManager;
    //public static SceneManager SceneManager => _sceneManager;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this); 
            return;
        }

        DontDestroyOnLoad(this); 
        _instance = this;
        InitManagers();
    }

    private void InitManagers()
    {
        GameObject captureObj = new GameObject() { name = "CaptureManager" };
        captureObj.transform.SetParent(transform);
        _captureManager = captureObj.AddComponent<CaptureManager>();
    }
}