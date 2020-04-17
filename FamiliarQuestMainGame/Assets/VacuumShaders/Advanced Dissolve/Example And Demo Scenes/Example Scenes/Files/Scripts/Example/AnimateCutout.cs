using UnityEngine;

namespace AdvancedDissolve_Example
{
    public class AnimateCutout : MonoBehaviour
    {
        public bool updateGI;


        Renderer mRenderer;
        Material material;

        float offset;
        float speed;
        

        private void Start()
        {
            mRenderer = GetComponent<Renderer>();

            material = mRenderer.material;

            offset = Random.value;
            speed = Random.Range(0.1f, 0.2f);
        }

        // Update is called once per frame
        void Update()
        {
            //Animating cutout
            material.SetFloat("_DissolveCutoff", Mathf.PingPong(offset + Time.time * speed, 1));


            if (updateGI)
            {
                //Dynamic GI uses META pass, which has no info about mesh position in the world.
                //We have to provive mesh world position data manually 
                material.SetVector("_Dissolve_ObjectWorldPos", transform.position);


                //We need to update Unity GI every time we change material properties effecting GI
                RendererExtensions.UpdateGIMaterials(mRenderer);
            }
        }
    }
}