
#ifdef DISSOLVE_LEGACY_TEXTURE_SAMPLE
	#define DECLARE_TEXTURE_2D(t) sampler2D t; uniform sampler2D t##_Global;
#else
	#define DECLARE_TEXTURE_2D(t) TEXTURE2D(t); SAMPLER(sampler##t); uniform TEXTURE2D(t##_Global); SAMPLER(sampler##t##_Global);
#endif


float _DissolveCutoff;						uniform float _DissolveCutoff_Global;


float _DissolveMaskAxis;					uniform float _DissolveMaskAxis_Global;
float _DissolveMaskSpace;					uniform float _DissolveMaskSpace_Global;
float _DissolveMaskOffset;					uniform float _DissolveMaskOffset_Global;
float _DissolveMaskInvert;					uniform float _DissolveMaskInvert_Global;

float _DissolveEdgeWidth;					uniform float _DissolveEdgeWidth_Global;
float4 _DissolveEdgeColor;					uniform float4 _DissolveEdgeColor_Global;
float _DissolveGIMultiplier;				uniform float _DissolveGIMultiplier_Global;
float _DissolveEdgeColorIntensity;			uniform float _DissolveEdgeColorIntensity_Global;
float _DissolveEdgeShape;			        uniform float _DissolveEdgeShape_Global;

float _DissolveEdgeDistortionSource;		uniform float _DissolveEdgeDistortionSource_Global;
float _DissolveEdgeDistortionStrength;		uniform float _DissolveEdgeDistortionStrength_Global;

float _DissolveMainMapTiling;				uniform float _DissolveMainMapTiling_Global;

float _DissolveCombineWithMasterNodeAlpha;    uniform float _DissolveCombineWithMasterNodeAlpha_Global;
float _DissolveCombineWithMasterNodeColor;    uniform float _DissolveCombineWithMasterNodeColor_Global;

#if defined(_DISSOLVEEDGETEXTURESOURCE_GRADIENT) || defined(_DISSOLVEEDGETEXTURESOURCE_MAIN_MAP) || defined(_DISSOLVEEDGETEXTURESOURCE_CUSTOM)
	DECLARE_TEXTURE_2D(_DissolveEdgeTexture)

	float _DissolveEdgeTextureReverse;												uniform float _DissolveEdgeTextureReverse_Global;
	float _DissolveEdgeTexturePhaseOffset;											uniform float _DissolveEdgeTexturePhaseOffset_Global;
	float _DissolveEdgeTextureAlphaOffset;											uniform float _DissolveEdgeTextureAlphaOffset_Global;

	#if defined(_DISSOLVEEDGETEXTURESOURCE_GRADIENT)
		float _DissolveEdgeTextureIsDynamic;		uniform float _DissolveEdgeTextureIsDynamic_Global;
	#else
		float _DissolveEdgeTextureMipmap;           uniform float _DissolveEdgeTextureMipmap_Global;
	#endif
#endif
		 

#if defined(_DISSOLVEALPHASOURCE_CUSTOM_MAP) || defined(_DISSOLVEALPHASOURCE_TWO_CUSTOM_MAPS) || defined(_DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS)
	DECLARE_TEXTURE_2D(_DissolveMap1)		

	float4 _DissolveMap1_ST;											uniform float4 _DissolveMap1_ST_Global;
	float3 _DissolveMap1_Scroll;										uniform float3 _DissolveMap1_Scroll_Global;
	float  _DissolveMap1Intensity;										uniform float  _DissolveMap1Intensity_Global;
	int    _DissolveMap1Channel;					                    uniform int    _DissolveMap1Channel_Global;
#endif
	
#if defined(_DISSOLVEALPHASOURCE_TWO_CUSTOM_MAPS) || defined(_DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS)
	DECLARE_TEXTURE_2D(_DissolveMap2)        

	float4 _DissolveMap2_ST;											uniform float4 _DissolveMap2_ST_Global;
	float3 _DissolveMap2_Scroll;										uniform float3 _DissolveMap2_Scroll_Global;
	float  _DissolveMap2Intensity;										uniform float  _DissolveMap2Intensity_Global;
	int    _DissolveMap2Channel;					                    uniform int    _DissolveMap2Channel_Global;
#endif

#if defined(_DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS)
	DECLARE_TEXTURE_2D(_DissolveMap3)   

	float4 _DissolveMap3_ST;											uniform float4 _DissolveMap3_ST_Global;
	float3 _DissolveMap3_Scroll;										uniform float3 _DissolveMap3_Scroll_Global;
	float  _DissolveMap3Intensity;										uniform float  _DissolveMap3Intensity_Global;
	int    _DissolveMap3Channel;					                    uniform int    _DissolveMap3Channel_Global;
#endif  


float _DissolveSourceAlphaTexturesBlend;		uniform float _DissolveSourceAlphaTexturesBlend_Global;
float _DissolveAlphaSourceTexturesUVSet;		uniform float _DissolveAlphaSourceTexturesUVSet_Global;
float _DissolveNoiseStrength;					uniform float _DissolveNoiseStrength_Global;

float3 _DissolveMaskPosition;					uniform float3 _DissolveMaskPosition_Global;
float3 _DissolveMaskNormal;						uniform float3 _DissolveMaskNormal_Global;
float _DissolveMaskRadius;						uniform float  _DissolveMaskRadius_Global;
float _DissolveMaskHeight;						uniform float _DissolveMaskHeight_Global;

