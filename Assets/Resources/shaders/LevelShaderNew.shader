Shader "RunningFredRedux/LevelShader" {
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
            #pragma multi_compile_fwdadd_fullshadows

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

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
                float3 worldPos : TEXCOORD3;
                float4 vertex : SV_POSITION;
                SHADOW_COORDS(4)
            };

            v2f vert (appdata v) {
                v2f o;
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = worldNormal;
                o.viewDir = normalize(_WorldSpaceCameraPos - worldPos); // Correct direction: camera - model
                o.worldPos = worldPos;
                o.vertex = UnityObjectToClipPos(v.vertex);
                TRANSFER_SHADOW(o);
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
                float shadow = SHADOW_ATTENUATION(i);
                
                // Separate direct lighting and ambient lighting
                float3 directLighting = brdf * _LightColor0.rgb * shadow;
                float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb;
                
                // Combine direct and ambient lighting
                float3 lighting = directLighting + ambientLighting;

                fixed4 col;
                col.rgb = lighting * albedo;
                col.a = 1.0;
                return col;
            }
            ENDCG
        }
        
        // ForwardAdd pass for additional lights (point lights, spot lights)
        Pass {
            Name "FORWARD_ADD"
            Tags { "LightMode" = "ForwardAdd" }
            Blend One One
            ZWrite Off
            ZTest LEqual
            Cull Back
            ColorMask RGB
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdadd_fullshadows
            
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
            
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
                float3 worldPos : TEXCOORD3;
                float4 vertex : SV_POSITION;
                SHADOW_COORDS(4)
            };

            v2f vert (appdata v) {
                v2f o;
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = worldNormal;
                o.viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                o.worldPos = worldPos;
                o.vertex = UnityObjectToClipPos(v.vertex);
                TRANSFER_SHADOW(o);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target {
                float3 N = normalize(i.normal);
                float3 V = normalize(i.viewDir);
                float3 L = normalize(_WorldSpaceLightPos0.xyz - i.worldPos * _WorldSpaceLightPos0.w);
                
                float ndotl = saturate(dot(N, L));
                float ndotv = saturate(dot(N, V));
                
                float2 brdfUV;
                brdfUV.x = ndotl * 0.5 * 0.5;
                brdfUV.y = dot(N, V);
                
                float3 albedo = tex2D(_MainTex, i.uv).rgb;
                float3 brdf = tex2D(_BRDFTex, brdfUV).rgb;
                float shadow = SHADOW_ATTENUATION(i);
                
                // Calculate light attenuation for point/spot lights
                float attenuation = 1.0;
                #ifdef USING_DIRECTIONAL_LIGHT
                    attenuation = 1.0;
                #else
                    float3 lightCoord = mul(unity_WorldToLight, float4(i.worldPos, 1)).xyz;
                    attenuation = tex2D(_LightTexture0, dot(lightCoord, lightCoord).rr).UNITY_ATTEN_CHANNEL;
                #endif
                
                // For additional lights, we only apply the light contribution (no ambient)
                float3 lighting = brdf * _LightColor0.rgb * shadow * attenuation;
                
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
