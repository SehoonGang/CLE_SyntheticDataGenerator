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
				float2 uv : TEXCOORD0;
                float4 cameraSpacePosition: TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
				// Derive UV coordinates from vertex positions and flip them if necessary
                // Invert the z component of the vertex in camera space.
                float4 cameraSpacePosition = float4(UnityObjectToViewPos(v.vertex), 1);
                cameraSpacePosition.z = -cameraSpacePosition.z;
				cameraSpacePosition.x = -cameraSpacePosition.x;
                o.cameraSpacePosition = cameraSpacePosition;
                return o;
            }
			float4 frag(v2f i) : SV_TARGET
            {
				float depthInMeters = i.cameraSpacePosition.z; 
				//uint depthValue = uint(clamp(depthInMeters * 10000.0f, 0, 4294967295));
                return float4(depthInMeters, 0, 0, 1);
            }
            ENDHLSL
        }
    }
}