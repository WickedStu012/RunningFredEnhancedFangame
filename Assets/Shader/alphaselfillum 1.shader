Shader "GUI/AlphaSelfIllum" {
Properties {
 _Color ("Color Tint", Color) = (1,1,1,1)
 _MainTex ("SelfIllum Color (RGB) Alpha (A)", 2D) = "white" {}
}
SubShader { 
 Tags { "QUEUE"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" }
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { ConstantColor [_Color] combine texture * constant }
 }
}
}