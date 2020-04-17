using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedDissolve_Example
{
    public class UIEventReciever_ExampleScene_Mask_Cone : MonoBehaviour
    {
        //Updates all 'Plane' mask related parameters
        Controller_Mask_Cone maskController;

        

        // Use this for initialization
        void Start()
        {
            maskController = GetComponent<Controller_Mask_Cone>();

            maskController.spotLight2.gameObject.SetActive(false);
            maskController.spotLight3.gameObject.SetActive(false);
            maskController.spotLight4.gameObject.SetActive(false);

            UI_Count(0);
            UI_Invert(true);
        }

        void Update()
        {
            maskController.spotLight1.spotAngle += Input.mouseScrollDelta.y * 0.5f;

            maskController.spotLight1.spotAngle = Mathf.Clamp(maskController.spotLight1.spotAngle, 1, 90);
        }

        public void UI_Count(int value)
        {
            //UI dropdown displays "1", "2", "3", "4" are just item names.
            //value - is dropdown index starting from 0 (zero)
            value += 1;


            maskController.UpdateMaskCountKeyword(value);

            maskController.spotLight2.gameObject.SetActive(value > 1);
            maskController.spotLight3.gameObject.SetActive(value > 2);
            maskController.spotLight4.gameObject.SetActive(value > 3);
        }

        public void UI_Invert(bool value)
        {
            maskController.invert = value;
        }
    }
}