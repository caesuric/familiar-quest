// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.18 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.18;sub:START;pass:START;ps:flbk:Transparent/Cutout/Diffuse,iptp:0,cusa:False,bamd:0,lico:0,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:0,x:34116,y:32616,varname:node_0,prsc:2|diff-6062-RGB,clip-2162-OUT,voffset-4446-OUT;n:type:ShaderForge.SFN_Panner,id:2526,x:32155,y:32478,varname:node_2526,prsc:2,spu:0.02,spv:0;n:type:ShaderForge.SFN_Tex2d,id:6062,x:33526,y:32411,ptovrint:False,ptlb:Base Color,ptin:_BaseColor,varname:node_6062,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:53c365404778d9846b38b63b06368a9c,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:2212,x:33129,y:32734,varname:_node_6062_copy,prsc:2,tex:c8a1772124bc25946a5475b7646701a9,ntxv:0,isnm:False|UVIN-631-OUT,TEX-7636-TEX;n:type:ShaderForge.SFN_Panner,id:6997,x:32155,y:32617,varname:node_6997,prsc:2,spu:0,spv:0.01;n:type:ShaderForge.SFN_Tex2d,id:5166,x:33281,y:32541,ptovrint:False,ptlb:Wind Mask (A),ptin:_WindMaskA,varname:node_5166,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b3c7e7107c0e6b64da556d4bf1a175ee,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:4446,x:33805,y:32950,varname:node_4446,prsc:2|A-5166-A,B-9116-OUT;n:type:ShaderForge.SFN_Slider,id:4665,x:32955,y:33160,ptovrint:False,ptlb:Distort,ptin:_Distort,varname:node_4665,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.4,max:1;n:type:ShaderForge.SFN_Slider,id:6932,x:32478,y:32856,ptovrint:False,ptlb:Speed,ptin:_Speed,varname:node_6932,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:5,max:10;n:type:ShaderForge.SFN_OneMinus,id:8435,x:32531,y:32499,varname:node_8435,prsc:2|IN-2526-UVOUT;n:type:ShaderForge.SFN_OneMinus,id:7510,x:32531,y:32639,varname:node_7510,prsc:2|IN-6997-UVOUT;n:type:ShaderForge.SFN_Add,id:3173,x:32710,y:32662,varname:node_3173,prsc:2|A-8435-OUT,B-7510-OUT;n:type:ShaderForge.SFN_Multiply,id:631,x:32857,y:32818,varname:node_631,prsc:2|A-3173-OUT,B-6932-OUT;n:type:ShaderForge.SFN_Vector3,id:6790,x:33001,y:32945,varname:node_6790,prsc:2,v1:0,v2:0,v3:0;n:type:ShaderForge.SFN_Lerp,id:9116,x:33640,y:32996,varname:node_9116,prsc:2|A-6790-OUT,B-6575-OUT,T-4665-OUT;n:type:ShaderForge.SFN_Color,id:5929,x:33300,y:32724,ptovrint:False,ptlb:Direction,ptin:_Direction,varname:node_5929,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:-1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:6575,x:33418,y:32887,varname:node_6575,prsc:2|A-5929-RGB,B-2212-RGB;n:type:ShaderForge.SFN_Tex2dAsset,id:7636,x:32833,y:32560,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_7636,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c8a1772124bc25946a5475b7646701a9,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:6504,x:33583,y:32830,ptovrint:False,ptlb:Alpha Clip,ptin:_AlphaClip,varname:_AlphaClip_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:4;n:type:ShaderForge.SFN_Power,id:2162,x:33925,y:32722,varname:node_2162,prsc:2|VAL-6062-A,EXP-6504-OUT;proporder:6062-6504-5166-4665-6932-5929-7636;pass:END;sub:END;*/

