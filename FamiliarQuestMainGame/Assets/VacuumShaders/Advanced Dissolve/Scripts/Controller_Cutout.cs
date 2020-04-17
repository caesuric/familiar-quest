using UnityEngine;

namespace AdvancedDissolve_Example
{
    [ExecuteInEditMode]
    public class Controller_Cutout : MonoBehaviour
    {
        static public Controller_Cutout get;
        

        public enum CUTOUT_SOURCE { MainMapAlpha, CustomMap, TwoCustomMaps, ThreeCustomMaps }
        public enum MAPPING { Normal, Triplanar, ScreenSpace }
        public enum TRIPLANAR_SPACE { World, Local }
        public enum UVSET { UV0 = 0, UV1 }
        public enum TEXTURE_BLEND { Multiple = 0, Add }
        public enum TEXTURE_CHANNEL { Red, Green, Blue, Alpha }


        public bool updateGlobal;

        public Material[] materials;


        [Space(10)]
        public float noise;

        [Space(10)]
        public Texture texture1;
        public Vector2 texture1Tiling = Vector2.one;
        public Vector2 texture1Offset;
        public Vector3 texture1Scroll;
        public TEXTURE_CHANNEL texture1Channel = TEXTURE_CHANNEL.Alpha;
        [Range(0f, 1f)]
        public float texture1Intensity = 1;
        

        [Space(10)]
        public Texture texture2;
        public Vector2 texture2Tiling = Vector2.one;
        public Vector2 texture2Offset;
        public Vector3 texture2Scroll;
        public TEXTURE_CHANNEL texture2Channel = TEXTURE_CHANNEL.Alpha;
        [Range(0f, 1f)]
        public float texture2Intensity = 1;
        

        [Space(10)]
        public Texture texture3;
        public Vector2 texture3Tiling = Vector2.one;
        public Vector2 texture3Offset;
        public Vector3 texture3Scroll;
        public TEXTURE_CHANNEL texture3Channel = TEXTURE_CHANNEL.Alpha;
        [Range(0f, 1f)]
        public float texture3Intensity = 1;
        

        [Space(10)]
        public UVSET uvSet;
        public TEXTURE_BLEND textureBlend;


        void Start()
        {
            get = this;
        }
        void Update()
        {
            UpdateShaderData();
        }


        void UpdateShaderData()
        {
            if (updateGlobal)
            {
                Shader.SetGlobalTexture("_DissolveMap1_Global", texture1);
                Shader.SetGlobalVector("_DissolveMap1_ST_Global", new Vector4(texture1Tiling.x, texture1Tiling.y, texture1Offset.x, texture1Offset.y));
                Shader.SetGlobalVector("_DissolveMap1_Scroll_Global", texture1Scroll);
                Shader.SetGlobalFloat("_DissolveMap1Intensity_Global", texture1Intensity);
                Shader.SetGlobalFloat("_DissolveMap1Channel_Global", (int)texture1Channel);

                Shader.SetGlobalTexture("_DissolveMap2_Global", texture2);
                Shader.SetGlobalVector("_DissolveMap2_ST_Global", new Vector4(texture2Tiling.x, texture2Tiling.y, texture2Offset.x, texture2Offset.y));
                Shader.SetGlobalVector("_DissolveMap2_Scroll_Global", texture2Scroll);
                Shader.SetGlobalFloat("_DissolveMap2Intensity_Global", texture2Intensity);
                Shader.SetGlobalFloat("_DissolveMap2Channel_Global", (int)texture2Channel);

                Shader.SetGlobalTexture("_DissolveMap3_Global", texture3);
                Shader.SetGlobalVector("_DissolveMap3_ST_Global", new Vector4(texture3Tiling.x, texture3Tiling.y, texture3Offset.x, texture3Offset.y));
                Shader.SetGlobalVector("_DissolveMap3_Scroll_Global", texture3Scroll);
                Shader.SetGlobalFloat("_DissolveMap3Intensity_Global", texture3Intensity);
                Shader.SetGlobalFloat("_DissolveMap3Channel_Global", (int)texture3Channel);

                Shader.SetGlobalFloat("_DissolveNoiseStrength_Global", noise);
                Shader.SetGlobalFloat("_DissolveAlphaSourceTexturesUVSet_Global", uvSet == UVSET.UV0 ? 0 : 1);
                Shader.SetGlobalFloat("_DissolveSourceAlphaTexturesBlend_Global", textureBlend == TEXTURE_BLEND.Multiple ? 0 : 1);
            }
            else if (materials != null)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] == null)
                        continue;

