Shader "MADFINGER/Diffuse/Simple" 
{
    Properties 
    {
        _MainTex ("Base (RGB)", 2D) = "black" {}
        _AmbientIntensity ("Ambient Intensity", Range(0.0, 1.0)) = 0.5
    }
    
    SubShader 
    { 
        Tags { "RenderType"="Opaque" "IgnoreProjector"="True" }
        LOD 100
        
        Pass 
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma skip_variants SHADOWS_SCREEN
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
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
                
                // Use ambient lighting plus directional light color (without shadows)
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb * _AmbientIntensity;
                fixed3 directionalColor = _LightColor0.rgb * 0.3; // Reduced directional light intensity
                
                // Combine lighting with texture (even illumination with directional color)
                fixed3 finalColor = texColor.rgb * (ambient + directionalColor);
                
                return fixed4(finalColor, texColor.a);
            }
            ENDCG
        }
    }
    
    FallBack "Diffuse"
}