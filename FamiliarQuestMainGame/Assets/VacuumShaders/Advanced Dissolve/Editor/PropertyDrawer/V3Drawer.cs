using UnityEngine;
using UnityEditor;

namespace VacuumShaders.AdvancedDissolve
{
    public class V3Drawer : MaterialPropertyDrawer
    {
        //////////////////////////////////////////////////////////////////////////////
        //                                                                          // 
        //Unity Functions                                                           //                
        //                                                                          //               
        //////////////////////////////////////////////////////////////////////////////

        public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
        {

            Vector3 vector = prop.vectorValue;

            EditorGUI.BeginChangeCheck();
            vector = EditorGUILayout.Vector3Field(label, vector);
            if (EditorGUI.EndChangeCheck())
            {
                prop.vectorValue = vector;
            }
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            return 0;
        }
    }
}