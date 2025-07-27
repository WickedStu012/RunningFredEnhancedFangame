Shader "RenderFX/Skybox" {
    Properties {
        [HDR] _Tint ("Tint Color", Color) = (1.0, 1.0, 1.0, 1.0)
        [NoScaleOffset] _FrontTex ("Front (+Z)", 2D) = "white" {}
        [NoScaleOffset] _BackTex ("Back (-Z)", 2D) = "white" {}
        [NoScaleOffset] _LeftTex ("Left (+X)", 2D) = "white" {}
        [NoScaleOffset] _RightTex ("Right (-X)", 2D) = "white" {}
        [NoScaleOffset] _UpTex ("Up (+Y)", 2D) = "white" {}
        [NoScaleOffset] _DownTex ("Down (-Y)", 2D) = "white" {}
    }

    SubShader {
        Tags {
            "Queue"="Background"
            "RenderType"="Background"
            "PreviewType"="Skybox"
        }

        Cull Off
        ZWrite Off
        Fog { Mode Off }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _FrontTex;
            fixed4 _Tint;

            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag (VertexOutput i) : SV_Target {
                fixed4 tex = tex2D(_FrontTex, i.uv);
                fixed4 col;
                col.rgb = tex.rgb * _Tint.rgb;
                col.a = tex.a * _Tint.a;
                return col;
            }
            ENDCG
        }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _BackTex;
            fixed4 _Tint;

            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag (VertexOutput i) : SV_Target {
                fixed4 tex = tex2D(_BackTex, i.uv);
                fixed4 col;
                col.rgb = tex.rgb * _Tint.rgb;
                col.a = tex.a * _Tint.a;
                return col;
            }
            ENDCG
        }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _LeftTex;
            fixed4 _Tint;

            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag (VertexOutput i) : SV_Target {
                fixed4 tex = tex2D(_LeftTex, i.uv);
                fixed4 col;
                col.rgb = tex.rgb * _Tint.rgb;
                col.a = tex.a * _Tint.a;
                return col;
            }
            ENDCG
        }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _RightTex;
            fixed4 _Tint;

            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag (VertexOutput i) : SV_Target {
                fixed4 tex = tex2D(_RightTex, i.uv);
                fixed4 col;
                col.rgb = tex.rgb * _Tint.rgb;
                col.a = tex.a * _Tint.a;
                return col;
            }
            ENDCG
        }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _UpTex;
            fixed4 _Tint;

            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag (VertexOutput i) : SV_Target {
                fixed4 tex = tex2D(_UpTex, i.uv);
                fixed4 col;
                col.rgb = tex.rgb * _Tint.rgb;
                col.a = tex.a * _Tint.a;
                return col;
            }
            ENDCG
        }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _DownTex;
            fixed4 _Tint;

            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag (VertexOutput i) : SV_Target {
                fixed4 tex = tex2D(_DownTex, i.uv);
                fixed4 col;
                col.rgb = tex.rgb * _Tint.rgb;
                col.a = tex.a * _Tint.a;
                return col;
            }
            ENDCG
        }
    }

    Fallback "Skybox/6 Sided"
}