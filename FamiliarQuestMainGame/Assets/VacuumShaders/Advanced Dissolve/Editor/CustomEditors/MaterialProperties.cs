using System;
using System.Linq;

using UnityEngine;
using UnityEditor;

using VacuumShaders;

namespace VacuumShaders.AdvancedDissolve
{
    static public class MaterialProperties
    {
        static string prefName_AdvancedDissolve = "AD_EditorPrefName_Advancedissolve";
        static public string prefName_SurfaceInputs = "AD_EditorPrefName_SurfaceInputs";
        static public string prefName_SurfaceOptions = "AD_EditorPrefName_SurfaceOptions";
        static string prefName_Mask = "AD_EditorPrefName_Mask";
        static string prefName_AlphaSource = "AD_EditorPrefName_AlphaSource";
        static string prefName_Edge = "AD_EditorPrefName_Edge";
        static string prefName_MainUVDistortion = "AD_EditorPrefName_MainUVDistortion";
        static string prefName_Global = "AD_EditorPrefName_Global";

        public enum BlendMode
        {
            Opaque,
            Cutout,
            Fade   // Old school alpha-blending mode, fresnel does not affect amount of transparency
        }


        static MaterialProperty _DissolveCutoff;

        static MaterialProperty _DissolveMask;
        static MaterialProperty _DissolveMaskSpace;
        static MaterialProperty _DissolveMaskCount;
        static MaterialProperty _DissolveMaskAxis;
        static MaterialProperty _DissolveMaskInvert;
        static MaterialProperty _DissolveMaskOffset;
        static MaterialProperty _DissolveEdgeWidth;
        static MaterialProperty _DissolveEdgeColor;
        static MaterialProperty _DissolveEdgeColorIntensity;
        static MaterialProperty _DissolveEdgeTextureReverse;
        static MaterialProperty _DissolveEdgeTexturePhaseOffset;
        static MaterialProperty _DissolveEdgeTextureAlphaOffset;
        static MaterialProperty _DissolveEdgeShape;
        static MaterialProperty _DissolveEdgeDistortionSource;
        static MaterialProperty _DissolveEdgeDistortionStrength;
        static MaterialProperty _DissolveGIMultiplier;
        static MaterialProperty _DissolveEdgeTextureIsDynamic;
        static MaterialProperty _DissolveEdgeTextureSource;
        static MaterialProperty _DissolveEdgeTexture;
        static MaterialProperty _DissolveEdgeTextureMipmap;
        static MaterialProperty _DissolveAlphaSource;
        static MaterialProperty _DissolveMap1;
        static MaterialProperty _DissolveMap1_Scroll;
        static MaterialProperty _DissolveMap1Intensity;
        static MaterialProperty _DissolveMap1Channel;
        static MaterialProperty _DissolveMap2;
        static MaterialProperty _DissolveMap2_Scroll;
        static MaterialProperty _DissolveMap2Intensity;
        static MaterialProperty _DissolveMap2Channel;
        static MaterialProperty _DissolveMap3;
        static MaterialProperty _DissolveMap3_Scroll;
        static MaterialProperty _DissolveMap3Intensity;
        static MaterialProperty _DissolveMap3Channel;
        static MaterialProperty _DissolveNoiseStrength;
        static MaterialProperty _DissolveSourceAlphaTexturesBlend;
        static MaterialProperty _DissolveAlphaSourceTexturesUVSet;

        static MaterialProperty _DissolveMappingType;
        static MaterialProperty _DissolveMainMapTiling;
        static MaterialProperty _DissolveTriplanarMappingSpace;

        static MaterialProperty _DissolveGlobalControl;

        static MaterialProperty _Mode;
        static MaterialProperty _Cull;

        

