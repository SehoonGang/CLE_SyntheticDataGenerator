using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopButtonHandler : MonoBehaviour
{
    public void OnClickStopButton()
    {
        Debug.Log("STOP");
        ScenarioManagerHandler.Instance._isSceneStop = true;
    }
}
