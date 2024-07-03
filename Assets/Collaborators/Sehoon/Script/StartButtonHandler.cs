using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Perception.Randomization.Scenarios;
using TMPro;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Samplers;
using UnityEngine.Perception.Randomization.Parameters;

public class StartButtonHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera MainCamera;
    public Button StartButton;
    public Button PauseButton;
    public FixedLengthScenario Scenario;
    public TMP_Dropdown Dropdown;

    public TMP_InputField XMinValue;
    public TMP_InputField XMaxValue;
    public TMP_InputField YMinValue;
    public TMP_InputField YMaxValue;
    public TMP_InputField ZMinValue;
    public TMP_InputField ZMaxValue;

    public TMP_InputField RMinValue;
    public TMP_InputField RMaxValue;
    public TMP_InputField GMinValue;
    public TMP_InputField GMaxValue;
    public TMP_InputField BMinValue;
    public TMP_InputField BMaxValue;
    public TMP_InputField LightIntensity;

    //public Scrollbar DepthScrolbar;
    //public Scrollbar ObjectDistanceScrollbar;
    public TMP_InputField DepthInputField;
    public TMP_InputField SeparationDistanceInputField;
    public TMP_InputField XPlacement;
    public TMP_InputField YPlacement;
    public bool IsPauseMoving = true;

    private RotationRandomizer _rotationRandomizer;
    private LightRandomizer _lightRandomizer;

    void Start()
    {
        _rotationRandomizer = Scenario.GetRandomizer<RotationRandomizer>();
        _lightRandomizer = Scenario.GetRandomizer<LightRandomizer>();
        StartButton.onClick.AddListener(OnButtonClick);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!IsPauseMoving)
            {
                PauseScenario();
            }
            else
            {
                StartScenario();
            }
            IsPauseMoving = !IsPauseMoving;
        }

    }

    void OnButtonClick()
    {
        IsPauseMoving = !IsPauseMoving;
        if (IsPauseMoving)
        {
            PauseScenario();
        }
        else
        {
            StartScenario();
        }
    }

    private void StartScenario()
    {
        if (Scenario != null)
        {
            Scenario.enabled = true;
            SetRotationParameters();
            SetLightParameters();
        }
    }

    private void PauseScenario()
    {
        if (Scenario != null)
        {
            Scenario.enabled = false;
        }
    }

    public void SetRotationParameters()
    {
        _rotationRandomizer.rotation = new Vector3Parameter
        {
            x = new UniformSampler(float.Parse(XMinValue.text), float.Parse(XMaxValue.text)),
            y = new UniformSampler(float.Parse(YMinValue.text), float.Parse(YMaxValue.text)),
            z = new UniformSampler(float.Parse(ZMinValue.text), float.Parse(ZMaxValue.text))
        };
    }

    public void SetLightParameters()
    {
        _lightRandomizer.lightIntensity = new() { value = new UniformSampler(0, float.Parse(LightIntensity.text)) };
        _lightRandomizer.color.red = new UniformSampler(float.Parse(RMinValue.text), float.Parse(RMaxValue.text));
        _lightRandomizer.color.green = new UniformSampler(float.Parse(GMinValue.text), float.Parse(GMaxValue.text));
        _lightRandomizer.color.blue = new UniformSampler(float.Parse(BMinValue.text), float.Parse(BMaxValue.text));
    }
}
