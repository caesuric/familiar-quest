using UnityEngine;


namespace AdvancedDissolve_Example
{
    [ExecuteInEditMode]
    public class Controller_Mask_XYZ_Axis : MonoBehaviour
    {
        public static Controller_Mask_XYZ_Axis get;


        public enum AXIS { X = 0, Y, Z }
        public enum SPACE { World = 0, Local }
        

        
        public bool updateGlobal;

        public Material[] materials;

        [Space(10)]
        public AXIS axis;
        public SPACE space;
        public float offset;
        public bool invert;

        void Start()
        {
            get = this;

            EnableMaskKeyword();
        }

        void Update()
        {
            UpdateShaderData();
        }
       

        public void EnableMaskKeyword()
        {
            if (materials != null)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] == null)
                        continue;


                    //Enable proper mask keyword
                    materials[i].DisableKeyword("_DISSOLVEMASK_PLANE");
                    materials[i].DisableKeyword("_DISSOLVEMASK_SPHERE");
                    materials[i].DisableKeyword("_DISSOLVEMASK_BOX");
                    materials[i].DisableKeyword("_DISSOLVEMASK_CYLINDER");
                    materials[i].DisableKeyword("_DISSOLVEMASK_CONE");


                    materials[i].EnableKeyword("_DISSOLVEMASK_XYZ_AXIS");

                    //For material editor to select proper mask name inside dropdown
                    materials[i].SetFloat("_DissolveMask", 1);
                }                
            }
        }

        void UpdateShaderData()
        {
            if (updateGlobal)
            {
                Shader.SetGlobalFloat("_DissolveMaskInvert_Global", invert ? 1 : -1);

                Shader.SetGlobalFloat("_DissolveMaskAxis_Global", (int)axis);
                Shader.SetGlobalFloat("_DissolveMaskSpace_Global", (int)space);
                Shader.SetGlobalFloat("_DissolveMaskOffset_Global", offset);
            }
            else if (materials != null)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] == null)
                        continue;


                    materials[i].SetFloat("_DissolveMaskInvert", invert ? 1 : -1);

                    materials[i].SetFloat("_DissolveMaskAxis", (int)axis);
                    materials[i].SetFloat("_DissolveMaskSpace", (int)space);
                    materials[i].SetFloat("_DissolveMaskOffset", offset);
                }                
            }
        }

    }
}