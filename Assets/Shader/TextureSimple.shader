Shader "MADFINGER/Diffuse/Simple" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "black" {}
}
SubShader { 
 Pass {
  SetTexture [_MainTex] { combine texture, texture alpha }
 }
}
}