using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopButtonHandler : MonoBehaviour
{
    public void OnClickStopButton()
    {
        ScenarioManagerHandler.Instance._isSceneStop = true;
    }
}
