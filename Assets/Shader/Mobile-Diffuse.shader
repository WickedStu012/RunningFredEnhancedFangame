Shader "Mobile/Diffuse" {
Properties {
    _MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 150

    // ------------------------------------------------------------------
    // Base forward pass (directional light, lightmaps, shadows)
    Pass {
        Name "FORWARD"
        Tags { "LightMode" = "ForwardBase" }
        
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_fwdbase
        #pragma multi_compile_fog
        #include "UnityCG.cginc"
        #include "Lighting.cginc"
        #include "AutoLight.cginc"

        struct appdata {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float2 texcoord : TEXCOORD0;
            float2 texcoord1 : TEXCOORD1;
        };

        struct v2f {
            float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
            fixed3 diff : COLOR0;
            fixed3 ambient : COLOR1;
            UNITY_FOG_COORDS(2)
            SHADOW_COORDS(3) // shadow coordinates
        };

        sampler2D _MainTex;
        float4 _MainTex_ST;

        v2f vert (appdata v) {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
            
            half3 worldNormal = UnityObjectToWorldNormal(v.normal);
            half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
            o.diff = nl * _LightColor0.rgb;
            o.ambient = ShadeSH9(half4(worldNormal,1));
            
            UNITY_TRANSFER_FOG(o,o.pos);
            TRANSFER_SHADOW(o) // transfer shadow coordinates
            return o;
        }

        fixed4 frag (v2f i) : SV_Target {
            fixed4 col = tex2D(_MainTex, i.uv);
            fixed shadow = SHADOW_ATTENUATION(i);
            fixed3 lighting = i.diff * shadow + i.ambient;
            col.rgb *= lighting;
            
            UNITY_APPLY_FOG(i.fogCoord, col);
            return col;
        }
        ENDCG
    }

    // ------------------------------------------------------------------
    // Additive forward pass (additional lights)
    Pass {
        Name "FORWARD_DELTA"
        Tags { "LightMode" = "ForwardAdd" }
        Blend One One
        ZWrite Off
        
        CGPROGRAM
        #pragma vertex vert_add
        #pragma fragment frag_add
        #pragma multi_compile_fwdadd
        #pragma multi_compile_fog
        #include "UnityCG.cginc"
        #include "Lighting.cginc"
        #include "AutoLight.cginc"

        struct appdata {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float2 texcoord : TEXCOORD0;
        };

        struct v2f {
            float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
            fixed3 diff : COLOR0;
            UNITY_FOG_COORDS(1)
            LIGHTING_COORDS(2,3) // lighting coordinates
        };

        sampler2D _MainTex;
        float4 _MainTex_ST;

        v2f vert_add (appdata v) {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
            
            half3 worldNormal = UnityObjectToWorldNormal(v.normal);
            half3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
            half nl = max(0, dot(worldNormal, lightDir));
            o.diff = nl * _LightColor0.rgb;
            
            UNITY_TRANSFER_FOG(o,o.pos);
            TRANSFER_VERTEX_TO_FRAGMENT(o) // transfer lighting
            return o;
        }

        fixed4 frag_add (v2f i) : SV_Target {
            fixed4 col = tex2D(_MainTex, i.uv);
            fixed atten = LIGHT_ATTENUATION(i);
            col.rgb *= i.diff * atten;
            
            UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0));
            return col;
        }
        ENDCG
    }

    // ------------------------------------------------------------------
    // Shadow caster pass
    Pass {
        Name "ShadowCaster"
        Tags { "LightMode" = "ShadowCaster" }
        
        CGPROGRAM
        #pragma vertex vert_shadow
        #pragma fragment frag_shadow
        #pragma multi_compile_shadowcaster
        #include "UnityCG.cginc"

        struct v2f_shadow { 
            V2F_SHADOW_CASTER;
        };

        v2f_shadow vert_shadow(appdata_base v) {
            v2f_shadow o;
            TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
            return o;
        }

        float4 frag_shadow(v2f_shadow i) : SV_Target {
            SHADOW_CASTER_FRAGMENT(i)
        }
        ENDCG
    }
}

Fallback "Mobile/VertexLit"
}