        static GUIStyle guiStyleTitle;
        static GUIStyle guiStyleFoldout;
        static public bool foldoutAdvancedDissolve;
        static public bool foldoutSurfaceOptions;
        static public bool foldoutSurfaceInputs;
        static public bool foldoutMask;
        static public bool foldoutAlphaSource;
        static public bool foldoutEdge;
        static public bool foldoutMainUVDistortion;
        static public bool foldoutGlobal;

        static public void Init(MaterialEditor _editor, MaterialProperty[] props)
        {
            _DissolveCutoff = FindProperty("_DissolveCutoff", props);


            _DissolveMask = FindProperty("_DissolveMask", props);
            _DissolveMaskSpace = FindProperty("_DissolveMaskSpace", props);
            _DissolveMaskCount = FindProperty("_DissolveMaskCount", props);
            _DissolveMaskAxis = FindProperty("_DissolveMaskAxis", props);
            _DissolveMaskInvert = FindProperty("_DissolveMaskInvert", props);
            _DissolveMaskOffset = FindProperty("_DissolveMaskOffset", props);
            _DissolveEdgeWidth = FindProperty("_DissolveEdgeWidth", props);
            _DissolveEdgeColor = FindProperty("_DissolveEdgeColor", props);
            _DissolveEdgeColorIntensity = FindProperty("_DissolveEdgeColorIntensity", props);
            _DissolveEdgeTextureReverse = FindProperty("_DissolveEdgeTextureReverse", props);
            _DissolveEdgeTexturePhaseOffset = FindProperty("_DissolveEdgeTexturePhaseOffset", props);
            _DissolveEdgeTextureAlphaOffset = FindProperty("_DissolveEdgeTextureAlphaOffset", props);
            _DissolveEdgeShape = FindProperty("_DissolveEdgeShape", props);
            _DissolveEdgeDistortionSource = FindProperty("_DissolveEdgeDistortionSource", props);
            _DissolveEdgeDistortionStrength = FindProperty("_DissolveEdgeDistortionStrength", props);
            _DissolveGIMultiplier = FindProperty("_DissolveGIMultiplier", props);
            _DissolveEdgeTextureIsDynamic = FindProperty("_DissolveEdgeTextureIsDynamic", props);
            _DissolveEdgeTextureSource = FindProperty("_DissolveEdgeTextureSource", props);
            _DissolveEdgeTexture = FindProperty("_DissolveEdgeTexture", props);
            _DissolveEdgeTextureMipmap = FindProperty("_DissolveEdgeTextureMipmap", props);
            _DissolveAlphaSource = FindProperty("_DissolveAlphaSource", props);
            _DissolveMap1 = FindProperty("_DissolveMap1", props);
            _DissolveMap1_Scroll = FindProperty("_DissolveMap1_Scroll", props);
            _DissolveMap1Intensity = FindProperty("_DissolveMap1Intensity", props);
            _DissolveMap1Channel = FindProperty("_DissolveMap1Channel", props);
            _DissolveMap2 = FindProperty("_DissolveMap2", props);
            _DissolveMap2_Scroll = FindProperty("_DissolveMap2_Scroll", props);
            _DissolveMap2Intensity = FindProperty("_DissolveMap2Intensity", props);
            _DissolveMap2Channel = FindProperty("_DissolveMap2Channel", props);
            _DissolveMap3 = FindProperty("_DissolveMap3", props);
            _DissolveMap3_Scroll = FindProperty("_DissolveMap3_Scroll", props);
            _DissolveMap3Intensity = FindProperty("_DissolveMap3Intensity", props);
            _DissolveMap3Channel = FindProperty("_DissolveMap3Channel", props);
            _DissolveNoiseStrength = FindProperty("_DissolveNoiseStrength", props);
            _DissolveSourceAlphaTexturesBlend = FindProperty("_DissolveSourceAlphaTexturesBlend", props);
            _DissolveAlphaSourceTexturesUVSet = FindProperty("_DissolveAlphaSourceTexturesUVSet", props);


            _DissolveMappingType = FindProperty("_DissolveMappingType", props);
            _DissolveMainMapTiling = FindProperty("_DissolveMainMapTiling", props);
            _DissolveTriplanarMappingSpace = FindProperty("_DissolveTriplanarMappingSpace", props);

            _DissolveGlobalControl = FindProperty("_DissolveGlobalControl", props);


            _Mode = FindProperty("_Mode", props, false);
            _Cull = FindProperty("_Cull", props, false);


            if (guiStyleTitle == null)
            {
                //guiStyle = (GUIStyle)"ProgressBarBack";
                guiStyleTitle = (GUIStyle)"ProjectBrowserBottomBarBg";

            }
            if (guiStyleFoldout == null)
            {
                guiStyleFoldout = EditorStyles.foldout;
                guiStyleFoldout.fontStyle = FontStyle.Bold;
            }


            foldoutAdvancedDissolve = EditorPrefs.GetBool(prefName_AdvancedDissolve, true);
            foldoutSurfaceOptions = EditorPrefs.GetBool(prefName_SurfaceOptions, true);
            foldoutSurfaceInputs = EditorPrefs.GetBool(prefName_SurfaceInputs, true);
            foldoutMask = EditorPrefs.GetBool(prefName_Mask, true);
            foldoutAlphaSource = EditorPrefs.GetBool(prefName_AlphaSource, true);
            foldoutEdge = EditorPrefs.GetBool(prefName_Edge, true);
            foldoutMainUVDistortion = EditorPrefs.GetBool(prefName_MainUVDistortion, true);
            foldoutGlobal = EditorPrefs.GetBool(prefName_Global, true);
        }

