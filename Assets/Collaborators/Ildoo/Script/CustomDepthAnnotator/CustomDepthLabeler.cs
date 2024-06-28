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
using SixLabors.ImageSharp.Processing;

namespace UnityEngine.Perception.GroundTruth.Labelers
{
    [Serializable]
    [MovedFrom("UnityEngine.Perception.GroundTruth")]
    public class CustomDepthLabeler : CameraLabeler, IOverlayPanelProvider
    {
        RenderTexture _depthTexture;
        public Texture overlayImage => _depthTexture;
        CustomDepthDefinition m_AnnotationDefinition;
        Dictionary<int, AsyncFuture<Annotation>> m_AsyncAnnotations;
        NativeArray<ushort> depthData = new NativeArray<ushort>();
        public string label => "CustomDepth";
        public string annotationId = "CustomDepth";
        private const LosslessImageEncodingFormat _encodingFormat = LosslessImageEncodingFormat.Png;
        public DepthMeasurementStrategy measurementStrategy = DepthMeasurementStrategy.Range;
        public override string description => "2 Byte Depth Map .png";

        public override string labelerId => annotationId;

        protected override bool supportsVisualization => true;

        // Start is called before the first frame update
        protected override void Setup()
        {
            m_AsyncAnnotations = new Dictionary<int, AsyncFuture<Annotation>>();

           
            if (measurementStrategy == DepthMeasurementStrategy.Depth)
            {
                var channel = perceptionCamera.EnableChannel<ShortDepthChannel>();
                channel.outputTextureReadback += OnDepthTextureReadback;
                _depthTexture = channel.outputTexture;
            }
            else
            {
                Debug.LogWarning("Set Depth Measurement Strategy as Depth!");
                Application.Quit();
                return;
            }
            m_AnnotationDefinition = new CustomDepthDefinition(annotationId);
            DatasetCapture.RegisterAnnotationDefinition(m_AnnotationDefinition);
            visualizationEnabled = supportsVisualization;
        }

        void OnDepthTextureReadback(int frameCount, NativeArray<float4> data)
        {
            if (!m_AsyncAnnotations.TryGetValue(frameCount, out var future))
                return;
            m_AsyncAnnotations.Remove(frameCount);

            if (depthData.Length <= 0)
            {
                depthData = new NativeArray<ushort>(data.Length, Allocator.Persistent);
            }
            
            for (int i = 0; i < data.Length; i++) 
            {
                float depth = data[i].x;  // Assuming depth is stored in the red channel as meters
                ushort depthValue = (ushort)Mathf.Clamp(depth * 10000f, 0, 65535);  // Convert meters to 0.1 mm units and clamp to 16-bit range
                depthData[i] = depthValue;
            }

            var slice = new NativeSlice<ushort>(depthData).SliceConvert<byte>();
            var bytes = new byte[slice.Length];
            slice.CopyTo(bytes);

            byte[]? pngEncodedBytes; 
            using (var image = Image.LoadPixelData<L16>(bytes, _depthTexture.width, _depthTexture.height))
            {
                image.Mutate(x => x.Flip(FlipMode.Vertical));
                using (MemoryStream mStream = new MemoryStream())
                {
                    image.Save(mStream, new PngEncoder());
                    pngEncodedBytes = mStream.ToArray();
                    //File.WriteAllBytes(filePath, mStream.ToArray());
                }
                // Save the image as PNG
            }

            if (pngEncodedBytes == null)
            {
                Debug.LogWarning("Bytes is Null!!");
                return;
            }
            var toReport = new CustomDepthAnnotation(
                        m_AnnotationDefinition,
                        perceptionCamera.SensorHandle.Id,
                        measurementStrategy,
                        ImageEncoder.ConvertFormat(_encodingFormat),
                        new Vector2(_depthTexture.width, _depthTexture.height),
                        pngEncodedBytes);
            future.Report(toReport);

            
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
    #region Deprecated 
    //ImageEncoder.EncodeImage(depthData, _depthTexture.width, _depthTexture.height,
            //    GraphicsFormat.R16_UNorm, _encodingFormat, encodedImageData =>
            //    {
            //        var toReport = new CustomDepthAnnotation(
            //            m_AnnotationDefinition,
            //            perceptionCamera.SensorHandle.Id,
            //            measurementStrategy,
            //            ImageEncoder.ConvertFormat(_encodingFormat),
            //            new Vector2(_depthTexture.width, _depthTexture.height),
            //            encodedImageData.ToArray());

            //        future.Report(toReport);
            //    }
            //);

    //Texture2D depthTexture2D = new Texture2D(_depthTexture.width, _depthTexture.height, TextureFormat.R16, false);
            //depthTexture2D.SetPixelData(depthData, 0);
            //depthTexture2D.Apply();

            //byte[] encodedImageData = depthTexture2D.EncodeToPNG();
            //Debug.Log(encodedImageData.Length);
            //Debug.Log($"byte Information : {(ushort)encodedImageData[2000]}");
                //            for (int y = 0; y < _depthTexture.height; y++)
                //{
                //    for (int x = 0; x < _depthTexture.width; x++)
                //    {
                //        ushort depthValue = depthData[y * _depthTexture.width + x];
                //        image[x, y] = new L16(depthValue);
                //    }
                //}
    #endregion
}
