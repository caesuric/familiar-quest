using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

using UnityEditor;

[CanEditMultipleObjects]
internal class SpeedTreeGUI : MaterialEditor
{

    MaterialEditor m_MaterialEditor;


    private string[] speedTreeGeometryTypeString = new string[5] { "GEOM_TYPE_BRANCH", "GEOM_TYPE_BRANCH_DETAIL", "GEOM_TYPE_FROND", "GEOM_TYPE_LEAF", "GEOM_TYPE_MESH" };


    private bool ShouldEnableAlphaTest(SpeedTreeGUI.SpeedTreeGeometryType geomType)
    {
        return geomType == SpeedTreeGUI.SpeedTreeGeometryType.Frond || geomType == SpeedTreeGUI.SpeedTreeGeometryType.Leaf;
    }

    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        SerializedProperty property = this.serializedObject.FindProperty("m_Shader");
        if (!this.isVisible || property.hasMultipleDifferentValues || property.objectReferenceValue == (UnityEngine.Object)null)
            return;
        List<MaterialProperty> materialPropertyList = new List<MaterialProperty>((IEnumerable<MaterialProperty>)MaterialEditor.GetMaterialProperties(this.targets));


        m_MaterialEditor = this;


        GUILayout.Space(5);
        if ((m_MaterialEditor.target as Material).shader.name == "VacuumShaders/Advanced Dissolve/Nature/SpeedTree")
        {
            VacuumShaders.AdvancedDissolve.MaterialProperties.Init(this, materialPropertyList.ToArray());
        }

