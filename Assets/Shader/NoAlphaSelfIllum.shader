Shader "GUI/NoAlphaSelfIllum" {
Properties {
 _Color ("Color Tint", Color) = (1,1,1,1)
 _MainTex ("SelfIllum Color (RGB) Alpha (A)", 2D) = "white" {}
}
SubShader { 
 Pass {
  Fog { Mode Off }
  SetTexture [_MainTex] { ConstantColor [_Color] combine texture * constant }
 }
}
}