#if defined(_DISSOLVEMASKCOUNT_FOUR)
	
	float3 _DissolveMask2Position;				uniform float3 _DissolveMask2Position_Global;
	float3 _DissolveMask2Normal;				uniform float3 _DissolveMask2Normal_Global;
	float _DissolveMask2Radius;					uniform float  _DissolveMask2Radius_Global;
	float _DissolveMask2Height;					uniform float _DissolveMask2Height_Global;

	float3 _DissolveMask3Position;				uniform float3 _DissolveMask3Position_Global;
	float3 _DissolveMask3Normal;				uniform float3 _DissolveMask3Normal_Global;
	float _DissolveMask3Radius;					uniform float  _DissolveMask3Radius_Global;
	float _DissolveMask3Height;					uniform float _DissolveMask3Height_Global;

	float3 _DissolveMask4Position;				uniform float3 _DissolveMask4Position_Global;
	float3 _DissolveMask4Normal;				uniform float3 _DissolveMask4Normal_Global;
	float _DissolveMask4Radius;					uniform float  _DissolveMask4Radius_Global;
	float _DissolveMask4Height;					uniform float _DissolveMask4Height_Global;

	#if defined(_DISSOLVEMASK_BOX)
		float3 _DissolveMask2BoundsMin;			uniform float3 _DissolveMask2BoundsMin_Global;
		float3 _DissolveMask2BoundsMax;			uniform float3 _DissolveMask2BoundsMax_Global;
		float4x4 _DissolveMask2TRS;				uniform float4x4 _DissolveMask2TRS_Global;

		float3 _DissolveMask3BoundsMin;			uniform float3 _DissolveMask3BoundsMin_Global;
		float3 _DissolveMask3BoundsMax;			uniform float3 _DissolveMask3BoundsMax_Global;
		float4x4 _DissolveMask3TRS;				uniform float4x4 _DissolveMask3TRS_Global;

		float3 _DissolveMask4BoundsMin;			uniform float3 _DissolveMask4BoundsMin_Global;
		float3 _DissolveMask4BoundsMax;			uniform float3 _DissolveMask4BoundsMax_Global;
		float4x4 _DissolveMask4TRS;				uniform float4x4 _DissolveMask4TRS_Global;
	#endif

#elif defined(_DISSOLVEMASKCOUNT_THREE)
	
	float3 _DissolveMask2Position;				uniform float3 _DissolveMask2Position_Global;
	float3 _DissolveMask2Normal;				uniform float3 _DissolveMask2Normal_Global;
	float _DissolveMask2Radius;					uniform float  _DissolveMask2Radius_Global;
	float _DissolveMask2Height;					uniform float _DissolveMask2Height_Global;

	float3 _DissolveMask3Position;				uniform float3 _DissolveMask3Position_Global;
	float3 _DissolveMask3Normal;				uniform float3 _DissolveMask3Normal_Global;
	float _DissolveMask3Radius;					uniform float  _DissolveMask3Radius_Global;	
	float _DissolveMask3Height;					uniform float _DissolveMask3Height_Global;

	#if defined(_DISSOLVEMASK_BOX)
		float3 _DissolveMask2BoundsMin;			uniform float3 _DissolveMask2BoundsMin_Global;
		float3 _DissolveMask2BoundsMax;			uniform float3 _DissolveMask2BoundsMax_Global;
		float4x4 _DissolveMask2TRS;				uniform float4x4 _DissolveMask2TRS_Global;

		float3 _DissolveMask3BoundsMin;			uniform float3 _DissolveMask3BoundsMin_Global;
		float3 _DissolveMask3BoundsMax;			uniform float3 _DissolveMask3BoundsMax_Global;
		float4x4 _DissolveMask3TRS;				uniform float4x4 _DissolveMask3TRS_Global;
	#endif

#elif defined(_DISSOLVEMASKCOUNT_TWO)
	
	float3 _DissolveMask2Position;				uniform float3 _DissolveMask2Position_Global;
	float3 _DissolveMask2Normal;				uniform float3 _DissolveMask2Normal_Global;
	float _DissolveMask2Radius;					uniform float  _DissolveMask2Radius_Global;
	float _DissolveMask2Height;					uniform float _DissolveMask2Height_Global;

	#if defined(_DISSOLVEMASK_BOX)
		float3 _DissolveMask2BoundsMin;			uniform float3 _DissolveMask2BoundsMin_Global;
		float3 _DissolveMask2BoundsMax;			uniform float3 _DissolveMask2BoundsMax_Global;
		float4x4 _DissolveMask2TRS;				uniform float4x4 _DissolveMask2TRS_Global;
	#endif

#endif


#if defined(_DISSOLVEMASK_BOX)
	float3 _DissolveMaskBoundsMin;				float3 _DissolveMaskBoundsMin_Global;
	float3 _DissolveMaskBoundsMax;				float3 _DissolveMaskBoundsMax_Global;
	float4x4 _DissolveMaskTRS;					float4x4 _DissolveMaskTRS_Global;
#endif


#ifdef _DISSOLVEMAPPINGTYPE_TRIPLANAR	
	float _DissolveTriplanarMappingSpace;		uniform float _DissolveTriplanarMappingSpace_Global;
#endif


float3 _Dissolve_ObjectWorldPos;

const float3 const_zero = float3(0, 0, 0);


