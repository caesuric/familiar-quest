using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrackerToggle : MonoBehaviour {

    public GameObject content;
    public RectTransform questTrackerRect;
    public DuloGames.UI.UIFlippable arrow;
    public DuloGames.UI.UIFlippable arrow2;
    public UnityEngine.UI.Toggle toggle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        content.SetActive(toggle.isOn);
        arrow.vertical = !toggle.isOn;
        arrow2.vertical = !toggle.isOn;
        questTrackerRect.sizeDelta = toggle.isOn ? new Vector2(400, 300) : new Vector2(400, 50);
    }
}
