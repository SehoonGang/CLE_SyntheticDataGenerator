using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Samplers;

public class LightRandomizerTag : RandomizerTag {}

[Serializable]
[AddRandomizerMenu("LightRandomizer")]
public class LightRandomizer : Randomizer
{
    public FloatParameter lightIntensity = new() { value = new UniformSampler(0, 1) };
    public ColorRgbParameter color;

    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<LightRandomizerTag>();
        foreach (var tag in tags)
        {
            var tagLight = tag.GetComponent<Light>();
            tagLight.intensity = lightIntensity.Sample();
            tagLight.color = color.Sample();
        }
    }
}
