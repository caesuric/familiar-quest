using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedDissolve_Example
{
    public class UIEventReciever_ExampleScene_Mask_Plane : MonoBehaviour
    {
        //Updates all 'Plane' mask related parameters
        Controller_Mask_Plane maskController;

        

        // Use this for initialization
        void Start()
        {
            maskController = GetComponent<Controller_Mask_Plane>();
           
            UI_Count(1);
            UI_Invert(false);
        }


        public void UI_Count(int value)
        {
            maskController.UpdateMaskCountKeyword(value);
        }

        public void UI_Invert(bool value)
        {
            maskController.invert = value;
        }
    }
}