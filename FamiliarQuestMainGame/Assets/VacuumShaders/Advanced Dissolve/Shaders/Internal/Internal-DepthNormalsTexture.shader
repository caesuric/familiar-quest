// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Hidden/Internal-DepthNormalsTexture" {
Properties {
    _MainTex ("", 2D) = "white" {}
    _Cutoff ("", Float) = 0.5
    _Color ("", Color) = (1,1,1,1)



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
    Tags { "RenderType"="Opaque" }
    Pass {
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
struct v2f {
    float4 pos : SV_POSITION;
    float4 nz : TEXCOORD0;
    UNITY_VERTEX_OUTPUT_STEREO
};
v2f vert( appdata_base v ) {
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    o.pos = UnityObjectToClipPos(v.vertex);
    o.nz.xyz = COMPUTE_VIEW_NORMAL;
    o.nz.w = COMPUTE_DEPTH_01;
    return o;
}
fixed4 frag(v2f i) : SV_Target {
    return EncodeDepthNormal (i.nz.w, i.nz.xyz);
}
ENDCG
    }
}

SubShader {
    Tags { "RenderType"="TransparentCutout" }
    Pass {
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
struct v2f {
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
    float4 nz : TEXCOORD1;
    UNITY_VERTEX_OUTPUT_STEREO
};
uniform float4 _MainTex_ST;
v2f vert( appdata_base v ) {
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
    o.nz.xyz = COMPUTE_VIEW_NORMAL;
    o.nz.w = COMPUTE_DEPTH_01;
    return o;
}
uniform sampler2D _MainTex;
uniform fixed _Cutoff;
uniform fixed4 _Color;
fixed4 frag(v2f i) : SV_Target {
    fixed4 texcol = tex2D( _MainTex, i.uv );
    clip( texcol.a*_Color.a - _Cutoff );
    return EncodeDepthNormal (i.nz.w, i.nz.xyz);
}
ENDCG
    }
}

SubShader {
    Tags { "RenderType"="TreeBark" }
    Pass {
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "UnityBuiltin3xTreeLibrary.cginc"
struct v2f {
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
    float4 nz : TEXCOORD1;
    UNITY_VERTEX_OUTPUT_STEREO
};
v2f vert( appdata_full v ) {
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    TreeVertBark(v);

    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.texcoord.xy;
    o.nz.xyz = COMPUTE_VIEW_NORMAL;
    o.nz.w = COMPUTE_DEPTH_01;
    return o;
}
fixed4 frag( v2f i ) : SV_Target {
    return EncodeDepthNormal (i.nz.w, i.nz.xyz);
}
ENDCG
    }
}

SubShader {
    Tags { "RenderType"="TreeLeaf" }
    Pass {
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "UnityBuiltin3xTreeLibrary.cginc"
struct v2f {
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
    float4 nz : TEXCOORD1;
    UNITY_VERTEX_OUTPUT_STEREO
};
v2f vert( appdata_full v ) {
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    TreeVertLeaf(v);

    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.texcoord.xy;
    o.nz.xyz = COMPUTE_VIEW_NORMAL;
    o.nz.w = COMPUTE_DEPTH_01;
    return o;
}
uniform sampler2D _MainTex;
uniform fixed _Cutoff;
fixed4 frag( v2f i ) : SV_Target {
    half alpha = tex2D(_MainTex, i.uv).a;

    clip (alpha - _Cutoff);
    return EncodeDepthNormal (i.nz.w, i.nz.xyz);
}
ENDCG
    }
}

