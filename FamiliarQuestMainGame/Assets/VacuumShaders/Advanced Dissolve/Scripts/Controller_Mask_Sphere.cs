using UnityEngine;

namespace AdvancedDissolve_Example
{
    [ExecuteInEditMode]
    public class Controller_Mask_Sphere : MonoBehaviour
    {
        static public Controller_Mask_Sphere get;


        public bool updateGlobal;

        public Material[] materials;

        [Space(10)]
        public GameObject sphere1;
        public GameObject sphere2;
        public GameObject sphere3;
        public GameObject sphere4;


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
            UpdateShaderData(1, sphere1);
            UpdateShaderData(2, sphere2);
            UpdateShaderData(3, sphere3);
            UpdateShaderData(4, sphere4);
        }

        void UpdateShaderData(int maskID, GameObject sphere)
        {
            if (sphere == null)
                return;


            Vector3 position = sphere.transform.position;
            float radius = sphere.transform.localScale.x * 0.5f;

            if (updateGlobal)
            {
                Shader.SetGlobalFloat("_DissolveMaskInvert_Global", invert ? 1 : -1);

                switch (maskID)
                {
                    case 1:
                        {
                            Shader.SetGlobalVector("_DissolveMaskPosition_Global", position);
                            Shader.SetGlobalFloat("_DissolveMaskRadius_Global", radius);
                        }
                        break;

                    case 2:
                        {
                            Shader.SetGlobalVector("_DissolveMask2Position_Global", position);
                            Shader.SetGlobalFloat("_DissolveMask2Radius_Global", radius);
                        }
                        break;

                    case 3:
                        {
                            Shader.SetGlobalVector("_DissolveMask3Position_Global", position);
                            Shader.SetGlobalFloat("_DissolveMask3Radius_Global", radius);
                        }
                        break;

                    case 4:
                        {
                            Shader.SetGlobalVector("_DissolveMask4Position_Global", position);
                            Shader.SetGlobalFloat("_DissolveMask4Radius_Global", radius);
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
                                materials[i].SetFloat("_DissolveMaskRadius", radius);
                            }
                            break;

                        case 2:
                            {
                                materials[i].SetVector("_DissolveMask2Position", position);
                                materials[i].SetFloat("_DissolveMask2Radius", radius);
                            }
                            break;

                        case 3:
                            {
                                materials[i].SetVector("_DissolveMask3Position", position);
                                materials[i].SetFloat("_DissolveMask3Radius", radius);
                            }
                            break;

                        case 4:
                            {
                                materials[i].SetVector("_DissolveMask4Position", position);
                                materials[i].SetFloat("_DissolveMask4Radius", radius);
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
                    materials[i].DisableKeyword("_DISSOLVEMASK_BOX");
                    materials[i].DisableKeyword("_DISSOLVEMASK_CYLINDER");
                    materials[i].DisableKeyword("_DISSOLVEMASK_CONE");


                    materials[i].EnableKeyword("_DISSOLVEMASK_SPHERE");

                    //For material editor to select proper name inside dropdown
                    materials[i].SetFloat("_DissolveMask", 3);
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
}