using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearanceSelectorButton : MonoBehaviour {

    public int number;
    public CharacterSelectScreen characterSelectScreen;
    public GameObject selectionFrame;

    public void Click() {
        characterSelectScreen.ClickAppearanceButton(this);
    }
}

