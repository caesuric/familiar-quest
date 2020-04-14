using UnityEngine;
using LOS;

/// <summary>
/// Disables a gameobjects renderer if the object is outside the line of sight
/// </summary>
[RequireComponent(typeof(LOSCuller))]
public class LOSCanvasHider : MonoBehaviour {
    private LOSCuller m_Culler;
    private LOSVisibilityInfo m_VisibilityInfo;

    private void OnEnable() {
        m_Culler = GetComponent<LOSCuller>();

        enabled &= Util.Verify(m_Culler != null, "LOS culler component missing.");
        enabled &= Util.Verify(GetComponent<Canvas>() != null, "No canvas attached to this GameObject! LOS Culler component must be added to a GameObject containing a Canvas!");
    }

    private void Start() {
        m_VisibilityInfo = GetComponent<LOSVisibilityInfo>();

        // Disable LOSCuller script and use Visibilty Info instead if both are present
        if (m_VisibilityInfo != null && m_VisibilityInfo.isActiveAndEnabled) {
            m_Culler.enabled = false;
        }
    }

    private void LateUpdate() {
        if (m_Culler.enabled) {
            //GetComponent<Canvas>().enabled = m_Culler.Visibile;
            //var renderers = GetComponentsInChildren<MeshRenderer>();
            //foreach (var renderer in renderers) renderer.enabled = m_Culler.Visibile;
        }
        else if (m_VisibilityInfo != null && m_VisibilityInfo.isActiveAndEnabled) {
            //GetComponent<Canvas>().enabled = m_VisibilityInfo.Visibile;
            //var renderers = GetComponentsInChildren<MeshRenderer>();
            //foreach (var renderer in renderers) renderer.enabled = m_VisibilityInfo.Visibile;
        }
    }
}