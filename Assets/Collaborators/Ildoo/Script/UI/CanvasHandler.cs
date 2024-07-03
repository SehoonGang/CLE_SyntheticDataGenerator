using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Implementation Based on Sehoon's UI Panel Controller <see cref="UiPanelController"/>
/// </summary>
public class CanvasHandler : MonoBehaviour
{

    [SerializeField] Canvas _uiCanvas;
    [SerializeField] ScenarioMode _currentMode;
    [SerializeField] PauseButtonHandler _pauseButtonHandler;
    private void Awake()
    {
        _uiCanvas = GetComponent<Canvas>();
        _pauseButtonHandler = GetComponent<PauseButtonHandler>();
        _currentMode = ScenarioMode.Stop;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Space))
        {
            ButtonClick();
        }
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
                break;
            default: break;
        }
    }

    private void ButtonClick()
    {
        if (_currentMode == ScenarioMode.Start)
        {
            _pauseButtonHandler.OnButtonClick();
        }
    }
}

public enum ScenarioMode
{
    Start, 
    Stop
}