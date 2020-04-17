using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedDissolve_Example
{
    [ExecuteInEditMode]
    public class UpdateWorldPos : MonoBehaviour
    {

        Material material;
        Renderer mRenderer;
        // Use this for initialization
        void Start()
        {
            mRenderer = GetComponent<Renderer>();
            material = mRenderer.sharedMaterial;
        }

        // Update is called once per frame
        void Update()
        {
            //Dynamic GI uses META pass, which has no info about mesh position in the world.
            //We have to provive mesh world position data manually 
            material.SetVector("_Dissolve_ObjectWorldPos", transform.position);

            //We need to update Unity GI every time we change material properties effecting GI
            RendererExtensions.UpdateGIMaterials(mRenderer);
        }
    }
}