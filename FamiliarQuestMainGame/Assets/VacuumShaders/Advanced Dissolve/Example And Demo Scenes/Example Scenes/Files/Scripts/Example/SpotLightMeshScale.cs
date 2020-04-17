using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AdvancedDissolve_Example
{
    [ExecuteInEditMode]
    public class SpotLightMeshScale : MonoBehaviour
    {
        public Light spotLight;

        public float spotAngleMin = 4, spotAngleMax = 40;
        public float meshScaleMin = 1, meshScaleMax = 6;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (spotLight != null)
            {
                Vector3 scale = transform.localScale;

                float xy = Remap(spotLight.spotAngle, spotAngleMin, spotAngleMax, meshScaleMin, meshScaleMax);

                transform.localScale = new Vector3(xy, xy, scale.z);
            }

        }

        public float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
        {
            var fromAbs = from - fromMin;
            var fromMaxAbs = fromMax - fromMin;

            var normal = fromAbs / fromMaxAbs;

            var toMaxAbs = toMax - toMin;
            var toAbs = toMaxAbs * normal;

            var to = toAbs + toMin;

            return to;
        }

    }
}