SubShader {
    Tags { "RenderType"="TreeOpaque" "DisableBatching"="True" }
    Pass {
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
#include "TerrainEngine.cginc"
struct v2f {
    float4 pos : SV_POSITION;
    float4 nz : TEXCOORD0;
    UNITY_VERTEX_OUTPUT_STEREO
};
struct appdata {
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    fixed4 color : COLOR;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
v2f vert( appdata v ) {
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    TerrainAnimateTree(v.vertex, v.color.w);
    o.pos = UnityObjectToClipPos(v.vertex);
    o.nz.xyz = COMPUTE_VIEW_NORMAL;
    o.nz.w = COMPUTE_DEPTH_01;
    return o;
}
fixed4 frag(v2f i) : SV_Target {
    return EncodeDepthNormal (i.nz.w, i.nz.xyz);
}
ENDCG
    }
}

SubShader {
    Tags { "RenderType"="TreeTransparentCutout" "DisableBatching"="True" }
    Pass {
        Cull Back
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
#include "TerrainEngine.cginc"

struct v2f {
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
    float4 nz : TEXCOORD1;
    UNITY_VERTEX_OUTPUT_STEREO
};
struct appdata {
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    fixed4 color : COLOR;
    float4 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
v2f vert( appdata v ) {
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    TerrainAnimateTree(v.vertex, v.color.w);
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.texcoord.xy;
    o.nz.xyz = COMPUTE_VIEW_NORMAL;
    o.nz.w = COMPUTE_DEPTH_01;
    return o;
}
uniform sampler2D _MainTex;
uniform fixed _Cutoff;
fixed4 frag(v2f i) : SV_Target {
    half alpha = tex2D(_MainTex, i.uv).a;

    clip (alpha - _Cutoff);
    return EncodeDepthNormal (i.nz.w, i.nz.xyz);
}
ENDCG
    }
    Pass {
        Cull Front
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
#include "TerrainEngine.cginc"

struct v2f {
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
    float4 nz : TEXCOORD1;
    UNITY_VERTEX_OUTPUT_STEREO
};
struct appdata {
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    fixed4 color : COLOR;
    float4 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};
v2f vert( appdata v ) {
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    TerrainAnimateTree(v.vertex, v.color.w);
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.texcoord.xy;
    o.nz.xyz = -COMPUTE_VIEW_NORMAL;
    o.nz.w = COMPUTE_DEPTH_01;
    return o;
}
uniform sampler2D _MainTex;
uniform fixed _Cutoff;
fixed4 frag(v2f i) : SV_Target {
    fixed4 texcol = tex2D( _MainTex, i.uv );
    clip( texcol.a - _Cutoff );
    return EncodeDepthNormal (i.nz.w, i.nz.xyz);
}
ENDCG
    }

}

SubShader {
    Tags { "RenderType"="TreeBillboard" }
    Pass {
        Cull Off
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
#include "TerrainEngine.cginc"
struct v2f {
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
    float4 nz : TEXCOORD1;
    UNITY_VERTEX_OUTPUT_STEREO
};
v2f vert (appdata_tree_billboard v) {
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    TerrainBillboardTree(v.vertex, v.texcoord1.xy, v.texcoord.y);
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv.x = v.texcoord.x;
    o.uv.y = v.texcoord.y > 0;
    o.nz.xyz = float3(0,0,1);
    o.nz.w = COMPUTE_DEPTH_01;
    return o;
}
uniform sampler2D _MainTex;
fixed4 frag(v2f i) : SV_Target {
    fixed4 texcol = tex2D( _MainTex, i.uv );
    clip( texcol.a - 0.001 );
    return EncodeDepthNormal (i.nz.w, i.nz.xyz);
}
ENDCG
    }
}

SubShader {
    Tags { "RenderType"="GrassBillboard" }
    Pass {
        Cull Off
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
#include "TerrainEngine.cginc"

struct v2f {
    float4 pos : SV_POSITION;
    fixed4 color : COLOR;
    float2 uv : TEXCOORD0;
    float4 nz : TEXCOORD1;
    UNITY_VERTEX_OUTPUT_STEREO
};

v2f vert (appdata_full v) {
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    WavingGrassBillboardVert (v);
    o.color = v.color;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.texcoord.xy;
    o.nz.xyz = COMPUTE_VIEW_NORMAL;
    o.nz.w = COMPUTE_DEPTH_01;
    return o;
}
uniform sampler2D _MainTex;
uniform fixed _Cutoff;
fixed4 frag(v2f i) : SV_Target {
    fixed4 texcol = tex2D( _MainTex, i.uv );
    fixed alpha = texcol.a * i.color.a;
    clip( alpha - _Cutoff );
    return EncodeDepthNormal (i.nz.w, i.nz.xyz);
}
ENDCG
    }
}

SubShader {
    Tags { "RenderType"="Grass" }
    Pass {
        Cull Off
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
#include "TerrainEngine.cginc"
struct v2f {
    float4 pos : SV_POSITION;
    fixed4 color : COLOR;
    float2 uv : TEXCOORD0;
    float4 nz : TEXCOORD1;
    UNITY_VERTEX_OUTPUT_STEREO
};

v2f vert (appdata_full v) {
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    WavingGrassVert (v);
    o.color = v.color;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.texcoord;
    o.nz.xyz = COMPUTE_VIEW_NORMAL;
    o.nz.w = COMPUTE_DEPTH_01;
    return o;
}
uniform sampler2D _MainTex;
uniform fixed _Cutoff;
fixed4 frag(v2f i) : SV_Target {
    fixed4 texcol = tex2D( _MainTex, i.uv );
    fixed alpha = texcol.a * i.color.a;
    clip( alpha - _Cutoff );
    return EncodeDepthNormal (i.nz.w, i.nz.xyz);
}
ENDCG
    }
}






SubShader{
	Tags { "RenderType"="AdvancedDissolveCutout" }

    Pass 
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		

		uniform float4 _MainTex_ST;
		uniform sampler2D _MainTex;
		uniform fixed4 _Color;
		uniform fixed _Cutoff;

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


		struct v2f 
		{
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float4 nz : TEXCOORD1;

			//VacuumShaders
			float3 worldPos : TEXCOORD2;
			float3 normalWP : TEXCOORD3;
			ADVANCED_DISSOLVE_DATA(4)


			UNITY_VERTEX_OUTPUT_STEREO
		};

		

		v2f vert(appdata_full v )
		{
			v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


			CURVED_WORLD_TRANSFORM_POINT(v. vertex)


			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.nz.xyz = COMPUTE_VIEW_NORMAL;
			o.nz.w = COMPUTE_DEPTH_01;



			//VacuumShaders
			o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			o.normalWP = UnityObjectToWorldNormal(v.normal);
			ADVANCED_DISSOLVE_INIT_DATA(o.pos, v.texcoord, v.texcoord1, o);			


			return o;
		}

		


		fixed4 frag(v2f i) : SV_Target 
		{
			
			float4 alpha = AdvancedDissolveGetAlpha(i.uv, i.worldPos, i.normalWP, i.dissolveUV);
			DoDissolveClip(alpha);
			

#ifdef _ALPHATEST_ON
			clip(tex2D(_MainTex, i.uv).a * _Color.a - _Cutoff);
#endif

			return EncodeDepthNormal (i.nz.w, i.nz.xyz);
		}
		ENDCG
    }
}

SubShader{
			Tags{ "RenderType" = "AdvancedDissolveTreeOpaque" "DisableBatching" = "True" }
			
			Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "TerrainEngine.cginc"


			uniform float4 _MainTex_ST;
			uniform sampler2D _MainTex;
			uniform fixed4 _Color;
			uniform fixed _Cutoff;

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

			struct v2f 
			{
			float4 pos : SV_POSITION;
			float4 nz : TEXCOORD0;
			UNITY_VERTEX_OUTPUT_STEREO


			float2 uv : TEXCOORD1;
			float3 worldPos : TEXCOORD2;
			float3 normalWP : TEXCOORD3;
			ADVANCED_DISSOLVE_DATA(4)
		};

			v2f vert(appdata_full v) {
			v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			TerrainAnimateTree(v.vertex, v.color.w);


			CURVED_WORLD_TRANSFORM_POINT(v. vertex)


			o.pos = UnityObjectToClipPos(v.vertex);
			o.nz.xyz = COMPUTE_VIEW_NORMAL;
			o.nz.w = COMPUTE_DEPTH_01;


			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			o.normalWP = UnityObjectToWorldNormal(v.normal);
			ADVANCED_DISSOLVE_INIT_DATA(o.pos, v.texcoord, v.texcoord1, o);		

			return o;
		}
		fixed4 frag(v2f i) : SV_Target{


		float4 alpha = AdvancedDissolveGetAlpha(i.uv, i.worldPos, i.normalWP, i.dissolveUV);
		DoDissolveClip(alpha);
		


#ifdef _ALPHATEST_ON
		clip(tex2D(_MainTex, i.uv).a * _Color.a - _Cutoff);
#endif

			return EncodeDepthNormal(i.nz.w, i.nz.xyz);
		}
			ENDCG
			}
		}

