Shader "Hidden/VacuumShaders/Advanced Dissolve/Shadow_AlphaTest" 
{
	Properties  
	{ 		
		 _Color ("Main Color", Color) = (1,1,1,1)
		 _SpecColor ("Spec Color", Color) = (1,1,1,0)
		 _Shininess ("Shininess", Range(0.100000,1)) = 0.700000
		 _MainTex ("Base (RGB) Trans (A)", 2D) = "white" { }	
		[Cutout]_Cutoff("   Alpha Cutoff", Range(0,1)) = 0.5



		// Blending state
		[HideInInspector] _Mode("__mode", Float) = 0.0
		[HideInInspector] _SrcBlend("__src", Float) = 1.0
		[HideInInspector] _DstBlend("__dst", Float) = 0.0
		[HideInInspector] _ZWrite("__zw", Float) = 1.0

		[HideInInspector][MaterialEnum(Front,2,Back,1,Both,0)] _Cull("Face Cull", Int) = 0

		//Advanced Dissolve
		[HideInInspector] _DissolveCutoff("Dissolve", Range(0,1)) = 0.25
		
		//Mask
		[HideInInspector][KeywordEnum(None, XYZ Axis, Plane, Sphere, Box, Cylinder, Cone)]  _DissolveMask("Mak", Float) = 0
		[HideInInspector][Enum(X,0,Y,1,Z,2)]                                                _DissolveMaskAxis("Axis", Float) = 0
		[HideInInspector][Enum(World,0,Local,1)]                                            _DissolveMaskSpace("Space", Float) = 0	 
		[HideInInspector]																   _DissolveMaskOffset("Offset", Float) = 0
		[HideInInspector]																   _DissolveMaskInvert("Invert", Float) = 1		
		[HideInInspector][KeywordEnum(One, Two, Three, Four)]							   _DissolveMaskCount("Count", Float) = 0		
	
		[HideInInspector]  _DissolveMaskPosition("", Vector) = (0,0,0,0)
		[HideInInspector]  _DissolveMaskNormal("", Vector) = (1,0,0,0)
		[HideInInspector]  _DissolveMaskRadius("", Float) = 1

		//Alpha Source
		[HideInInspector] [KeywordEnum(Main Map Alpha, Custom Map, Two Custom Maps, Three Custom Maps)] _DissolveAlphaSource("Alpha Source", Float) = 0
		[HideInInspector] _DissolveMap1("", 2D) = "white" { }
		[HideInInspector] [UVScroll] _DissolveMap1_Scroll("", Vector) = (0,0,0,0)
		[HideInInspector] _DissolveMap1Intensity("", Range(0, 1)) = 1
		[HideInInspector] [Enum(Red, 0, Green, 1, Blue, 2, Alpha, 3)] _DissolveMap1Channel("", INT) = 3
		[HideInInspector] _DissolveMap2("", 2D) = "white" { }
		[HideInInspector] [UVScroll] _DissolveMap2_Scroll("", Vector) = (0,0,0,0)
		[HideInInspector] _DissolveMap2Intensity("", Range(0, 1)) = 1
	    [HideInInspector] [Enum(Red, 0, Green, 1, Blue, 2, Alpha, 3)] _DissolveMap2Channel("", INT) = 3
		[HideInInspector] _DissolveMap3("", 2D) = "white" { }
		[HideInInspector] [UVScroll] _DissolveMap3_Scroll("", Vector) = (0,0,0,0)
		[HideInInspector] _DissolveMap3Intensity("", Range(0, 1)) = 1
	    [HideInInspector] [Enum(Red, 0, Green, 1, Blue, 2, Alpha, 3)] _DissolveMap3Channel("", INT) = 3

		[HideInInspector][Enum(Multiply, 0, Add, 1)]  _DissolveSourceAlphaTexturesBlend("Texture Blend", Float) = 0
		[HideInInspector]							  _DissolveNoiseStrength("Noise", Float) = 0.1
		[HideInInspector][Enum(UV0,0,UV1,1)]          _DissolveAlphaSourceTexturesUVSet("UV Set", Float) = 0

		[HideInInspector][KeywordEnum(Normal, Triplanar, Screen Space)] _DissolveMappingType("Triplanar", Float) = 0
		[HideInInspector][Enum(World,0,Local,1)]                        _DissolveTriplanarMappingSpace("Mapping", Float) = 0	
		[HideInInspector]                                               _DissolveMainMapTiling("", FLOAT) = 1	

		//Edge
		[HideInInspector]                                       _DissolveEdgeWidth("Edge Size", Range(0,1)) = 0.25
		[HideInInspector][Enum(Cutout Source,0,Main Map,1)]     _DissolveEdgeDistortionSource("Distortion Source", Float) = 0
		[HideInInspector]                                       _DissolveEdgeDistortionStrength("Distortion Strength", Range(0, 2)) = 0

		//Color
		[HideInInspector]                _DissolveEdgeColor("Edge Color", Color) = (0,1,0,1)
		[HideInInspector][PositiveFloat] _DissolveEdgeColorIntensity("Intensity", FLOAT) = 0
		[HideInInspector][Enum(Solid,0,Smooth,1, Smooth Squared,2)]      _DissolveEdgeShape("Shape", INT) = 0

		[HideInInspector][KeywordEnum(None, Gradient, Main Map, Custom)] _DissolveEdgeTextureSource("", Float) = 0
		[HideInInspector][NoScaleOffset]								 _DissolveEdgeTexture("Edge Texture", 2D) = "white" { }
		[HideInInspector][Toggle]										 _DissolveEdgeTextureReverse("Reverse", FLOAT) = 0
		[HideInInspector]												 _DissolveEdgeTexturePhaseOffset("Offset", FLOAT) = 0
		[HideInInspector]												 _DissolveEdgeTextureAlphaOffset("Offset", Range(-1, 1)) = 0	
		[HideInInspector]												 _DissolveEdgeTextureMipmap("", Range(0, 10)) = 1		
		[HideInInspector][Toggle]										 _DissolveEdgeTextureIsDynamic("", Float) = 0

		[HideInInspector][PositiveFloat] _DissolveGIMultiplier("GI Strength", Float) = 1	
		
		//Global
		[HideInInspector][KeywordEnum(None, Mask Only, Mask And Edge, All)] _DissolveGlobalControl("Global Controll", Float) = 0

		//Meta
		[HideInInspector] _Dissolve_ObjectWorldPos("", Vector) = (0,0,0,0)		
	}


	SubShader 
	{ 
		LOD 100
		Tags { "RenderType"="AdvancedDissolveCutout" "DisableBatching" = "True" }
		Cull[_Cull]

	
		Pass 
		{
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
			ZWrite On ZTest LEqual


			CGPROGRAM
			// compile directives
			#pragma vertex vert_surf
			#pragma fragment frag_surf
			#pragma multi_compile_instancing
			#pragma multi_compile_shadowcaster
			#include "HLSLSupport.cginc"

			#ifndef UNITY_INSTANCED_LOD_FADE
			#define UNITY_INSTANCED_LOD_FADE
			#endif

			#ifndef UNITY_INSTANCED_SH
			#define UNITY_INSTANCED_SH
			#endif

			#ifndef UNITY_INSTANCED_LIGHTMAPSTS
			#define UNITY_INSTANCED_LIGHTMAPSTS
			#endif

			#include "UnityShaderVariables.cginc"
			#include "UnityShaderUtilities.cginc"
			// -------- variant for: <when no other keywords are defined>


			#include "UnityCG.cginc"
			#include "Lighting.cginc"






			uniform float4 _MainTex_ST;  
			uniform sampler2D _MainTex;
			uniform fixed4 _Color;
			fixed _Cutoff;


			//Always defined
			#define _ALPHATEST_ON
			//#pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON

			#pragma shader_feature_local _ _DISSOLVEGLOBALCONTROL_MASK_ONLY _DISSOLVEGLOBALCONTROL_MASK_AND_EDGE _DISSOLVEGLOBALCONTROL_ALL
			#pragma shader_feature_local _ _DISSOLVEMAPPINGTYPE_TRIPLANAR _DISSOLVEMAPPINGTYPE_SCREEN_SPACE
			#pragma shader_feature_local _ _DISSOLVEALPHASOURCE_CUSTOM_MAP _DISSOLVEALPHASOURCE_TWO_CUSTOM_MAPS _DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS
			#pragma shader_feature_local _ _DISSOLVEMASK_XYZ_AXIS _DISSOLVEMASK_PLANE _DISSOLVEMASK_SPHERE _DISSOLVEMASK_BOX _DISSOLVEMASK_CYLINDER _DISSOLVEMASK_CONE
			#pragma shader_feature_local _ _DISSOLVEEDGETEXTURESOURCE_GRADIENT _DISSOLVEEDGETEXTURESOURCE_MAIN_MAP _DISSOLVEEDGETEXTURESOURCE_CUSTOM
			#pragma shader_feature_local _ _DISSOLVEMASKCOUNT_TWO _DISSOLVEMASKCOUNT_THREE _DISSOLVEMASKCOUNT_FOUR
					

			#define DISSOLVE_LEGACY_RENDER_PIPELIN
			#define DISSOLVE_LEGACY_MAINTEX
			#define DISSOLVE_LEGACY_TEXTURE_SAMPLE

			#include "../cginc/AdvancedDissolve.cginc"
			#include "../cginc/Integration_CurvedWorld.cginc"



			// vertex-to-fragment interpolation data
			struct v2f_surf {
			V2F_SHADOW_CASTER;
			float2 uv : TEXCOORD1; // _MainTex
			float3 worldPos : TEXCOORD2;
			UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO


			float3 normalWP : TEXCOORD4;
				
			ADVANCED_DISSOLVE_DATA(5)	


			};

			// vertex shader
			v2f_surf vert_surf (appdata_full v) {
			UNITY_SETUP_INSTANCE_ID(v);
			v2f_surf o;
			UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
			UNITY_TRANSFER_INSTANCE_ID(v,o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);



			CURVED_WORLD_TRANSFORM_POINT_AND_NORMAL(v.vertex, v.normal, v.tangent)



			o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			float3 worldNormal = UnityObjectToWorldNormal(v.normal);
			o.worldPos.xyz = worldPos;
			TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)


			//VacuumShaders
			o.worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1)).xyz;
			o.normalWP = UnityObjectToWorldNormal(v.normal);

			ADVANCED_DISSOLVE_INIT_DATA(UnityObjectToClipPos(v.vertex), v.texcoord, v.texcoord1, o);


			return o;
			}

			// fragment shader
			fixed4 frag_surf (v2f_surf IN) : SV_Target {
			UNITY_SETUP_INSTANCE_ID(IN);
			// prepare and unpack data

			//No shadow on transparent shaders
			#ifdef _ALPHABLEND_ON
				clip(-1);
				return 0;
			#endif

			float4 alpha = AdvancedDissolveGetAlpha(IN.uv.xy, IN.worldPos, IN.normalWP, IN.dissolveUV);
			DoDissolveClip(alpha);


			#ifdef _ALPHATEST_ON
				clip(tex2D(_MainTex, IN.uv.xy).a * _Color.a - _Cutoff * 1.01);
			#endif

			SHADOW_CASTER_FRAGMENT(IN)
			}



			ENDCG
		 }
	}


CustomEditor "VacuumShaders.AdvancedDissolve.ShadowGUI"
} 