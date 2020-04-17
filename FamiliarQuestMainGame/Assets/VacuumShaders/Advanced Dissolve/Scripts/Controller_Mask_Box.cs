using UnityEngine;

namespace AdvancedDissolve_Example
{
    [ExecuteInEditMode]
    public class Controller_Mask_Box : MonoBehaviour
    {
        static public Controller_Mask_Box get;


        public bool updateGlobal;       

        public Material[] materials;

        public GameObject box1;
        public GameObject box2;
        public GameObject box3;
        public GameObject box4;

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
            UpdateShaderData(1, box1);
            UpdateShaderData(2, box2);
            UpdateShaderData(3, box3);
            UpdateShaderData(4, box4);
        }


        void UpdateShaderData(int maskID, GameObject box)
        {
            if (box == null)
                return;

            Bounds bounds = box.GetComponent<MeshFilter>().sharedMesh.bounds;



            Vector3 boundMin = Vector3.Scale(bounds.min, box.transform.localScale);
            Vector3 boundMax = Vector3.Scale(bounds.max, box.transform.localScale);

            Matrix4x4 trs = Matrix4x4.TRS(box.transform.position, box.transform.rotation, Vector3.one).inverse;


            if (updateGlobal)
            {
                Shader.SetGlobalFloat("_DissolveMaskInvert_Global", invert ? 1 : -1);

                switch (maskID)
                {
                    case 1:
                        {
                            Shader.SetGlobalVector("_DissolveMaskBoundsMin_Global", boundMin);
                            Shader.SetGlobalVector("_DissolveMaskBoundsMax_Global", boundMax);
                            Shader.SetGlobalMatrix("_DissolveMaskTRS_Global", trs);
                        }
                        break;

                    case 2:
                        {
                            Shader.SetGlobalVector("_DissolveMask2BoundsMin_Global", boundMin);
                            Shader.SetGlobalVector("_DissolveMask2BoundsMax_Global", boundMax);
                            Shader.SetGlobalMatrix("_DissolveMask2TRS_Global", trs);
                        }
                        break;

                    case 3:
                        {
                            Shader.SetGlobalVector("_DissolveMask3BoundsMin_Global", boundMin);
                            Shader.SetGlobalVector("_DissolveMask3BoundsMax_Global", boundMax);
                            Shader.SetGlobalMatrix("_DissolveMask3TRS_Global", trs);
                        }
                        break;

                    case 4:
                        {
                            Shader.SetGlobalVector("_DissolveMask4BoundsMin_Global", boundMin);
                            Shader.SetGlobalVector("_DissolveMask4BoundsMax_Global", boundMax);
                            Shader.SetGlobalMatrix("_DissolveMask4TRS_Global", trs);
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
                                materials[i].SetVector("_DissolveMaskBoundsMin", boundMin);
                                materials[i].SetVector("_DissolveMaskBoundsMax", boundMax);
                                materials[i].SetMatrix("_DissolveMaskTRS", trs);
                            }
                            break;

                        case 2:
                            {
                                materials[i].SetVector("_DissolveMask2BoundsMin", boundMin);
                                materials[i].SetVector("_DissolveMask2BoundsMax", boundMax);
                                materials[i].SetMatrix("_DissolveMask2TRS", trs);
                            }
                            break;

                        case 3:
                            {
                                materials[i].SetVector("_DissolveMask3BoundsMin", boundMin);
                                materials[i].SetVector("_DissolveMask3BoundsMax", boundMax);
                                materials[i].SetMatrix("_DissolveMask3TRS", trs);
                            }
                            break;

                        case 4:
                            {
                                materials[i].SetVector("_DissolveMask4BoundsMin", boundMin);
                                materials[i].SetVector("_DissolveMask4BoundsMax", boundMax);
                                materials[i].SetMatrix("_DissolveMask4TRS", trs);
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
                    materials[i].DisableKeyword("_DISSOLVEMASK_CYLINDER");
                    materials[i].DisableKeyword("_DISSOLVEMASK_CONE");


                    materials[i].EnableKeyword("_DISSOLVEMASK_BOX");

                    //For material editor to select proper name inside dropdown
                    materials[i].SetFloat("_DissolveMask", 4);
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