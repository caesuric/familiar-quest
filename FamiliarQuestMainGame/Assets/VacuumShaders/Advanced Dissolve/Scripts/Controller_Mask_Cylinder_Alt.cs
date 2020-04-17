using UnityEngine;


namespace AdvancedDissolve_Example
{
    [ExecuteInEditMode]
    public class Controller_Mask_Cylinder_Alt : MonoBehaviour
    {
        static public Controller_Mask_Cylinder_Alt get;


        public bool updateGlobal;

        public Material[] materials;

        [Space(10)]
        public CylinderParameters cylinder1;
        public CylinderParameters cylinder2;
        public CylinderParameters cylinder3;
        public CylinderParameters cylinder4;

       
        [Space(10)]
        public bool invert;
        

        void Start()
        {
            get = this;

            UpdateMaskKeyword();
            UpdateMaskCountKeyword(1);
        }

        void Update()
        {
            UpdateShaderData(1, cylinder1);
            UpdateShaderData(2, cylinder2);
            UpdateShaderData(3, cylinder3);
            UpdateShaderData(4, cylinder4);
        }


        void UpdateShaderData(int maskID, CylinderParameters cylinder)
        {
            if (cylinder == null)
                return;


            Vector3 position = cylinder.position;
            Vector3 normal   = cylinder.normal;
            float radius     = cylinder.radius;
            float height     = cylinder.height;


            if (updateGlobal)
            {
                Shader.SetGlobalFloat("_DissolveMaskInvert_Global", invert ? 1 : -1);

                switch (maskID)
                {
                    case 1:
                        {
                            Shader.SetGlobalVector("_DissolveMaskPosition_Global", position);
                            Shader.SetGlobalVector("_DissolveMaskNormal_Global", normal);
                            Shader.SetGlobalFloat("_DissolveMaskRadius_Global", radius);
                            Shader.SetGlobalFloat("_DissolveMaskHeight_Global", height);
                        }
                        break;

                    case 2:
                        {
                            Shader.SetGlobalVector("_DissolveMask2Position_Global", position);
                            Shader.SetGlobalVector("_DissolveMask2Normal_Global", normal);
                            Shader.SetGlobalFloat("_DissolveMask2Radius_Global", radius);
                            Shader.SetGlobalFloat("_DissolveMask2Height_Global", height);
                        }
                        break;

                    case 3:
                        {
                            Shader.SetGlobalVector("_DissolveMask3Position_Global", position);
                            Shader.SetGlobalVector("_DissolveMask3Normal_Global", normal);
                            Shader.SetGlobalFloat("_DissolveMask3Radius_Global", radius);
                            Shader.SetGlobalFloat("_DissolveMask3Height_Global", height);
                        }
                        break;

                    case 4:
                        {
                            Shader.SetGlobalVector("_DissolveMask4Position_Global", position);
                            Shader.SetGlobalVector("_DissolveMask4Normal_Global", normal);
                            Shader.SetGlobalFloat("_DissolveMask4Radius_Global", radius);
                            Shader.SetGlobalFloat("_DissolveMask4Height_Global", height);
                        }
                        break;
                }
            }
            else if(materials != null)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] == null)
                        continue;


                    materials[i].SetFloat("_DissolveMaskInvert", invert ? 1 : -1);

                    switch (maskID)
                    {
                        case 1:
                            {
                                materials[i].SetVector("_DissolveMaskPosition", position);
                                materials[i].SetVector("_DissolveMaskNormal", normal);
                                materials[i].SetFloat("_DissolveMaskRadius", radius);
                                materials[i].SetFloat("_DissolveMaskHeight", height);
                            }
                            break;

                        case 2:
                            {
                                materials[i].SetVector("_DissolveMask2Position", position);
                                materials[i].SetVector("_DissolveMask2Normal", normal);
                                materials[i].SetFloat("_DissolveMask2Radius", radius);
                                materials[i].SetFloat("_DissolveMask2Height", height);
                            }
                            break;

                        case 3:
                            {
                                materials[i].SetVector("_DissolveMask3Position", position);
                                materials[i].SetVector("_DissolveMask3Normal", normal);
                                materials[i].SetFloat("_DissolveMask3Radius", radius);
                                materials[i].SetFloat("_DissolveMask3Height", height);
                            }
                            break;

                        case 4:
                            {
                                materials[i].SetVector("_DissolveMask4Position", position);
                                materials[i].SetVector("_DissolveMask4Normal", normal);
                                materials[i].SetFloat("_DissolveMask4Radius", radius);
                                materials[i].SetFloat("_DissolveMask4Height", height);
                            }
                            break;
                    }
                }                
            }
        }

        public void UpdateMaskKeyword()
        {
            if (materials != null)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] == null)
                        continue;

                    //Enable proper keyword only
                    materials[i].DisableKeyword("_DISSOLVEMASK_XYZ_AXIS");
                    materials[i].DisableKeyword("_DISSOLVEMASK_PLANE");
                    materials[i].DisableKeyword("_DISSOLVEMASK_SPHERE");
                    materials[i].DisableKeyword("_DISSOLVEMASK_BOX");
                    materials[i].DisableKeyword("_DISSOLVEMASK_CONE");


                    materials[i].EnableKeyword("_DISSOLVEMASK_CYLINDER");

                    //For material editor to select proper name inside dropdown
                    materials[i].SetFloat("_DissolveMask", 5);
                }
            }
        }

        public void UpdateMaskCountKeyword(int count)
        {
            if (materials != null)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] == null)
                        continue;

                    //Enable proper keyword only
                    materials[i].DisableKeyword("_DISSOLVEMASKCOUNT_FOUR");
                    materials[i].DisableKeyword("_DISSOLVEMASKCOUNT_THREE");
                    materials[i].DisableKeyword("_DISSOLVEMASKCOUNT_TWO");

                    switch (count)
                    {
                        case 1: break;
                        case 2: materials[i].EnableKeyword("_DISSOLVEMASKCOUNT_TWO"); break;
                        case 3: materials[i].EnableKeyword("_DISSOLVEMASKCOUNT_THREE"); break;
                        case 4: materials[i].EnableKeyword("_DISSOLVEMASKCOUNT_FOUR"); break;
                    }

                    //For material editor to select proper name inside dropdown
                    materials[i].SetFloat("_DissolveMaskCount", count - 1);
                }
            }
        }

    }

    [System.Serializable]
    public class CylinderParameters
    {
        public Vector3 position = Vector3.zero;
        public Vector3 normal = Vector3.forward;
        public float radius = 1;
        public float height = 5;
    }
}