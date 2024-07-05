using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopButtonHandler : MonoBehaviour
{
    public void OnClickStopButton()
    {
        if (ScenarioManagerHandler.Instance._scenario.state == UnityEngine.Perception.Randomization.Scenarios.ScenarioBase.State.Playing)
        {
            ScenarioManagerHandler.Instance._isSceneStop = true;
        }
    }
}
