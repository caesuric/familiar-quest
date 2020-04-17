using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AdvancedDissolve_Example
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(LineRenderer), typeof(Light))]
    public class SpotLight_LineRenderer : MonoBehaviour
    {
        Light spotLight;
        LineRenderer lineRenderer;

        // Use this for initialization
        void Start()
        {
            spotLight = GetComponent<Light>();
            spotLight.type = LightType.Spot;


            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.alignment = LineAlignment.View;
        }

        // Update is called once per frame
        void Update()
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * spotLight.range);
            lineRenderer.startWidth = 0;
            lineRenderer.endWidth = Mathf.Tan(spotLight.spotAngle * Mathf.Deg2Rad * 0.5f) * spotLight.range * 2;
        }
    }
}