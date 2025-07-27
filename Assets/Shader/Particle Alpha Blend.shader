Shader "Particles/Alpha Blended" {
    Properties {
        [HDR] _TintColor ("Tint Color", Color) = (0.5, 0.5, 0.5, 0.5)
        _MainTex ("Particle Texture", 2D) = "white" {}
        _SoftnessFactor ("Soft Particles Factor", Range(0.01, 3)) = 1.0
    }

    SubShader {
        Tags {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
        }

        Cull Off
        ZWrite Off
        ColorMask RGB
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile __ SOFTPARTICLES_ON
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _TintColor;
            float _SoftnessFactor;

            #ifdef SOFTPARTICLES_ON
                sampler2D_float _CameraDepthTexture;
                float4 _CameraDepthTexture_TexelSize;
            #endif

            struct VertexInput {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
                #ifdef SOFTPARTICLES_ON
                    float4 projPos : TEXCOORD1;
                #endif
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                
                #ifdef SOFTPARTICLES_ON
                    o.projPos = ComputeScreenPos(o.pos);
                    COMPUTE_EYEDEPTH(o.projPos.z);
                #endif
                
                return o;
            }

            fixed4 frag (VertexOutput i) : SV_Target {
                #ifdef SOFTPARTICLES_ON
                    // Soft particles calculation
                    float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
                    float partZ = i.projPos.z;
                    float fade = saturate(_SoftnessFactor * (sceneZ - partZ));
                    i.color.a *= fade;
                #endif

                fixed4 tex = tex2D(_MainTex, i.uv);
                return 2.0 * i.color * _TintColor * tex;
            }
            ENDCG
        }
    }
    
    Fallback "Particles/Alpha"
}   