#if defined(_DISSOLVEGLOBALCONTROL_MASK_ONLY)
	
	//Globals-----------------------------------------------------------------------
	#define VALUE_CUTOFF					_DissolveCutoff_Global

	#define VALUE_CUTOFF_AXIS				_DissolveMaskAxis_Global
	#define VALUE_MASK_SPACE				_DissolveMaskSpace_Global
	#define VALUE_MASK_OFFSET				_DissolveMaskOffset_Global
	#define VALUE_AXIS_INVERT				_DissolveMaskInvert_Global

	#define VALUE_MASK_POSITION				_DissolveMaskPosition_Global
	#define VALUE_MASK_NORMAL			    _DissolveMaskNormal_Global
	#define VALUE_MASK_RADIUS		        _DissolveMaskRadius_Global
	#define VALUE_MASK_HEIGHT				_DissolveMaskHeight_Global

	#if defined(_DISSOLVEMASKCOUNT_FOUR)
		#define VALUE_MASK_2_POSITION			_DissolveMask2Position_Global
		#define VALUE_MASK_2_NORMAL				_DissolveMask2Normal_Global
		#define VALUE_MASK_2_RADIUS				_DissolveMask2Radius_Global
		#define VALUE_MASK_2_HEIGHT				_DissolveMask2Height_Global

		#define VALUE_MASK_3_POSITION			_DissolveMask3Position_Global
		#define VALUE_MASK_3_NORMAL				_DissolveMask3Normal_Global
		#define VALUE_MASK_3_RADIUS				_DissolveMask3Radius_Global
		#define VALUE_MASK_3_HEIGHT				_DissolveMask3Height_Global

		#define VALUE_MASK_4_POSITION			_DissolveMask4Position_Global
		#define VALUE_MASK_4_NORMAL				_DissolveMask4Normal_Global
		#define VALUE_MASK_4_RADIUS				_DissolveMask4Radius_Global
		#define VALUE_MASK_4_HEIGHT				_DissolveMask4Height_Global

		#if defined(_DISSOLVEMASK_BOX)
			#define VALUE_MASK_2_BOUNDS_MIN		_DissolveMask2BoundsMin_Global
			#define VALUE_MASK_2_BOUNDS_MAX		_DissolveMask2BoundsMax_Global
			#define VALUE_MASK_2_TRS			_DissolveMask2TRS_Global

			#define VALUE_MASK_3_BOUNDS_MIN		_DissolveMask3BoundsMin_Global
			#define VALUE_MASK_3_BOUNDS_MAX		_DissolveMask3BoundsMax_Global
			#define VALUE_MASK_3_TRS			_DissolveMask3TRS_Global

			#define VALUE_MASK_4_BOUNDS_MIN		_DissolveMask4BoundsMin_Global
			#define VALUE_MASK_4_BOUNDS_MAX		_DissolveMask4BoundsMax_Global
			#define VALUE_MASK_4_TRS			_DissolveMask4TRS_Global
		#endif
	#elif defined(_DISSOLVEMASKCOUNT_THREE)
		#define VALUE_MASK_2_POSITION			_DissolveMask2Position_Global
		#define VALUE_MASK_2_NORMAL				_DissolveMask2Normal_Global
		#define VALUE_MASK_2_RADIUS				_DissolveMask2Radius_Global
		#define VALUE_MASK_2_HEIGHT				_DissolveMask2Height_Global

		#define VALUE_MASK_3_POSITION			_DissolveMask3Position_Global
		#define VALUE_MASK_3_NORMAL				_DissolveMask3Normal_Global
		#define VALUE_MASK_3_RADIUS				_DissolveMask3Radius_Global
		#define VALUE_MASK_3_HEIGHT				_DissolveMask3Height_Global

		#if defined(_DISSOLVEMASK_BOX)
			#define VALUE_MASK_2_BOUNDS_MIN		_DissolveMask2BoundsMin_Global
			#define VALUE_MASK_2_BOUNDS_MAX		_DissolveMask2BoundsMax_Global
			#define VALUE_MASK_2_TRS			_DissolveMask2TRS_Global

			#define VALUE_MASK_3_BOUNDS_MIN		_DissolveMask3BoundsMin_Global
			#define VALUE_MASK_3_BOUNDS_MAX		_DissolveMask3BoundsMax_Global
			#define VALUE_MASK_3_TRS			_DissolveMask3TRS_Global
		#endif
	#elif defined(_DISSOLVEMASKCOUNT_TWO)
		#define VALUE_MASK_2_POSITION			_DissolveMask2Position_Global
		#define VALUE_MASK_2_NORMAL				_DissolveMask2Normal_Global
		#define VALUE_MASK_2_RADIUS				_DissolveMask2Radius_Global
		#define VALUE_MASK_2_HEIGHT				_DissolveMask2Height_Global

		#if defined(_DISSOLVEMASK_BOX)
			#define VALUE_MASK_2_BOUNDS_MIN		_DissolveMask2BoundsMin_Global
			#define VALUE_MASK_2_BOUNDS_MAX		_DissolveMask2BoundsMax_Global
			#define VALUE_MASK_2_TRS			_DissolveMask2TRS_Global
		#endif
	#endif


	#if defined(_DISSOLVEMASK_BOX)
		#define VALUE_MASK_BOUNDS_MIN			_DissolveMaskBoundsMin_Global
		#define VALUE_MASK_BOUNDS_MAX			_DissolveMaskBoundsMax_Global
		#define VALUE_MASK_TRS					_DissolveMaskTRS_Global
	#endif

	//Per material---------------------------------------------------------------------
	
	#define VALUE_MAIN_MAP_TILING				_DissolveMainMapTiling

	#if defined(_DISSOLVEALPHASOURCE_CUSTOM_MAP) || defined(_DISSOLVEALPHASOURCE_TWO_CUSTOM_MAPS) || defined(_DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS)
		#define VALUE_MAP1						_DissolveMap1
		#define VALUE_MAP1_SAMPLER				sampler_DissolveMap1
		#define VALUE_MAP1_ST					_DissolveMap1_ST
		#define VALUE_MAP1_SCROLL				_DissolveMap1_Scroll
		#define VALUE_MAP1_INTENSITY			_DissolveMap1Intensity
		#define VALUE_MAP1_CHANNEL				_DissolveMap1Channel
	#endif

	#if defined(_DISSOLVEALPHASOURCE_TWO_CUSTOM_MAPS) || defined(_DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS)
		#define VALUE_MAP2						_DissolveMap2
		#define VALUE_MAP2_SAMPLER				sampler_DissolveMap2
		#define VALUE_MAP2_ST					_DissolveMap2_ST
		#define VALUE_MAP2_SCROLL				_DissolveMap2_Scroll
		#define VALUE_MAP2_INTENSITY			_DissolveMap2Intensity
		#define VALUE_MAP2_CHANNEL				_DissolveMap2Channel
	#endif

	#if defined(_DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS)
		#define VALUE_MAP3						_DissolveMap3
		#define VALUE_MAP3_SAMPLER				sampler_DissolveMap3
		#define VALUE_MAP3_ST					_DissolveMap3_ST
		#define VALUE_MAP3_SCROLL				_DissolveMap3_Scroll
		#define VALUE_MAP3_INTENSITY			_DissolveMap3Intensity
		#define VALUE_MAP3_CHANNEL				_DissolveMap3Channel
	#endif

	#define VALUE_EDGE_SIZE					_DissolveEdgeWidth
	#define VALUE_EDGE_COLOR				_DissolveEdgeColor
	#define VALUE_EDGE_DISTORTION_SOURCE    _DissolveEdgeDistortionSource
	#define VALUE_EDGE_DISTORTION			_DissolveEdgeDistortionStrength
	#define VALUE_EDGE_COLOR_INTENSITY		_DissolveEdgeColorIntensity
	#define VALUE_EDGE_SHAPE				_DissolveEdgeShape
	#define VALUE_GI_MULTIPLIER				_DissolveGIMultiplier


	#if defined(_DISSOLVEEDGETEXTURESOURCE_GRADIENT) || defined(_DISSOLVEEDGETEXTURESOURCE_MAIN_MAP) || defined(_DISSOLVEEDGETEXTURESOURCE_CUSTOM)

		#define VALUE_EDGE_TEXTURE					_DissolveEdgeTexture
		#define VALUE_EDGE_TEXTURE_SAMPLER          sampler_DissolveEdgeTexture
		#define VALUE_EDGE_TEXTURE_REVERSE			_DissolveEdgeTextureReverse
		#define VALUE_EDGE_TEXTURE_OFFSET			_DissolveEdgeTexturePhaseOffset
		#define VALUE_EDGETEXTUREALPHAOFFSET		_DissolveEdgeTextureAlphaOffset
 
		#if defined(_DISSOLVEEDGETEXTURESOURCE_GRADIENT)
			#define VALUE_EDGE_TEXTURE_IS_DYNAMIC	_DissolveEdgeTextureIsDynamic
		#else
			#define VALUE_EDGE_TEXTURE_MIPMAP       _DissolveEdgeTextureMipmap
		#endif
	#endif


	#define VALUE_ALPHATEXTUREBLEND			_DissolveSourceAlphaTexturesBlend
	#define VALUE_UVSET						_DissolveAlphaSourceTexturesUVSet
	#define VALUE_NOISE_STRENGTH			_DissolveNoiseStrength

	#ifdef _DISSOLVEMAPPINGTYPE_TRIPLANAR
		#define VALUE_TRIPLANARMAPPINGSPACE          _DissolveTriplanarMappingSpace
	#endif

	#define VALUE_COMBINE_WITH_MASTER_NODE_ALPHA     _DissolveCombineWithMasterNodeAlpha
	#define VALUE_COMBINE_WITH_MASTER_NODE_COLOR     _DissolveCombineWithMasterNodeColor

