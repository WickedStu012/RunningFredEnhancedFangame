Shader "GUI3D/DistanceFieldTextShader" {
Properties {
 _Color ("Color Tint", Color) = (1,1,1,1)
 _Threshold ("Threshold", Range(0,1)) = 0.5
 _StrokeColor ("Stroke Color", Color) = (1,1,1,1)
 _ShadowColor ("Shadow Color", Color) = (0,0,0,1)
 _MainTex ("SelfIllum Color (RGB) Alpha (A)", 2D) = "white" {}
 _Stroke ("Stroke", Range(0.5,1)) = 1
 _Gamma ("Gamma", Range(0,0.1)) = 0.01
 _StrokeGamma ("StrokeGamma", Range(0,0.1)) = 0
 _ShadowThreshold ("ShadowThreshold", Range(0,1)) = 1
 _ShadowSoftness ("ShadowSoftness", Range(0,0.5)) = 0.1
}
	//DummyShaderTextExporter
	
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard fullforwardshadows
#pragma target 3.0
		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}
		ENDCG
	}
}