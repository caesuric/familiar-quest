// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.17 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.17;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:1,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:2865,x:32719,y:32712,varname:node_2865,prsc:2|diff-6343-OUT,spec-358-OUT,gloss-1813-OUT,normal-5964-RGB,clip-6930-OUT,voffset-996-OUT;n:type:ShaderForge.SFN_Multiply,id:6343,x:32109,y:32641,varname:node_6343,prsc:2|A-8019-RGB,B-6665-RGB;n:type:ShaderForge.SFN_Color,id:6665,x:31917,y:32849,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:5964,x:32407,y:32978,ptovrint:True,ptlb:Normal Map,ptin:_BumpMap,varname:_BumpMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Slider,id:358,x:32294,y:32406,ptovrint:False,ptlb:Metallic,ptin:_Metallic,varname:node_358,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:1813,x:32294,y:32505,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:_Metallic_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:8353,x:30903,y:32900,ptovrint:False,ptlb:Distort,ptin:_Distort,varname:node_4665,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.4,max:1;n:type:ShaderForge.SFN_Vector3,id:5657,x:30949,y:32685,varname:node_5657,prsc:2,v1:0,v2:0,v3:0;n:type:ShaderForge.SFN_Tex2d,id:1288,x:31077,y:32474,varname:_node_6062_copy,prsc:2,tex:c8a1772124bc25946a5475b7646701a9,ntxv:0,isnm:False|UVIN-8254-OUT,TEX-1900-TEX;n:type:ShaderForge.SFN_Color,id:7746,x:31248,y:32464,ptovrint:False,ptlb:Direction,ptin:_Direction,varname:node_5929,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:-1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:2177,x:31366,y:32627,varname:node_2177,prsc:2|A-7746-RGB,B-1288-RGB;n:type:ShaderForge.SFN_Lerp,id:4162,x:31588,y:32736,varname:node_4162,prsc:2|A-5657-OUT,B-2177-OUT,T-8353-OUT;n:type:ShaderForge.SFN_Multiply,id:996,x:31753,y:32690,varname:node_996,prsc:2|A-7516-A,B-4162-OUT;n:type:ShaderForge.SFN_Tex2d,id:7516,x:31227,y:32281,ptovrint:False,ptlb:Wind Mask (A),ptin:_WindMaskA,varname:node_5166,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b3c7e7107c0e6b64da556d4bf1a175ee,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:8019,x:31886,y:32474,ptovrint:False,ptlb:Base Color,ptin:_BaseColor,varname:node_6062,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:53c365404778d9846b38b63b06368a9c,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2dAsset,id:1900,x:30820,y:32255,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_7636,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c8a1772124bc25946a5475b7646701a9,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:8254,x:30801,y:32558,varname:node_8254,prsc:2|A-6810-OUT,B-4939-OUT;n:type:ShaderForge.SFN_Slider,id:4939,x:30422,y:32596,ptovrint:False,ptlb:Speed,ptin:_Speed,varname:node_6932,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:5,max:10;n:type:ShaderForge.SFN_Add,id:6810,x:30654,y:32402,varname:node_6810,prsc:2|A-8050-OUT,B-7403-OUT;n:type:ShaderForge.SFN_OneMinus,id:7403,x:30475,y:32379,varname:node_7403,prsc:2|IN-1491-UVOUT;n:type:ShaderForge.SFN_OneMinus,id:8050,x:30475,y:32239,varname:node_8050,prsc:2|IN-5819-UVOUT;n:type:ShaderForge.SFN_Panner,id:5819,x:30099,y:32218,varname:node_5819,prsc:2,spu:0.02,spv:0;n:type:ShaderForge.SFN_Panner,id:1491,x:30099,y:32357,varname:node_1491,prsc:2,spu:0,spv:0.01;n:type:ShaderForge.SFN_Slider,id:9482,x:32073,y:32924,ptovrint:False,ptlb:Alpha Clip,ptin:_AlphaClip,varname:node_9482,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:4;n:type:ShaderForge.SFN_Power,id:6930,x:32380,y:32740,varname:node_6930,prsc:2|VAL-8019-A,EXP-9482-OUT;proporder:358-1813-6665-8019-9482-5964-8353-7746-7516-1900-4939;pass:END;sub:END;*/

