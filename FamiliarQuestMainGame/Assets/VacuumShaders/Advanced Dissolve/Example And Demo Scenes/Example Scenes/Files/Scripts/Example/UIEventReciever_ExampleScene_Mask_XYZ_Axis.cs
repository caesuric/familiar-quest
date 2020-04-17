using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedDissolve_Example
{
    public class UIEventReciever_ExampleScene_Mask_XYZ_Axis : MonoBehaviour
    {
        //Updates all 'XYZ Axis' mask related parameters
        Controller_Mask_XYZ_Axis maskController;


        //X - 0, Y - 1, Z - 2
        int axis;


        // Use this for initialization
        void Start()
        {
            maskController = GetComponent<Controller_Mask_XYZ_Axis>();

            UI_Axis(1);
            UI_Offset(0.5f);
            UI_Invert(false);
        }


        public void UI_Axis(int value)
        {
            axis = value;

            maskController.axis = (Controller_Mask_XYZ_Axis.AXIS)axis;
        }

        public void UI_Offset(float value)
        {
            maskController.offset = RemapSliderValue(value);
        }

        public void UI_Invert(bool value)
        {
            maskController.invert = value;
        }
        

        //UI slider value changes from 0 to 1
        //We adjust those value to make cutout effect more precision depending on XYZ Axis
        float RemapSliderValue(float value)
        {
            switch (axis)
            {
                case 0: //X
                    return Mathf.Lerp(-7f, 7.4f, value);

                case 1: //Y
                    return Mathf.Lerp(-1f, 4f, value);

                case 2: //Z
                    return Mathf.Lerp(-7.4f, 8f, value);

                default:
                    return 0.5f;
            }

        }
    }
}