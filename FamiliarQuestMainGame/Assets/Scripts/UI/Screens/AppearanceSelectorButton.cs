using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearanceSelectorButton : MonoBehaviour {

    public int number;
    public CharacterSelectScreen characterSelectScreen;
    public GameObject selectionFrame;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Click() {
        characterSelectScreen.ClickAppearanceButton(this);
    }
}

