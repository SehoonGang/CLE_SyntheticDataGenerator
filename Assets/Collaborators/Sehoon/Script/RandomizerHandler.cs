using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Samplers;
using UnityEngine.Perception.Randomization.Scenarios;

// Add this Component to any GameObject that you would like to be randomized. This class must have an identical name to
// the .cs file it is defined in.
[Serializable]
[AddRandomizerMenu("Randomizer Handler")]
public class RandomizerHandler: Randomizer
{
    public FixedLengthScenario _scenario;
}
