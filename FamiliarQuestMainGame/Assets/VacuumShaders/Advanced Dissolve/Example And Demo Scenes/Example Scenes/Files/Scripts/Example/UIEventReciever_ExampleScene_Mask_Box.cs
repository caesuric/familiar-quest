using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedDissolve_Example
{
    public class UIEventReciever_ExampleScene_Mask_Box : MonoBehaviour
    {
        //Updates all 'Plane' mask related parameters
        Controller_Mask_Box maskController;

        

        // Use this for initialization
        void Start()
        {
            maskController = GetComponent<Controller_Mask_Box>();

            maskController.box2.SetActive(false);
            maskController.box3.SetActive(false);
            maskController.box4.SetActive(false);

            UI_Count(0);
            UI_Invert(false);
        }


        public void UI_Count(int value)
        {
            //UI dropdown displays "1", "2", "3", "4" are just item names.
            //value - is dropdown index starting from 0 (zero)
            value += 1;


            maskController.UpdateMaskCountKeyword(value);

            maskController.box2.SetActive(value > 1);
            maskController.box3.SetActive(value > 2);
            maskController.box4.SetActive(value > 3);
        }

        public void UI_Invert(bool value)
        {
            maskController.invert = value;
        }
    }
}