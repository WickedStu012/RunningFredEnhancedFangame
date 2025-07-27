Shader "RunningFred/DiffuseSimpleFade"
{
    Properties
    {
        _MainTex ("Base Mask", 2D) = "black" {}
        _Alpha ("Alpha", Range(0, 1)) = 1
        _Brightness ("Brightness Control", Range(0.1, 2.0)) = 1.0
        _AmbientIntensity ("Ambient Intensity", Range(0.0, 1.0)) = 0.3
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Alpha;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                texColor.a = _Alpha;
                return texColor;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