Shader "MK4/Vegetation" {
    Properties {
        _BaseColor ("Base Color", 2D) = "white" {}
        _AlphaClip ("Alpha Clip", Range(0, 4)) = 1
        _WindMaskA ("Wind Mask (A)", 2D) = "white" {}
        _Distort ("Distort", Range(0, 1)) = 0.4
        _Speed ("Speed", Range(0, 10)) = 5
        _Direction ("Direction", Color) = (-1,1,1,1)
        _Noise ("Noise", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles xbox360 ps3 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform sampler2D _BaseColor; uniform float4 _BaseColor_ST;
            uniform sampler2D _WindMaskA; uniform float4 _WindMaskA_ST;
            uniform float _Distort;
            uniform float _Speed;
            uniform float4 _Direction;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _AlphaClip;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD10;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #elif UNITY_SHOULD_SAMPLE_SH
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 _WindMaskA_var = tex2Dlod(_WindMaskA,float4(TRANSFORM_TEX(o.uv0, _WindMaskA),0.0,0));
                float4 node_1357 = _Time + _TimeEditor;
                float2 node_631 = (((1.0 - (o.uv0+node_1357.g*float2(0.02,0)))+(1.0 - (o.uv0+node_1357.g*float2(0,0.01))))*_Speed);
                float4 _node_6062_copy = tex2Dlod(_Noise,float4(TRANSFORM_TEX(node_631, _Noise),0.0,0));
                v.vertex.xyz += (_WindMaskA_var.a*lerp(float3(0,0,0),(_Direction.rgb*_node_6062_copy.rgb),_Distort));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _BaseColor_var = tex2D(_BaseColor,TRANSFORM_TEX(i.uv0, _BaseColor));
                clip(pow(_BaseColor_var.a,_AlphaClip) - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                UnityGI gi = UnityGlobalIllumination (d, 1, 0, normalDirection);
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float3 diffuseColor = _BaseColor_var.rgb;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles xbox360 ps3 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform sampler2D _BaseColor; uniform float4 _BaseColor_ST;
            uniform sampler2D _WindMaskA; uniform float4 _WindMaskA_ST;
            uniform float _Distort;
            uniform float _Speed;
            uniform float4 _Direction;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _AlphaClip;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float2 uv1 : TEXCOORD2;
                float2 uv2 : TEXCOORD3;
                float4 posWorld : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                float4 _WindMaskA_var = tex2Dlod(_WindMaskA,float4(TRANSFORM_TEX(o.uv0, _WindMaskA),0.0,0));
                float4 node_2223 = _Time + _TimeEditor;
                float2 node_631 = (((1.0 - (o.uv0+node_2223.g*float2(0.02,0)))+(1.0 - (o.uv0+node_2223.g*float2(0,0.01))))*_Speed);
                float4 _node_6062_copy = tex2Dlod(_Noise,float4(TRANSFORM_TEX(node_631, _Noise),0.0,0));
                v.vertex.xyz += (_WindMaskA_var.a*lerp(float3(0,0,0),(_Direction.rgb*_node_6062_copy.rgb),_Distort));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 _BaseColor_var = tex2D(_BaseColor,TRANSFORM_TEX(i.uv0, _BaseColor));
                clip(pow(_BaseColor_var.a,_AlphaClip) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles xbox360 ps3 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform sampler2D _BaseColor; uniform float4 _BaseColor_ST;
            uniform sampler2D _WindMaskA; uniform float4 _WindMaskA_ST;
            uniform float _Distort;
            uniform float _Speed;
            uniform float4 _Direction;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                float4 _WindMaskA_var = tex2Dlod(_WindMaskA,float4(TRANSFORM_TEX(o.uv0, _WindMaskA),0.0,0));
                float4 node_7196 = _Time + _TimeEditor;
                float2 node_631 = (((1.0 - (o.uv0+node_7196.g*float2(0.02,0)))+(1.0 - (o.uv0+node_7196.g*float2(0,0.01))))*_Speed);
                float4 _node_6062_copy = tex2Dlod(_Noise,float4(TRANSFORM_TEX(node_631, _Noise),0.0,0));
                v.vertex.xyz += (_WindMaskA_var.a*lerp(float3(0,0,0),(_Direction.rgb*_node_6062_copy.rgb),_Distort));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                o.Emission = 0;
                
                float4 _BaseColor_var = tex2D(_BaseColor,TRANSFORM_TEX(i.uv0, _BaseColor));
                float3 diffColor = _BaseColor_var.rgb;
                o.Albedo = diffColor;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Transparent/Cutout/Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