#elif defined(_DISSOLVEGLOBALCONTROL_MASK_AND_EDGE)

	//Globals-----------------------------------------------------------------------
	#define VALUE_CUTOFF					_DissolveCutoff_Global

	#define VALUE_CUTOFF_AXIS				_DissolveMaskAxis_Global
	#define VALUE_MASK_SPACE				_DissolveMaskSpace_Global
	#define VALUE_MASK_OFFSET				_DissolveMaskOffset_Global
	#define VALUE_AXIS_INVERT				_DissolveMaskInvert_Global

	#define VALUE_MASK_POSITION				_DissolveMaskPosition_Global
	#define VALUE_MASK_NORMAL				_DissolveMaskNormal_Global
	#define VALUE_MASK_RADIUS				_DissolveMaskRadius_Global
	#define VALUE_MASK_HEIGHT				_DissolveMaskHeight_Global

	#if defined(_DISSOLVEMASKCOUNT_FOUR)
		#define VALUE_MASK_2_POSITION			_DissolveMask2Position_Global
		#define VALUE_MASK_2_NORMAL				_DissolveMask2Normal_Global
		#define VALUE_MASK_2_RADIUS				_DissolveMask2Radius_Global
		#define VALUE_MASK_2_HEIGHT				_DissolveMask2Height_Global

		#define VALUE_MASK_3_POSITION			_DissolveMask3Position_Global
		#define VALUE_MASK_3_NORMAL				_DissolveMask3Normal_Global
		#define VALUE_MASK_3_RADIUS				_DissolveMask3Radius_Global
		#define VALUE_MASK_3_HEIGHT				_DissolveMask3Height_Global

		#define VALUE_MASK_4_POSITION			_DissolveMask4Position_Global
		#define VALUE_MASK_4_NORMAL				_DissolveMask4Normal_Global
		#define VALUE_MASK_4_RADIUS				_DissolveMask4Radius_Global
		#define VALUE_MASK_4_HEIGHT				_DissolveMask4Height_Global

		#if defined(_DISSOLVEMASK_BOX)
			#define VALUE_MASK_2_BOUNDS_MIN		_DissolveMask2BoundsMin_Global
			#define VALUE_MASK_2_BOUNDS_MAX		_DissolveMask2BoundsMax_Global
			#define VALUE_MASK_2_TRS			_DissolveMask2TRS_Global

			#define VALUE_MASK_3_BOUNDS_MIN		_DissolveMask3BoundsMin_Global
			#define VALUE_MASK_3_BOUNDS_MAX		_DissolveMask3BoundsMax_Global
			#define VALUE_MASK_3_TRS			_DissolveMask3TRS_Global

			#define VALUE_MASK_4_BOUNDS_MIN		_DissolveMask4BoundsMin_Global
			#define VALUE_MASK_4_BOUNDS_MAX		_DissolveMask4BoundsMax_Global
			#define VALUE_MASK_4_TRS			_DissolveMask4TRS_Global
		#endif
	#elif defined(_DISSOLVEMASKCOUNT_THREE)
		#define VALUE_MASK_2_POSITION			_DissolveMask2Position_Global
		#define VALUE_MASK_2_NORMAL				_DissolveMask2Normal_Global
		#define VALUE_MASK_2_RADIUS				_DissolveMask2Radius_Global
		#define VALUE_MASK_2_HEIGHT				_DissolveMask2Height_Global
		
		#define VALUE_MASK_3_POSITION			_DissolveMask3Position_Global
		#define VALUE_MASK_3_NORMAL				_DissolveMask3Normal_Global
		#define VALUE_MASK_3_RADIUS				_DissolveMask3Radius_Global
		#define VALUE_MASK_3_HEIGHT				_DissolveMask3Height_Global

		#if defined(_DISSOLVEMASK_BOX)
			#define VALUE_MASK_2_BOUNDS_MIN		_DissolveMask2BoundsMin_Global
			#define VALUE_MASK_2_BOUNDS_MAX		_DissolveMask2BoundsMax_Global
			#define VALUE_MASK_2_TRS			_DissolveMask2TRS_Global

			#define VALUE_MASK_3_BOUNDS_MIN		_DissolveMask3BoundsMin_Global
			#define VALUE_MASK_3_BOUNDS_MAX		_DissolveMask3BoundsMax_Global
			#define VALUE_MASK_3_TRS			_DissolveMask3TRS_Global
		#endif
	#elif defined(_DISSOLVEMASKCOUNT_TWO)
		#define VALUE_MASK_2_POSITION			_DissolveMask2Position_Global
		#define VALUE_MASK_2_NORMAL				_DissolveMask2Normal_Global
		#define VALUE_MASK_2_RADIUS				_DissolveMask2Radius_Global
		#define VALUE_MASK_2_HEIGHT				_DissolveMask2Height_Global

		#if defined(_DISSOLVEMASK_BOX)
			#define VALUE_MASK_2_BOUNDS_MIN			_DissolveMask2BoundsMin_Global
			#define VALUE_MASK_2_BOUNDS_MAX		_DissolveMask2BoundsMax_Global
			#define VALUE_MASK_2_TRS			_DissolveMask2TRS_Global
		#endif
	#endif


	#if defined(_DISSOLVEMASK_BOX)
		#define VALUE_MASK_BOUNDS_MIN		_DissolveMaskBoundsMin_Global
		#define VALUE_MASK_BOUNDS_MAX		_DissolveMaskBoundsMax_Global
		#define VALUE_MASK_TRS				_DissolveMaskTRS_Global
	#endif

	#define VALUE_EDGE_SIZE					_DissolveEdgeWidth_Global
	#define VALUE_EDGE_SHAPE				_DissolveEdgeShape_Global
	#define VALUE_EDGE_COLOR				_DissolveEdgeColor_Global
	#define VALUE_EDGE_COLOR_INTENSITY		_DissolveEdgeColorIntensity_Global
	#define VALUE_GI_MULTIPLIER				_DissolveGIMultiplier

	#if defined(_DISSOLVEEDGETEXTURESOURCE_GRADIENT) || defined(_DISSOLVEEDGETEXTURESOURCE_MAIN_MAP) || defined(_DISSOLVEEDGETEXTURESOURCE_CUSTOM)

		#define VALUE_EDGE_TEXTURE					_DissolveEdgeTexture_Global
		#define VALUE_EDGE_TEXTURE_SAMPLER          sampler_DissolveEdgeTexture_Global
		#define VALUE_EDGE_TEXTURE_REVERSE			_DissolveEdgeTextureReverse_Global
		#define VALUE_EDGE_TEXTURE_OFFSET			_DissolveEdgeTexturePhaseOffset_Global
		#define VALUE_EDGETEXTUREALPHAOFFSET		_DissolveEdgeTextureAlphaOffset_Global
 
		#if defined(_DISSOLVEEDGETEXTURESOURCE_GRADIENT)
			#define VALUE_EDGE_TEXTURE_IS_DYNAMIC	_DissolveEdgeTextureIsDynamic_Global
		#else
			#define VALUE_EDGE_TEXTURE_MIPMAP       _DissolveEdgeTextureMipmap_Global
		#endif
	#endif

	//Per material---------------------------------------------------------------------	
	
	#define VALUE_MAIN_MAP_TILING				_DissolveMainMapTiling

	#if defined(_DISSOLVEALPHASOURCE_CUSTOM_MAP) || defined(_DISSOLVEALPHASOURCE_TWO_CUSTOM_MAPS) || defined(_DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS)
		#define VALUE_MAP1						_DissolveMap1
		#define VALUE_MAP1_SAMPLER				sampler_DissolveMap1
		#define VALUE_MAP1_ST					_DissolveMap1_ST
		#define VALUE_MAP1_SCROLL				_DissolveMap1_Scroll
		#define VALUE_MAP1_INTENSITY			_DissolveMap1Intensity
		#define VALUE_MAP1_CHANNEL				_DissolveMap1Channel
	#endif

	#if defined(_DISSOLVEALPHASOURCE_TWO_CUSTOM_MAPS) || defined(_DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS)
		#define VALUE_MAP2						_DissolveMap2
		#define VALUE_MAP2_SAMPLER				sampler_DissolveMap2
		#define VALUE_MAP2_ST					_DissolveMap2_ST
		#define VALUE_MAP2_SCROLL				_DissolveMap2_Scroll
		#define VALUE_MAP2_INTENSITY			_DissolveMap2Intensity
		#define VALUE_MAP2_CHANNEL				_DissolveMap2Channel
	#endif

	#if defined(_DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS)
		#define VALUE_MAP3						_DissolveMap3
		#define VALUE_MAP3_SAMPLER				sampler_DissolveMap3
		#define VALUE_MAP3_ST					_DissolveMap3_ST
		#define VALUE_MAP3_SCROLL				_DissolveMap3_Scroll
		#define VALUE_MAP3_INTENSITY			_DissolveMap3Intensity
		#define VALUE_MAP3_CHANNEL				_DissolveMap3Channel
	#endif


	#define VALUE_EDGE_DISTORTION_SOURCE   _DissolveEdgeDistortionSource
	#define VALUE_EDGE_DISTORTION			_DissolveEdgeDistortionStrength	


	#define VALUE_ALPHATEXTUREBLEND			_DissolveSourceAlphaTexturesBlend
	#define VALUE_UVSET						_DissolveAlphaSourceTexturesUVSet
	#define VALUE_NOISE_STRENGTH			_DissolveNoiseStrength

	#ifdef _DISSOLVEMAPPINGTYPE_TRIPLANAR
		#define VALUE_TRIPLANARMAPPINGSPACE          _DissolveTriplanarMappingSpace
	#endif

	#define VALUE_COMBINE_WITH_MASTER_NODE_ALPHA     _DissolveCombineWithMasterNodeAlpha
	#define VALUE_COMBINE_WITH_MASTER_NODE_COLOR     _DissolveCombineWithMasterNodeColor

