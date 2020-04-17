﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VacuumShaders.AdvancedDissolve
{
    public class ShadowGUI : ShaderGUI
    {
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            VacuumShaders.AdvancedDissolve.MaterialProperties.Init(materialEditor, properties);


            VacuumShaders.AdvancedDissolve.MaterialProperties.DrawSurfaceOptions(materialEditor,
                                                                                (materialEditor.target as Material).shader.name.Contains("AlphaTest") == false,
                                                                                true);

            if (VacuumShaders.AdvancedDissolve.MaterialProperties.DrawSurfaceInputs(materialEditor))
            {
                base.OnGUI(materialEditor, properties);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();


            VacuumShaders.AdvancedDissolve.MaterialProperties.DrawDissolveOptions(materialEditor, false);

        }
    }
}