        if (VacuumShaders.AdvancedDissolve.MaterialProperties.DrawSurfaceInputs(this))
        {
            GUILayout.Space(5);

            this.SetDefaultGUIWidths();
            SpeedTreeGUI.SpeedTreeGeometryType[] treeGeometryTypeArray = new SpeedTreeGUI.SpeedTreeGeometryType[this.targets.Length];
            for (int index1 = 0; index1 < this.targets.Length; ++index1)
            {
                treeGeometryTypeArray[index1] = SpeedTreeGUI.SpeedTreeGeometryType.Branch;
                for (int index2 = 0; index2 < this.speedTreeGeometryTypeString.Length; ++index2)
                {
                    if (((IEnumerable<string>)((Material)this.targets[index1]).shaderKeywords).Contains<string>(this.speedTreeGeometryTypeString[index2]))
                    {
                        treeGeometryTypeArray[index1] = (SpeedTreeGUI.SpeedTreeGeometryType)index2;
                        break;
                    }
                }
            }
            EditorGUI.showMixedValue = ((IEnumerable<SpeedTreeGUI.SpeedTreeGeometryType>)treeGeometryTypeArray).Distinct<SpeedTreeGUI.SpeedTreeGeometryType>().Count<SpeedTreeGUI.SpeedTreeGeometryType>() > 1;
            EditorGUI.BeginChangeCheck();
            SpeedTreeGUI.SpeedTreeGeometryType geomType = (SpeedTreeGUI.SpeedTreeGeometryType)EditorGUILayout.EnumPopup("Geometry Type", (Enum)treeGeometryTypeArray[0], new GUILayoutOption[0]);
            if (EditorGUI.EndChangeCheck())
            {
                bool flag = this.ShouldEnableAlphaTest(geomType);
                CullMode cullMode = !flag ? CullMode.Back : CullMode.Off;
                foreach (Material material in this.targets.Cast<Material>())
                {
                    if (flag)
                        material.SetOverrideTag("RenderType", "AdvancedDissolveTreeTransparentCutout");
                    for (int index = 0; index < this.speedTreeGeometryTypeString.Length; ++index)
                        material.DisableKeyword(this.speedTreeGeometryTypeString[index]);
                    material.EnableKeyword(this.speedTreeGeometryTypeString[(int)geomType]);
                    material.renderQueue = !flag ? 2000 : 2450;
                    material.SetInt("_Cull", (int)cullMode);
                }
            }
            EditorGUI.showMixedValue = false;
            MaterialProperty prop1 = materialPropertyList.Find((Predicate<MaterialProperty>)(prop => prop.name == "_MainTex"));
            if (prop1 != null)
            {
                materialPropertyList.Remove(prop1);
                this.ShaderProperty(prop1, prop1.displayName);
            }
            MaterialProperty prop2 = materialPropertyList.Find((Predicate<MaterialProperty>)(prop => prop.name == "_BumpMap"));
            if (prop2 != null)
            {
                materialPropertyList.Remove(prop2);
                IEnumerable<bool> source = ((IEnumerable<UnityEngine.Object>)this.targets).Select<UnityEngine.Object, bool>((Func<UnityEngine.Object, bool>)(t => ((IEnumerable<string>)((Material)t).shaderKeywords).Contains<string>("EFFECT_BUMP")));
                bool? nullable = this.ToggleShaderProperty(prop2, source.First<bool>(), source.Distinct<bool>().Count<bool>() > 1);
                if (nullable.HasValue)
                {
                    foreach (Material material in this.targets.Cast<Material>())
                    {
                        if (nullable.Value)
                            material.EnableKeyword("EFFECT_BUMP");
                        else
                            material.DisableKeyword("EFFECT_BUMP");
                    }
                }
            }
            MaterialProperty prop3 = materialPropertyList.Find((Predicate<MaterialProperty>)(prop => prop.name == "_DetailTex"));
            if (prop3 != null)
            {
                materialPropertyList.Remove(prop3);
                if (((IEnumerable<SpeedTreeGUI.SpeedTreeGeometryType>)treeGeometryTypeArray).Contains<SpeedTreeGUI.SpeedTreeGeometryType>(SpeedTreeGUI.SpeedTreeGeometryType.BranchDetail))
                    this.ShaderProperty(prop3, prop3.displayName);
            }
            IEnumerable<bool> source1 = ((IEnumerable<UnityEngine.Object>)this.targets).Select<UnityEngine.Object, bool>((Func<UnityEngine.Object, bool>)(t => ((IEnumerable<string>)((Material)t).shaderKeywords).Contains<string>("EFFECT_HUE_VARIATION")));
            MaterialProperty prop4 = materialPropertyList.Find((Predicate<MaterialProperty>)(prop => prop.name == "_HueVariation"));
            if (source1 != null && prop4 != null)
            {
                materialPropertyList.Remove(prop4);
                bool? nullable = this.ToggleShaderProperty(prop4, source1.First<bool>(), source1.Distinct<bool>().Count<bool>() > 1);
                if (nullable.HasValue)
                {
                    foreach (Material material in this.targets.Cast<Material>())
                    {
                        if (nullable.Value)
                            material.EnableKeyword("EFFECT_HUE_VARIATION");
                        else
                            material.DisableKeyword("EFFECT_HUE_VARIATION");
                    }
                }
            }
            MaterialProperty prop5 = materialPropertyList.Find((Predicate<MaterialProperty>)(prop => prop.name == "_Cutoff"));
            if (prop5 != null)
            {
                materialPropertyList.Remove(prop5);
                if (((IEnumerable<SpeedTreeGUI.SpeedTreeGeometryType>)treeGeometryTypeArray).Any<SpeedTreeGUI.SpeedTreeGeometryType>((Func<SpeedTreeGUI.SpeedTreeGeometryType, bool>)(t => this.ShouldEnableAlphaTest(t))))
                    this.ShaderProperty(prop5, prop5.displayName);
            }
            foreach (MaterialProperty prop6 in materialPropertyList)
            {
                if ((prop6.flags & (MaterialProperty.PropFlags.HideInInspector | MaterialProperty.PropFlags.PerRendererData)) == MaterialProperty.PropFlags.None)
                    this.ShaderProperty(prop6, prop6.displayName);
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            this.RenderQueueField();
            this.EnableInstancingField();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        if ((m_MaterialEditor.target as Material).shader.name == "VacuumShaders/Advanced Dissolve/Nature/SpeedTree")
        {
            VacuumShaders.AdvancedDissolve.MaterialProperties.DrawDissolveOptions(this, false);
        }

    }

    private bool? ToggleShaderProperty(MaterialProperty prop, bool enable, bool hasMixedEnable)
    {
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = hasMixedEnable;
        enable = EditorGUI.ToggleLeft(EditorGUILayout.GetControlRect(false, GUILayout.ExpandWidth(false)), prop.displayName, enable);
        EditorGUI.showMixedValue = false;
        bool? nullable = !EditorGUI.EndChangeCheck() ? new bool?() : new bool?(enable);
        GUILayout.Space(-EditorGUIUtility.singleLineHeight);
        using (new EditorGUI.DisabledScope(!enable && !hasMixedEnable))
        {
            EditorGUI.showMixedValue = prop.hasMixedValue;
            this.ShaderProperty(prop, " ");
            EditorGUI.showMixedValue = false;
        }
        return nullable;
    }

    private enum SpeedTreeGeometryType
    {
        Branch,
        BranchDetail,
        Frond,
        Leaf,
        Mesh,
    }

}

