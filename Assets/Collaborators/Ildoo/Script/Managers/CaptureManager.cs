using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class CaptureManager : MonoBehaviour
{
    private string _captuerPath; 
    private bool _captureEnabled;

    public string CapturePath
    {
        get => _captuerPath; 
        set => _captuerPath = value;
    }

    public bool CaptureEnabled
    {
        get => _captureEnabled; 
        set => _captureEnabled = value;
    }

    Camera _cameraMain; 
    private void Awake()
    {
        
    }

    private void Start()
    {
        _cameraMain = Camera.main;
        //StartCoroutine(captureTimer());
    }

    public void Capture()
    {

    }
    WaitForSeconds interval = new WaitForSeconds(1);
    private IEnumerator captureTimer()
    {
        while (true)
        {
            yield return interval;
            
            string directoryPath = Path.Combine(Application.dataPath, $"ScreenShots");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string colorPath = Path.Combine(Application.dataPath, $"ScreenShots\\rgb_{Time.frameCount}");
            string depthPath = Path.Combine(Application.dataPath, $"ScreenShots\\depth_{Time.frameCount}");
            /*
            CaptureCamera.CaptureColorAndDepthToFile(_cameraMain, GraphicsFormat.R8G8B8A8_UNorm, colorPath, 
                CaptureImageEncoder.ImageFormat.Jpg, GraphicsFormat.R32_SFloat, depthPath, CaptureImageEncoder.ImageFormat.Exr);
            */
        }
    }
}