Shader "MK4/Vegetation pbr" {
    Properties {
        _Metallic ("Metallic", Range(0, 1)) = 0
        _Gloss ("Gloss", Range(0, 1)) = 0
        _Color ("Color", Color) = (1,1,1,1)
        _BaseColor ("Base Color", 2D) = "white" {}
        _AlphaClip ("Alpha Clip", Range(0, 4)) = 1
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _Distort ("Distort", Range(0, 1)) = 0.4
        _Direction ("Direction", Color) = (-1,1,1,1)
        _WindMaskA ("Wind Mask (A)", 2D) = "white" {}
        _Noise ("Noise", 2D) = "white" {}
        _Speed ("Speed", Range(0, 10)) = 5
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "DEFERRED"
            Tags {
                "LightMode"="Deferred"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_DEFERRED
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile ___ UNITY_HDR_ON
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform float _Distort;
            uniform float4 _Direction;
            uniform sampler2D _WindMaskA; uniform float4 _WindMaskA_ST;
            uniform sampler2D _BaseColor; uniform float4 _BaseColor_ST;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _Speed;
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
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD7;
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
            float4 node_6513 = _Time + _TimeEditor;
            float2 node_8254 = (((1.0 - (o.uv0+node_6513.g*float2(0.02,0)))+(1.0 - (o.uv0+node_6513.g*float2(0,0.01))))*_Speed);
            float4 _node_6062_copy = tex2Dlod(_Noise,float4(TRANSFORM_TEX(node_8254, _Noise),0.0,0));
            v.vertex.xyz += (_WindMaskA_var.a*lerp(float3(0,0,0),(_Direction.rgb*_node_6062_copy.rgb),_Distort));
            o.posWorld = mul(unity_ObjectToWorld, v.vertex);
            o.pos = UnityObjectToClipPos(v.vertex);
            return o;
        }
        void frag(
            VertexOutput i,
            out half4 outDiffuse : SV_Target0,
            out half4 outSpecSmoothness : SV_Target1,
            out half4 outNormal : SV_Target2,
            out half4 outEmission : SV_Target3 )
        {
            i.normalDir = normalize(i.normalDir);
            float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/// Vectors:
            float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
            float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
            float3 normalLocal = _BumpMap_var.rgb;
            float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
            float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
            float4 _BaseColor_var = tex2D(_BaseColor,TRANSFORM_TEX(i.uv0, _BaseColor));
            clip(pow(_BaseColor_var.a,_AlphaClip) - 0.5);
// Lighting:
            float Pi = 3.141592654;
            float InvPi = 0.31830988618;
///// Gloss:
            float gloss = _Gloss;
/// GI Data:
            UnityLight light; // Dummy light
            light.color = 0;
            light.dir = half3(0,1,0);
            light.ndotl = max(0,dot(normalDirection,light.dir));
            UnityGIInput d;
            d.light = light;
            d.worldPos = i.posWorld.xyz;
            d.worldViewDir = viewDirection;
            d.atten = 1;
            #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                d.ambient = 0;
                d.lightmapUV = i.ambientOrLightmapUV;
            #else
                d.ambient = i.ambientOrLightmapUV;
            #endif
            d.boxMax[0] = unity_SpecCube0_BoxMax;
            d.boxMin[0] = unity_SpecCube0_BoxMin;
            d.probePosition[0] = unity_SpecCube0_ProbePosition;
            d.probeHDR[0] = unity_SpecCube0_HDR;
            d.boxMax[1] = unity_SpecCube1_BoxMax;
            d.boxMin[1] = unity_SpecCube1_BoxMin;
            d.probePosition[1] = unity_SpecCube1_ProbePosition;
            d.probeHDR[1] = unity_SpecCube1_HDR;
            UnityGI gi = UnityGlobalIllumination (d, 1, gloss, normalDirection);
// Specular:
            float3 diffuseColor = (_BaseColor_var.rgb*_Color.rgb); // Need this for specular when using metallic
            float specularMonochrome;
            float3 specularColor;
            diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, _Metallic, specularColor, specularMonochrome );
            specularMonochrome = 1-specularMonochrome;
            float NdotV = max(0.0,dot( normalDirection, viewDirection ));
            half grazingTerm = saturate( gloss + specularMonochrome );
            float3 indirectSpecular = (gi.indirect.specular);
            indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
/// Diffuse:
            float3 indirectDiffuse = float3(0,0,0);
            indirectDiffuse += gi.indirect.diffuse;
// Final Color:
            outDiffuse = half4( diffuseColor, 1 );
            outSpecSmoothness = half4( specularColor, gloss );
            outNormal = half4( normalDirection * 0.5 + 0.5, 1 );
            outEmission = half4(0,0,0,1);
            outEmission.rgb += indirectSpecular * 1;
            outEmission.rgb += indirectDiffuse * diffuseColor;
            #ifndef UNITY_HDR_ON
                outEmission.rgb = exp2(-outEmission.rgb);
            #endif
        }
        ENDCG
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
        #define _GLOSSYENV 1
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
        #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
        #pragma target 3.0
        #pragma glsl
        uniform float4 _TimeEditor;
        uniform float4 _Color;
        uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
        uniform float _Metallic;
        uniform float _Gloss;
        uniform float _Distort;
        uniform float4 _Direction;
        uniform sampler2D _WindMaskA; uniform float4 _WindMaskA_ST;
        uniform sampler2D _BaseColor; uniform float4 _BaseColor_ST;
        uniform sampler2D _Noise; uniform float4 _Noise_ST;
        uniform float _Speed;
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
        float4 node_3442 = _Time + _TimeEditor;
        float2 node_8254 = (((1.0 - (o.uv0+node_3442.g*float2(0.02,0)))+(1.0 - (o.uv0+node_3442.g*float2(0,0.01))))*_Speed);
        float4 _node_6062_copy = tex2Dlod(_Noise,float4(TRANSFORM_TEX(node_8254, _Noise),0.0,0));
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
// Vectors:
        float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
        float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
        float3 normalLocal = _BumpMap_var.rgb;
        float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
        float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
        float4 _BaseColor_var = tex2D(_BaseColor,TRANSFORM_TEX(i.uv0, _BaseColor));
        clip(pow(_BaseColor_var.a,_AlphaClip) - 0.5);
        float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
        float3 lightColor = _LightColor0.rgb;
        float3 halfDirection = normalize(viewDirection+lightDirection);
// Lighting:
        float attenuation = LIGHT_ATTENUATION(i);
        float3 attenColor = attenuation * _LightColor0.xyz;
        float Pi = 3.141592654;
        float InvPi = 0.31830988618;
// Gloss:
        float gloss = _Gloss;
        float specPow = exp2( gloss * 10.0+1.0);
// GI Data:
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
        d.boxMax[0] = unity_SpecCube0_BoxMax;
        d.boxMin[0] = unity_SpecCube0_BoxMin;
        d.probePosition[0] = unity_SpecCube0_ProbePosition;
        d.probeHDR[0] = unity_SpecCube0_HDR;
        d.boxMax[1] = unity_SpecCube1_BoxMax;
        d.boxMin[1] = unity_SpecCube1_BoxMin;
        d.probePosition[1] = unity_SpecCube1_ProbePosition;
        d.probeHDR[1] = unity_SpecCube1_HDR;
        UnityGI gi = UnityGlobalIllumination (d, 1, gloss, normalDirection);
        lightDirection = gi.light.dir;
        lightColor = gi.light.color;
// Specular:
        float NdotL = max(0, dot( normalDirection, lightDirection ));
        float LdotH = max(0.0,dot(lightDirection, halfDirection));
        float3 diffuseColor = (_BaseColor_var.rgb*_Color.rgb); // Need this for specular when using metallic
        float specularMonochrome;
        float3 specularColor;
        diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, _Metallic, specularColor, specularMonochrome );
        specularMonochrome = 1-specularMonochrome;
        float NdotV = max(0.0,dot( normalDirection, viewDirection ));
        float NdotH = max(0.0,dot( normalDirection, halfDirection ));
        float VdotH = max(0.0,dot( viewDirection, halfDirection ));
        float visTerm = SmithBeckmannVisibilityTerm( NdotL, NdotV, 1.0-gloss );
        float normTerm = max(0.0, NDFBlinnPhongNormalizedTerm(NdotH, RoughnessToSpecPower(1.0-gloss)));
        float specularPBL = max(0, (NdotL*visTerm*normTerm) * unity_LightGammaCorrectionConsts_PIDiv4 );
        float3 directSpecular = 1 * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
        half grazingTerm = saturate( gloss + specularMonochrome );
        float3 indirectSpecular = (gi.indirect.specular);
        indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
        float3 specular = (directSpecular + indirectSpecular);
