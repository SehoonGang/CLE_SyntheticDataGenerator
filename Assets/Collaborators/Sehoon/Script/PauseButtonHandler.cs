using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Perception.Randomization.Scenarios;
using UnityEngine.UI;

public class PauseButtonHandler : MonoBehaviour
{
    public Button PauseButton;
    public FixedLengthScenario Scenario;

    void Start()
    {
        PauseButton.onClick.AddListener(OnButtonClick);
    }

     void OnButtonClick()
    {
        PauseScenario();
    }

    private void PauseScenario()
    {
        if (Scenario != null)
        {
            Scenario.enabled = false;
        }
    }
}
