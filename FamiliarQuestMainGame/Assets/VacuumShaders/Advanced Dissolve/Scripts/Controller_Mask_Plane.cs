using UnityEngine;

namespace AdvancedDissolve_Example
{
    [ExecuteInEditMode]
    public class Controller_Mask_Plane : MonoBehaviour
    {
        static public Controller_Mask_Plane get;


        public bool updateGlobal;

        public Material[] materials;

        public GameObject plane1;
        public GameObject plane2;
        public GameObject plane3;
        public GameObject plane4;

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
            UpdateShaderData(1, plane1);
            UpdateShaderData(2, plane2);
            UpdateShaderData(3, plane3);
            UpdateShaderData(4, plane4);
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
                    materials[i].DisableKeyword("_DISSOLVEMASK_SPHERE");
                    materials[i].DisableKeyword("_DISSOLVEMASK_BOX");
                    materials[i].DisableKeyword("_DISSOLVEMASK_CYLINDER");
                    materials[i].DisableKeyword("_DISSOLVEMASK_CONE");


                    materials[i].EnableKeyword("_DISSOLVEMASK_PLANE");

                    //For material editor to select proper name inside dropdown
                    materials[i].SetFloat("_DissolveMask", 2);
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


        void UpdateShaderData(int maskID, GameObject plane)
        {
            if (plane == null)
                return;


            Vector3 position = plane.transform.position;
            Vector3 normal = plane.transform.up;


            if (updateGlobal)
            {
                Shader.SetGlobalFloat("_DissolveMaskInvert_Global", invert ? 1 : -1);

                switch (maskID)
                {
                    case 1:
                        {
                            Shader.SetGlobalVector("_DissolveMaskPosition_Global", position);
                            Shader.SetGlobalVector("_DissolveMaskNormal_Global", normal);
                        }
                        break;

                    case 2:
                        {
                            Shader.SetGlobalVector("_DissolveMask2Position_Global", position);
                            Shader.SetGlobalVector("_DissolveMask2Normal_Global", normal);
                        }
                        break;

                    case 3:
                        {
                            Shader.SetGlobalVector("_DissolveMask3Position_Global", position);
                            Shader.SetGlobalVector("_DissolveMask3Normal_Global", normal);
                        }
                        break;

                    case 4:
                        {
                            Shader.SetGlobalVector("_DissolveMask4Position_Global", position);
                            Shader.SetGlobalVector("_DissolveMask4Normal_Global", normal);
                        }
                        break;
                }
            }
            else if (materials != null)
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
                            }
                            break;

                        case 2:
                            {
                                materials[i].SetVector("_DissolveMask2Position", position);
                                materials[i].SetVector("_DissolveMask2Normal", normal);
                            }
                            break;

                        case 3:
                            {
                                materials[i].SetVector("_DissolveMask3Position", position);
                                materials[i].SetVector("_DissolveMask3Normal", normal);
                            }
                            break;

                        case 4:
                            {
                                materials[i].SetVector("_DissolveMask4Position", position);
                                materials[i].SetVector("_DissolveMask4Normal", normal);
                            }
                            break;
                    }
                }              
            }
        }
    }
}