#elif defined(_DISSOLVEGLOBALCONTROL_ALL)

	#define VALUE_CUTOFF					_DissolveCutoff_Global

	#define VALUE_CUTOFF_AXIS				_DissolveMaskAxis_Global
	#define VALUE_MASK_SPACE				_DissolveMaskSpace_Global
	#define VALUE_MASK_OFFSET				_DissolveMaskOffset_Global
	#define VALUE_AXIS_INVERT				_DissolveMaskInvert_Global

	#define VALUE_EDGE_SIZE					_DissolveEdgeWidth_Global
	#define VALUE_EDGE_COLOR				_DissolveEdgeColor_Global
	#define VALUE_EDGE_DISTORTION_SOURCE   _DissolveEdgeDistortionSource_Global
	#define VALUE_EDGE_DISTORTION			_DissolveEdgeDistortionStrength_Global
	#define VALUE_EDGE_COLOR_INTENSITY		_DissolveEdgeColorIntensity_Global
	#define VALUE_EDGE_SHAPE				_DissolveEdgeShape_Global
	#define VALUE_GI_MULTIPLIER				_DissolveGIMultiplier_Global


	#if defined(_DISSOLVEEDGETEXTURESOURCE_GRADIENT) || defined(_DISSOLVEEDGETEXTURESOURCE_MAIN_MAP) || defined(_DISSOLVEEDGETEXTURESOURCE_CUSTOM)

		#define VALUE_EDGE_TEXTURE					_DissolveEdgeTexture_Global
		#define VALUE_EDGE_TEXTURE_SAMPLER          sampler_DissolveEdgeTexture_Global
		#define VALUE_EDGE_TEXTURE_REVERSE			_DissolveEdgeTextureReverse_Global
		#define VALUE_EDGE_TEXTURE_OFFSET			_DissolveEdgeTexturePhaseOffset_Global
		#define VALUE_EDGETEXTUREALPHAOFFSET		_DissolveEdgeTextureAlphaOffset_Global
 
		#if defined(_DISSOLVEEDGETEXTURESOURCE_GRADIENT)
			#define VALUE_EDGE_TEXTURE_IS_DYNAMIC	_DissolveEdgeTextureIsDynamic_Global
		#else
			#define VALUE_EDGE_TEXTURE_MIPMAP       _DissolveEdgeTextureMipmap_Global
		#endif
	#endif


	#define VALUE_MAIN_MAP_TILING				_DissolveMainMapTiling_Global

	#if defined(_DISSOLVEALPHASOURCE_CUSTOM_MAP) || defined(_DISSOLVEALPHASOURCE_TWO_CUSTOM_MAPS) || defined(_DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS)
		#define VALUE_MAP1						_DissolveMap1_Global
		#define VALUE_MAP1_SAMPLER				sampler_DissolveMap1_Global
		#define VALUE_MAP1_ST					_DissolveMap1_ST_Global
		#define VALUE_MAP1_SCROLL				_DissolveMap1_Scroll_Global
		#define VALUE_MAP1_INTENSITY			_DissolveMap1Intensity_Global
		#define VALUE_MAP1_CHANNEL				_DissolveMap1Channel_Global
	#endif

	#if defined(_DISSOLVEALPHASOURCE_TWO_CUSTOM_MAPS) || defined(_DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS)
		#define VALUE_MAP2						_DissolveMap2_Global
		#define VALUE_MAP2_SAMPLER				sampler_DissolveMap2_Global
		#define VALUE_MAP2_ST					_DissolveMap2_ST_Global
		#define VALUE_MAP2_SCROLL				_DissolveMap2_Scroll_Global
		#define VALUE_MAP2_INTENSITY			_DissolveMap2Intensity_Global
		#define VALUE_MAP2_CHANNEL				_DissolveMap2Channel_Global
	#endif

	#if defined(_DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS)
		#define VALUE_MAP3						_DissolveMap3_Global
		#define VALUE_MAP3_SAMPLER				sampler_DissolveMap3_Global
		#define VALUE_MAP3_ST					_DissolveMap3_ST_Global
		#define VALUE_MAP3_SCROLL				_DissolveMap3_Scroll_Global
		#define VALUE_MAP3_INTENSITY			_DissolveMap3Intensity_Global
		#define VALUE_MAP3_CHANNEL				_DissolveMap3Channel_Global
	#endif
	
	#define VALUE_ALPHATEXTUREBLEND			_DissolveSourceAlphaTexturesBlend_Global
	#define VALUE_UVSET						_DissolveAlphaSourceTexturesUVSet_Global
	#define VALUE_NOISE_STRENGTH			_DissolveNoiseStrength_Global

	#define VALUE_MASK_POSITION				_DissolveMaskPosition_Global
	#define VALUE_MASK_NORMAL				_DissolveMaskNormal_Global
	#define VALUE_MASK_RADIUS				_DissolveMaskRadius_Global
	#define VALUE_MASK_HEIGHT				_DissolveMaskHeight_Global

	#if defined(_DISSOLVEMASKCOUNT_FOUR)
		#define VALUE_MASK_2_POSITION			_DissolveMask2Position_Global
		#define VALUE_MASK_2_NORMAL				_DissolveMask2Normal_Global
		#define VALUE_MASK_2_RADIUS				_DissolveMask2Radius_Global
		#define VALUE_MASK_2_HEIGHT				_DissolveMask2Height_Global

		#define VALUE_MASK_3_POSITION			_DissolveMask3Position_Global
		#define VALUE_MASK_3_NORMAL				_DissolveMask3Normal_Global
		#define VALUE_MASK_3_RADIUS				_DissolveMask3Radius_Global
		#define VALUE_MASK_3_HEIGHT				_DissolveMask3Height_Global

		#define VALUE_MASK_4_POSITION			_DissolveMask4Position_Global
		#define VALUE_MASK_4_NORMAL				_DissolveMask4Normal_Global
		#define VALUE_MASK_4_RADIUS				_DissolveMask4Radius_Global
		#define VALUE_MASK_4_HEIGHT				_DissolveMask4Height_Global

		#if defined(_DISSOLVEMASK_BOX)
			#define VALUE_MASK_2_BOUNDS_MIN		_DissolveMask2BoundsMin_Global
			#define VALUE_MASK_2_BOUNDS_MAX		_DissolveMask2BoundsMax_Global
			#define VALUE_MASK_2_TRS			_DissolveMask2TRS_Global

			#define VALUE_MASK_3_BOUNDS_MIN		_DissolveMask3BoundsMin_Global
			#define VALUE_MASK_3_BOUNDS_MAX		_DissolveMask3BoundsMax_Global
			#define VALUE_MASK_3_TRS			_DissolveMask3TRS_Global

			#define VALUE_MASK_4_BOUNDS_MIN		_DissolveMask4BoundsMin_Global
			#define VALUE_MASK_4_BOUNDS_MAX		_DissolveMask4BoundsMax_Global
			#define VALUE_MASK_4_TRS			_DissolveMask4TRS_Global
		#endif
	#elif defined(_DISSOLVEMASKCOUNT_THREE)
		#define VALUE_MASK_2_POSITION			_DissolveMask2Position_Global
		#define VALUE_MASK_2_NORMAL				_DissolveMask2Normal_Global
		#define VALUE_MASK_2_RADIUS				_DissolveMask2Radius_Global
		#define VALUE_MASK_2_HEIGHT				_DissolveMask2Height_Global

		#define VALUE_MASK_3_POSITION			_DissolveMask3Position_Global
		#define VALUE_MASK_3_NORMAL				_DissolveMask3Normal_Global
		#define VALUE_MASK_3_RADIUS				_DissolveMask3Radius_Global
		#define VALUE_MASK_3_HEIGHT				_DissolveMask3Height_Global

		#if defined(_DISSOLVEMASK_BOX)
			#define VALUE_MASK_2_BOUNDS_MIN		_DissolveMask2BoundsMin_Global
			#define VALUE_MASK_2_BOUNDS_MAX		_DissolveMask2BoundsMax_Global
			#define VALUE_MASK_2_TRS			_DissolveMask2TRS_Global

			#define VALUE_MASK_3_BOUNDS_MIN		_DissolveMask3BoundsMin_Global
			#define VALUE_MASK_3_BOUNDS_MAX		_DissolveMask3BoundsMax_Global
			#define VALUE_MASK_3_TRS			_DissolveMask3TRS_Global
		#endif
	#elif defined(_DISSOLVEMASKCOUNT_TWO)
		#define VALUE_MASK_2_POSITION			_DissolveMask2Position_Global
		#define VALUE_MASK_2_NORMAL				_DissolveMask2Normal_Global
		#define VALUE_MASK_2_RADIUS				_DissolveMask2Radius_Global
		#define VALUE_MASK_2_HEIGHT				_DissolveMask2Height_Global

		#if defined(_DISSOLVEMASK_BOX)
			#define VALUE_MASK_2_BOUNDS_MIN		_DissolveMask2BoundsMin_Global
			#define VALUE_MASK_2_BOUNDS_MAX		_DissolveMask2BoundsMax_Global
			#define VALUE_MASK_2_TRS			_DissolveMask2TRS_Global
		#endif
	#endif


	#if defined(_DISSOLVEMASK_BOX)
		#define VALUE_MASK_BOUNDS_MIN		_DissolveMaskBoundsMin_Global
		#define VALUE_MASK_BOUNDS_MAX		_DissolveMaskBoundsMax_Global
		#define VALUE_MASK_TRS				_DissolveMaskTRS_Global
	#endif


	#ifdef _DISSOLVEMAPPINGTYPE_TRIPLANAR
		#define VALUE_TRIPLANARMAPPINGSPACE          _DissolveTriplanarMappingSpace_Global
	#endif

	#define VALUE_COMBINE_WITH_MASTER_NODE_ALPHA     _DissolveCombineWithMasterNodeAlpha_Global
	#define VALUE_COMBINE_WITH_MASTER_NODE_COLOR     _DissolveCombineWithMasterNodeColor_Global
	