        static MaterialProperty FindProperty(string propertyName, MaterialProperty[] properties, bool mandatory = true)
        {
            for (int index = 0; index < properties.Length; ++index)
            {
                if (properties[index] != null && properties[index].name == propertyName)
                    return properties[index];
            }

            if (mandatory)
                throw new System.ArgumentException("Could not find MaterialProperty: '" + propertyName + "', Num properties: " + (object)properties.Length);
            else
                return null;
        }

        static public void DrawDissolveOptions(MaterialEditor m_MaterialEditor, bool drawEmission)
        {
            EditorGUIUtility.labelWidth = 0f;

            Material material = m_MaterialEditor.target as Material;
            

            float defaultFieldWidth = EditorGUIUtility.fieldWidth;
            float defaultLabelWidth = EditorGUIUtility.labelWidth;


            //Unity Standard shader has some limitations
            bool isStandard = material.shader.name.Contains("Standard");
            bool isSM4 = material.shader.name.Contains("SM4");


            bool globalControll_Mask = material.shaderKeywords.Contains("_DISSOLVEGLOBALCONTROL_MASK_ONLY");
            bool globalControll_MaskAndEdge = material.shaderKeywords.Contains("_DISSOLVEGLOBALCONTROL_MASK_AND_EDGE");
            bool globalControll_All = material.shaderKeywords.Contains("_DISSOLVEGLOBALCONTROL_ALL");




            //Anchor
            GUILayout.Space(1);
            EditorGUI.BeginChangeCheck();
            using (new VacuumEditorGUIUtility.GUIBackgroundColor(Color.gray))
            {
                foldoutAdvancedDissolve = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutAdvancedDissolve, "Advanced Dissolve");
            }
            if (EditorGUI.EndChangeCheck())
                EditorPrefs.SetBool(prefName_AdvancedDissolve, foldoutAdvancedDissolve);


