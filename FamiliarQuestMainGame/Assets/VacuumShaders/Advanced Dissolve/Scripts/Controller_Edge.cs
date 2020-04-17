using UnityEngine;

namespace AdvancedDissolve_Example
{
    [ExecuteInEditMode]
    public class Controller_Edge : MonoBehaviour
    {
        static public Controller_Edge get;


        public enum TEXTURE_TYPE { None, Gradient, MainMap, Custom }
        public enum SHAPE { Solid, Smooth, Smooth_Squared }


        public bool updateGlobal;

        public Material[] materials;


        [Range(0f, 1f), Space(10)]
        public float width = 0.25f;
        public SHAPE shape;
        public Color color = Color.green;
        public float intensity;
        

        [Space(10)]
        public Texture texture;
        public bool reverse;        
        [Range(-1f, 1f)]
        public float alphaOffset;
        public float phaseOffset;
        [Range(1, 10)]
        public float blur;
        public bool isDynamic;

        [Space(10)]
        public float GIMultyplier;


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
                Shader.SetGlobalFloat("_DissolveEdgeWidth_Global", width);
                Shader.SetGlobalFloat("_DissolveEdgeShape_Global", (int)shape);
                Shader.SetGlobalColor("_DissolveEdgeColor_Global", color);
                Shader.SetGlobalFloat("_DissolveEdgeColorIntensity_Global", intensity);

                Shader.SetGlobalTexture("_DissolveEdgeTexture_Global", texture);
                Shader.SetGlobalFloat("_DissolveEdgeTextureReverse_Global", reverse ? 1 : 0);
                Shader.SetGlobalFloat("_DissolveEdgeTextureMipmap_Global", blur);
                Shader.SetGlobalFloat("_DissolveEdgeTextureAlphaOffset_Global", alphaOffset);
                Shader.SetGlobalFloat("_DissolveEdgeTexturePhaseOffset_Global", phaseOffset);
                Shader.SetGlobalFloat("_DissolveEdgeTextureIsDynamic_Global", isDynamic ? 1 : 0);

                Shader.SetGlobalFloat("_DissolveGIMultiplier_Global", GIMultyplier > 0 ? GIMultyplier : 0);
            }
            else if (materials != null)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] == null)
                        continue;


                    materials[i].SetFloat("_DissolveEdgeWidth", width);
                    materials[i].SetFloat("_DissolveEdgeShape", (int)shape);
                    materials[i].SetColor("_DissolveEdgeColor", color);
                    materials[i].SetFloat("_DissolveEdgeColorIntensity", intensity);

                    materials[i].SetTexture("_DissolveEdgeTexture", texture);
                    materials[i].SetFloat("_DissolveEdgeTextureReverse", reverse ? 1 : 0);
                    materials[i].SetFloat("_DissolveEdgeTextureMipmap", blur);
                    materials[i].SetFloat("_DissolveEdgeTextureAlphaOffset", alphaOffset);
                    materials[i].SetFloat("_DissolveEdgeTexturePhaseOffset", phaseOffset);
                    materials[i].SetFloat("_DissolveEdgeTextureIsDynamic", isDynamic ? 1 : 0);

                    materials[i].SetFloat("_DissolveGIMultiplier", GIMultyplier > 0 ? GIMultyplier : 0);
                }               
            }
        }

        public void UpdateTextureTypeKeyword(TEXTURE_TYPE textureType)
        {
            if (materials != null)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] == null)
                        continue;


                    //Enable proper texture type keyword
                    materials[i].DisableKeyword("_DISSOLVEEDGETEXTURESOURCE_GRADIENT");
                    materials[i].DisableKeyword("_DISSOLVEEDGETEXTURESOURCE_MAIN_MAP");
                    materials[i].DisableKeyword("_DISSOLVEEDGETEXTURESOURCE_CUSTOM");


                    switch (textureType)
                    {
                        case TEXTURE_TYPE.None:
                            {
                                //For material editor to select proper name inside dropdown
                                materials[i].SetFloat("_DissolveEdgeTextureSource", 0);
                            }
                            break;

                        case TEXTURE_TYPE.Gradient:
                            {
                                materials[i].EnableKeyword("_DISSOLVEEDGETEXTURESOURCE_GRADIENT");

                                //For material editor to select proper name inside dropdown
                                materials[i].SetFloat("_DissolveEdgeTextureSource", 1);
                            }
                            break;

                        case TEXTURE_TYPE.MainMap:
                            {
                                materials[i].EnableKeyword("_DISSOLVEEDGETEXTURESOURCE_MAIN_MAP");

                                //For material editor to select proper name inside dropdown
                                materials[i].SetFloat("_DissolveEdgeTextureSource", 2);
                            }
                            break;

                        case TEXTURE_TYPE.Custom:
                            {
                                materials[i].EnableKeyword("_DISSOLVEEDGETEXTURESOURCE_CUSTOM");

                                //For material editor to select proper name inside dropdown
                                materials[i].SetFloat("_DissolveEdgeTextureSource", 3);
                            }
                            break;
                    }
                }
            }
        }

    }
}