#else

	#define VALUE_CUTOFF					_DissolveCutoff

	#define VALUE_CUTOFF_AXIS				_DissolveMaskAxis
	#define VALUE_MASK_SPACE				_DissolveMaskSpace
	#define VALUE_MASK_OFFSET				_DissolveMaskOffset	
	#define VALUE_AXIS_INVERT				_DissolveMaskInvert

	#define VALUE_EDGE_SIZE					_DissolveEdgeWidth
	#define VALUE_EDGE_COLOR				_DissolveEdgeColor
	#define VALUE_EDGE_DISTORTION_SOURCE   _DissolveEdgeDistortionSource
	#define VALUE_EDGE_DISTORTION			_DissolveEdgeDistortionStrength
	#define VALUE_EDGE_COLOR_INTENSITY		_DissolveEdgeColorIntensity
	#define VALUE_EDGE_SHAPE				_DissolveEdgeShape
	#define VALUE_GI_MULTIPLIER				_DissolveGIMultiplier


	#if defined(_DISSOLVEEDGETEXTURESOURCE_GRADIENT) || defined(_DISSOLVEEDGETEXTURESOURCE_MAIN_MAP) || defined(_DISSOLVEEDGETEXTURESOURCE_CUSTOM)

		#define VALUE_EDGE_TEXTURE					_DissolveEdgeTexture
		#define VALUE_EDGE_TEXTURE_SAMPLER          sampler_DissolveEdgeTexture
		#define VALUE_EDGE_TEXTURE_REVERSE			_DissolveEdgeTextureReverse
		#define VALUE_EDGE_TEXTURE_OFFSET			_DissolveEdgeTexturePhaseOffset
		#define VALUE_EDGETEXTUREALPHAOFFSET		_DissolveEdgeTextureAlphaOffset
 
		#if defined(_DISSOLVEEDGETEXTURESOURCE_GRADIENT)
			#define VALUE_EDGE_TEXTURE_IS_DYNAMIC	_DissolveEdgeTextureIsDynamic
		#else
			#define VALUE_EDGE_TEXTURE_MIPMAP       _DissolveEdgeTextureMipmap
		#endif
	#endif


	#define VALUE_MAIN_MAP_TILING				_DissolveMainMapTiling

	#if defined(_DISSOLVEALPHASOURCE_CUSTOM_MAP) || defined(_DISSOLVEALPHASOURCE_TWO_CUSTOM_MAPS) || defined(_DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS)
		#define VALUE_MAP1						_DissolveMap1
		#define VALUE_MAP1_SAMPLER				sampler_DissolveMap1
		#define VALUE_MAP1_ST					_DissolveMap1_ST
		#define VALUE_MAP1_SCROLL				_DissolveMap1_Scroll
		#define VALUE_MAP1_INTENSITY			_DissolveMap1Intensity
		#define VALUE_MAP1_CHANNEL				_DissolveMap1Channel
	#endif

	#if defined(_DISSOLVEALPHASOURCE_TWO_CUSTOM_MAPS) || defined(_DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS)
		#define VALUE_MAP2						_DissolveMap2
		#define VALUE_MAP2_SAMPLER				sampler_DissolveMap2
		#define VALUE_MAP2_ST					_DissolveMap2_ST
		#define VALUE_MAP2_SCROLL				_DissolveMap2_Scroll
		#define VALUE_MAP2_INTENSITY			_DissolveMap2Intensity
		#define VALUE_MAP2_CHANNEL				_DissolveMap2Channel
	#endif

	#if defined(_DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS)
		#define VALUE_MAP3						_DissolveMap3
		#define VALUE_MAP3_SAMPLER				sampler_DissolveMap3
		#define VALUE_MAP3_ST					_DissolveMap3_ST
		#define VALUE_MAP3_SCROLL				_DissolveMap3_Scroll
		#define VALUE_MAP3_INTENSITY			_DissolveMap3Intensity
		#define VALUE_MAP3_CHANNEL				_DissolveMap3Channel
	#endif

	#define VALUE_ALPHATEXTUREBLEND			_DissolveSourceAlphaTexturesBlend
	#define VALUE_UVSET						_DissolveAlphaSourceTexturesUVSet
	#define VALUE_NOISE_STRENGTH			_DissolveNoiseStrength

	#define VALUE_MASK_POSITION				_DissolveMaskPosition
	#define VALUE_MASK_NORMAL				_DissolveMaskNormal
	#define VALUE_MASK_RADIUS				_DissolveMaskRadius
	#define VALUE_MASK_HEIGHT				_DissolveMaskHeight
	
	#if defined(_DISSOLVEMASKCOUNT_FOUR)
		#define VALUE_MASK_2_POSITION			_DissolveMask2Position
		#define VALUE_MASK_2_NORMAL				_DissolveMask2Normal
		#define VALUE_MASK_2_RADIUS				_DissolveMask2Radius
		#define VALUE_MASK_2_HEIGHT				_DissolveMask2Height

		#define VALUE_MASK_3_POSITION			_DissolveMask3Position
		#define VALUE_MASK_3_NORMAL				_DissolveMask3Normal
		#define VALUE_MASK_3_RADIUS				_DissolveMask3Radius
		#define VALUE_MASK_3_HEIGHT				_DissolveMask3Height

		#define VALUE_MASK_4_POSITION			_DissolveMask4Position
		#define VALUE_MASK_4_NORMAL				_DissolveMask4Normal
		#define VALUE_MASK_4_RADIUS				_DissolveMask4Radius
		#define VALUE_MASK_4_HEIGHT				_DissolveMask4Height

		#if defined(_DISSOLVEMASK_BOX)
			#define VALUE_MASK_2_BOUNDS_MIN		_DissolveMask2BoundsMin
			#define VALUE_MASK_2_BOUNDS_MAX		_DissolveMask2BoundsMax
			#define VALUE_MASK_2_TRS			_DissolveMask2TRS

			#define VALUE_MASK_3_BOUNDS_MIN		_DissolveMask3BoundsMin
			#define VALUE_MASK_3_BOUNDS_MAX		_DissolveMask3BoundsMax
			#define VALUE_MASK_3_TRS			_DissolveMask3TRS

			#define VALUE_MASK_4_BOUNDS_MIN		_DissolveMask4BoundsMin
			#define VALUE_MASK_4_BOUNDS_MAX		_DissolveMask4BoundsMax
			#define VALUE_MASK_4_TRS			_DissolveMask4TRS
		#endif
	#elif defined(_DISSOLVEMASKCOUNT_THREE)
		#define VALUE_MASK_2_POSITION			_DissolveMask2Position
		#define VALUE_MASK_2_NORMAL				_DissolveMask2Normal
		#define VALUE_MASK_2_RADIUS				_DissolveMask2Radius
		#define VALUE_MASK_2_HEIGHT				_DissolveMask2Height

		#define VALUE_MASK_3_POSITION			_DissolveMask3Position
		#define VALUE_MASK_3_NORMAL				_DissolveMask3Normal
		#define VALUE_MASK_3_RADIUS				_DissolveMask3Radius
		#define VALUE_MASK_3_HEIGHT				_DissolveMask3Height

		#if defined(_DISSOLVEMASK_BOX)
			#define VALUE_MASK_2_BOUNDS_MIN		_DissolveMask2BoundsMin
			#define VALUE_MASK_2_BOUNDS_MAX		_DissolveMask2BoundsMax
			#define VALUE_MASK_2_TRS			_DissolveMask2TRS

			#define VALUE_MASK_3_BOUNDS_MIN		_DissolveMask3BoundsMin
			#define VALUE_MASK_3_BOUNDS_MAX		_DissolveMask3BoundsMax
			#define VALUE_MASK_3_TRS			_DissolveMask3TRS
		#endif
	#elif defined(_DISSOLVEMASKCOUNT_TWO)
		#define VALUE_MASK_2_POSITION			_DissolveMask2Position
		#define VALUE_MASK_2_NORMAL				_DissolveMask2Normal
		#define VALUE_MASK_2_RADIUS				_DissolveMask2Radius
		#define VALUE_MASK_2_HEIGHT				_DissolveMask2Height

		#if defined(_DISSOLVEMASK_BOX)
			#define VALUE_MASK_2_BOUNDS_MIN		_DissolveMask2BoundsMin
			#define VALUE_MASK_2_BOUNDS_MAX		_DissolveMask2BoundsMax
			#define VALUE_MASK_2_TRS			_DissolveMask2TRS
		#endif
	#endif


	#if defined(_DISSOLVEMASK_BOX)
		#define VALUE_MASK_BOUNDS_MIN		_DissolveMaskBoundsMin
		#define VALUE_MASK_BOUNDS_MAX		_DissolveMaskBoundsMax
		#define VALUE_MASK_TRS				_DissolveMaskTRS
	#endif

	#ifdef _DISSOLVEMAPPINGTYPE_TRIPLANAR
		#define VALUE_TRIPLANARMAPPINGSPACE          _DissolveTriplanarMappingSpace
	#endif

	#define VALUE_COMBINE_WITH_MASTER_NODE_ALPHA     _DissolveCombineWithMasterNodeAlpha
	#define VALUE_COMBINE_WITH_MASTER_NODE_COLOR     _DissolveCombineWithMasterNodeColor

#endif