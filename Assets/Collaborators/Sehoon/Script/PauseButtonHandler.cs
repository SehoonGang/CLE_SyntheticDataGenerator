using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Perception.Randomization.Scenarios;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PauseButtonHandler : MonoBehaviour
{
    public CustomScenario _scenario;
    public Button PauseButton;

    private void Start()
    {
        PauseButton.onClick.AddListener(OnClickResetButton);
    }

    public async void OnClickResetButton()
    {
        Debug.Log("RESET");
        ScenarioManagerHandler.Instance._isSceneReset = true;
    }
}
