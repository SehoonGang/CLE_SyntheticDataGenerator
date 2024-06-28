using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Perception.GroundTruth.DataModel;

public class CustomRGBDefinition : AnnotationDefinition
{
    public override string description => "Replaces regular RGB screenshot image without UI layer";

    public override string modelType => "type.unity.com/unity.CustomRGBDefinition";

    public CustomRGBDefinition(string id): base(id)
    {
    }
}
