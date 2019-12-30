using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreationMenuStatMouseover : MonoBehaviour {

    public List<Text> highlightElements;
    private Color highlightColor = Color.cyan;
    private Color baseColor;
	// Use this for initialization
	void Start () {
        baseColor = highlightElements[0].color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MouseEnter() {
        foreach (var text in highlightElements) text.color = highlightColor;
    }

    public void MouseExit() {
        foreach (var text in highlightElements) text.color = baseColor;
    }
}
