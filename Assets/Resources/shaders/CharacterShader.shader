Shader "RunningFred/CharacterShader" {
    Properties {
        _MainTex ("Diffuse", 2D) = "white" {}
        _BRDFTex ("BRDF", 2D) = "gray" {}
    }
    SubShader {
        Tags { "RenderType" = "Opaque" }
        LOD 600

        Pass {
            Name "FORWARD"
            Tags { "LightMode" = "ForwardBase" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            sampler2D _MainTex;
            sampler2D _BRDFTex;

            float4 _MainTex_ST;

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v) {
                v2f o;
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = worldNormal;
                o.viewDir = normalize(_WorldSpaceCameraPos - worldPos); // Correct direction: camera - model
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float3 N = normalize(i.normal);
                float3 V = normalize(i.viewDir);
                float3 L = normalize(_WorldSpaceLightPos0.xyz);

                float ndotl = saturate(dot(N, L));
                float ndotv = saturate(dot(N, V));

                float2 brdfUV;
                brdfUV.x = ndotl * 0.5 * 0.5; // Match original GLSL logic
                brdfUV.y = dot(N, V);         // Use raw dot for accurate rimlight positioning

                float3 albedo = tex2D(_MainTex, i.uv).rgb;
                float3 brdf = tex2D(_BRDFTex, brdfUV).rgb;
                float3 lighting = brdf * _LightColor0.rgb;

                fixed4 col;
                col.rgb = lighting * albedo;
                col.a = 1.0;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
