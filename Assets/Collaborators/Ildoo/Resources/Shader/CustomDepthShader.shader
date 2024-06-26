Shader "Custom/CustomDepthShader"
{
    SubShader
    {
        Pass
        {
            Tags { "LightMode" = "SRPDefaultUnlit" }

            Cull Off
            ZWrite On

            HLSLPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag

			float _near; 
			float _far; 

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float4 cameraSpacePosition: TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);

                // Invert the z component of the vertex in camera space.
                float4 cameraSpacePosition = float4(UnityObjectToViewPos(v.vertex), 1);
                cameraSpacePosition.z = -cameraSpacePosition.z;
                o.cameraSpacePosition = cameraSpacePosition;

                return o;
            }
			float4 frag(v2f i) : SV_TARGET
            {
                return float4(i.cameraSpacePosition.z, 0, 0, 1);
            }
			// half4 frag(v2f i) : SV_TARGET
			// {
			// 	// Use the z component of the camera space position to get the depth
   //              float depthInMeters = i.cameraSpacePosition.z;

   //              // Convert depth to 0.1 mm units
   //              uint depthInMM = uint(depthInMeters * 1000.0);
				
   //              // Normalize to [0, 1] range for 16-bit storage
   //              half normalizedDepth = depthInMM / 65535.0;
				
   //              // Store normalized value in the red channel
   //              return half4(depthInMeters, 0, 0, 1);
			// }
            ENDHLSL
        }
    }
}