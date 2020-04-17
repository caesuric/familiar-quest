Shader "Hidden/VacuumShaders/Advanced Dissolve/Nature/Tree Creator/Leaves Rendertex" 
{
Properties 
{
		_TranslucencyColor ("Translucency Color", Color) = (0.73,0.85,0.41,1) // (187,219,106,255)
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		_HalfOverCutoff ("0.5 / alpha cutoff", Range(0,1)) = 1.0
		_TranslucencyViewDependency ("View dependency", Range(0,1)) = 0.7

		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
		_BumpSpecMap ("Normalmap (GA) Spec (R) Shadow Offset (B)", 2D) = "bump" {}
		_TranslucencyMap ("Trans (B) Gloss(A)", 2D) = "white" {}



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
#pragma target 3.0
#include "UnityCG.cginc"
#include "UnityBuiltin3xTreeLibrary.cginc"


sampler2D _MainTex;
sampler2D _BumpSpecMap;
sampler2D _TranslucencyMap;
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
    float3 backContrib : TEXCOORD2;
    float3 nl : TEXCOORD3;
    float3 nh : TEXCOORD4;
	UNITY_VERTEX_OUTPUT_STEREO

		float3 worldPos : TEXCOORD5;
		float3 normalWS : TEXCOORD6;
		ADVANCED_DISSOLVE_DATA(7)
};

CBUFFER_START(UnityTerrainImposter)
    float3 _TerrainTreeLightDirections[4];
    float4 _TerrainTreeLightColors[4];
CBUFFER_END

v2f vert (appdata_full v) {
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


	CURVED_WORLD_TRANSFORM_POINT_AND_NORMAL(v.vertex, v.normal, v.tangent)


    ExpandBillboard (UNITY_MATRIX_IT_MV, v.vertex, v.normal, v.tangent);
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.texcoord.xy;
    float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));

    for (int j = 0; j < 3; j++)
    {
        float3 lightDir = _TerrainTreeLightDirections[j];

        half nl = dot (v.normal, lightDir);

        // view dependent back contribution for translucency
        half backContrib = saturate(dot(viewDir, -lightDir));

        // normally translucency is more like -nl, but looks better when it's view dependent
        backContrib = lerp(saturate(-nl), backContrib, _TranslucencyViewDependency);
        o.backContrib[j] = backContrib * 2;

        // wrap-around diffuse
        nl = max (0, nl * 0.6 + 0.4);
        o.nl[j] = nl;

        half3 h = normalize (lightDir + viewDir);
        float nh = max (0, dot (v.normal, h));
        o.nh[j] = nh;
    }

    o.color = v.color.a;



	o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
	//VacuumShaders
	o.normalWS = UnityObjectToWorldNormal(v.normal);
	ADVANCED_DISSOLVE_INIT_DATA(UnityObjectToClipPos(v.vertex), v.texcoord, v.texcoord1.xy, o)

    return o;
}


half3 CalcTreeLighting(half3 lightColor, fixed3 albedo, half backContrib, half nl, half nh, half specular, half gloss)
{
    half3 translucencyColor = backContrib * _TranslucencyColor;

    half spec = pow (nh, specular) * gloss;
    return (albedo * (translucencyColor + nl) + _SpecColor.rgb * spec) * lightColor;
}

fixed4 frag (v2f i) : SV_Target {



float4 alpha = AdvancedDissolveGetAlpha(i.uv.xy, i.worldPos, i.normalWS, i.dissolveUV);
DoDissolveClip(alpha);


float3 dissolveAlbedo = 0;
float3 dissolveEmission = 0;
float dissolveBlend = DoDissolveAlbedoEmission(alpha, dissolveAlbedo, dissolveEmission, i.uv.xy, 0);




    fixed4 col = tex2D (_MainTex, i.uv);
    clip (col.a - _Cutoff);

    fixed3 albedo = col.rgb * i.color;

	albedo = lerp(albedo, dissolveAlbedo, dissolveBlend);

    half specular = tex2D (_BumpSpecMap, i.uv).r * 128.0;

    fixed4 trngls = tex2D (_TranslucencyMap, i.uv);
    half gloss = trngls.a;

    half3 light = UNITY_LIGHTMODEL_AMBIENT * albedo;

    half3 backContribs = i.backContrib * trngls.b;

/*  This is unrolled below, indexing into a vec3 components is a terrible idea
    for (int j = 0; j < 3; j++)
    {
        half3 lightColor = _TerrainTreeLightColors[j].rgb;
        half3 translucencyColor = backContribs[j] * _TranslucencyColor;

        half nl = i.nl[j];
        half nh = i.nh[j];
        half spec = pow (nh, specular) * gloss;
        light += (albedo * (translucencyColor + nl) + _SpecColor.rgb * spec) * lightColor;
    }*/

    light += CalcTreeLighting(_TerrainTreeLightColors[0].rgb, albedo, backContribs.x, i.nl.x, i.nh.x, specular, gloss);
    light += CalcTreeLighting(_TerrainTreeLightColors[1].rgb, albedo, backContribs.y, i.nl.y, i.nh.y, specular, gloss);
    light += CalcTreeLighting(_TerrainTreeLightColors[2].rgb, albedo, backContribs.z, i.nl.z, i.nh.z, specular, gloss);

    fixed4 c;
    c.rgb = light;

	c.rgb += dissolveEmission * dissolveBlend;


    c.a = 1;
    return c;
}
ENDCG
    }
}

FallBack Off
CustomEditor "VacuumShaders.AdvancedDissolve.TreeGUI"
}
