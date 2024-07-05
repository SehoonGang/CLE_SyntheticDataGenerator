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
    public CustomScenario _scenario;

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
    private bool _isVisibleCanvas = false;
    private CanvasHandler _canvasHandler;

    void Start()
    {
        _rotationRandomizer = _scenario.GetRandomizer<RotationRandomizer>();
        _lightRandomizer = _scenario.GetRandomizer<LightRandomizer>();
        StartButton.onClick.AddListener(OnButtonClick);
        _canvasHandler = GetComponent<CanvasHandler>();
    }

    void OnButtonClick()
    {
        StartScenario();
    }

    private void StartScenario()
    {
        if (ScenarioManagerHandler.Instance._isSceneReset)
        {
            _scenario.ResetScenario(ScenarioManagerHandler.Instance._sceneName);
            ScenarioManagerHandler.Instance._isSceneReset = false;
        }

        if (_scenario != null)
        {
            _scenario.enabled = true;
            SetRotationParameters();
            SetLightParameters();
        }

        _isVisibleCanvas = !_isVisibleCanvas;
        _canvasHandler.ScenarioStateChange(_isVisibleCanvas ? ScenarioMode.Start : ScenarioMode.Stop);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space))
        {
            _canvasHandler.ScenarioStateChange(ScenarioMode.Stop);
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
