using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VacuumShaders.AdvancedDissolve
{
    public class ParticleGUI : ShaderGUI
    {
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            VacuumShaders.AdvancedDissolve.MaterialProperties.Init(materialEditor, properties);


            if (VacuumShaders.AdvancedDissolve.MaterialProperties.DrawSurfaceInputs(materialEditor))
            {
                base.OnGUI(materialEditor, properties);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();


            VacuumShaders.AdvancedDissolve.MaterialProperties.DrawDissolveOptions(materialEditor, false);
        }
    }
}