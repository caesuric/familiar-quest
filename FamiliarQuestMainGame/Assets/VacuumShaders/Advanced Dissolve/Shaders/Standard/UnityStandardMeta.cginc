// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

#ifndef UNITY_STANDARD_META_INCLUDED
#define UNITY_STANDARD_META_INCLUDED

// Functionality for Standard shader "meta" pass
// (extracts albedo/emission for lightmapper etc.)

#include "UnityCG.cginc"
#include "UnityStandardInput.cginc"
#include "UnityMetaPass.cginc"
#include "UnityStandardCore.cginc"

struct v2f_meta
{
    float4 uv       : TEXCOORD0;
    float4 pos      : SV_POSITION;

	float3 worldPos : TEXCOORD1;
	float3 normalWS :TEXCOORD2;
	ADVANCED_DISSOLVE_DATA(3)
};

v2f_meta vert_meta (VertexInput v)
{
    v2f_meta o;
    o.pos = UnityMetaVertexPosition(v.vertex, v.uv1.xy, v.uv2.xy, unity_LightmapST, unity_DynamicLightmapST);
    o.uv = TexCoords(v);



	//VacuumShaders
	o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
	o.normalWS = UnityObjectToWorldNormal(v.normal);
	ADVANCED_DISSOLVE_INIT_DATA(UnityObjectToClipPos(v.vertex), v.uv0, v.uv1, o);

#if defined(_DISSOLVEMASK_XYZ_AXIS) || defined(_DISSOLVEMASK_PLANE) || defined(_DISSOLVEMASK_SPHERE) || defined(_DISSOLVEMASK_BOX) || defined(_DISSOLVEMASK_CYLINDER) || defined(_DISSOLVEMASK_CONE)
	o.worldPos += _Dissolve_ObjectWorldPos;
#endif


    return o;
}

// Albedo for lightmapping should basically be diffuse color.
// But rough metals (black diffuse) still scatter quite a lot of light around, so
// we want to take some of that into account too.
half3 UnityLightmappingAlbedo (half3 diffuse, half3 specular, half smoothness)
{
    half roughness = SmoothnessToRoughness(smoothness);
    half3 res = diffuse;
    res += specular * roughness * 0.5;
    return res;
}

float4 frag_meta (v2f_meta i) : SV_Target
{
	// we're interested in diffuse & specular colors,
	// and surface roughness to produce final albedo.
	FragmentCommonData data = UNITY_SETUP_BRDF_INPUT(i.uv);


float4 alpha = AdvancedDissolveGetAlpha(i.uv.xy, i.worldPos, i.normalWS, i.dissolveUV);

#ifdef _ALPHATEST_ON
	clip(tex2D(_MainTex, i.uv).a * _Color.a - _Cutoff * 1.01);
#endif

float3 dissolveAlbedo = 0;
float3 dissolveEmission = 0;
float dissolveBlend = DoDissolveAlbedoEmission(alpha, dissolveAlbedo, dissolveEmission, i.uv.xy, 0);


	UnityMetaInput o;
	UNITY_INITIALIZE_OUTPUT(UnityMetaInput, o);


#if defined(_ALPHABLEND_ON) || defined(_ALPHAPREMULTIPLY_ON)
	dissolveBlend *= tex2D(_MainTex, i.uv).a * _Color.a;
#endif

	data.diffColor = lerp(data.diffColor, dissolveAlbedo, dissolveBlend);


#if defined(EDITOR_VISUALIZATION)
    o.Albedo = data.diffColor;
#else
    o.Albedo = UnityLightmappingAlbedo (data.diffColor, data.specColor, data.smoothness);
#endif
    o.SpecularColor = data.specColor;
    o.Emission = Emission(i.uv.xy);


	o.Emission = lerp(o.Emission, dissolveEmission, dissolveBlend);

    return UnityMetaFragment(o);
}

#endif // UNITY_STANDARD_META_INCLUDED
