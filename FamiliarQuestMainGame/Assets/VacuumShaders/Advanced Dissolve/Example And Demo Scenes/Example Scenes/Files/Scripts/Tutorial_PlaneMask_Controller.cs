using UnityEngine;

namespace AdvancedDissolve_Example
{
    [ExecuteInEditMode]
    public class Tutorial_PlaneMask_Controller : MonoBehaviour
    {
        public GameObject maskObject;
        public Material targetMaterial;


        void Update()
        {
            if (maskObject != null && targetMaterial != null)
            {
                targetMaterial.SetVector("_DissolveMaskPosition", maskObject.transform.position);
                targetMaterial.SetVector("_DissolveMaskNormal", maskObject.transform.up);
            }
        }
    }
}