using UnityEngine;
using UnityEngine.UI;

public class CharacterItemUpdater : MonoBehaviour {

    public Text nameText;
    public Text levelText;
    public string characterName;
    public int level;
    public int furType;
    public CharacterSelectScreen characterSelectScreen;
    public GameObject selectionFrame;

    public void Initialize(string name, int level, int furType, CharacterSelectScreen characterSelectScreen) {
        this.level = level;
        this.characterSelectScreen = characterSelectScreen;
        this.furType = furType;
        nameText.text = characterName = name;
        levelText.text = "L" + level.ToString();
    }

    public void Click() {
        characterSelectScreen.ChooseCharacter(characterName, furType);
    }
}
