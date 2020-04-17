#ifndef ADVANCED_DISSOLVE_STANDARD_INCLUDED
#define ADVANCED_DISSOLVE_STANDARD_INCLUDED


//For correct cutout shadows
#if defined(_ALPHATEST_ON) || defined(_ALPHABLEND_ON) || defined(_ALPHAPREMULTIPLY_ON)
	//Do nothing
#else
	#define _ALPHATEST_ON
#endif


#ifndef UNITY_REQUIRE_FRAG_WORLDPOS
#define UNITY_REQUIRE_FRAG_WORLDPOS
#endif

#if (SHADER_TARGET <= 30)

	#if defined(_DISSOLVEMAPPINGTYPE_TRIPLANAR)
		#undef _DISSOLVEMAPPINGTYPE_TRIPLANAR
	#endif

	#if defined(_DETAIL_MULX2)
		#undef _DETAIL_MULX2
	#endif	

	#if defined(_DISSOLVEALPHASOURCE_TWO_CUSTOM_MAPS) || defined(_DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS)
		#if defined(_PARALLAXMAP)
			#undef _PARALLAXMAP
		#endif
	#endif 

#endif

#if (SHADER_TARGET <= 40)
	#if defined(_DISSOLVEMAPPINGTYPE_TRIPLANAR) && defined(_DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS)
		#if defined(_PARALLAXMAP)
			#undef _PARALLAXMAP
		#endif
	#endif 
#endif



#include "../cginc/AdvancedDissolve.cginc"


#endif // STANDARD_DISSOLVE_PRO_INCLUDED
