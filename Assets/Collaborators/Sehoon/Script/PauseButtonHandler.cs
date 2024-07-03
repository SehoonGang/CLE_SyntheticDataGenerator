using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Perception.Randomization.Scenarios;
using UnityEngine.UI;

public class PauseButtonHandler : MonoBehaviour
{
    public Button PauseButton;
    public FixedLengthScenario Scenario;
    public bool IsPauseMoving = false;

    void Start()
    {
        PauseButton.onClick.AddListener(OnButtonClick);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            IsPauseMoving = !IsPauseMoving;
        }
        if (IsPauseMoving)
        {
            PauseScenario();
        }
    }

    void OnButtonClick()
    {
        PauseScenario();
        IsPauseMoving = true;
    }

    private void PauseScenario()
    {
        if (Scenario != null)
        {
            Scenario.enabled = false;
        }
    }
}
