using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Perception.GroundTruth;
using UnityEngine.Perception.Randomization.Scenarios;

public class StartButtonHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public Button StartButton;
    public FixedLengthScenario Scenario;

    void Start()
    {
        StartButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        StartScenario();
    }

    private void StartScenario()
    {
        if (Scenario != null)
        {
            Scenario.enabled = true;
        }
    }
}
