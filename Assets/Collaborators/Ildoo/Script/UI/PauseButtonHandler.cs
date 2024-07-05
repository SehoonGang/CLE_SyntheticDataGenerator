using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Perception.Randomization.Scenarios;
using UnityEngine.UI;

public class PauseButtonHandler : MonoBehaviour
{
    private Button _pause;
    [SerializeField] private ScenarioBase _scenario;
    private void Awake()
    {
        _pause = GetComponent<Button>();
    }

    private void Start()
    {
        _scenario = GameObject.FindWithTag("Scenario").GetComponent<ScenarioBase>();
    }

    private void OnEnable()
    {
        _pause.onClick.AddListener(TriggleButton);
    }

    private void OnDisable()
    {
        _pause.onClick.RemoveListener(TriggleButton);
    }

    private void TriggleButton()
    {
        SingletonManager.CaptureManager.SimulationPaused = true;
        _scenario.enabled = false;
    }
}