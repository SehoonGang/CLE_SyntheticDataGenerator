using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Perception.GroundTruth.DataModel;
using UnityEngine.Scripting.APIUpdating;
[MovedFrom("UnityEngine.Perception.GroundTruth")]
public class CustomDepthDefinition : AnnotationDefinition
{
    public override string description => "Geneates a 16-bit depth image in .png format where each pixel contains normalized distance based on unity units from the camera to the object in the scene.";

    public override string modelType => "type.unity.com/unity.CustomDepthAnnotation";

    public CustomDepthDefinition(string id): base(id)
    {
    }
}
