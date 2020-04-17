// Upgrade NOTE: replaced 'defined FOG_COMBINED_WITH_WORLD_POS' with 'defined (FOG_COMBINED_WITH_WORLD_POS)'

// Upgrade NOTE: replaced 'defined FOG_COMBINED_WITH_WORLD_POS' with 'defined (FOG_COMBINED_WITH_WORLD_POS)'

Shader "VacuumShaders/Advanced Dissolve/TextMeshPro/Distance Field (Surface)" {

Properties {
	_FaceTex			("Fill Texture", 2D) = "white" {}
	_FaceUVSpeedX		("Face UV Speed X", Range(-5, 5)) = 0.0
	_FaceUVSpeedY		("Face UV Speed Y", Range(-5, 5)) = 0.0
	_FaceColor			("Fill Color", Color) = (1,1,1,1)
	_FaceDilate			("Face Dilate", Range(-1,1)) = 0

	_OutlineColor		("Outline Color", Color) = (0,0,0,1)
	_OutlineTex			("Outline Texture", 2D) = "white" {}
	_OutlineUVSpeedX	("Outline UV Speed X", Range(-5, 5)) = 0.0
	_OutlineUVSpeedY	("Outline UV Speed Y", Range(-5, 5)) = 0.0
	_OutlineWidth		("Outline Thickness", Range(0, 1)) = 0
	_OutlineSoftness	("Outline Softness", Range(0,1)) = 0

	_Bevel				("Bevel", Range(0,1)) = 0.5
	_BevelOffset		("Bevel Offset", Range(-0.5,0.5)) = 0
	_BevelWidth			("Bevel Width", Range(-.5,0.5)) = 0
	_BevelClamp			("Bevel Clamp", Range(0,1)) = 0
	_BevelRoundness		("Bevel Roundness", Range(0,1)) = 0

	_BumpMap 			("Normalmap", 2D) = "bump" {}
	_BumpOutline		("Bump Outline", Range(0,1)) = 0.5
	_BumpFace			("Bump Face", Range(0,1)) = 0.5

	_ReflectFaceColor		("Face Color", Color) = (0,0,0,1)
	_ReflectOutlineColor	("Outline Color", Color) = (0,0,0,1)
	_Cube 					("Reflection Cubemap", Cube) = "black" { /* TexGen CubeReflect */ }
	_EnvMatrixRotation		("Texture Rotation", vector) = (0, 0, 0, 0)
	_SpecColor				("Specular Color", Color) = (0,0,0,1)

	_FaceShininess		("Face Shininess", Range(0,1)) = 0
	_OutlineShininess	("Outline Shininess", Range(0,1)) = 0

	_GlowColor			("Color", Color) = (0, 1, 0, 0.5)
	_GlowOffset			("Offset", Range(-1,1)) = 0
	_GlowInner			("Inner", Range(0,1)) = 0.05
	_GlowOuter			("Outer", Range(0,1)) = 0.05
	_GlowPower			("Falloff", Range(1, 0)) = 0.75

	_WeightNormal		("Weight Normal", float) = 0
	_WeightBold			("Weight Bold", float) = 0.5

	// Should not be directly exposed to the user
	_ShaderFlags		("Flags", float) = 0
	_ScaleRatioA		("Scale RatioA", float) = 1
	_ScaleRatioB		("Scale RatioB", float) = 1
	_ScaleRatioC		("Scale RatioC", float) = 1

	_MainTex			("Font Atlas", 2D) = "white" {}
	_TextureWidth		("Texture Width", float) = 512
	_TextureHeight		("Texture Height", float) = 512
	_GradientScale		("Gradient Scale", float) = 5.0
	_ScaleX				("Scale X", float) = 1.0
	_ScaleY				("Scale Y", float) = 1.0
	_PerspectiveFilter	("Perspective Correction", Range(0, 1)) = 0.875

	_VertexOffsetX		("Vertex OffsetX", float) = 0
	_VertexOffsetY		("Vertex OffsetY", float) = 0
	//_MaskCoord		("Mask Coords", vector) = (0,0,0,0)
	//_MaskSoftness		("Mask Softness", float) = 0



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

SubShader {

	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }	

	LOD 300
	Cull [_CullMode]

	
	// ------------------------------------------------------------
	// Surface shader code generated out of a CGPROGRAM block:
	ZWrite Off ColorMask RGB
	

	// ---- forward rendering base pass:
	Pass {
		Name "FORWARD"
		Tags { "LightMode" = "ForwardBase" }
		Blend SrcAlpha OneMinusSrcAlpha

CGPROGRAM
// compile directives
#pragma vertex vert_PixShader
#pragma fragment frag_PixShader
#pragma target 3.0
#pragma glsl
#pragma multi_compile_instancing
#pragma multi_compile_fog
#pragma multi_compile_fwdbasealpha noshadowmask nodynlightmap nodirlightmap nolightmap noshadow
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
// Surface shader code generated based on:
// vertex modifier: 'VertShader'
// writes to per-pixel normal: YES
// writes to emission: YES
// writes to occlusion: no
// needs world space reflection vector: no
// needs world space normal vector: no
// needs screen space position: no
// needs world space position: no
// needs view direction: no
// needs world space view direction: no
// needs world space position for lighting: YES
// needs world space view direction for lighting: YES
// needs world space view direction for lightmaps: no
// needs vertex color: YES
// needs VFACE: no
// passes tangent-to-world matrix to pixel shader: YES
// reads from normal: no
// 3 texcoords actually used
//   float2 _MainTex
//   float2 _FaceTex
//   float2 _OutlineTex
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"

#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))

// Original surface shader snippet:
#line 107 ""
#ifdef DUMMY_PREPROCESSOR_TO_WORK_AROUND_HLSL_COMPILER_LINE_HANDLING
#endif
/* UNITY: Original start of shader */
	//#pragma surface PixShader BlinnPhong alpha:blend vertex:VertShader nolightmap nodirlightmap
	//#pragma target 3.0
	#pragma shader_feature __ GLOW_ON
	//#pragma glsl

	#include "TMPro_Properties.cginc"
	#include "TMPro.cginc"

	half _FaceShininess;
	half _OutlineShininess;

	struct Input
	{
		fixed4	color			: COLOR;
		float2	uv_MainTex;
		float2	uv2_FaceTex;
		float2  uv2_OutlineTex;
		float2	param;						// Weight, Scale
		float3	viewDirEnv;		
	};

	
	#define BEVEL_ON 1
	#include "TMPro_Surface.cginc"

	
	float4 _MainTex_ST;
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


// vertex-to-fragment interpolation data
// no lightmaps:
#ifndef LIGHTMAP_ON
// half-precision fragment shader registers:
#ifdef UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS
#define FOG_COMBINED_WITH_TSPACE
struct v2f_PixShader {
  UNITY_POSITION(pos);
  float4 pack0 : TEXCOORD0; // _MainTex _FaceTex
  float2 pack1 : TEXCOORD1; // _OutlineTex
  float4 tSpace0 : TEXCOORD2;
  float4 tSpace1 : TEXCOORD3;
  float4 tSpace2 : TEXCOORD4;
  fixed4 color : COLOR0;
  float2 custompack0 : TEXCOORD5; // param
  float3 custompack1 : TEXCOORD6; // viewDirEnv
  #if UNITY_SHOULD_SAMPLE_SH
  half3 sh : TEXCOORD7; // SH
  #endif
  DECLARE_LIGHT_COORDS(8)
  UNITY_VERTEX_INPUT_INSTANCE_ID
  UNITY_VERTEX_OUTPUT_STEREO


ADVANCED_DISSOLVE_DATA(9)
};
#endif
// high-precision fragment shader registers:
#ifndef UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS
struct v2f_PixShader {
  UNITY_POSITION(pos);
  float4 pack0 : TEXCOORD0; // _MainTex _FaceTex
  float2 pack1 : TEXCOORD1; // _OutlineTex
  float4 tSpace0 : TEXCOORD2;
  float4 tSpace1 : TEXCOORD3;
  float4 tSpace2 : TEXCOORD4;
  fixed4 color : COLOR0;
  float2 custompack0 : TEXCOORD5; // param
  float3 custompack1 : TEXCOORD6; // viewDirEnv
  #if UNITY_SHOULD_SAMPLE_SH
  half3 sh : TEXCOORD7; // SH
  #endif
  UNITY_FOG_COORDS(8)
  DECLARE_LIGHT_COORDS(9)
  UNITY_VERTEX_INPUT_INSTANCE_ID
  UNITY_VERTEX_OUTPUT_STEREO


ADVANCED_DISSOLVE_DATA(10)

};
#endif
#endif
// with lightmaps:
#ifdef LIGHTMAP_ON
// half-precision fragment shader registers:
#ifdef UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS
#define FOG_COMBINED_WITH_TSPACE
struct v2f_PixShader {
  UNITY_POSITION(pos);
  float4 pack0 : TEXCOORD0; // _MainTex _FaceTex
  float2 pack1 : TEXCOORD1; // _OutlineTex
  float4 tSpace0 : TEXCOORD2;
  float4 tSpace1 : TEXCOORD3;
  float4 tSpace2 : TEXCOORD4;
  fixed4 color : COLOR0;
  float2 custompack0 : TEXCOORD5; // param
  float3 custompack1 : TEXCOORD6; // viewDirEnv
  float4 lmap : TEXCOORD7;
  DECLARE_LIGHT_COORDS(8)
  UNITY_VERTEX_INPUT_INSTANCE_ID
  UNITY_VERTEX_OUTPUT_STEREO


ADVANCED_DISSOLVE_DATA(9)
};
#endif
// high-precision fragment shader registers:
#ifndef UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS
struct v2f_PixShader {
  UNITY_POSITION(pos);
  float4 pack0 : TEXCOORD0; // _MainTex _FaceTex
  float2 pack1 : TEXCOORD1; // _OutlineTex
  float4 tSpace0 : TEXCOORD2;
  float4 tSpace1 : TEXCOORD3;
  float4 tSpace2 : TEXCOORD4;
  fixed4 color : COLOR0;
  float2 custompack0 : TEXCOORD5; // param
  float3 custompack1 : TEXCOORD6; // viewDirEnv
  float4 lmap : TEXCOORD7;
  UNITY_FOG_COORDS(8)
  DECLARE_LIGHT_COORDS(9)
  UNITY_VERTEX_INPUT_INSTANCE_ID
  UNITY_VERTEX_OUTPUT_STEREO


ADVANCED_DISSOLVE_DATA(10)	
};
#endif
#endif

float4 _FaceTex_ST;
float4 _OutlineTex_ST;

// vertex shader
v2f_PixShader vert_PixShader (appdata_full v) {
  UNITY_SETUP_INSTANCE_ID(v);
  v2f_PixShader o;
  UNITY_INITIALIZE_OUTPUT(v2f_PixShader,o);
  UNITY_TRANSFER_INSTANCE_ID(v,o);
  UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
  Input customInputData;
  VertShader (v, customInputData);
  o.custompack0.xy = customInputData.param;
  o.custompack1.xyz = customInputData.viewDirEnv;
  o.pos = UnityObjectToClipPos(v.vertex);
  o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
  o.pack0.zw = TRANSFORM_TEX(v.texcoord1, _FaceTex);
  o.pack1.xy = TRANSFORM_TEX(v.texcoord1, _OutlineTex);
  float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
  float3 worldNormal = UnityObjectToWorldNormal(v.normal);
  fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
  fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
  fixed3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;
  o.tSpace0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
  o.tSpace1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
  o.tSpace2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);
  o.color = v.color;
  #ifdef LIGHTMAP_ON
  o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
  #endif

  // SH/ambient and vertex lights
  #ifndef LIGHTMAP_ON
    #if UNITY_SHOULD_SAMPLE_SH && !UNITY_SAMPLE_FULL_SH_PER_PIXEL
      o.sh = 0;
      // Approximated illumination from non-important point lights
      #ifdef VERTEXLIGHT_ON
        o.sh += Shade4PointLights (
          unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
          unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
          unity_4LightAtten0, worldPos, worldNormal);
      #endif
      o.sh = ShadeSHPerVertex (worldNormal, o.sh);
    #endif
  #endif // !LIGHTMAP_ON

  COMPUTE_LIGHT_COORDS(o); // pass light cookie coordinates to pixel shader
  #ifdef FOG_COMBINED_WITH_TSPACE
    UNITY_TRANSFER_FOG_COMBINED_WITH_TSPACE(o,o.pos); // pass fog coordinates to pixel shader
  #elif defined (FOG_COMBINED_WITH_WORLD_POS)
    UNITY_TRANSFER_FOG_COMBINED_WITH_WORLD_POS(o,o.pos); // pass fog coordinates to pixel shader
  #else
    UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
  #endif


	//VacuumShaders 
	ADVANCED_DISSOLVE_INIT_DATA(o.pos, v.texcoord, v.texcoord1, o)

  return o;
}

// fragment shader
fixed4 frag_PixShader (v2f_PixShader IN) : SV_Target {
  UNITY_SETUP_INSTANCE_ID(IN);

float3 worldPos = float3(IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w);
float3 normalWS = float3(IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z);
float4 alpha = AdvancedDissolveGetAlpha(IN.pack0.xy, worldPos, normalWS, IN.dissolveUV);
		
DoDissolveClip(alpha);



float3 dissolveAlbedo = 0;
float3 dissolveEmission = 0;
float dissolveBlend = DoDissolveAlbedoEmission(alpha, dissolveAlbedo, dissolveEmission, IN.pack0.xy, 0);




  // prepare and unpack data
  Input surfIN;
  #ifdef FOG_COMBINED_WITH_TSPACE
    UNITY_EXTRACT_FOG_FROM_TSPACE(IN);
  #elif defined (FOG_COMBINED_WITH_WORLD_POS)
    UNITY_EXTRACT_FOG_FROM_WORLD_POS(IN);
  #else
    UNITY_EXTRACT_FOG(IN);
  #endif
  #ifdef FOG_COMBINED_WITH_TSPACE
    UNITY_RECONSTRUCT_TBN(IN);
  #else
    UNITY_EXTRACT_TBN(IN);
  #endif
  UNITY_INITIALIZE_OUTPUT(Input,surfIN);
  surfIN.color.x = 1.0;
  surfIN.uv_MainTex.x = 1.0;
  surfIN.uv2_FaceTex.x = 1.0;
  surfIN.uv2_OutlineTex.x = 1.0;
  surfIN.param.x = 1.0;
  surfIN.viewDirEnv.x = 1.0;
  surfIN.uv_MainTex = IN.pack0.xy;
  surfIN.uv2_FaceTex = IN.pack0.zw;
  surfIN.uv2_OutlineTex = IN.pack1.xy;
  surfIN.param = IN.custompack0.xy;
  surfIN.viewDirEnv = IN.custompack1.xyz;
  #ifndef USING_DIRECTIONAL_LIGHT
    fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
  #else
    fixed3 lightDir = _WorldSpaceLightPos0.xyz;
  #endif
  float3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
  surfIN.color = IN.color;
  #ifdef UNITY_COMPILER_HLSL
  SurfaceOutput o = (SurfaceOutput)0;
  #else
  SurfaceOutput o;
  #endif
  o.Albedo = 0.0;
  o.Emission = 0.0;
  o.Specular = 0.0;
  o.Alpha = 0.0;
  o.Gloss = 0.0;
  fixed3 normalWorldVertex = fixed3(0,0,1);
  o.Normal = fixed3(0,0,1);

  // call surface function
  PixShader (surfIN, o);


   o.Albedo = lerp(o.Albedo, dissolveAlbedo, dissolveBlend);
   o.Emission = lerp(o.Emission, dissolveEmission, dissolveBlend);

  // compute lighting & shadowing factor
  UNITY_LIGHT_ATTENUATION(atten, IN, worldPos)
  fixed4 c = 0;
  float3 worldN;
  worldN.x = dot(_unity_tbn_0, o.Normal);
  worldN.y = dot(_unity_tbn_1, o.Normal);
  worldN.z = dot(_unity_tbn_2, o.Normal);
  worldN = normalize(worldN);
  o.Normal = worldN;

  // Setup lighting environment
  UnityGI gi;
  UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
  gi.indirect.diffuse = 0;
  gi.indirect.specular = 0;
  gi.light.color = _LightColor0.rgb;
  gi.light.dir = lightDir;
  // Call GI (lightmaps/SH/reflections) lighting function
  UnityGIInput giInput;
  UNITY_INITIALIZE_OUTPUT(UnityGIInput, giInput);
  giInput.light = gi.light;
  giInput.worldPos = worldPos;
  giInput.worldViewDir = worldViewDir;
  giInput.atten = atten;
  #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
    giInput.lightmapUV = IN.lmap;
  #else
    giInput.lightmapUV = 0.0;
  #endif
  #if UNITY_SHOULD_SAMPLE_SH && !UNITY_SAMPLE_FULL_SH_PER_PIXEL
    giInput.ambient = IN.sh;
  #else
    giInput.ambient.rgb = 0.0;
  #endif
  giInput.probeHDR[0] = unity_SpecCube0_HDR;
  giInput.probeHDR[1] = unity_SpecCube1_HDR;
  #if defined(UNITY_SPECCUBE_BLENDING) || defined(UNITY_SPECCUBE_BOX_PROJECTION)
    giInput.boxMin[0] = unity_SpecCube0_BoxMin; // .w holds lerp value for blending
  #endif
  #ifdef UNITY_SPECCUBE_BOX_PROJECTION
    giInput.boxMax[0] = unity_SpecCube0_BoxMax;
    giInput.probePosition[0] = unity_SpecCube0_ProbePosition;
    giInput.boxMax[1] = unity_SpecCube1_BoxMax;
    giInput.boxMin[1] = unity_SpecCube1_BoxMin;
    giInput.probePosition[1] = unity_SpecCube1_ProbePosition;
  #endif
  LightingBlinnPhong_GI(o, giInput, gi);

  // realtime lighting: call lighting function
  c += LightingBlinnPhong (o, worldViewDir, gi);
  c.rgb += o.Emission;
  UNITY_APPLY_FOG(_unity_fogCoord, c); // apply fog
  return c;
}





ENDCG

}

	// ---- forward rendering additive lights pass:
	Pass {
		Name "FORWARD"
		Tags { "LightMode" = "ForwardAdd" }
		ZWrite Off Blend One One
		Blend SrcAlpha One

CGPROGRAM
// compile directives
#pragma vertex vert_PixShader
#pragma fragment frag_PixShader
#pragma target 3.0
#pragma glsl
#pragma multi_compile_instancing
#pragma multi_compile_fog
#pragma skip_variants INSTANCING_ON
#pragma multi_compile_fwdadd noshadowmask nodynlightmap nodirlightmap nolightmap noshadow
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
// Surface shader code generated based on:
// vertex modifier: 'VertShader'
// writes to per-pixel normal: YES
// writes to emission: YES
// writes to occlusion: no
// needs world space reflection vector: no
// needs world space normal vector: no
// needs screen space position: no
// needs world space position: no
// needs view direction: no
// needs world space view direction: no
// needs world space position for lighting: YES
// needs world space view direction for lighting: YES
// needs world space view direction for lightmaps: no
// needs vertex color: YES
// needs VFACE: no
// passes tangent-to-world matrix to pixel shader: YES
// reads from normal: no
// 3 texcoords actually used
//   float2 _MainTex
//   float2 _FaceTex
//   float2 _OutlineTex
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"

#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))

// Original surface shader snippet:
#line 107 ""
#ifdef DUMMY_PREPROCESSOR_TO_WORK_AROUND_HLSL_COMPILER_LINE_HANDLING
#endif
/* UNITY: Original start of shader */
	//#pragma surface PixShader BlinnPhong alpha:blend vertex:VertShader nolightmap nodirlightmap
	//#pragma target 3.0
	#pragma shader_feature __ GLOW_ON
	//#pragma glsl

	#include "TMPro_Properties.cginc"
	#include "TMPro.cginc"

	half _FaceShininess;
	half _OutlineShininess;


	float4 _MainTex_ST;
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


	struct Input
	{
		fixed4	color			: COLOR;
		float2	uv_MainTex;
		float2	uv2_FaceTex;
		float2  uv2_OutlineTex;
		float2	param;						// Weight, Scale
		float3	viewDirEnv;		
	};

	
	#define BEVEL_ON 1
	#include "TMPro_Surface.cginc"

	

// vertex-to-fragment interpolation data
struct v2f_PixShader {
  UNITY_POSITION(pos);
  float4 pack0 : TEXCOORD0; // _MainTex _FaceTex
  float2 pack1 : TEXCOORD1; // _OutlineTex
  float3 tSpace0 : TEXCOORD2;
  float3 tSpace1 : TEXCOORD3;
  float3 tSpace2 : TEXCOORD4;
  float3 worldPos : TEXCOORD5;
  fixed4 color : COLOR0;
  float2 custompack0 : TEXCOORD6; // param
  float3 custompack1 : TEXCOORD7; // viewDirEnv
  DECLARE_LIGHT_COORDS(8)
  UNITY_FOG_COORDS(9)
  UNITY_VERTEX_INPUT_INSTANCE_ID
  UNITY_VERTEX_OUTPUT_STEREO



ADVANCED_DISSOLVE_DATA(10)

};
float4 _FaceTex_ST;
float4 _OutlineTex_ST;

// vertex shader
v2f_PixShader vert_PixShader (appdata_full v) {
  UNITY_SETUP_INSTANCE_ID(v);
  v2f_PixShader o;
  UNITY_INITIALIZE_OUTPUT(v2f_PixShader,o);
  UNITY_TRANSFER_INSTANCE_ID(v,o);
  UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
  Input customInputData;
  VertShader (v, customInputData);
  o.custompack0.xy = customInputData.param;
  o.custompack1.xyz = customInputData.viewDirEnv;
  o.pos = UnityObjectToClipPos(v.vertex);
  o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
  o.pack0.zw = TRANSFORM_TEX(v.texcoord1, _FaceTex);
  o.pack1.xy = TRANSFORM_TEX(v.texcoord1, _OutlineTex);
  float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
  float3 worldNormal = UnityObjectToWorldNormal(v.normal);
  fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
  fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
  fixed3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;
  o.tSpace0 = float3(worldTangent.x, worldBinormal.x, worldNormal.x);
  o.tSpace1 = float3(worldTangent.y, worldBinormal.y, worldNormal.y);
  o.tSpace2 = float3(worldTangent.z, worldBinormal.z, worldNormal.z);
  o.worldPos.xyz = worldPos;
  o.color = v.color;

  COMPUTE_LIGHT_COORDS(o); // pass light cookie coordinates to pixel shader
  UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader



//VacuumShaders 
ADVANCED_DISSOLVE_INIT_DATA(o.pos, v.texcoord, v.texcoord1, o)

  return o;
}

// fragment shader
fixed4 frag_PixShader (v2f_PixShader IN) : SV_Target {
  UNITY_SETUP_INSTANCE_ID(IN);


  float3 normalWS = float3(IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z);
  float4 alpha = AdvancedDissolveGetAlpha(IN.pack0.xy, IN.worldPos, normalWS, IN.dissolveUV);
			
DoDissolveClip(alpha);


float3 dissolveAlbedo = 0;
float3 dissolveEmission = 0;
float dissolveBlend = DoDissolveAlbedoEmission(alpha, dissolveAlbedo, dissolveEmission, IN.pack0.xy, 0);




  // prepare and unpack data
  Input surfIN;
  #ifdef FOG_COMBINED_WITH_TSPACE
    UNITY_EXTRACT_FOG_FROM_TSPACE(IN);
  #elif defined (FOG_COMBINED_WITH_WORLD_POS)
    UNITY_EXTRACT_FOG_FROM_WORLD_POS(IN);
  #else
    UNITY_EXTRACT_FOG(IN);
  #endif
  #ifdef FOG_COMBINED_WITH_TSPACE
    UNITY_RECONSTRUCT_TBN(IN);
  #else
    UNITY_EXTRACT_TBN(IN);
  #endif
  UNITY_INITIALIZE_OUTPUT(Input,surfIN);
  surfIN.color.x = 1.0;
  surfIN.uv_MainTex.x = 1.0;
  surfIN.uv2_FaceTex.x = 1.0;
  surfIN.uv2_OutlineTex.x = 1.0;
  surfIN.param.x = 1.0;
  surfIN.viewDirEnv.x = 1.0;
  surfIN.uv_MainTex = IN.pack0.xy;
  surfIN.uv2_FaceTex = IN.pack0.zw;
  surfIN.uv2_OutlineTex = IN.pack1.xy;
  surfIN.param = IN.custompack0.xy;
  surfIN.viewDirEnv = IN.custompack1.xyz;
  float3 worldPos = IN.worldPos.xyz;
  #ifndef USING_DIRECTIONAL_LIGHT
    fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
  #else
    fixed3 lightDir = _WorldSpaceLightPos0.xyz;
  #endif
  float3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
  surfIN.color = IN.color;
  #ifdef UNITY_COMPILER_HLSL
  SurfaceOutput o = (SurfaceOutput)0;
  #else
  SurfaceOutput o;
  #endif
  o.Albedo = 0.0;
  o.Emission = 0.0;
  o.Specular = 0.0;
  o.Alpha = 0.0;
  o.Gloss = 0.0;
  fixed3 normalWorldVertex = fixed3(0,0,1);
  o.Normal = fixed3(0,0,1);

  // call surface function
  PixShader (surfIN, o);

  o.Albedo = lerp(o.Albedo, dissolveAlbedo, dissolveBlend);


  UNITY_LIGHT_ATTENUATION(atten, IN, worldPos)
  fixed4 c = 0;
  float3 worldN;
  worldN.x = dot(_unity_tbn_0, o.Normal);
  worldN.y = dot(_unity_tbn_1, o.Normal);
  worldN.z = dot(_unity_tbn_2, o.Normal);
  worldN = normalize(worldN);
  o.Normal = worldN;

  // Setup lighting environment
  UnityGI gi;
  UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
  gi.indirect.diffuse = 0;
  gi.indirect.specular = 0;
  gi.light.color = _LightColor0.rgb;
  gi.light.dir = lightDir;
  gi.light.color *= atten;
  c += LightingBlinnPhong (o, worldViewDir, gi);
  UNITY_APPLY_FOG(_unity_fogCoord, c); // apply fog
  return c;
}




ENDCG

}

	// ---- end of surface shader generated code


	// Pass to render object as a shadow caster
	Pass
	{
		Name "Caster"
		Tags { "LightMode" = "ShadowCaster" }
		Offset 1, 1

		Fog {Mode Off}
		ZWrite On
		ZTest LEqual
		Cull Off

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_shadowcaster
		#include "UnityCG.cginc"


		sampler2D _MainTex;
	float4 _MainTex_ST;
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

		struct v2f {
			V2F_SHADOW_CASTER;
			float2	uv			: TEXCOORD1;
			float2	uv2			: TEXCOORD3;
			float	alphaClip	: TEXCOORD2;


			float3 worldPos : TEXCOORD4;
			float3 normalWS : TEXCOORD5;
			ADVANCED_DISSOLVE_DATA(6)

		};

		uniform float4 _OutlineTex_ST;
		float _OutlineWidth;
		float _FaceDilate;
		float _ScaleRatioA;

		v2f vert( appdata_full v )
		{
			v2f o;
			TRANSFER_SHADOW_CASTER(o)
			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.uv2 = TRANSFORM_TEX(v.texcoord, _OutlineTex);
			o.alphaClip = (1.0 - _OutlineWidth * _ScaleRatioA - _FaceDilate * _ScaleRatioA) / 2;



			//VacuumShaders 
			o.worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1)).xyz;
			o.normalWS = UnityObjectToWorldNormal(v.normal);
			ADVANCED_DISSOLVE_INIT_DATA(UnityObjectToClipPos(v.vertex), v.texcoord, v.texcoord1, o);

			return o;
		}


		float4 frag(v2f i) : COLOR
		{
		
		float4 alpha = AdvancedDissolveGetAlpha(i.uv.xy, i.worldPos, i.normalWS, i.dissolveUV);		
		DoDissolveClip(alpha);
		

			fixed4 texcol = tex2D(_MainTex, i.uv).a;
			clip(texcol.a - i.alphaClip);
			SHADOW_CASTER_FRAGMENT(i)
		}
		ENDCG
	}
}

CustomEditor "TMPro.EditorUtilities.AdvancedDisolve_TMP_SDFShaderGUI"
}

