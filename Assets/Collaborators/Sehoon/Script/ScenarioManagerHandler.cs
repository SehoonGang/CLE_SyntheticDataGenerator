using TMPro;
using UnityEngine;
using UnityEngine.Perception.Randomization.Scenarios;

public class ScenarioManagerHandler : MonoBehaviour
{
    private static ScenarioManagerHandler _instance;
    public static ScenarioManagerHandler Instance
    {
        get => _instance;
    }

    public bool _isSceneStop = false;
    public bool _isSceneReset= false;
    public string _sceneName;
    public CustomScenario _scenario;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(_instance);
    }
}