// Diffuse:
        NdotL = max(0.0,dot( normalDirection, lightDirection ));
        half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
        float3 directDiffuse = ((1 +(fd90 - 1)*pow((1.00001-NdotL), 5)) * (1 + (fd90 - 1)*pow((1.00001-NdotV), 5)) * NdotL) * attenColor;
        float3 indirectDiffuse = float3(0,0,0);
        indirectDiffuse += gi.indirect.diffuse;
        float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
// Final Color:
        float3 finalColor = diffuse + specular;
        fixed4 finalRGBA = fixed4(finalColor,1);
        UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
        return finalRGBA;
    }
    ENDCG
}
Pass {
    Name "FORWARD_DELTA"
    Tags {
        "LightMode"="ForwardAdd"
    }
    Blend One One
    
    
    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #define UNITY_PASS_FORWARDADD
    #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
    #define _GLOSSYENV 1
    #include "UnityCG.cginc"
    #include "AutoLight.cginc"
    #include "Lighting.cginc"
    #include "UnityPBSLighting.cginc"
    #include "UnityStandardBRDF.cginc"
    #pragma multi_compile_fwdadd_fullshadows
    #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
    #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
    #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
    #pragma multi_compile_fog
    #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
    #pragma target 3.0
    #pragma glsl
    uniform float4 _TimeEditor;
    uniform float4 _Color;
    uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
    uniform float _Metallic;
    uniform float _Gloss;
    uniform float _Distort;
    uniform float4 _Direction;
    uniform sampler2D _WindMaskA; uniform float4 _WindMaskA_ST;
    uniform sampler2D _BaseColor; uniform float4 _BaseColor_ST;
    uniform sampler2D _Noise; uniform float4 _Noise_ST;
    uniform float _Speed;
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
    };
    VertexOutput vert (VertexInput v) {
        VertexOutput o = (VertexOutput)0;
        o.uv0 = v.texcoord0;
        o.uv1 = v.texcoord1;
        o.uv2 = v.texcoord2;
        o.normalDir = UnityObjectToWorldNormal(v.normal);
        o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
        o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
        float4 _WindMaskA_var = tex2Dlod(_WindMaskA,float4(TRANSFORM_TEX(o.uv0, _WindMaskA),0.0,0));
        float4 node_5444 = _Time + _TimeEditor;
        float2 node_8254 = (((1.0 - (o.uv0+node_5444.g*float2(0.02,0)))+(1.0 - (o.uv0+node_5444.g*float2(0,0.01))))*_Speed);
        float4 _node_6062_copy = tex2Dlod(_Noise,float4(TRANSFORM_TEX(node_8254, _Noise),0.0,0));
        v.vertex.xyz += (_WindMaskA_var.a*lerp(float3(0,0,0),(_Direction.rgb*_node_6062_copy.rgb),_Distort));
        o.posWorld = mul(unity_ObjectToWorld, v.vertex);
        float3 lightColor = _LightColor0.rgb;
        o.pos = UnityObjectToClipPos(v.vertex);
        TRANSFER_VERTEX_TO_FRAGMENT(o)
        return o;
    }
    float4 frag(VertexOutput i) : COLOR {
        i.normalDir = normalize(i.normalDir);
        float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
// Vectors:
        float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
        float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
        float3 normalLocal = _BumpMap_var.rgb;
        float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
        float4 _BaseColor_var = tex2D(_BaseColor,TRANSFORM_TEX(i.uv0, _BaseColor));
        clip(pow(_BaseColor_var.a,_AlphaClip) - 0.5);
        float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
        float3 lightColor = _LightColor0.rgb;
        float3 halfDirection = normalize(viewDirection+lightDirection);
// Lighting:
        float attenuation = LIGHT_ATTENUATION(i);
        float3 attenColor = attenuation * _LightColor0.xyz;
        float Pi = 3.141592654;
        float InvPi = 0.31830988618;
// Gloss:
        float gloss = _Gloss;
        float specPow = exp2( gloss * 10.0+1.0);
// Specular:
        float NdotL = max(0, dot( normalDirection, lightDirection ));
        float LdotH = max(0.0,dot(lightDirection, halfDirection));
        float3 diffuseColor = (_BaseColor_var.rgb*_Color.rgb); // Need this for specular when using metallic
        float specularMonochrome;
        float3 specularColor;
        diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, _Metallic, specularColor, specularMonochrome );
        specularMonochrome = 1-specularMonochrome;
        float NdotV = max(0.0,dot( normalDirection, viewDirection ));
        float NdotH = max(0.0,dot( normalDirection, halfDirection ));
        float VdotH = max(0.0,dot( viewDirection, halfDirection ));
        float visTerm = SmithBeckmannVisibilityTerm( NdotL, NdotV, 1.0-gloss );
        float normTerm = max(0.0, NDFBlinnPhongNormalizedTerm(NdotH, RoughnessToSpecPower(1.0-gloss)));
        float specularPBL = max(0, (NdotL*visTerm*normTerm) * unity_LightGammaCorrectionConsts_PIDiv4 );
        float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
        float3 specular = directSpecular;
