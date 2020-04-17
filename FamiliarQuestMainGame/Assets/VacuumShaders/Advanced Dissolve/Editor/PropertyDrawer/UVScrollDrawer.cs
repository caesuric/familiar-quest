//VacuumShaders 2015
// https://www.facebook.com/VacuumShaders

using UnityEngine;
using UnityEditor;
using System.Collections;


namespace VacuumShaders.AdvancedDissolve
{
    class UVScrollDrawer : MaterialPropertyDrawer
    {
        static GUIContent text = new GUIContent("Scroll");
        //////////////////////////////////////////////////////////////////////////////
        //                                                                          // 
        //Unity Functions                                                           //                
        //                                                                          //               
        //////////////////////////////////////////////////////////////////////////////

        public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
        {
            position.y -= 2;

            float width = EditorGUIUtility.labelWidth;


            EditorGUI.PrefixLabel(new Rect(position.x, position.y, width, 16f), text);


            Vector2 vector = prop.vectorValue;

            EditorGUI.BeginChangeCheck();
            vector = EditorGUI.Vector2Field(new Rect(position.x + width - 30, position.y, position.width - width + 30, 16f), GUIContent.none, vector);
            if (EditorGUI.EndChangeCheck())
            {
                prop.vectorValue = vector;
            }
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            return 16;
        }
    }
}