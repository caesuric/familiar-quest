using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBar : MonoBehaviour {

    public GameObject characterSheet;
    public GameObject inventory;
    public GameObject abilityScreen;
    public GameObject questLog;
    public GameObject gameLog;
    public GameObject bestiary;
    public GameObject settings;
    public GameObject canvas;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleCharacterSheet() {
        //characterSheet.SetActive(!characterSheet.activeSelf);
        SharedInventory.instance.CmdRefresh();
        canvas.GetComponent<Inventory>().Refresh();
        characterSheet.GetComponent<DuloGames.UI.UIWindow>().Toggle();
    }

    public void ToggleInventory() {
        SharedInventory.instance.CmdRefresh();
        canvas.GetComponent<Inventory>().Refresh();
        //inventory.SetActive(!inventory.activeSelf);
        inventory.GetComponent<DuloGames.UI.UIWindow>().Toggle();
    }

    public void ToggleAbilityScreen() {
        abilityScreen.SetActive(!abilityScreen.activeSelf);
    }

    public void ToggleQuestLog() {
        questLog.SetActive(!questLog.activeSelf);
    }

    public void ToggleGameLog() {
        gameLog.SetActive(!gameLog.activeSelf);
    }

    public void ToggleBestiary() {
        bestiary.SetActive(!bestiary.activeSelf);
    }

    public void ToggleSettings() {
        settings.SetActive(!settings.activeSelf);
    }
}
