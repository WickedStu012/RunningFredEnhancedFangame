Shader "RunningFred/PickupShineShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _ShineTex ("Shine (RGB)", 2D) = "black" {}
        _ShineValue ("Shine Value", Range(0,1)) = 0
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Fog { Mode Off }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _ShineTex;
            float4 _ShineTex_ST;
            float _ShineValue;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uvMain : TEXCOORD0;
                float2 uvShine : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uvMain = TRANSFORM_TEX(v.uv, _MainTex);
                o.uvShine = TRANSFORM_TEX(v.uv, _ShineTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 baseColor = tex2D(_MainTex, i.uvMain);
                fixed4 shineColor = tex2D(_ShineTex, i.uvShine);
                return baseColor + shineColor * _ShineValue;
            }
            ENDCG
        }
    }

    Fallback "Unlit/Texture"
}
