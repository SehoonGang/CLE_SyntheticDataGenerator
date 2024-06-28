using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Perception.GroundTruth.DataModel;
using UnityEngine.Perception.GroundTruth.Sensors.Channels;
using UnityEngine.Perception.GroundTruth.Utilities;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Png;
using System.Runtime.InteropServices.ComTypes;

namespace UnityEngine.Perception.GroundTruth.Labelers
{
    [Serializable]
    [MovedFrom("UnityEngine.Perception.GroundTruth")]
    public class CustomRGBLabeler : CameraLabeler, IOverlayPanelProvider
    {
        RenderTexture _depthTexture;
        public Texture overlayImage => _depthTexture;
        CustomRGBDefinition m_AnnotationDefinition;
        Dictionary<int, AsyncFuture<Annotation>> m_AsyncAnnotations;
        public string label => "CustomRGB";
        public string annotationId = "Custom RGB";
        private const LosslessImageEncodingFormat _encodingFormat = LosslessImageEncodingFormat.Png;
        public DepthMeasurementStrategy measurementStrategy = DepthMeasurementStrategy.Range;
        public override string description => "Custom RGB Screenshot without UI Layer";

        public override string labelerId => annotationId;

        protected override bool supportsVisualization => true;

        // Start is called before the first frame update
        protected override void Setup()
        {
            m_AsyncAnnotations = new Dictionary<int, AsyncFuture<Annotation>>();
            var channel = perceptionCamera.EnableChannel<CustomRGBChannel>();
            channel.outputTextureReadback += OnDepthTextureReadback;
            _depthTexture = channel.outputTexture;
            
            m_AnnotationDefinition = new CustomRGBDefinition(annotationId);
            DatasetCapture.RegisterAnnotationDefinition(m_AnnotationDefinition);
            visualizationEnabled = supportsVisualization;
        }

        void OnDepthTextureReadback(int frameCount, NativeArray<Color32> data)
        {
            if (!m_AsyncAnnotations.TryGetValue(frameCount, out var future))
                return;
            m_AsyncAnnotations.Remove(frameCount);
            
            ImageEncoder.EncodeImage(data, _depthTexture.width, _depthTexture.height,
                _depthTexture.graphicsFormat, _encodingFormat, encodedImageData =>
                {
                    var toReport = new CustomRGBAnnotation
                    (
                        m_AnnotationDefinition, 
                        perceptionCamera.SensorHandle.Id, 
                        measurementStrategy,
                        ImageEncoder.ConvertFormat(_encodingFormat), 
                        new Vector2(_depthTexture.width, _depthTexture.height),
                        encodedImageData.ToArray()
                    );
                    future.Report(toReport);
                }
            );
        }

        protected override void OnEndRendering(ScriptableRenderContext ctx)
        {
            m_AsyncAnnotations[Time.frameCount] =
                perceptionCamera.SensorHandle.ReportAnnotationAsync(m_AnnotationDefinition);
        }

        protected override void Cleanup()
        {
            _depthTexture = null;
        }
    }
}