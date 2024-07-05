using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Perception.Randomization.Scenarios;

/// <summary>
/// Implementation Based on Sehoon's UI Panel Controller <see cref="UiPanelController"/>
/// </summary>
public class CanvasHandler : MonoBehaviour
{

    [SerializeField] Canvas _uiCanvas;
    [SerializeField] ScenarioMode _currentMode;
    private void Awake()
    {
        _uiCanvas = GetComponent<Canvas>();
        _currentMode = ScenarioMode.Stop;
    }

    public void ScenarioStateChange(ScenarioMode mode)
    {
        switch(mode)
        {
            case ScenarioMode.Stop:
                _currentMode = ScenarioMode.Stop;
                _uiCanvas.enabled = true;
                
                break;
            case ScenarioMode.Start: 
                _currentMode = ScenarioMode.Start; 
                _uiCanvas.enabled = false;
                SingletonManager.CaptureManager.SimulationPaused = false; 
                break;
            default: break;
        }
    }
}

public enum ScenarioMode
{
    Start, 
    Stop
}