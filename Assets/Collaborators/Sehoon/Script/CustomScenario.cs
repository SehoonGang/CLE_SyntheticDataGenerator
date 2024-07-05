using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Perception.GroundTruth;
using UnityEngine.Perception.Randomization.Scenarios;
using UnityEngine.SceneManagement;

public class CustomScenario : FixedLengthScenario
{
    protected override bool isIterationComplete => (currentIterationFrame >= framesPerIteration) || ScenarioManagerHandler.Instance._isSceneStop;

    protected override void OnIterationEnd()
    {
        base.OnIterationEnd();
        if (ScenarioManagerHandler.Instance._isSceneStop)
        {
            currentIteration = constants.iterationCount;
        }
    }

    protected override void OnComplete()
    {
        DatasetCapture.ResetSimulation();
    }

    protected override void OnIdle()
    {
        base.OnIdle();
        if (ScenarioManagerHandler.Instance._isSceneReset)
        {
            return;
        }
    }

    public void ResetScenario(string scenario)
    {
        SceneManager.LoadScene(scenario);
    }
  }