// Diffuse:
        NdotL = max(0.0,dot( normalDirection, lightDirection ));
        half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
        float3 directDiffuse = ((1 +(fd90 - 1)*pow((1.00001-NdotL), 5)) * (1 + (fd90 - 1)*pow((1.00001-NdotV), 5)) * NdotL) * attenColor;
        float3 diffuse = directDiffuse * diffuseColor;
// Final Color:
        float3 finalColor = diffuse + specular;
        return fixed4(finalColor * 1,0);
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
    #define _GLOSSYENV 1
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
    #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
    #pragma target 3.0
    #pragma glsl
    uniform float4 _TimeEditor;
    uniform float _Distort;
    uniform float4 _Direction;
    uniform sampler2D _WindMaskA; uniform float4 _WindMaskA_ST;
    uniform sampler2D _BaseColor; uniform float4 _BaseColor_ST;
    uniform sampler2D _Noise; uniform float4 _Noise_ST;
    uniform float _Speed;
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
        float4 node_2047 = _Time + _TimeEditor;
        float2 node_8254 = (((1.0 - (o.uv0+node_2047.g*float2(0.02,0)))+(1.0 - (o.uv0+node_2047.g*float2(0,0.01))))*_Speed);
        float4 _node_6062_copy = tex2Dlod(_Noise,float4(TRANSFORM_TEX(node_8254, _Noise),0.0,0));
        v.vertex.xyz += (_WindMaskA_var.a*lerp(float3(0,0,0),(_Direction.rgb*_node_6062_copy.rgb),_Distort));
        o.posWorld = mul(unity_ObjectToWorld, v.vertex);
        o.pos = UnityObjectToClipPos(v.vertex);
        TRANSFER_SHADOW_CASTER(o)
        return o;
    }
    float4 frag(VertexOutput i) : COLOR {
// Vectors:
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
    #define _GLOSSYENV 1
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
    #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
    #pragma target 3.0
    #pragma glsl
    uniform float4 _TimeEditor;
    uniform float4 _Color;
    uniform float _Metallic;
    uniform float _Gloss;
    uniform float _Distort;
    uniform float4 _Direction;
    uniform sampler2D _WindMaskA; uniform float4 _WindMaskA_ST;
    uniform sampler2D _BaseColor; uniform float4 _BaseColor_ST;
    uniform sampler2D _Noise; uniform float4 _Noise_ST;
    uniform float _Speed;
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
        float4 node_115 = _Time + _TimeEditor;
        float2 node_8254 = (((1.0 - (o.uv0+node_115.g*float2(0.02,0)))+(1.0 - (o.uv0+node_115.g*float2(0,0.01))))*_Speed);
        float4 _node_6062_copy = tex2Dlod(_Noise,float4(TRANSFORM_TEX(node_8254, _Noise),0.0,0));
        v.vertex.xyz += (_WindMaskA_var.a*lerp(float3(0,0,0),(_Direction.rgb*_node_6062_copy.rgb),_Distort));
        o.posWorld = mul(unity_ObjectToWorld, v.vertex);
        o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
        return o;
    }
    float4 frag(VertexOutput i) : SV_Target {
// Vectors:
        float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
        UnityMetaInput o;
        UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
        
        o.Emission = 0;
        
        float4 _BaseColor_var = tex2D(_BaseColor,TRANSFORM_TEX(i.uv0, _BaseColor));
        float3 diffColor = (_BaseColor_var.rgb*_Color.rgb);
        float specularMonochrome;
        float3 specColor;
        diffColor = DiffuseAndSpecularFromMetallic( diffColor, _Metallic, specColor, specularMonochrome );
        float roughness = 1.0 - _Gloss;
        o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
        
        return UnityMetaFragment( o );
    }
    ENDCG
}
}
FallBack "Diffuse"
CustomEditor "ShaderForgeMaterialInspector"
}
