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

    public void ToggleCharacterSheet() {
        canvas.GetComponent<Inventory>().Refresh();
        characterSheet.GetComponent<DuloGames.UI.UIWindow>().Toggle();
    }

    public void ToggleInventory() {
        canvas.GetComponent<Inventory>().Refresh();
        inventory.GetComponent<DuloGames.UI.UIWindow>().Toggle();
        DropsArea.ClearDrops();
    }

    public void ToggleAbilityScreen() {
        abilityScreen.GetComponent<DuloGames.UI.UIWindow>().Toggle();
        DropsArea.ClearDrops();
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