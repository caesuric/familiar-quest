Shader "VacuumShaders/Advanced Dissolve/VertexLit" 
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

	//SM 3.0
	SubShader 
	{ 
		 LOD 100
		 Tags { "IGNOREPROJECTOR"="true" "RenderType"="AdvancedDissolveCutout" "DisableBatching" = "True" }
		Cull[_Cull]

		 Pass 
		 {
		  Tags { "LIGHTMODE"="Vertex"  "IGNOREPROJECTOR"="true" }

			Blend[_SrcBlend][_DstBlend]
			ZWrite[_ZWrite]


		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 3.0
		#include "UnityCG.cginc"
		#pragma multi_compile_fog
		#define USING_FOG (defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2))

		// ES2.0/WebGL/3DS can not do loops with non-constant-expression iteration counts :(
		#if defined(SHADER_API_GLES)
		  #define LIGHT_LOOP_LIMIT 8
		#elif defined(SHADER_API_N3DS)
		  #define LIGHT_LOOP_LIMIT 4
		#else
		  #define LIGHT_LOOP_LIMIT unity_VertexLightParams.x
		#endif
		#define ENABLE_SPECULAR (!defined(SHADER_API_N3DS))

		// Compile specialized variants for when positional (point/spot) and spot lights are present
		#pragma multi_compile __ POINT SPOT

		// Compute illumination from one light, given attenuation
		half3 computeLighting (int idx, half3 dirToLight, half3 eyeNormal, half3 viewDir, half4 diffuseColor, half shininess, half atten, inout half3 specColor) {
		  half NdotL = max(dot(eyeNormal, dirToLight), 0.0);
		  // diffuse
		  half3 color = NdotL * diffuseColor.rgb * unity_LightColor[idx].rgb;
		  // specular
		  if (NdotL > 0.0) { 
			half3 h = normalize(dirToLight + viewDir);
			half HdotN = max(dot(eyeNormal, h), 0.0);
			half sp = saturate(pow(HdotN, shininess));
			specColor += (atten * sp) * unity_LightColor[idx].rgb;
		  }
		  return color * atten;
		}

		// Compute attenuation & illumination from one light
		half3 computeOneLight(int idx, float3 eyePosition, half3 eyeNormal, half3 viewDir, half4 diffuseColor, half shininess, inout half3 specColor) {
		  float3 dirToLight = unity_LightPosition[idx].xyz;
		  half att = 1.0;
		  #if defined(POINT) || defined(SPOT)
			dirToLight -= eyePosition * unity_LightPosition[idx].w;
			// distance attenuation
			float distSqr = dot(dirToLight, dirToLight);
			att /= (1.0 + unity_LightAtten[idx].z * distSqr);
			if (unity_LightPosition[idx].w != 0 && distSqr > unity_LightAtten[idx].w) att = 0.0; // set to 0 if outside of range
			distSqr = max(distSqr, 0.000001); // don't produce NaNs if some vertex position overlaps with the light
			dirToLight *= rsqrt(distSqr);
			#if defined(SPOT)
			  // spot angle attenuation
			  half rho = max(dot(dirToLight, unity_SpotDirection[idx].xyz), 0.0);
			  half spotAtt = (rho - unity_LightAtten[idx].x) * unity_LightAtten[idx].y;
			  att *= saturate(spotAtt);
			#endif
		  #endif
		  att *= 0.5; // passed in light colors are 2x brighter than what used to be in FFP
		  return min (computeLighting (idx, dirToLight, eyeNormal, viewDir, diffuseColor, shininess, att, specColor), 1.0);
		}

		// uniforms
		half4 _Color;
		half4 _SpecColor;
		half _Shininess;
		int4 unity_VertexLightParams; // x: light count, y: zero, z: one (y/z needed by d3d9 vs loop instruction)
		float4 _MainTex_ST;
		sampler2D _MainTex;
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

		// vertex shader input data
		struct appdata {
		  float4 pos : POSITION;
		  float3 normal : NORMAL;
		  float3 uv0 : TEXCOORD0; 
		  float3 uv1 : TEXCOORD1;
		  float4 tangent : TANGENT;
		  UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		// vertex-to-fragment interpolators
		struct v2f {
		  fixed4 color : COLOR0;

		  float2 uv0 : TEXCOORD0;

		  #if ENABLE_SPECULAR
			fixed3 specColor : COLOR1;
		  #endif
		 
		  #if USING_FOG
			fixed fog : TEXCOORD2;
		  #endif
		  float4 pos : SV_POSITION;

		  float3 worldPos : TEXCOORD3;
		  float3 normalWP : TEXCOORD4;
		  
		  ADVANCED_DISSOLVE_DATA(5)


		  UNITY_VERTEX_OUTPUT_STEREO
		};

		// vertex shader
		v2f vert (appdata IN) 
		{
		  v2f o;
		  UNITY_SETUP_INSTANCE_ID(IN);
		  UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


		  CURVED_WORLD_TRANSFORM_POINT_AND_NORMAL(IN.pos, IN.normal, IN.tangent)


		  half4 color = half4(0,0,0,1.1);

		  //float3 eyePos = mul (UNITY_MATRIX_MV, float4(IN.pos,1)).xyz;
		  float3 eyePos = UnityObjectToViewPos(float4(IN.pos.xyz, 1)).xyz;

		  half3 eyeNormal = normalize (mul ((float3x3)UNITY_MATRIX_IT_MV, IN.normal).xyz);
		  half3 viewDir = 0.0;
		  viewDir = -normalize (eyePos);
		  // lighting
		  half3 lcolor = _Color.rgb * glstate_lightmodel_ambient.rgb;
		  half3 specColor = 0.0;
		  half shininess = _Shininess * 128.0;
		  for (int il = 0; il < LIGHT_LOOP_LIMIT; ++il) {
			lcolor += computeOneLight(il, eyePos, eyeNormal, viewDir, _Color, shininess, specColor);
		  }
		  color.rgb = lcolor.rgb;
		  color.a = _Color.a;
		  specColor *= _SpecColor.rgb;
		  o.color = saturate(color);
		  #if ENABLE_SPECULAR
		  o.specColor = saturate(specColor);
		  #endif
		  // compute texture coordinates
		  o.uv0 = IN.uv0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
		  // fog
		  #if USING_FOG
			float fogCoord = length(eyePos.xyz); // radial fog distance
			UNITY_CALC_FOG_FACTOR_RAW(fogCoord);
			o.fog = saturate(unityFogFactor);
		  #endif 
		  // transform position
		  o.pos = UnityObjectToClipPos(IN.pos);



		  //VacuumShaders
		  o.worldPos = mul(unity_ObjectToWorld, float4(IN.pos.xyz, 1)).xyz;
		  o.normalWP = UnityObjectToWorldNormal(IN.normal);

		  ADVANCED_DISSOLVE_INIT_DATA(o.pos, IN.uv0, IN.uv1, o)

		  return o;
		}


		// fragment shader
		fixed4 frag (v2f IN) : SV_Target 
		{
			float4 alpha = AdvancedDissolveGetAlpha(IN.uv0.xy, IN.worldPos, IN.normalWP, IN.dissolveUV);
			DoDissolveClip(alpha);

			float3 dissolveAlbedo = 0;
			float3 dissolveEmission = 0;
			float dissolveBlend = DoDissolveAlbedoEmission(alpha, dissolveAlbedo, dissolveEmission, IN.uv0.xy, 0);



		  fixed4 col;
		  fixed4 tex, tmp0, tmp1, tmp2;
		  // SetTexture #0
		  tex = tex2D (_MainTex, IN.uv0.xy);

		  float alphaFromSurface = tex.a * _Color.a;
#ifdef _ALPHATEST_ON
		  clip(alphaFromSurface - _Cutoff);
#endif
	  
		  //Diffuse
		  tex.rgb = lerp(tex.rgb, dissolveAlbedo, dissolveBlend);


		  col.rgb = tex.rgb * IN.color.rgb;
		  col.rgb *= 2;
		  col.a = 1;
		  #if ENABLE_SPECULAR
		  // add specular color
		  col.rgb += IN.specColor;
		  #endif


		  
		  //Emission
		  col.rgb += dissolveEmission * dissolveBlend;



		  // fog
		  #if USING_FOG
			col.rgb = lerp (unity_FogColor.rgb, col.rgb, IN.fog);
		  #endif

		  return DoOutputForward(col, alphaFromSurface);
		}

		ENDCG
		 }

		 Pass {
		  Tags { "LIGHTMODE"="VertexLM" "IGNOREPROJECTOR"="true"  }

			 Blend[_SrcBlend][_DstBlend]
			 ZWrite[_ZWrite]

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 3.0
		#include "UnityCG.cginc"
		#pragma multi_compile_fog
		#define USING_FOG (defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2))

		// uniforms
		float4 _MainTex_ST;
		 sampler2D _MainTex;
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


		// vertex shader input data
		struct appdata {
		  float4 pos : POSITION;
		  half4 color : COLOR;
		  float3 uv1 : TEXCOORD1;
		  float3 uv0 : TEXCOORD0;
		  float3 normal : NORMAL;
		  float4 tangent : TANGENT;
		  UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		// vertex-to-fragment interpolators
		struct v2f {
		  fixed4 color : COLOR0;
		  float2 uv0 : TEXCOORD0;
		  float2 uv1 : TEXCOORD1;
		  #if USING_FOG
			fixed fog : TEXCOORD2;
		  #endif
		  float4 pos : SV_POSITION;

		  float3 worldPos : TEXCOORD3;
	    float3 normalWP : TEXCOORD4;
		 
		 ADVANCED_DISSOLVE_DATA(5)

		  UNITY_VERTEX_OUTPUT_STEREO
		};

		// vertex shader
		v2f vert (appdata IN) {
		  v2f o;
		  UNITY_SETUP_INSTANCE_ID(IN);
		  UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


		  CURVED_WORLD_TRANSFORM_POINT_AND_NORMAL(IN.pos, IN.normal, IN.tangent)


		  half4 color = IN.color;

		  //float3 eyePos = mul (UNITY_MATRIX_MV, float4(IN.pos,1)).xyz;
		  float3 eyePos = UnityObjectToViewPos(float4(IN.pos.xyz, 1)).xyz;

		  half3 viewDir = 0.0;
		  o.color = saturate(color);
		  // compute texture coordinates
		  o.uv0 = IN.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
		  o.uv1 = IN.uv0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
		  // fog
		  #if USING_FOG
			float fogCoord = length(eyePos.xyz); // radial fog distance
			UNITY_CALC_FOG_FACTOR_RAW(fogCoord);
			o.fog = saturate(unityFogFactor);
		  #endif
		  // transform position
		  o.pos = UnityObjectToClipPos(IN.pos);


		  //VacuumShaders
		  o.worldPos = mul(unity_ObjectToWorld, float4(IN.pos.xyz, 1)).xyz;
	    o.normalWP = UnityObjectToWorldNormal(IN.normal);

		  ADVANCED_DISSOLVE_INIT_DATA(o.pos, IN.uv0.xy, IN.uv1.xy, o);


		  return o;
		}
				

		// fragment shader
		fixed4 frag (v2f IN) : SV_Target 
		{
			
	  float4 alpha = AdvancedDissolveGetAlpha(IN.uv1.xy, IN.worldPos, IN.normalWP, IN.dissolveUV);
		DoDissolveClip(alpha);

		float3 dissolveAlbedo = 0;
		float3 dissolveEmission = 0;
		float dissolveBlend = DoDissolveAlbedoEmission(alpha, dissolveAlbedo, dissolveEmission, IN.uv1.xy, 0);



		  fixed4 col;
		  fixed4 tex, tmp0, tmp1, tmp2;
		  // SetTexture #0
		  tex = UNITY_SAMPLE_TEX2D (unity_Lightmap, IN.uv0.xy);
		  col = tex * _Color;
		  // SetTexture #1
		  tex = tex2D (_MainTex, IN.uv1.xy);

		  float alphaFromSurface = tex.a * _Color.a;
#ifdef _ALPHATEST_ON
		  clip(alphaFromSurface - _Cutoff * 1.01);
#endif

		  //Diffuse
		  tex.rgb = lerp(tex.rgb, dissolveAlbedo, dissolveBlend);

		  col.rgb = tex * col;
		  col *= 2;
		  col.a = 1;
		 

		  //Emission
		  col.rgb += dissolveEmission * dissolveBlend;


		  // fog
		  #if USING_FOG
			col.rgb = lerp (unity_FogColor.rgb, col.rgb, IN.fog);
		  #endif
			return DoOutputForward(col, alphaFromSurface);
		}

		// texenvs
		//! TexEnv0: 01010102 01010102 [unity_Lightmap] [_Color] usesLightmapST
		//! TexEnv1: 02010100 01050107 [_MainTex]
		ENDCG
		 }

		 Pass {
		  Tags { "LIGHTMODE"="VertexLMRGBM" "IGNOREPROJECTOR"="true"  }

			 Blend[_SrcBlend][_DstBlend]
			 ZWrite[_ZWrite]


		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 3.0
		#include "UnityCG.cginc"
		#pragma multi_compile_fog
		#define USING_FOG (defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2))

		// uniforms
		float4 unity_Lightmap_ST;
		float4 _MainTex_ST;
		sampler2D _MainTex;
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


		// vertex shader input data
		struct appdata {
		  float4 pos : POSITION;
		  half4 color : COLOR;
		  float3 uv1 : TEXCOORD1;
		  float3 uv0 : TEXCOORD0;
		  float3 normal : NORMAL;
		  float4 tangent : TANGENT;
		  UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		// vertex-to-fragment interpolators
		struct v2f {
		  fixed4 color : COLOR0;
		  float2 uv0 : TEXCOORD0;
		  float2 uv1 : TEXCOORD1;
		  float2 uv2 : TEXCOORD2;
		  #if USING_FOG
			fixed fog : TEXCOORD3;
		  #endif
		  float4 pos : SV_POSITION;


		  float3 worldPos : TEXCOORD4;
		  float3 normalWP : TEXCOORD5;
		  ADVANCED_DISSOLVE_DATA(6)


		  UNITY_VERTEX_OUTPUT_STEREO
		};

		// vertex shader
		v2f vert (appdata IN) {
		  v2f o;
		  UNITY_SETUP_INSTANCE_ID(IN);
		  UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


		  CURVED_WORLD_TRANSFORM_POINT_AND_NORMAL(IN.pos, IN.normal, IN.tangent)


		  half4 color = IN.color;

		  //float3 eyePos = mul (UNITY_MATRIX_MV, float4(IN.pos,1)).xyz;
		  float3 eyePos = UnityObjectToViewPos(float4(IN.pos.xyz, 1)).xyz;

		  half3 viewDir = 0.0;
		  o.color = saturate(color);
		  // compute texture coordinates
		  o.uv0 = IN.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
		  o.uv1 = IN.uv1.xy * unity_Lightmap_ST.xy + unity_Lightmap_ST.zw;
		  o.uv2 = IN.uv0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
		  // fog
		  #if USING_FOG
			float fogCoord = length(eyePos.xyz); // radial fog distance
			UNITY_CALC_FOG_FACTOR_RAW(fogCoord);
			o.fog = saturate(unityFogFactor);
		  #endif
		  // transform position
		  o.pos = UnityObjectToClipPos(IN.pos);



		  //VacuumShaders
		  o.worldPos = mul(unity_ObjectToWorld, float4(IN.pos.xyz, 1)).xyz;
		  o.normalWP = UnityObjectToWorldNormal(IN.normal);

		  ADVANCED_DISSOLVE_INIT_DATA(o.pos, IN.uv0.xy, IN.uv1.xy, o);


		  return o;
		}

		

		// fragment shader
		fixed4 frag (v2f IN) : SV_Target 
		{
			
            float4 alpha = AdvancedDissolveGetAlpha(IN.uv2.xy, IN.worldPos, IN.normalWP, IN.dissolveUV);
			DoDissolveClip(alpha);

			float3 dissolveAlbedo = 0;
			float3 dissolveEmission = 0;
			float dissolveBlend = DoDissolveAlbedoEmission(alpha, dissolveAlbedo, dissolveEmission, IN.uv2.xy, 0);



		  fixed4 col;
		  fixed4 tex, tmp0, tmp1, tmp2;
		  // SetTexture #0
		  tex = UNITY_SAMPLE_TEX2D (unity_Lightmap, IN.uv0.xy);
		  col = tex * tex.a;
		  col *= 2;
		  // SetTexture #1
		  tex = UNITY_SAMPLE_TEX2D (unity_Lightmap, IN.uv1.xy);
		  col = col * _Color;
		  // SetTexture #2
		  tex = tex2D (_MainTex, IN.uv2.xy);


		  float alphaFromSurface = tex.a * _Color.a;
#ifdef _ALPHATEST_ON
		  clip(alphaFromSurface - _Cutoff * 1.01);
#endif 

		  //Diffuse
		  tex.rgb = lerp(tex.rgb, dissolveAlbedo, dissolveBlend);

		  col.rgb = tex * col;
		  col *= 4;
		  col.a = 1;
		  

		 //Emission
		  col.rgb += dissolveEmission * dissolveBlend;


		  // fog
		  #if USING_FOG
			col.rgb = lerp (unity_FogColor.rgb, col.rgb, IN.fog);
		  #endif
			return DoOutputForward(col, alphaFromSurface);
		}

		// texenvs
		//! TexEnv0: 02010105 02010105 [unity_Lightmap] usesLightmapST
		//! TexEnv1: 01000102 01000102 [unity_Lightmap] [_Color]
		//! TexEnv2: 04010100 01050107 [_MainTex]
		ENDCG
		 }

		
		 UsePass "Hidden/VacuumShaders/Advanced Dissolve/Shadow/SHADOWCASTER"
	}



	//SM 2.0
	SubShader 
	{ 
		 LOD 100
		 Tags { "IGNOREPROJECTOR"="true" "RenderType"="AdvancedDissolveCutout" "DisableBatching" = "True" }
		Cull[_Cull]

		 Pass 
		 {
		  Tags { "LIGHTMODE"="Vertex"  "IGNOREPROJECTOR"="true" }
		

		Blend[_SrcBlend][_DstBlend]
		ZWrite[_ZWrite]


		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 2.0
		#include "UnityCG.cginc"
		#pragma multi_compile_fog
		#define USING_FOG (defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2))

		// ES2.0/WebGL/3DS can not do loops with non-constant-expression iteration counts :(
		#if defined(SHADER_API_GLES)
		  #define LIGHT_LOOP_LIMIT 8
		#elif defined(SHADER_API_N3DS)
		  #define LIGHT_LOOP_LIMIT 4
		#else
		  #define LIGHT_LOOP_LIMIT unity_VertexLightParams.x
		#endif
		#define ENABLE_SPECULAR (!defined(SHADER_API_N3DS))

		// Compile specialized variants for when positional (point/spot) and spot lights are present
		#pragma multi_compile __ POINT SPOT

		// Compute illumination from one light, given attenuation
		half3 computeLighting (int idx, half3 dirToLight, half3 eyeNormal, half3 viewDir, half4 diffuseColor, half shininess, half atten, inout half3 specColor) {
		  half NdotL = max(dot(eyeNormal, dirToLight), 0.0);
		  // diffuse
		  half3 color = NdotL * diffuseColor.rgb * unity_LightColor[idx].rgb;
		  // specular
		  if (NdotL > 0.0) { 
			half3 h = normalize(dirToLight + viewDir);
			half HdotN = max(dot(eyeNormal, h), 0.0);
			half sp = saturate(pow(HdotN, shininess));
			specColor += (atten * sp) * unity_LightColor[idx].rgb;
		  }
		  return color * atten;
		}

		// Compute attenuation & illumination from one light
		half3 computeOneLight(int idx, float3 eyePosition, half3 eyeNormal, half3 viewDir, half4 diffuseColor, half shininess, inout half3 specColor) {
		  float3 dirToLight = unity_LightPosition[idx].xyz;
		  half att = 1.0;
		  #if defined(POINT) || defined(SPOT)
			dirToLight -= eyePosition * unity_LightPosition[idx].w;
			// distance attenuation
			float distSqr = dot(dirToLight, dirToLight);
			att /= (1.0 + unity_LightAtten[idx].z * distSqr);
			if (unity_LightPosition[idx].w != 0 && distSqr > unity_LightAtten[idx].w) att = 0.0; // set to 0 if outside of range
			distSqr = max(distSqr, 0.000001); // don't produce NaNs if some vertex position overlaps with the light
			dirToLight *= rsqrt(distSqr);
			#if defined(SPOT)
			  // spot angle attenuation
			  half rho = max(dot(dirToLight, unity_SpotDirection[idx].xyz), 0.0);
			  half spotAtt = (rho - unity_LightAtten[idx].x) * unity_LightAtten[idx].y;
			  att *= saturate(spotAtt);
			#endif
		  #endif
		  att *= 0.5; // passed in light colors are 2x brighter than what used to be in FFP
		  return min (computeLighting (idx, dirToLight, eyeNormal, viewDir, diffuseColor, shininess, att, specColor), 1.0);
		}

		// uniforms
		half4 _Color;
		half4 _SpecColor;
		half _Shininess;
		int4 unity_VertexLightParams; // x: light count, y: zero, z: one (y/z needed by d3d9 vs loop instruction)
		float4 _MainTex_ST;
		sampler2D _MainTex;
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

		// vertex shader input data
		struct appdata {
		  float4 pos : POSITION;
		  float3 normal : NORMAL;
		  float3 uv0 : TEXCOORD0;
		  float3 uv1 : TEXCOORD1;
		  float4 tangent : TANGENT;
		  UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		// vertex-to-fragment interpolators
		struct v2f {
		  fixed4 color : COLOR0;

		  float2 uv0 : TEXCOORD0;

		  #if ENABLE_SPECULAR
			fixed3 specColor : COLOR1;
		  #endif
		 
		  #if USING_FOG 
			fixed fog : TEXCOORD2;
		  #endif
		  float4 pos : SV_POSITION;

		  float3 worldPos : TEXCOORD3;
          float3 normalWP : TEXCOORD4;

		  ADVANCED_DISSOLVE_DATA(5)

		  UNITY_VERTEX_OUTPUT_STEREO
		};

		// vertex shader
		v2f vert (appdata IN) 
		{
		  v2f o;
		  UNITY_SETUP_INSTANCE_ID(IN);
		  UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


		  CURVED_WORLD_TRANSFORM_POINT_AND_NORMAL(IN.pos, IN.normal, IN.tangent)


		  half4 color = half4(0,0,0,1.1);

		  //float3 eyePos = mul (UNITY_MATRIX_MV, float4(IN.pos,1)).xyz;
		  float3 eyePos = UnityObjectToViewPos(float4(IN.pos.xyz, 1)).xyz;

		  half3 eyeNormal = normalize (mul ((float3x3)UNITY_MATRIX_IT_MV, IN.normal).xyz);
		  half3 viewDir = 0.0;
		  viewDir = -normalize (eyePos);
		  // lighting
		  half3 lcolor = _Color.rgb * glstate_lightmodel_ambient.rgb;
		  half3 specColor = 0.0;
		  half shininess = _Shininess * 128.0;
		  for (int il = 0; il < LIGHT_LOOP_LIMIT; ++il) {
			lcolor += computeOneLight(il, eyePos, eyeNormal, viewDir, _Color, shininess, specColor);
		  }
		  color.rgb = lcolor.rgb;
		  color.a = _Color.a;
		  specColor *= _SpecColor.rgb;
		  o.color = saturate(color);
		  #if ENABLE_SPECULAR
		  o.specColor = saturate(specColor);
		  #endif
		  // compute texture coordinates
		  o.uv0 = IN.uv0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
		  // fog
		  #if USING_FOG
			float fogCoord = length(eyePos.xyz); // radial fog distance
			UNITY_CALC_FOG_FACTOR_RAW(fogCoord);
			o.fog = saturate(unityFogFactor);
		  #endif
		  // transform position
		  o.pos = UnityObjectToClipPos(IN.pos);



		  //VacuumShaders
		  o.worldPos = mul(unity_ObjectToWorld, float4(IN.pos.xyz, 1)).xyz;
          o.normalWP = UnityObjectToWorldNormal(IN.normal);

		  ADVANCED_DISSOLVE_INIT_DATA(o.pos, IN.uv0, IN.uv1, o);

		  return o;
		}


		// fragment shader
		fixed4 frag (v2f IN) : SV_Target 
		{


			float4 alpha = AdvancedDissolveGetAlpha(IN.uv0.xy, IN.worldPos, IN.normalWP, IN.dissolveUV);
			DoDissolveClip(alpha);

			float3 dissolveAlbedo = 0;
float3 dissolveEmission = 0;
			float dissolveBlend = DoDissolveAlbedoEmission(alpha, dissolveAlbedo, dissolveEmission, IN.uv0.xy, 0);



		  fixed4 col;
		  fixed4 tex, tmp0, tmp1, tmp2;
		  // SetTexture #0
		  tex = tex2D (_MainTex, IN.uv0.xy);

		  float alphaFromSurface = tex.a * _Color.a;
#ifdef _ALPHATEST_ON
		  clip(alphaFromSurface - _Cutoff);
#endif
		  //Diffuse
		  tex.rgb = lerp(tex.rgb, dissolveAlbedo, dissolveBlend);

		  col.rgb = tex * IN.color;
		  col *= 2;
		  col.a = 1;
		  #if ENABLE_SPECULAR
		  // add specular color
		  col.rgb += IN.specColor;
		  #endif


		  //Emission		  
		  col.rgb += dissolveEmission * dissolveBlend;



		  // fog
		  #if USING_FOG
			col.rgb = lerp (unity_FogColor.rgb, col.rgb, IN.fog);
		  #endif

		  return DoOutputForward(col, alphaFromSurface);
		}

		// texenvs
		//! TexEnv0: 02010103 01050107 [_MainTex]
		ENDCG
		 }

		 Pass {
		  Tags { "LIGHTMODE"="VertexLM" "IGNOREPROJECTOR"="true"  }


			 Blend[_SrcBlend][_DstBlend]
			 ZWrite[_ZWrite]


		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 2.0
		#include "UnityCG.cginc"
		#pragma multi_compile_fog
		#define USING_FOG (defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2))

		// uniforms
		float4 _MainTex_ST;
		 sampler2D _MainTex;
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


		// vertex shader input data
		struct appdata {
		  float4 pos : POSITION;
		  half4 color : COLOR;
		  float3 uv1 : TEXCOORD1;
		  float3 uv0 : TEXCOORD0;
		  float3 normal : NORMAL;
		  float4 tangent : TANGENT;
		  UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		// vertex-to-fragment interpolators
		struct v2f {
		  fixed4 color : COLOR0;
		  float2 uv0 : TEXCOORD0;
		  float2 uv1 : TEXCOORD1;
		  #if USING_FOG
			fixed fog : TEXCOORD2;
		  #endif
		  float4 pos : SV_POSITION;

		  float3 worldPos : TEXCOORD3;
          float3 normalWP : TEXCOORD4;
		  ADVANCED_DISSOLVE_DATA(5)

		  UNITY_VERTEX_OUTPUT_STEREO
		};

		// vertex shader
		v2f vert (appdata IN) {
		  v2f o;
		  UNITY_SETUP_INSTANCE_ID(IN);
		  UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


		  CURVED_WORLD_TRANSFORM_POINT_AND_NORMAL(IN.pos, IN.normal, IN.tangent)


		  half4 color = IN.color;

		  //float3 eyePos = mul (UNITY_MATRIX_MV, float4(IN.pos,1)).xyz;
		  float3 eyePos = UnityObjectToViewPos(float4(IN.pos.xyz, 1)).xyz;

		  half3 viewDir = 0.0;
		  o.color = saturate(color);
		  // compute texture coordinates
		  o.uv0 = IN.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
		  o.uv1 = IN.uv0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
		  // fog
		  #if USING_FOG
			float fogCoord = length(eyePos.xyz); // radial fog distance
			UNITY_CALC_FOG_FACTOR_RAW(fogCoord);
			o.fog = saturate(unityFogFactor);
		  #endif
		  // transform position
		  o.pos = UnityObjectToClipPos(IN.pos);


		  //VacuumShaders
		  o.worldPos = mul(unity_ObjectToWorld, float4(IN.pos.xyz, 1)).xyz;
          o.normalWP = UnityObjectToWorldNormal(IN.normal);
		  
		  ADVANCED_DISSOLVE_INIT_DATA(o.pos, IN.uv0.xy, IN.uv1.xy, o);

		  return o;
		}
				

		// fragment shader
		fixed4 frag (v2f IN) : SV_Target 
		{

		float4 alpha = AdvancedDissolveGetAlpha(IN.uv1.xy, IN.worldPos, IN.normalWP, IN.dissolveUV);
		DoDissolveClip(alpha);

		float3 dissolveAlbedo = 0;
		float3 dissolveEmission = 0;
		float dissolveBlend = DoDissolveAlbedoEmission(alpha, dissolveAlbedo, dissolveEmission, IN.uv1.xy, 0);



		  fixed4 col;
		  fixed4 tex, tmp0, tmp1, tmp2;
		  // SetTexture #0
		  tex = UNITY_SAMPLE_TEX2D (unity_Lightmap, IN.uv0.xy);
		  col = tex * _Color;
		  // SetTexture #1
		  tex = tex2D (_MainTex, IN.uv1.xy);

		  float alphaFromSurface = tex.a * _Color.a;
#ifdef _ALPHATEST_ON
		  clip(alphaFromSurface - _Cutoff * 1.01);
#endif

		  //Diffuse
		  tex.rgb = lerp(tex.rgb, dissolveAlbedo, dissolveBlend);

		  col.rgb = tex * col;
		  col *= 2;
		  col.a = 1;
		 

		  //Emission
		  col.rgb += dissolveEmission * dissolveBlend;


		  // fog
		  #if USING_FOG
			col.rgb = lerp (unity_FogColor.rgb, col.rgb, IN.fog);
		  #endif
		  
			return DoOutputForward(col, alphaFromSurface);
		}

		// texenvs
		//! TexEnv0: 01010102 01010102 [unity_Lightmap] [_Color] usesLightmapST
		//! TexEnv1: 02010100 01050107 [_MainTex]
		ENDCG
		 }

		 Pass {
		  Tags { "LIGHTMODE"="VertexLMRGBM" "IGNOREPROJECTOR"="true"  }


			 Blend[_SrcBlend][_DstBlend]
			 ZWrite[_ZWrite]

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 2.0
		#include "UnityCG.cginc"
		#pragma multi_compile_fog
		#define USING_FOG (defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2))

		// uniforms
		float4 unity_Lightmap_ST;
		float4 _MainTex_ST;
		sampler2D _MainTex;
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


		// vertex shader input data
		struct appdata {
		  float4 pos : POSITION;
		  half4 color : COLOR;
		  float3 uv1 : TEXCOORD1;
		  float3 uv0 : TEXCOORD0;
		  float3 normal : NORMAL;
		  float4 tangent : TANGENT;
		  UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		// vertex-to-fragment interpolators
		struct v2f {
		  fixed4 color : COLOR0;
		  float2 uv0 : TEXCOORD0;
		  float2 uv1 : TEXCOORD1;
		  float2 uv2 : TEXCOORD2;
		  #if USING_FOG
			fixed fog : TEXCOORD3;
		  #endif
		  float4 pos : SV_POSITION;

		  float3 worldPos : TEXCOORD4;
		  float3 normalWP : TEXCOORD5;
		  ADVANCED_DISSOLVE_DATA(6)

		  UNITY_VERTEX_OUTPUT_STEREO
		};

		// vertex shader
		v2f vert (appdata IN) {
		  v2f o;
		  UNITY_SETUP_INSTANCE_ID(IN);
		  UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


		  CURVED_WORLD_TRANSFORM_POINT_AND_NORMAL(IN.pos, IN.normal, IN.tangent)


		  half4 color = IN.color;

		  //float3 eyePos = mul (UNITY_MATRIX_MV, float4(IN.pos,1)).xyz;
		  float3 eyePos = UnityObjectToViewPos(float4(IN.pos.xyz, 1)).xyz;

		  half3 viewDir = 0.0;
		  o.color = saturate(color);
		  // compute texture coordinates
		  o.uv0 = IN.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
		  o.uv1 = IN.uv1.xy * unity_Lightmap_ST.xy + unity_Lightmap_ST.zw;
		  o.uv2 = IN.uv0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
		  // fog
		  #if USING_FOG
			float fogCoord = length(eyePos.xyz); // radial fog distance
			UNITY_CALC_FOG_FACTOR_RAW(fogCoord);
			o.fog = saturate(unityFogFactor);
		  #endif
		  // transform position
		  o.pos = UnityObjectToClipPos(IN.pos);



		  //VacuumShaders
		  o.worldPos = mul(unity_ObjectToWorld, float4(IN.pos.xyz, 1)).xyz;
          o.normalWP = UnityObjectToWorldNormal(IN.normal);
		  
		  ADVANCED_DISSOLVE_INIT_DATA(o.pos, IN.uv0.xy, IN.uv1.xy, o);


		  return o;
		}
		 
		

		// fragment shader
		fixed4 frag (v2f IN) : SV_Target 
		{
			float4 alpha = AdvancedDissolveGetAlpha(IN.uv2.xy, IN.worldPos, IN.normalWP, IN.dissolveUV);
			DoDissolveClip(alpha);

			float3 dissolveAlbedo = 0;
			float3 dissolveEmission = 0;
			float dissolveBlend = DoDissolveAlbedoEmission(alpha, dissolveAlbedo, dissolveEmission, IN.uv2.xy, 0);



		  fixed4 col;
		  fixed4 tex, tmp0, tmp1, tmp2;
		  // SetTexture #0
		  tex = UNITY_SAMPLE_TEX2D (unity_Lightmap, IN.uv0.xy);
		  col = tex * tex.a;
		  col *= 2;
		  // SetTexture #1
		  tex = UNITY_SAMPLE_TEX2D (unity_Lightmap, IN.uv1.xy);
		  col = col * _Color;
		  // SetTexture #2
		  tex = tex2D (_MainTex, IN.uv2.xy);

		  float alphaFromSurface = tex.a * _Color.a;
#ifdef _ALPHATEST_ON
		  clip(alphaFromSurface - _Cutoff * 1.01);
#endif 

		  //Diffuse
		  tex.rgb = lerp(tex.rgb, dissolveAlbedo, dissolveBlend);

		  col.rgb = tex * col;
		  col *= 4;
		  col.a = 1;
		  

		  //Emission
		  col.rgb += dissolveEmission * dissolveBlend;


		  // fog
		  #if USING_FOG
			col.rgb = lerp (unity_FogColor.rgb, col.rgb, IN.fog);
		  #endif
		  
			return DoOutputForward(col, alphaFromSurface);
		}

		// texenvs
		//! TexEnv0: 02010105 02010105 [unity_Lightmap] usesLightmapST
		//! TexEnv1: 01000102 01000102 [unity_Lightmap] [_Color]
		//! TexEnv2: 04010100 01050107 [_MainTex]
		ENDCG
		 }
		  
		 
		 UsePass "Hidden/VacuumShaders/Advanced Dissolve/Shadow/SHADOWCASTER"
	}


CustomEditor "VacuumShaders.AdvancedDissolve.VertexLitGUI"
} 