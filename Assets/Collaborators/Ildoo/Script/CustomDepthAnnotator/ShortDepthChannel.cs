using Unity.Mathematics;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Perception.GroundTruth.Sensors.Channels
{
    public class ShortDepthChannel : CameraChannel<float4>
    {
        static Material s_DepthMaterial;

        /// <inheritdoc/>
        public override Color clearColor => Color.clear;

        /// <inheritdoc/>
        public override RenderTexture CreateOutputTexture(int width, int height)
        {
            var texture = new RenderTexture(
                width, height, 32, GraphicsFormat.R32G32B32A32_SFloat)
            {
                name = "Depth Channel",
                enableRandomWrite = true,
                filterMode = FilterMode.Point
            };
            texture.Create();
            return texture;
        }

        /// <inheritdoc/>
        public override void Execute(CameraChannelInputs inputs, RenderTexture renderTarget)
        {
            if (s_DepthMaterial == null)
                s_DepthMaterial = new(RenderUtilities.LoadPrewarmedShader("Custom/CustomDepthShader"));

            var rendererListDesc = RenderUtilities.CreateRendererListDesc(
                inputs.camera, inputs.cullingResults, s_DepthMaterial, 0, perceptionCamera.layerMask);
            var list = inputs.ctx.CreateRendererList(rendererListDesc);

            inputs.cmd.SetRenderTarget(renderTarget);
            inputs.cmd.ClearRenderTarget(true, true, clearColor);
            inputs.cmd.DrawRendererList(list);
        }
    }
}