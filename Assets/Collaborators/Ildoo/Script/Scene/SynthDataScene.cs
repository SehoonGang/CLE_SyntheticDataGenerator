using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;

public class SynthDataScene : BaseScene
{
    [SerializeField] public Vector3 _sceneBound;
    protected override IEnumerator LoadingRoutine()
    {
        yield return new WaitUntil(() => true);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
