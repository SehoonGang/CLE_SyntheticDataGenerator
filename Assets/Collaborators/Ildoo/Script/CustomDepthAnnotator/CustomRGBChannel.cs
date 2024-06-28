using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Perception.GroundTruth.Sensors.Channels
{
    /// <summary>
    /// A <see cref="CameraChannel{T}"/> that outputs the captured RGB color value for each pixel.
    /// </summary>
    [MovedFrom("UnityEngine.Perception.GroundTruth")]
    public class CustomRGBChannel : CameraChannel<Color32>
    { 
        /// <inheritdoc/>
        public override Color clearColor => Color.black;
        ShaderTagId[] shaderPasses = new[]
            {
                new ShaderTagId("Forward"), // HD Lit shader
                new ShaderTagId("ForwardOnly"), // HD Unlit shader
                new ShaderTagId("SRPDefaultUnlit"), // Cross SRP Unlit shader
                new ShaderTagId("UniversalForward"), // URP Forward
                new ShaderTagId("LightweightForward") // LWRP Forward
            };
            RenderStateBlock stateBlock = new RenderStateBlock(0)
            {
                depthState = new DepthState(true, CompareFunction.LessEqual),
            };

        /// <summary>
        /// Creates render texture with required sizes
        /// </summary>
        /// <param name="width">Width of render texture</param>
        /// <param name="height">Height of render texture</param>
        /// <returns>RenderTexture</returns>
        public override RenderTexture CreateOutputTexture(int width, int height)
        {
            var texture = new RenderTexture(width, height, 32, GraphicsFormat.R8G8B8A8_SRGB) { name = "Custom RGB Channel" };
            texture.Create();
            return texture;
        }

        /// <summary>
        /// Sets camera output to the specified renderTarget
        /// </summary>
        /// <param name="inputs">Input camera</param>
        /// <param name="renderTarget">Target texture to render</param>
        public override void Execute(CameraChannelInputs inputs, RenderTexture renderTarget)
        {
            if (inputs.cameraColorBuffer == (RenderTargetIdentifier)renderTarget)
                return;

            var rendererListDesc = new RendererListDesc(
                RenderUtilities.shaderPassNames, inputs.cullingResults, inputs.camera)
            {
                renderQueueRange = RenderQueueRange.all, 
                sortingCriteria = SortingCriteria.CommonOpaque, 
                excludeObjectMotionVectors = false,
                overrideMaterial = null, 
                overrideMaterialPassIndex = 0,
                layerMask = perceptionCamera.layerMask
            };
            var list = inputs.ctx.CreateRendererList(rendererListDesc); 
            inputs.cmd.SetRenderTarget(renderTarget);
            inputs.cmd.ClearRenderTarget(true, false, clearColor);
            inputs.cmd.DrawRendererList(list);
        }
    }
}