                    materials[i].SetTexture("_DissolveMap1", texture1);
                    materials[i].SetVector("_DissolveMap1_ST", new Vector4(texture1Tiling.x, texture1Tiling.y, texture1Offset.x, texture1Offset.y));
                    materials[i].SetVector("_DissolveMap1_Scroll", texture1Scroll);
                    materials[i].SetFloat("_DissolveMap1Intensity", texture1Intensity);
                    materials[i].SetFloat("_DissolveMap1Channel", (int)texture1Channel);

                    materials[i].SetTexture("_DissolveMap2", texture2);
                    materials[i].SetVector("_DissolveMap2_ST", new Vector4(texture2Tiling.x, texture2Tiling.y, texture2Offset.x, texture2Offset.y));
                    materials[i].SetVector("_DissolveMap2_Scroll", texture2Scroll);
                    materials[i].SetFloat("_DissolveMap2Intensity", texture2Intensity);
                    materials[i].SetFloat("_DissolveMap2Channel", (int)texture2Channel);

                    materials[i].SetTexture("_DissolveMap3", texture3);
                    materials[i].SetVector("_DissolveMap3_ST", new Vector4(texture3Tiling.x, texture3Tiling.y, texture3Offset.x, texture3Offset.y));
                    materials[i].SetVector("_DissolveMap3_Scroll", texture3Scroll);
                    materials[i].SetFloat("_DissolveMap3Intensity", texture3Intensity);
                    materials[i].SetFloat("_DissolveMap3Channel", (int)texture3Channel);

                    materials[i].SetFloat("_DissolveNoiseStrength", noise);
                    materials[i].SetFloat("_DissolveAlphaSourceTexturesUVSet", uvSet == UVSET.UV0 ? 0 : 1);
                    materials[i].SetFloat("_DissolveSourceAlphaTexturesBlend", textureBlend == TEXTURE_BLEND.Multiple ? 0 : 1);
                }
            }
        }

        public void UpdateCutoutSourceKeyword(CUTOUT_SOURCE cutoutSource)
        {
            if (materials != null)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] == null)
                        continue;


                    //Enable proper keywords only
                    materials[i].DisableKeyword("_DISSOLVEALPHASOURCE_CUSTOM_MAP");
                    materials[i].DisableKeyword("_DISSOLVEALPHASOURCE_TWO_CUSTOM_MAPS");
                    materials[i].DisableKeyword("_DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS");


                    switch(cutoutSource)
                    {
                        case CUTOUT_SOURCE.MainMapAlpha:
                            {
                                //For material editor to select proper name inside dropdown
                                materials[i].SetFloat("_DissolveAlphaSource", 0);
                            } break;

                        case CUTOUT_SOURCE.CustomMap:
                            {
                                materials[i].EnableKeyword("_DISSOLVEALPHASOURCE_CUSTOM_MAP");

                                //For material editor to select proper name inside dropdown
                                materials[i].SetFloat("_DissolveAlphaSource", 1);
                            } break;

                        case CUTOUT_SOURCE.TwoCustomMaps:
                            {
                                materials[i].EnableKeyword("_DISSOLVEALPHASOURCE_TWO_CUSTOM_MAPS");

                                //For material editor to select proper name inside dropdown
                                materials[i].SetFloat("_DissolveAlphaSource", 2);
                            }
                            break;

                        case CUTOUT_SOURCE.ThreeCustomMaps:
                            {
                                materials[i].EnableKeyword("_DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS");

                                //For material editor to select proper name inside dropdown
                                materials[i].SetFloat("_DissolveAlphaSource", 3);
                            }
                            break;
                    }
                }
            }
        }

        public void UpdateMappingKeyword(MAPPING mapping)
        {
            if (materials != null)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] == null)
                        continue;


                    //Enable proper keywords only
                    materials[i].DisableKeyword("_DISSOLVEMAPPINGTYPE_TRIPLANAR");
                    materials[i].DisableKeyword("_DISSOLVEMAPPINGTYPE_SCREEN_SPACE");
                    

                    switch (mapping)
                    {
                        case MAPPING.Normal:
                            {
                                //For material editor to select proper name inside dropdown
                                materials[i].SetFloat("_DissolveMappingType", 0);
                            }
                            break;

                        case MAPPING.Triplanar:
                            {
                                materials[i].EnableKeyword("_DISSOLVEMAPPINGTYPE_TRIPLANAR");

                                //For material editor to select proper name inside dropdown
                                materials[i].SetFloat("_DissolveMappingType", 1);
                            }
                            break;

                        case MAPPING.ScreenSpace:
                            {
                                materials[i].EnableKeyword("_DISSOLVEMAPPINGTYPE_SCREEN_SPACE");

                                //For material editor to select proper name inside dropdown
                                materials[i].SetFloat("_DissolveMappingType", 2);
                            }
                            break;
                    }
                }
            }
        }

    }
}