SubShader{
			Tags{ "RenderType" = "AdvancedDissolveTreeBark" }
			Pass{
			CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#include "UnityBuiltin3xTreeLibrary.cginc"



		uniform sampler2D _MainTex;
		uniform fixed _Cutoff;

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


			struct v2f {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float4 nz : TEXCOORD1;
			UNITY_VERTEX_OUTPUT_STEREO

				//VacuumShaders
				float3 worldPos : TEXCOORD2;
				float3 normalWP : TEXCOORD3;
			ADVANCED_DISSOLVE_DATA(4)
		};
		v2f vert(appdata_full v) {
			v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			TreeVertBark(v);


			CURVED_WORLD_TRANSFORM_POINT(v. vertex)


			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = v.texcoord.xy;
			o.nz.xyz = COMPUTE_VIEW_NORMAL;
			o.nz.w = COMPUTE_DEPTH_01;


			o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			o.normalWP = UnityObjectToWorldNormal(v.normal);
			ADVANCED_DISSOLVE_INIT_DATA(o.pos, v.texcoord, v.texcoord1, o);		


			return o;
		}
		fixed4 frag(v2f i) : SV_Target{
		
		float4 alpha = AdvancedDissolveGetAlpha(i.uv, i.worldPos, i.normalWP, i.dissolveUV);
		DoDissolveClip(alpha);

		
#ifdef _ALPHATEST_ON
		clip(tex2D(_MainTex, i.uv).a * _Color.a - _Cutoff);
#endif


			return EncodeDepthNormal(i.nz.w, i.nz.xyz);
		}
			ENDCG
		}
		}

