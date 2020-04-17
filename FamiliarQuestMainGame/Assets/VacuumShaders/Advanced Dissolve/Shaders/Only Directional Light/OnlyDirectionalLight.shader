Shader "VacuumShaders/Advanced Dissolve/Only Directional Light"  
{
    Properties 
    {
        _Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		
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
        Tags{ "RenderType" = "AdvancedDissolveCutout" "DisableBatching" = "True" }
		LOD 80
		Cull[_Cull]

    Pass {
        Name "FORWARD"
        Tags { "LightMode" = "ForwardBase" }
CGPROGRAM
#pragma vertex vert_surf
#pragma fragment frag_surf
#pragma target 2.0
#pragma multi_compile_fwdbase
#pragma multi_compile_fog
#include "HLSLSupport.cginc"
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"





        inline float3 LightingLambertVS (float3 normal, float3 lightDir)
        {
            fixed diff = max (0, dot (normal, lightDir));
            return _LightColor0.rgb * diff;
        }

       
sampler2D _MainTex;
float4 _MainTex_ST;
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


        struct v2f_surf {
  float4 pos : SV_POSITION;
  float2 pack0 : TEXCOORD0;
  #ifndef LIGHTMAP_ON
  fixed3 normal : TEXCOORD1;
  #endif
  #ifdef LIGHTMAP_ON
  float2 lmap : TEXCOORD2;
  #endif
  #ifndef LIGHTMAP_ON
  fixed3 vlight : TEXCOORD2;
  #endif
  LIGHTING_COORDS(3,4)
  UNITY_FOG_COORDS(5)
  UNITY_VERTEX_OUTPUT_STEREO


  				float3 worldPos : TEXCOORD6;
				float3 normalWS :TEXCOORD7;
				ADVANCED_DISSOLVE_DATA(8)

};


v2f_surf vert_surf (appdata_full v)
{
    v2f_surf o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


    CURVED_WORLD_TRANSFORM_POINT_AND_NORMAL(v.vertex, v.normal, v.tangent)


    o.pos = UnityObjectToClipPos(v.vertex);
    o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
    #ifdef LIGHTMAP_ON
    o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
    #endif
    float3 worldN = UnityObjectToWorldNormal(v.normal);
    #ifndef LIGHTMAP_ON
    o.normal = worldN;
    #endif
    #ifndef LIGHTMAP_ON

    o.vlight = ShadeSH9 (float4(worldN,1.0));
    o.vlight += LightingLambertVS (worldN, _WorldSpaceLightPos0.xyz);

    #endif
    TRANSFER_VERTEX_TO_FRAGMENT(o);
    UNITY_TRANSFER_FOG(o,o.pos);


    o.worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1)).xyz;
	o.normalWS = UnityObjectToWorldNormal(v.normal);
	ADVANCED_DISSOLVE_INIT_DATA(o.pos, v.texcoord, v.texcoord1, o);


    return o;
}
fixed4 frag_surf (v2f_surf IN) : SV_Target
{
			float4 alpha = AdvancedDissolveGetAlpha(IN.pack0.xy, IN.worldPos, IN.normalWS, IN.dissolveUV);
			DoDissolveClip(alpha);
				


			float3 dissolveAlbedo = 0;
			float3 dissolveEmission = 0;
			float dissolveBlend = DoDissolveAlbedoEmission(alpha, dissolveAlbedo, dissolveEmission, IN.pack0.xy, 0);


  	fixed4 tex = tex2D(_MainTex, IN.pack0.xy) * _Color;

				float alphaFromSurface = tex.a * _Color.a;
#ifdef _ALPHATEST_ON
				clip(alphaFromSurface - _Cutoff);
#endif

				//VacuumShaders
				tex.rgb = lerp(tex.rgb, dissolveAlbedo, dissolveBlend);
    
    



    fixed atten = LIGHT_ATTENUATION(IN);
    fixed4 c = 0;
    #ifndef LIGHTMAP_ON
    c.rgb = tex * IN.vlight * atten;
    #endif
    #ifdef LIGHTMAP_ON
    fixed3 lm = DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, IN.lmap.xy));
    #ifdef SHADOWS_SCREEN
    c.rgb += tex * min(lm, atten*2);
    #else
    c.rgb += tex * lm;
    #endif
    c.a = tex.a;
    #endif
    UNITY_APPLY_FOG(IN.fogCoord, c);
    UNITY_OPAQUE_ALPHA(c.a);
    return c;
}

ENDCG
    }


    UsePass "Hidden/VacuumShaders/Advanced Dissolve/Shadow/SHADOWCASTER"

}

	CustomEditor "VacuumShaders.AdvancedDissolve.OneDirectionalLightGUI"
}
