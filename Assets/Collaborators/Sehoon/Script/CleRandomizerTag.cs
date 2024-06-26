using System;
using TMPro;
using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Samplers;
using UnityEngine.Perception.Randomization.Scenarios;

// Add this Component to any GameObject that you would like to be randomized. This class must have an identical name to
// the .cs file it is defined in.
public class CleRandomizerTag : RandomizerTag {}

[Serializable]
[AddRandomizerMenu("Cle Randomizer")]
public class CleRandomizer : Randomizer
{
    public FixedLengthScenario Scenario;

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

    protected override void OnIterationStart()
    {
        SetRotationParameters();
        SetLightParameters();
        SetForegroundParameters();
    }

    public void SetRotationParameters()
    {
        Scenario.GetRandomizer<RotationRandomizer>().rotation = new Vector3Parameter
        {
            x = new UniformSampler(float.Parse(XMinValue.text), float.Parse(XMaxValue.text)),
            y = new UniformSampler(float.Parse(YMinValue.text), float.Parse(YMaxValue.text)),
            z = new UniformSampler(float.Parse(ZMinValue.text), float.Parse(ZMaxValue.text))
        };
    }

    public void SetLightParameters()
    {
        Scenario.GetRandomizer<LightRandomizer>().lightIntensity = new() { value = new UniformSampler(0, float.Parse(LightIntensity.text)) };
        Scenario.GetRandomizer<LightRandomizer>().color.red = new UniformSampler(float.Parse(RMinValue.text), float.Parse(RMaxValue.text));
        Scenario.GetRandomizer<LightRandomizer>().color.green = new UniformSampler(float.Parse(GMinValue.text), float.Parse(GMaxValue.text));
        Scenario.GetRandomizer<LightRandomizer>().color.blue = new UniformSampler(float.Parse(BMinValue.text), float.Parse(BMaxValue.text));
    }

    private void SetForegroundParameters()
    {
        Scenario.GetRandomizer<ForegroundObjectPlacementRandomizer>().depth = float.Parse(DepthInputField.text);
        Scenario.GetRandomizer<ForegroundObjectPlacementRandomizer>().separationDistance = float.Parse(SeparationDistanceInputField.text);
        Scenario.GetRandomizer<ForegroundObjectPlacementRandomizer>().placementArea = new Vector2(float.Parse(XPlacement.text), float.Parse(YPlacement.text));
    }
}
