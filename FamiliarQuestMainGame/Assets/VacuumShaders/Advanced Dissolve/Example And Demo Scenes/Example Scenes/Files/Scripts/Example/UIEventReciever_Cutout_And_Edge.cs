using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedDissolve_Example
{
    public class UIEventReciever_Cutout_And_Edge : MonoBehaviour
    {
        //Updates all material 'Cutout' part related parameters
        Controller_Cutout cutoutController;

        //Updates all 'Edge' part related parameters
        Controller_Edge edgeController;



        public Texture2D edgeTexture;
        public Transform uiColor;

        float colorHue;
        float colorIntensity;


        // Use this for initialization
        void Start()
        {
            cutoutController = GetComponent<Controller_Cutout>();
            edgeController = GetComponent<Controller_Edge>();

             UI_Texture(0);
            UI_Color(0.333f);
            UI_Intensity(1);
            UI_Width(0.2f);
            UI_Solid(false);

            UI_Noise(0);
            UI_Speed(0.5f);
        }
        

        public void UI_Texture(int value)
        {
            Controller_Edge.TEXTURE_TYPE textureType = (Controller_Edge.TEXTURE_TYPE)value;


            //Update materials keyword based on texture type
            edgeController.UpdateTextureTypeKeyword(textureType);

            if (textureType == Controller_Edge.TEXTURE_TYPE.None)
            {
                uiColor.gameObject.SetActive(true);

                UI_Color(colorHue);
            }
            else
            {
                uiColor.gameObject.SetActive(false);

                edgeController.texture = edgeTexture;

                //We can use random color, but white color makes 'edgeTexture' more visible
                edgeController.color = Color.white;
            }
        }
        public void UI_Color(float value)
        {
            colorHue = value;

            //Converting slider [0, 1] range value into color;
            Color color = Color.HSVToRGB(colorHue, 1, 1);

            edgeController.color = color;
        }

        public void UI_Intensity(float value)
        {
            edgeController.intensity = value;
        }

        public void UI_Width(float value)
        {
            edgeController.width = value;
        }

        public void UI_Solid(bool value)
        {
            edgeController.shape = value ? Controller_Edge.SHAPE.Solid : Controller_Edge.SHAPE.Smooth;
        }

        public void UI_Noise(float value)
        {
            cutoutController.noise = value;
        }

        public void UI_Speed(float value)
        {
            cutoutController.texture1Scroll.y = value * -1;
        }
    }
}