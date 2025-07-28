Shader "RunningFred/DiffuseSimpleFade"
{
    Properties
    {
        _MainTex ("Base Mask", 2D) = "black" {}
        _Alpha ("Alpha", Range(0, 1)) = 1
        _Brightness ("Brightness Control", Range(0.1, 2.0)) = 1.0
        _AmbientIntensity ("Ambient Intensity", Range(0.0, 1.0)) = 0.5
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
        LOD 100
        
        // First pass: write to depth buffer
        Pass
        {
            ZWrite On
            ZTest LEqual
            ColorMask 0
        }
        
        // Second pass: render with transparency and lighting
        Pass
        {
            ZWrite Off
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Alpha;
            float _Brightness;
            float _AmbientIntensity;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                
                // Check if texture is null/black - make it opaque black
                if (texColor.r + texColor.g + texColor.b < 0.01 && _Alpha == 1.0)
                {
                    return fixed4(0, 0, 0, 1); // Opaque black
                }
                
                // Use ambient lighting plus directional light color (without shadows)
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb * _AmbientIntensity;
                fixed3 directionalColor = _LightColor0.rgb * 0.3; // Reduced directional light intensity
                
                // Combine lighting with texture (even illumination with directional color)
                fixed3 finalColor = texColor.rgb * (ambient + directionalColor) * _Brightness;
                
                // Combine texture alpha with the _Alpha property
                float finalAlpha = texColor.a * _Alpha;
                
                return fixed4(finalColor, finalAlpha);
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
