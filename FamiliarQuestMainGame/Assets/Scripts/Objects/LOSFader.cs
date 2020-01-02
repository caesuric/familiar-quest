using System.Collections.Generic;
using UnityEngine;

public class LOSFader : MonoBehaviour {

    private readonly float fadeTime = 1f;
    private float timer = 0f;
    private List<Color> alphaColors = new List<Color>();
    private List<Color> originalColors = new List<Color>();
    private List<Renderer> renderers = new List<Renderer>();

    // Use this for initialization
    void Start() {
        foreach (var renderer in GetComponentsInChildren<Renderer>()) if (renderer.material.HasProperty("_Color")) renderers.Add(renderer);
        foreach (var renderer in renderers) originalColors.Add(renderer.material.color);
        foreach (var renderer in renderers) renderer.material.color = new Color(0, 0, 0, 0);
        for (int i = 0; i < originalColors.Count; i++) {
            //var alphaColor = new Color(originalColors[i].r, originalColors[i].g, originalColors[i].b, 0);
            var alphaColor = new Color(0, 0, 0, 0);
            alphaColors.Add(alphaColor);
        }
    }

    // Update is called once per frame
    void Update() {
        var enabled = RenderersEnabled();
        if (timer < fadeTime && enabled) timer += Time.deltaTime;
        else if (timer > 0 && !enabled) timer -= Time.deltaTime;
        if (timer < fadeTime && timer > 0) {
            for (int i = 0; i < renderers.Count; i++) {
                var renderer = renderers[i];
                var alphaColor = alphaColors[i];
                timer += Time.deltaTime;
                renderer.material.color = Color.Lerp(alphaColors[i], originalColors[i], timer / fadeTime);
            }
        }
    }

    private bool RenderersEnabled() {
        foreach (var renderer in renderers) if (renderer.enabled) return true;
        return false;
    }
}
