
Shader "VacuumShaders/Advanced Dissolve/Unlit" 
{
	Properties
	{ 
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" { }
		
		[Cutout]_Cutoff("   Alpha Cutoff", Range(0,1)) = 0.5
		[NoScaleOffset]_OcclusionMap("Occlusion", 2D) = "white" {}


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
		     
	//SM 3.0
	SubShader 
	{
		Tags { "RenderType" = "AdvancedDissolveCutout" "DisableBatching" = "True" }
		LOD 100 

		Cull[_Cull]

		Pass
		{ 

			Blend[_SrcBlend][_DstBlend]
			ZWrite[_ZWrite]


			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag 
			#pragma multi_compile_fog
			#pragma target 3.0
			#include "UnityCG.cginc"


			sampler2D _MainTex;
			float4 _MainTex_ST; 
			sampler2D _OcclusionMap; 
			fixed4 _Color; 
			fixed _Cutoff; 


#pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON

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
			 
			
			struct appdata_t    
			{
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;  
				float2 uv1 : TEXCOORD1; 
				float3 normal : NORMAL;
				 
				UNITY_VERTEX_INPUT_INSTANCE_ID 
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				UNITY_FOG_COORDS(1)

				float3 worldPos : TEXCOORD3;
				float3 normalWP : TEXCOORD4;
				
				ADVANCED_DISSOLVE_DATA(5)

				UNITY_VERTEX_OUTPUT_STEREO
			};
			  
			 
			  
			v2f vert(appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


				CURVED_WORLD_TRANSFORM_POINT(v. vertex)


				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv0.xy = TRANSFORM_TEX(v.uv0, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);


				//VacuumShaders 
				o.worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1)).xyz;
				o.normalWP = UnityObjectToWorldNormal(v.normal);

				ADVANCED_DISSOLVE_INIT_DATA(o.vertex, v.uv0, v.uv1, o);
				 
				return o;  
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//VacuumShaders
				float4 alpha = AdvancedDissolveGetAlpha(i.uv0, i.worldPos, i.normalWP, i.dissolveUV);
				DoDissolveClip(alpha);


				float3 dissolveAlbedo = 0;
float3 dissolveEmission = 0;
				float dissolveBlend = DoDissolveAlbedoEmission(alpha, dissolveAlbedo, dissolveEmission, i.uv0, 0);

				

			//	return float4 (i.uv0.xxx, 1);

				fixed4 col = tex2D(_MainTex, i.uv0) * _Color;

				float alphaFromSurface = col.a;
#ifdef _ALPHATEST_ON
				clip(alphaFromSurface - _Cutoff);
#endif

				//Albedo
				col.rgb = lerp(col.rgb, dissolveAlbedo, dissolveBlend);
				

				col.rgb *= tex2D(_OcclusionMap, i.uv0).rgb;


				//Emission
				col.rgb += dissolveEmission * dissolveBlend;


				UNITY_APPLY_FOG(i.fogCoord, col);
				
				return DoOutputForward(col, alphaFromSurface);
			}
			ENDCG
		}
	}


	//SM 2.0
	SubShader 
	{
		Tags { "RenderType" = "AdvancedDissolveCutout" "DisableBatching" = "True" }
		LOD 100 

		Cull[_Cull]

		Pass
		{ 

			Blend[_SrcBlend][_DstBlend]
			ZWrite[_ZWrite]


			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag 
			#pragma multi_compile_fog
			#pragma target 2.0
			#include "UnityCG.cginc"


			sampler2D _MainTex;
			float4 _MainTex_ST; 
			sampler2D _OcclusionMap; 
			fixed4 _Color; 
			fixed _Cutoff; 


#pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON

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
			 
			struct appdata_t    
			{ 
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;  
				float2 uv1 : TEXCOORD1; 
				float3 normal : NORMAL;
				 
				UNITY_VERTEX_INPUT_INSTANCE_ID 
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				UNITY_FOG_COORDS(1)

				float3 worldPos : TEXCOORD3;
				float3 normalWP : TEXCOORD4;
				
				ADVANCED_DISSOLVE_DATA(5)			

				UNITY_VERTEX_OUTPUT_STEREO
			};
			  
			 
			  
			v2f vert(appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


				CURVED_WORLD_TRANSFORM_POINT(v. vertex)


				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv0.xy = TRANSFORM_TEX(v.uv0, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);


				//VacuumShaders 
				o.worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1)).xyz;
				o.normalWP = UnityObjectToWorldNormal(v.normal);

				ADVANCED_DISSOLVE_INIT_DATA(o.vertex, v.uv0, v.uv1, o);
				  
				 
				return o;  
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//VacuumShaders
				float4 alpha = AdvancedDissolveGetAlpha(i.uv0, i.worldPos, i.normalWP, i.dissolveUV);
				DoDissolveClip(alpha);


				float3 dissolveAlbedo = 0;
				float3 dissolveEmission = 0;
				float dissolveBlend = DoDissolveAlbedoEmission(alpha, dissolveAlbedo, dissolveEmission, i.uv0, 0);
								

				fixed4 col = tex2D(_MainTex, i.uv0) * _Color;

#ifdef _ALPHATEST_ON
				clip(col.a - _Cutoff);
#endif

				//Albedo
				col.rgb = lerp(col.rgb, dissolveAlbedo, dissolveBlend);
				

				col.rgb *= tex2D(_OcclusionMap, i.uv0).rgb;


				//Emission
				col.rgb += dissolveEmission * dissolveBlend;


				UNITY_APPLY_FOG(i.fogCoord, col);
				
				return DoOutputForward(col, col.a);
			}
			ENDCG
		}
	}


	CustomEditor "VacuumShaders.AdvancedDissolve.UnlitGUI"
}
