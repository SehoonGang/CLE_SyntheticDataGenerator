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

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(_instance);
    }
}
