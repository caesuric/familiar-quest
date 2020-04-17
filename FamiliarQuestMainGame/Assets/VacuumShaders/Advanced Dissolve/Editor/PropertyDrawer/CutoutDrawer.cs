using System.Linq;

using UnityEngine;
using UnityEditor;

namespace VacuumShaders.AdvancedDissolve
{
    class CutoutDrawer : MaterialPropertyDrawer
    {
        //////////////////////////////////////////////////////////////////////////////
        //                                                                          // 
        //Unity Functions                                                           //                
        //                                                                          //               
        //////////////////////////////////////////////////////////////////////////////

        public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
        {
            Material material = editor.target as Material;

            if (material != null && material.shaderKeywords.Contains("_ALPHATEST_ON"))
            {
                editor.SetDefaultGUIWidths();

                float value = prop.floatValue;

                EditorGUI.BeginChangeCheck();

                // Show the toggle control
                value = EditorGUI.Slider(position, label, value, 0f, 1f);

                if (EditorGUI.EndChangeCheck())
                {
                    prop.floatValue = Mathf.Clamp01(value);
                }
            }
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            Material material = editor.target as Material;

            if (material != null && material.shaderKeywords.Contains("_ALPHATEST_ON"))
                return 16;
            else
                return 0;
        }
    }

}
