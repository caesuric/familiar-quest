using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedDissolve_Example
{
    public class ObjectSceneMove : MonoBehaviour
    {
        public bool rotate;
        public bool scale;

        public LayerMask layerMask;
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, layerMask.value))
                {
                    transform.position = new Vector3(hit.point.x, 1, hit.point.z); //Just a little bit height than hit point
                }
            }

            if (rotate)
            {
                if (Input.GetKey(KeyCode.Q)) RotateUp(true);
                if (Input.GetKey(KeyCode.A)) RotateUp(false);
                if (Input.GetKey(KeyCode.W)) RotateRight(true);
                if (Input.GetKey(KeyCode.S)) RotateRight(false);
                if (Input.GetKey(KeyCode.E)) RotateForward(true);
                if (Input.GetKey(KeyCode.D)) RotateForward(false);
            }

            if(scale)
            {
                float minScale = transform.localScale.x;
                minScale += Input.mouseScrollDelta.y * 0.2f;

                if (minScale < 0.1f)
                    minScale = 0.1f;

                transform.localScale = Vector3.one * minScale;
            }
        }

        protected void RotateUp(bool positive)
        {
            float step = Time.deltaTime;
            float fOrbitCircumfrance = 2F * Mathf.PI;
            float fDistanceRadians = (1f / fOrbitCircumfrance) * 2 * Mathf.PI;
            if (positive)
            {
                transform.RotateAround(transform.position, Vector3.up, -fDistanceRadians);
            }
            else
                transform.RotateAround(transform.position, Vector3.up, fDistanceRadians);
        }

        protected void RotateRight(bool positive)
        {
            float step = Time.deltaTime;
            float fOrbitCircumfrance = 2F * Mathf.PI;
            float fDistanceRadians = (1f / fOrbitCircumfrance) * 2 * Mathf.PI;
            if (positive)
            {
                transform.RotateAround(transform.position, Vector3.right, -fDistanceRadians);
            }
            else
                transform.RotateAround(transform.position, Vector3.right, fDistanceRadians);
        }

        protected void RotateForward(bool positive)
        {
            float step = Time.deltaTime;
            float fOrbitCircumfrance = 2F * Mathf.PI;
            float fDistanceRadians = (1f / fOrbitCircumfrance) * 2 * Mathf.PI;
            if (positive)
            {
                transform.RotateAround(transform.position, Vector3.forward, -fDistanceRadians);
            }
            else
                transform.RotateAround(transform.position, Vector3.forward, fDistanceRadians);
        }
    }
}