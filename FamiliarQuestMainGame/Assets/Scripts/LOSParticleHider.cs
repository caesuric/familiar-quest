using UnityEngine;
using LOS;

    /// <summary>
    /// Disables a gameobjects renderer if the object is outside the line of sight
    /// </summary>
    [RequireComponent(typeof(LOSCuller))]
    public class LOSParticleHider : MonoBehaviour
    {
        private LOSCuller m_Culler;
        private LOSVisibilityInfo m_VisibilityInfo;

        private void OnEnable()
        {
            m_Culler = GetComponent<LOSCuller>();

            enabled &= Util.Verify(m_Culler != null, "LOS culler component missing.");
            enabled &= Util.Verify(GetComponent<ParticleSystem>() != null, "No particle system attached to this GameObject! LOS Culler component must be added to a GameObject containing a particle system!");
        }

        private void Start()
        {
            m_VisibilityInfo = GetComponent<LOSVisibilityInfo>();

            // Disable LOSCuller script and use Visibilty Info instead if both are present
            if (m_VisibilityInfo != null && m_VisibilityInfo.isActiveAndEnabled)
            {
                m_Culler.enabled = false;
            }
        }

        private void LateUpdate()
        {
            if (m_Culler.enabled)
            {
                if (m_Culler.Visibile && !GetComponent<ParticleSystem>().isPlaying) GetComponent<ParticleSystem>().Play();
                else if (!m_Culler.Visibile && GetComponent<ParticleSystem>().isPlaying) GetComponent<ParticleSystem>().Stop();
            }
            else if (m_VisibilityInfo != null && m_VisibilityInfo.isActiveAndEnabled)
            {
                if (m_VisibilityInfo.Visibile && !GetComponent<ParticleSystem>().isPlaying) GetComponent<ParticleSystem>().Play();
                else if (!m_VisibilityInfo.Visibile && GetComponent<ParticleSystem>().isPlaying) GetComponent<ParticleSystem>().Stop();
            }
        }
    }
