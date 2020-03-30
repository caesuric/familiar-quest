using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterItemUpdater : MonoBehaviour {

    public Text nameText;
    public Text levelText;
    public string characterName;
    public int level;
    public int furType;
    public CharacterSelectScreen characterSelectScreen;
    public GameObject selectionFrame;
    private float timer = 0f;
    private int clickCount = 0;

    public void Initialize(string name, int level, int furType, CharacterSelectScreen characterSelectScreen) {
        this.level = level;
        this.characterSelectScreen = characterSelectScreen;
        this.furType = furType;
        nameText.text = characterName = name;
        levelText.text = "L" + level.ToString();
    }

    void Update() {
        if (clickCount > 0) timer += Time.deltaTime;
        if (timer >= 0.5f) {
            timer = 0f;
            clickCount = 0;
        }
    }

    public void Click() {
        clickCount++;
        characterSelectScreen.ChooseCharacter(characterName, furType);
        if (clickCount == 2) characterSelectScreen.PressPlay();
    }
}
