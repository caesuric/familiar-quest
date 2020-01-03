using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FPSUpdater : MonoBehaviour {

    public Text text;
    public int framesToCount = 300;
    private List<float> counts = new List<float>();

    // Update is called once per frame
    void Update() {
        counts.Add(Mathf.Floor(1 / Time.deltaTime));
        if (counts.Count > framesToCount) counts.RemoveAt(0);
        text.text = AverageCount().ToString() + " FPS";
    }

    private int AverageCount() {
        var total = 0f;
        foreach (var item in counts) total += item;
        total /= counts.Count;
        return (int)Mathf.Floor(total);
    }
}