// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "Mobile/VertexLit" {
Properties {
    _MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 80
    
    // ------------------------------------------------------------------
    // Vertex lighting pass (main directional light)
    Pass {
        Tags { "LightMode"="Vertex" }
        Lighting On
        
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"
        #include "Lighting.cginc"  // Added this include for lighting variables
        
        struct appdata {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float2 texcoord : TEXCOORD0;
        };
        
        struct v2f {
            float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
            fixed4 color : COLOR;
        };
        
        sampler2D _MainTex;
        float4 _MainTex_ST;
        
        v2f vert (appdata v) {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
            
            // Basic vertex lighting
            float3 worldNormal = UnityObjectToWorldNormal(v.normal);
            float nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
            o.color = nl * _LightColor0;
            o.color.rgb += ShadeSH9(float4(worldNormal,1));
            return o;
        }
        
        fixed4 frag (v2f i) : SV_Target {
            fixed4 col = tex2D(_MainTex, i.uv);
            col.rgb *= i.color.rgb;
            return col;
        }
        ENDCG
    }
    
    // ------------------------------------------------------------------
    // Lightmapping pass (non-RGBM)
    Pass {
        Tags { "LightMode"="VertexLM" }
        
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"
        #include "UnityLightingCommon.cginc" // For lightmap decoding
        
        struct appdata {
            float4 vertex : POSITION;
            float2 texcoord : TEXCOORD0;
            float2 texcoord1 : TEXCOORD1;
        };
        
        struct v2f {
            float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
            float2 uv2 : TEXCOORD1;
        };
        
        sampler2D _MainTex;
        float4 _MainTex_ST;
        // sampler2D unity_Lightmap;
        // float4 unity_LightmapST;
        
        v2f vert (appdata v) {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
            o.uv2 = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
            return o;
        }
        
        fixed4 frag (v2f i) : SV_Target {
            fixed4 col = tex2D(_MainTex, i.uv);
            fixed3 lm = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv2));
            col.rgb *= lm;
            return col;
        }
        ENDCG
    }
    
    // ------------------------------------------------------------------
    // Lightmapping pass (RGBM encoded)
    Pass {
        Tags { "LightMode"="VertexLMRGBM" }
        
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"
        #include "UnityLightingCommon.cginc"
        
        struct appdata {
            float4 vertex : POSITION;
            float2 texcoord : TEXCOORD0;
            float2 texcoord1 : TEXCOORD1;
        };
        
        struct v2f {
            float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
            float2 uv2 : TEXCOORD1;
        };
        
        sampler2D _MainTex;
        float4 _MainTex_ST;
        // sampler2D unity_Lightmap;
        // float4 unity_LightmapST;
        
        v2f vert (appdata v) {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
            o.uv2 = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
            return o;
        }
        
        fixed4 frag (v2f i) : SV_Target {
            fixed4 col = tex2D(_MainTex, i.uv);
            fixed3 lm = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv2));
            col.rgb *= lm;
            return col;
        }
        ENDCG
    }
    
    // ------------------------------------------------------------------
    // Shadow caster pass
    Pass {
        Name "ShadowCaster"
        Tags { "LightMode"="ShadowCaster" }
        
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_shadowcaster
        #include "UnityCG.cginc"
        
        struct v2f { 
            V2F_SHADOW_CASTER;
        };
        
        v2f vert(appdata_base v) {
            v2f o;
            TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
            return o;
        }
        
        float4 frag(v2f i) : SV_Target {
            SHADOW_CASTER_FRAGMENT(i)
        }
        ENDCG
    }
    
    // ------------------------------------------------------------------
    // Shadow collector pass (removed to prevent infinite fallback loop)
    // This was causing the fallback cycle with Mobile/Diffuse
}

Fallback "Mobile/Diffuse"
}