SubShader {
    Tags { "RenderType"="AdvancedDissolveTreeLeaf" }
    Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#include "UnityBuiltin3xTreeLibrary.cginc"

		uniform sampler2D _MainTex;
				uniform fixed _Cutoff;

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


		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float4 nz : TEXCOORD1;
			UNITY_VERTEX_OUTPUT_STEREO


			float3 worldPos : TEXCOORD2;
			float3 normalWP : TEXCOORD3;
			ADVANCED_DISSOLVE_DATA(4)
		};
		v2f vert( appdata_full v ) {
			v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			TreeVertLeaf(v);


			CURVED_WORLD_TRANSFORM_POINT(v. vertex)


			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = v.texcoord.xy;
			o.nz.xyz = COMPUTE_VIEW_NORMAL;
			o.nz.w = COMPUTE_DEPTH_01;


			o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			//VacuumShaders
			o.normalWP = UnityObjectToWorldNormal(v.normal);
			ADVANCED_DISSOLVE_INIT_DATA(o.pos, v.texcoord, v.texcoord1, o);		


			return o;
		}

		fixed4 frag( v2f i ) : SV_Target {


		float4 dAlpha = AdvancedDissolveGetAlpha(i.uv, i.worldPos, i.normalWP, i.dissolveUV);
		DoDissolveClip(dAlpha);
		


			half alpha = tex2D(_MainTex, i.uv).a;

			clip (alpha - _Cutoff);
			return EncodeDepthNormal (i.nz.w, i.nz.xyz);
		}
		ENDCG
    }
}

SubShader{
			Tags{ "RenderType" = "AdvancedDissolveTreeTransparentCutout" "DisableBatching" = "True" }
			
			
	Pass{
			Cull Back
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "TerrainEngine.cginc"


			uniform sampler2D _MainTex;
		uniform fixed _Cutoff;

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

			struct v2f {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float4 nz : TEXCOORD1;
			UNITY_VERTEX_OUTPUT_STEREO

				float3 worldPos : TEXCOORD2;
				float3 normalWP : TEXCOORD3;
			ADVANCED_DISSOLVE_DATA(4)
		};


		v2f vert(appdata_full v) {
			v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			TerrainAnimateTree(v.vertex, v.color.w);


			CURVED_WORLD_TRANSFORM_POINT(v. vertex)


			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = v.texcoord.xy;
			o.nz.xyz = COMPUTE_VIEW_NORMAL;
			o.nz.w = COMPUTE_DEPTH_01;


			o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			//VacuumShaders
			o.normalWP = UnityObjectToWorldNormal(v.normal);
			ADVANCED_DISSOLVE_INIT_DATA(o.pos, v.texcoord, v.texcoord1, o);		

			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target{


		float4 dAlpha = AdvancedDissolveGetAlpha(i.uv, i.worldPos, i.normalWP, i.dissolveUV);
		DoDissolveClip(dAlpha);


			half alpha = tex2D(_MainTex, i.uv).a;

		clip(alpha - _Cutoff);
		return EncodeDepthNormal(i.nz.w, i.nz.xyz);
		}
			ENDCG
		}
			
	Pass{
			Cull Front
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
#include "TerrainEngine.cginc"


			uniform sampler2D _MainTex;
		uniform fixed _Cutoff;


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



			struct v2f {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float4 nz : TEXCOORD1;
			UNITY_VERTEX_OUTPUT_STEREO


				float3 worldPos : TEXCOORD2;
				float3 normalWP : TEXCOORD3;
			ADVANCED_DISSOLVE_DATA(4)
		};
		v2f vert(appdata_full v) {
			v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			TerrainAnimateTree(v.vertex, v.color.w);


			CURVED_WORLD_TRANSFORM_POINT(v. vertex)


			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = v.texcoord.xy;
			o.nz.xyz = -COMPUTE_VIEW_NORMAL;
			o.nz.w = COMPUTE_DEPTH_01;


			o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			//VacuumShaders
			o.normalWP = UnityObjectToWorldNormal(v.normal);
			ADVANCED_DISSOLVE_INIT_DATA(o.pos, v.texcoord, v.texcoord1, o);		

			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target{

		float4 alpha = AdvancedDissolveGetAlpha(i.uv, i.worldPos, i.normalWP, i.dissolveUV);
		DoDissolveClip(alpha);

		

			fixed4 texcol = tex2D(_MainTex, i.uv);
		clip(texcol.a - _Cutoff);
		return EncodeDepthNormal(i.nz.w, i.nz.xyz);
		}
			ENDCG
		}

		}


Fallback Off
}