            if (foldoutAdvancedDissolve)
            {
                using (new VacuumEditorGUIUtility.EditorGUIIndentLevel(1))
                {

                    EditorGUI.BeginChangeCheck();
                    foldoutMask = EditorGUILayout.Foldout(foldoutMask, " Mask", guiStyleFoldout);
                    if (EditorGUI.EndChangeCheck())
                        EditorPrefs.SetBool(prefName_Mask, foldoutMask);

                    if (foldoutMask)
                    {
                        using (new VacuumEditorGUIUtility.EditorGUIIndentLevel(1))
                        {
                            m_MaterialEditor.ShaderProperty(_DissolveMask, "Type");


                            using (new VacuumEditorGUIUtility.GUIEnabled(!(globalControll_Mask || globalControll_MaskAndEdge || globalControll_All)))
                            {
                                //None
                                if (_DissolveMask.floatValue < 0.5f)
                                {
                                    m_MaterialEditor.ShaderProperty(_DissolveCutoff, "Dissolve");
                                }
                                //XYZ Axis
                                else if (_DissolveMask.floatValue > 0.5f && _DissolveMask.floatValue < 1.5f)
                                {
                                    m_MaterialEditor.ShaderProperty(_DissolveMaskAxis, "Axis");
                                    m_MaterialEditor.ShaderProperty(_DissolveMaskSpace, "Space");
                                    m_MaterialEditor.ShaderProperty(_DissolveMaskOffset, "Offset");
                                }

                                if (_DissolveMask.floatValue > 0.5f)
                                {
                                    bool axisInvert = _DissolveMaskInvert.floatValue < 0 ? true : false;

                                    EditorGUI.BeginChangeCheck();
                                    axisInvert = EditorGUILayout.Toggle("Invert", axisInvert);
                                    if (EditorGUI.EndChangeCheck())
                                    {
                                        _DissolveMaskInvert.floatValue = axisInvert ? -1 : 1;
                                    }
                                }
                            }

                            using (new VacuumEditorGUIUtility.GUIEnabled(true))
                            {
                                if (_DissolveMask.floatValue > 1.5f)
                                    m_MaterialEditor.ShaderProperty(_DissolveMaskCount, "Count");

                            }
                        }

                        GUILayout.Space(5);
                    }



                    EditorGUI.BeginChangeCheck();
                    foldoutAlphaSource = EditorGUILayout.Foldout(foldoutAlphaSource, " Cutout", guiStyleFoldout);
                    if (EditorGUI.EndChangeCheck())
                        EditorPrefs.SetBool(prefName_AlphaSource, foldoutAlphaSource);

                    if (foldoutAlphaSource)
                    {
                        using (new VacuumEditorGUIUtility.EditorGUIIndentLevel(1))
                        {
                            m_MaterialEditor.ShaderProperty(_DissolveAlphaSource, "Source");


                            m_MaterialEditor.ShaderProperty(_DissolveMappingType, "Mapping");
                            bool isTriplanar = _DissolveMappingType.floatValue > 0.5f && _DissolveMappingType.floatValue < 1.5f;
                            bool isScreen = _DissolveMappingType.floatValue > 1.5f && _DissolveMappingType.floatValue < 2.5f;

                            if (isStandard && isSM4 == false && isTriplanar)
                            {
                                EditorGUILayout.HelpBox("Shader Model 4.0 is required for Standard shader to use Triplanar mapping.", MessageType.Warning);
                            }


                            using (new VacuumEditorGUIUtility.GUIEnabled(!(globalControll_All)))
                            {
                                if (isStandard && isSM4 == false)
                                    isTriplanar = false;


                                if (isTriplanar)
                                    m_MaterialEditor.ShaderProperty(_DissolveTriplanarMappingSpace, new GUIContent("UV Space", "If disabled calculation is done in World Space"));

                                if (_DissolveMask.floatValue > 0.5f)    //Mask != None
                                    m_MaterialEditor.ShaderProperty(_DissolveNoiseStrength, "Noise");


                                if (_DissolveAlphaSource.floatValue < .5f)
                                {
                                    if (material.mainTexture == null)
                                    {
                                        EditorGUILayout.HelpBox("Main Map (Albedo) is not assinged", MessageType.Warning);
                                    }
                                    else
                                    {
                                        m_MaterialEditor.ShaderProperty(_DissolveMainMapTiling, "Tiling");
                                    }
                                }
                                if (_DissolveAlphaSource.floatValue >= .5f)
                                {
                                    GUILayout.Space(5);
                                    m_MaterialEditor.TexturePropertySingleLine(new GUIContent("Texture" + (_DissolveAlphaSource.floatValue > 1.5f ? " #1" : string.Empty), "Cutout (A) Distortion (RG)"), _DissolveMap1, _DissolveMap1Intensity, _DissolveMap1Channel);

                                    if (isTriplanar)
                                    {
                                        DrawTextureTriplanarTiling(_DissolveMap1, _DissolveMap1_Scroll);
                                    }
                                    else
                                    {
                                        m_MaterialEditor.TextureScaleOffsetProperty(_DissolveMap1);
                                        m_MaterialEditor.ShaderProperty(_DissolveMap1_Scroll, string.Empty);
                                    }
                                }
                                if (_DissolveAlphaSource.floatValue >= 1.5f)
                                {
                                    GUILayout.Space(5);
                                    m_MaterialEditor.TexturePropertySingleLine(new GUIContent("Texture #2", "Cutout (A) Distortion (RG)"), _DissolveMap2, _DissolveMap2Intensity, _DissolveMap2Channel);

                                    if (isTriplanar)
                                    {
                                        DrawTextureTriplanarTiling(_DissolveMap2, _DissolveMap2_Scroll);
                                    }
                                    else
                                    {
                                        m_MaterialEditor.TextureScaleOffsetProperty(_DissolveMap2);
                                        m_MaterialEditor.ShaderProperty(_DissolveMap2_Scroll, string.Empty);
                                    }
                                }
                                if (_DissolveAlphaSource.floatValue >= 2.5f)
                                {
                                    GUILayout.Space(5);
                                    m_MaterialEditor.TexturePropertySingleLine(new GUIContent("Texture #3", "Cutout (A) Distortion (RG)"), _DissolveMap3, _DissolveMap3Intensity, _DissolveMap3Channel);

                                    if (isTriplanar)
                                    {
                                        DrawTextureTriplanarTiling(_DissolveMap3, _DissolveMap3_Scroll);
                                    }
                                    else
                                    {
                                        m_MaterialEditor.TextureScaleOffsetProperty(_DissolveMap3);
                                        m_MaterialEditor.ShaderProperty(_DissolveMap3_Scroll, string.Empty);
                                    }
                                }


                                if (_DissolveAlphaSource.floatValue > 0.5f && isTriplanar == false && isScreen == false)
                                {
                                    GUILayout.Space(5);
                                    m_MaterialEditor.ShaderProperty(_DissolveAlphaSourceTexturesUVSet, "UV Set");
                                }

                                if (_DissolveAlphaSource.floatValue >= 1.5f)
                                    m_MaterialEditor.ShaderProperty(_DissolveSourceAlphaTexturesBlend, "Texture Blend");
                            }
                        }

                        GUILayout.Space(5);
                    }



                    EditorGUI.BeginChangeCheck();
                    foldoutEdge = EditorGUILayout.Foldout(foldoutEdge, " Edge", guiStyleFoldout);
                    if (EditorGUI.EndChangeCheck())
                        EditorPrefs.SetBool(prefName_Edge, foldoutEdge);

                    if (foldoutEdge)
                    {
                        using (new VacuumShaders.VacuumEditorGUIUtility.EditorGUIIndentLevel(1))
                        {
                            using (new VacuumEditorGUIUtility.GUIEnabled(!(globalControll_MaskAndEdge || globalControll_All)))
                            {
                                m_MaterialEditor.ShaderProperty(_DissolveEdgeWidth, "Width");

                                m_MaterialEditor.ShaderProperty(_DissolveEdgeShape, "Shape");
                                if (_DissolveEdgeShape.floatValue > 0 && material.shaderKeywords.Contains("_DISSOLVEMASK_BOX"))
                                {
                                    EditorGUILayout.HelpBox("Edge always is Solid with Box mask enabled.", MessageType.Warning);
                                }


                                m_MaterialEditor.SetDefaultGUIWidths();
                                m_MaterialEditor.ShaderProperty(_DissolveEdgeColor, "Color (RGB) Trans (A)");
                                EditorGUIUtility.fieldWidth = defaultFieldWidth;
                                EditorGUIUtility.labelWidth = defaultLabelWidth;

                                m_MaterialEditor.ShaderProperty(_DissolveEdgeColorIntensity, "Intensity");
                            }

                            using (new VacuumEditorGUIUtility.GUIEnabled(true))
                            {
                                m_MaterialEditor.ShaderProperty(_DissolveEdgeTextureSource, "Texture");
                            }

                            using (new VacuumEditorGUIUtility.GUIEnabled(!(globalControll_MaskAndEdge || globalControll_All)))
                            {
                                m_MaterialEditor.SetDefaultGUIWidths();

                                if (_DissolveEdgeTextureSource.floatValue > 0.5f && _DissolveEdgeTextureSource.floatValue < 1.5f)  //Gradient texture
                                {
                                    if (material.shaderKeywords.Contains("_DISSOLVEMASK_BOX"))
                                    {
                                        EditorGUILayout.HelpBox("Box masking does not support Gradient texture.", MessageType.Warning);
                                    }
                                    else
                                    {
                                        m_MaterialEditor.TexturePropertySingleLine(new GUIContent("Color (RGB) Trans (A)"), _DissolveEdgeTexture);


                                        EditorGUIUtility.fieldWidth = defaultFieldWidth;
                                        EditorGUIUtility.labelWidth = defaultLabelWidth;
                                        m_MaterialEditor.ShaderProperty(_DissolveEdgeTextureReverse, "Reverse");


                                        m_MaterialEditor.SetDefaultGUIWidths();
                                        m_MaterialEditor.ShaderProperty(_DissolveEdgeTextureAlphaOffset, "Alpha Offset");
                                        m_MaterialEditor.ShaderProperty(_DissolveEdgeTexturePhaseOffset, "Phase Offset");


                                        EditorGUIUtility.fieldWidth = defaultFieldWidth;
                                        EditorGUIUtility.labelWidth = defaultLabelWidth;

                                        m_MaterialEditor.ShaderProperty(_DissolveEdgeTextureIsDynamic, "Is Dynamic");
                                        if (_DissolveEdgeTextureIsDynamic.floatValue > 0.5f)
                                        {
                                            if (material.shaderKeywords.Contains("_DISSOLVEMASK_XYZ_AXIS") || material.shaderKeywords.Contains("_DISSOLVEMASK_PLANE") ||
                                                material.shaderKeywords.Contains("_DISSOLVEMASK_SPHERE") || material.shaderKeywords.Contains("_DISSOLVEMASK_BOX") ||
                                                material.shaderKeywords.Contains("_DISSOLVEMASK_CYLINDER") || material.shaderKeywords.Contains("_DISSOLVEMASK_CONE"))
                                            {
                                                EditorGUILayout.HelpBox("To make Dynamic Gradient work with Mask enabled, animate \"_DissolveCutoff\" property from script.", MessageType.Warning);
                                            }
                                        }

                                    }
                                }
                                else if (_DissolveEdgeTextureSource.floatValue > 1.5f && _DissolveEdgeTextureSource.floatValue < 2.5f)  //Main Map
                                {
                                    m_MaterialEditor.ShaderProperty(_DissolveEdgeTextureAlphaOffset, "Alpha Offset");

                                    m_MaterialEditor.ShaderProperty(_DissolveEdgeTextureMipmap, "Blur (Mip Map)");
                                }
                                else if (_DissolveEdgeTextureSource.floatValue > 2.5f && _DissolveEdgeTextureSource.floatValue < 3.5f)  //Custom Map
                                {
                                    m_MaterialEditor.TexturePropertySingleLine(new GUIContent("Color (RGB) Trans (A)"), _DissolveEdgeTexture);
                                    m_MaterialEditor.ShaderProperty(_DissolveEdgeTextureAlphaOffset, "Alpha Offset");

                                    m_MaterialEditor.ShaderProperty(_DissolveEdgeTextureMipmap, "Blur (Mip Map)");
                                }


                                EditorGUIUtility.fieldWidth = defaultFieldWidth;
                                EditorGUIUtility.labelWidth = defaultLabelWidth;
                            }


                            if(drawEmission)
                            {
                                if (_DissolveEdgeTextureSource.floatValue > 0.5f)
                                    GUILayout.Space(5);

                                if (m_MaterialEditor.EmissionEnabledProperty())
                                {
                                    using (new VacuumEditorGUIUtility.GUIEnabled(!(globalControll_MaskAndEdge || globalControll_All)))
                                    {
                                        m_MaterialEditor.SetDefaultGUIWidths();
                                        m_MaterialEditor.ShaderProperty(_DissolveGIMultiplier, new GUIContent("GI Multiplier", "Global Illumination multiplier used in the Meta pass"));
                                        EditorGUIUtility.fieldWidth = defaultFieldWidth;
                                        EditorGUIUtility.labelWidth = defaultLabelWidth;
                                    }
                                }
                            }
                        }

                        GUILayout.Space(5);
                    }



                    EditorGUI.BeginChangeCheck();
                    foldoutMainUVDistortion = EditorGUILayout.Foldout(foldoutMainUVDistortion, " Main UV Distortion", guiStyleFoldout);
                    if (EditorGUI.EndChangeCheck())
                        EditorPrefs.SetBool(prefName_MainUVDistortion, foldoutMainUVDistortion);

                    if (foldoutMainUVDistortion)
                    {
                        using (new VacuumShaders.VacuumEditorGUIUtility.EditorGUIIndentLevel(1))
                        {
                            using (new VacuumEditorGUIUtility.GUIEnabled(true))
                            {
                                m_MaterialEditor.ShaderProperty(_DissolveEdgeDistortionSource, "Noise Source");
                            }

                            using (new VacuumEditorGUIUtility.GUIEnabled(!globalControll_All))
                            {
                                m_MaterialEditor.ShaderProperty(_DissolveEdgeDistortionStrength, "Strength");
                            }
                        }

                        GUILayout.Space(5);
                    }



                    EditorGUI.BeginChangeCheck();
                    foldoutGlobal = EditorGUILayout.Foldout(foldoutGlobal, " Global", guiStyleFoldout);
                    if (EditorGUI.EndChangeCheck())
                        EditorPrefs.SetBool(prefName_Global, foldoutGlobal);

                    if (foldoutGlobal)
                    {
                        using (new VacuumEditorGUIUtility.EditorGUIIndentLevel(1))
                        {
                            using (new VacuumEditorGUIUtility.GUIColor((globalControll_Mask || globalControll_MaskAndEdge || globalControll_All) ? Color.yellow : Color.white))
                            {
                                m_MaterialEditor.ShaderProperty(_DissolveGlobalControl, " Control");
                            }
                        }
                    }
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        static public void DrawSurfaceOptions(MaterialEditor m_MaterialEditor, bool drawBlendModes, bool drawCull)
        {
            //Anchor
            GUILayout.Space(5);
            EditorGUI.BeginChangeCheck();
            using (new VacuumEditorGUIUtility.GUIBackgroundColor(Color.gray))
            {
                foldoutSurfaceOptions = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutSurfaceOptions, "Surface Options");
            }
            if (EditorGUI.EndChangeCheck())
                EditorPrefs.SetBool(prefName_SurfaceOptions, foldoutSurfaceOptions);

            if(foldoutSurfaceOptions)
            {
                if (drawBlendModes)
                    BlendModePopup(m_MaterialEditor);
                if (drawCull)
                    m_MaterialEditor.ShaderProperty(_Cull, "Render Face");
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        static public bool DrawSurfaceInputs(MaterialEditor m_MaterialEditor)
        {
            //Anchor
            GUILayout.Space(1);
            EditorGUI.BeginChangeCheck();
            using (new VacuumEditorGUIUtility.GUIBackgroundColor(Color.gray))
            {
                foldoutSurfaceInputs = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutSurfaceInputs, "Surface Inputs");
            }
            if (EditorGUI.EndChangeCheck())
                EditorPrefs.SetBool(prefName_SurfaceInputs, foldoutSurfaceInputs);

            return foldoutSurfaceInputs;
        }

        static public void BlendModePopup(MaterialEditor m_MaterialEditor)
        {
            EditorGUI.showMixedValue = _Mode.hasMixedValue;
            var mode = (BlendMode)_Mode.floatValue;

            EditorGUI.BeginChangeCheck();
            mode = (BlendMode)EditorGUILayout.Popup("Rendering Mode", (int)mode, new string[] { "Opaque", "Cutout", "Fade (Transparent)" });
            if (EditorGUI.EndChangeCheck())
            {
                m_MaterialEditor.RegisterPropertyChangeUndo("Rendering Mode");
                _Mode.floatValue = (float)mode;


                foreach (var obj in _Mode.targets)
                    SetupMaterialWithBlendMode((Material)obj, mode);
            }

            EditorGUI.showMixedValue = false;
        }

        public static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode)
        {
            switch (blendMode)
            {
                case BlendMode.Opaque:
                    //material.SetOverrideTag("RenderType", "");
                    material.SetOverrideTag("RenderType", "AdvancedDissolveCutout"); //Need cutout for Advanced Dissolve

                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = -1;

                    if (material.shader.name.Contains("Standard"))
                        material.SetFloat("_Cutoff", 0);

                    break;
                case BlendMode.Cutout:
                    material.SetOverrideTag("RenderType", "AdvancedDissolveCutout");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.EnableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
                    break;
                case BlendMode.Fade:
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    break;
                    //case BlendMode.Transparent:
                    //    material.SetOverrideTag("RenderType", "Transparent");
                    //    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    //    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    //    material.SetInt("_ZWrite", 0);
                    //    material.DisableKeyword("_ALPHATEST_ON");
                    //    material.DisableKeyword("_ALPHABLEND_ON");
                    //    material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    //    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    //    break;
            }
        }


        static void DrawTextureTriplanarTiling(MaterialProperty _tiling, MaterialProperty _scroll)
        {
            float tiling = _tiling.textureScaleAndOffset.x;
            Vector3 scroll = new Vector3(_scroll.vectorValue.x, _scroll.vectorValue.y, _scroll.vectorValue.z);


            //Tiling
            EditorGUI.BeginChangeCheck();
            tiling = EditorGUILayout.FloatField("Tiling", tiling);


            //Scroll
            EditorGUILayout.LabelField(" ");

            Rect position = GUILayoutUtility.GetLastRect();
            position.y -= 1;

            float width = EditorGUIUtility.labelWidth;
            EditorGUI.PrefixLabel(new Rect(position.x, position.y, width, 16f), new GUIContent("Scroll"));
            scroll = EditorGUI.Vector3Field(new Rect(position.x + width - 30, position.y, position.width - width + 30, 16f), GUIContent.none, scroll);

            if (EditorGUI.EndChangeCheck())
            {
                _tiling.textureScaleAndOffset = new Vector4(tiling, _tiling.textureScaleAndOffset.y, _tiling.textureScaleAndOffset.z, _tiling.textureScaleAndOffset.w);
                _scroll.vectorValue = scroll;
            }
        }
    }
}