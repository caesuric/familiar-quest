Shader "Hidden/VacuumShaders/Advanced Dissolve/Nature/Tree Creator/Bark Rendertex" {
Properties {
    _MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}

[Cutout]_Cutoff("   Alpha Cutoff", Range(0,1)) = 0.5

    _BumpSpecMap ("Normalmap (GA) Spec (R)", 2D) = "bump" {}
    _TranslucencyMap ("Trans (RGB) Gloss(A)", 2D) = "white" {}

    // These are here only to provide default values
    _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)




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

SubShader {

			Cull[_Cull]

    Pass {
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"



sampler2D _MainTex;
sampler2D _BumpSpecMap;
sampler2D _TranslucencyMap;
fixed4 _SpecColor;
fixed _Cutoff;


#pragma shader_feature_local _ _DISSOLVEGLOBALCONTROL_MASK_ONLY _DISSOLVEGLOBALCONTROL_MASK_AND_EDGE _DISSOLVEGLOBALCONTROL_ALL
#pragma shader_feature_local _ _DISSOLVEMAPPINGTYPE_TRIPLANAR _DISSOLVEMAPPINGTYPE_SCREEN_SPACE
#pragma shader_feature_local _ _DISSOLVEALPHASOURCE_CUSTOM_MAP _DISSOLVEALPHASOURCE_TWO_CUSTOM_MAPS _DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS
#pragma shader_feature_local _ _DISSOLVEMASK_XYZ_AXIS _DISSOLVEMASK_PLANE _DISSOLVEMASK_SPHERE _DISSOLVEMASK_BOX _DISSOLVEMASK_CYLINDER _DISSOLVEMASK_CONE
#pragma shader_feature_local _ _DISSOLVEEDGETEXTURESOURCE_GRADIENT _DISSOLVEEDGETEXTURESOURCE_MAIN_MAP _DISSOLVEEDGETEXTURESOURCE_CUSTOM
#pragma shader_feature_local _ _DISSOLVEMASKCOUNT_TWO _DISSOLVEMASKCOUNT_THREE _DISSOLVEMASKCOUNT_FOUR

#define DISSOLVE_LEGACY_RENDER_PIPELIN
#define DISSOLVE_LEGACY_MAINTEX
#define DISSOLVE_LEGACY_TEXTURE_SAMPLE


#include "../../cginc/AdvancedDissolve.cginc"
#include "../../cginc/Integration_CurvedWorld.cginc"



struct v2f {
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
    float3 color : TEXCOORD1;
    float2 params1: TEXCOORD2;
    float2 params2: TEXCOORD3;
    float2 params3: TEXCOORD4;
	UNITY_VERTEX_OUTPUT_STEREO


		float3 worldPos : TEXCOORD5;
		float3 normalWS : TEXCOORD6;
		ADVANCED_DISSOLVE_DATA(7)

};



CBUFFER_START(UnityTerrainImposter)
    float3 _TerrainTreeLightDirections[4];
    float4 _TerrainTreeLightColors[4];
CBUFFER_END

float2 CalcTreeLightingParams(float3 normal, float3 lightDir, float3 viewDir)
{
    float2 output;
    half nl = dot (normal, lightDir);
    output.r = max (0, nl);

    half3 h = normalize (lightDir + viewDir);
    float nh = max (0, dot (normal, h));
    output.g = nh;
    return output;
}

v2f vert (appdata_full v) {
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


	CURVED_WORLD_TRANSFORM_POINT_AND_NORMAL(v.vertex, v.normal, v.tangent)

    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.texcoord.xy;
    float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));

    /* We used to do a for loop and store params as a texcoord array[3].
     * HLSL compiler, however, unrolls this loop and opens up the uniforms
     * into 3 separate texcoords, but doesn't do it on fragment shader.
     * This discrepancy causes error on iOS, so do it manually. */
    o.params1 = CalcTreeLightingParams(v.normal, _TerrainTreeLightDirections[0], viewDir);
    o.params2 = CalcTreeLightingParams(v.normal, _TerrainTreeLightDirections[1], viewDir);
    o.params3 = CalcTreeLightingParams(v.normal, _TerrainTreeLightDirections[2], viewDir);

    o.color = v.color.a;


	o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
	//VacuumShaders
	o.normalWS = o.normalWS =  UnityObjectToWorldNormal(v.normal);
	ADVANCED_DISSOLVE_INIT_DATA(o.pos, v.texcoord, v.texcoord1.xy, o);

    return o;
}





void ApplyTreeLighting(inout half3 light, half3 albedo, half gloss, half specular, half3 lightColor, float2 param)
{
    half nl = param.r;
    light += albedo * lightColor * nl;

    float nh = param.g;
    float spec = pow (nh, specular) * gloss;
    light += lightColor * _SpecColor.rgb * spec;
}

fixed4 frag (v2f i) : SV_Target
{


float4 alpha = AdvancedDissolveGetAlpha(i.uv.xy, i.worldPos, i.normalWS, i.dissolveUV);
DoDissolveClip(alpha);


float3 dissolveAlbedo = 0;
float3 dissolveEmission = 0;
float dissolveBlend = DoDissolveAlbedoEmission(alpha, dissolveAlbedo, dissolveEmission, i.uv.xy, 0);




    fixed4 albedo = tex2D (_MainTex, i.uv);



	albedo.rgb = lerp(albedo.rgb, dissolveAlbedo, dissolveBlend);

	albedo.rgb *= i.color;
    half gloss = tex2D(_TranslucencyMap, i.uv).a;
    half specular = tex2D (_BumpSpecMap, i.uv).r * 128.0;

    half3 light = UNITY_LIGHTMODEL_AMBIENT * albedo.rgb;

    ApplyTreeLighting(light, albedo.rgb, gloss, specular, _TerrainTreeLightColors[0], i.params1);
    ApplyTreeLighting(light, albedo.rgb, gloss, specular, _TerrainTreeLightColors[1], i.params2);
    ApplyTreeLighting(light, albedo.rgb, gloss, specular, _TerrainTreeLightColors[2], i.params3);

    fixed4 c;
    c.rgb = light;

	c.rgb += dissolveEmission * dissolveBlend;


    c.a = 1.0;
    UNITY_OPAQUE_ALPHA(c.a);
    return c;
}
ENDCG
    }
}

FallBack Off
CustomEditor "VacuumShaders.AdvancedDissolve.TreeGUI"
}
