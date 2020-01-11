using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StatSelector : MonoBehaviour {

    public Text strengthText;
    public Text dexterityText;
    public Text constitutionText;
    public Text intelligenceText;
    public Text wisdomText;
    public Text luckText;
    public Text spareText;
    public Text descriptionText;
    public Text classField;
    public int strengthMin;
    public int dexterityMin;
    public int constitutionMin;
    public int intelligenceMin;
    public int wisdomMin;
    public int luckMin;
    public InputField nameField;
    public GameObject appearanceSelector;
    private Dictionary<string, string> statDescriptions = new Dictionary<string, string>();

    // Use this for initialization
    void Start() {
        var data = TextReader.ReadSets("StatDescriptions");
        foreach (var item in data) statDescriptions.Add(item[0], item[1]);
        strengthMin = ClassSelectMenu.strength;
        dexterityMin = ClassSelectMenu.dexterity;
        constitutionMin = ClassSelectMenu.constitution;
        intelligenceMin = ClassSelectMenu.intelligence;
        wisdomMin = ClassSelectMenu.wisdom;
        luckMin = ClassSelectMenu.luck;
        if (ClassSelectMenu.selectedClass == "infernoMage") classField.text = "Level 1 Inferno Mage";
        else classField.text = "Level 1 " + ClassSelectMenu.selectedClass.Substring(0, 1).ToUpper() + ClassSelectMenu.selectedClass.Substring(1);
        UpdateBoxes();
    }

    private void UpdateBoxes() {
        strengthText.text = ClassSelectMenu.strength.ToString();
        dexterityText.text = ClassSelectMenu.dexterity.ToString();
        constitutionText.text = ClassSelectMenu.constitution.ToString();
        intelligenceText.text = ClassSelectMenu.intelligence.ToString();
        wisdomText.text = ClassSelectMenu.wisdom.ToString();
        luckText.text = ClassSelectMenu.luck.ToString();
        spareText.text = ClassSelectMenu.sparePoints.ToString();
    }

    public void Plus(string type) {
        switch (type) {
            case "strength":
                if (ClassSelectMenu.sparePoints > 0) {
                    ClassSelectMenu.strength += 1;
                    ClassSelectMenu.sparePoints -= 1;
                }
                break;
            case "dexterity":
                if (ClassSelectMenu.sparePoints > 0) {
                    ClassSelectMenu.dexterity += 1;
                    ClassSelectMenu.sparePoints -= 1;
                }
                break;
            case "constitution":
                if (ClassSelectMenu.sparePoints > 0) {
                    ClassSelectMenu.constitution += 1;
                    ClassSelectMenu.sparePoints -= 1;
                }
                break;
            case "intelligence":
                if (ClassSelectMenu.sparePoints > 0) {
                    ClassSelectMenu.intelligence += 1;
                    ClassSelectMenu.sparePoints -= 1;
                }
                break;
            case "wisdom":
                if (ClassSelectMenu.sparePoints > 0) {
                    ClassSelectMenu.wisdom += 1;
                    ClassSelectMenu.sparePoints -= 1;
                }
                break;
            case "luck":
                if (ClassSelectMenu.sparePoints > 0) {
                    ClassSelectMenu.luck += 1;
                    ClassSelectMenu.sparePoints -= 1;
                }
                break;
        }
        UpdateBoxes();
    }

    public void Minus(string type) {
        switch (type) {
            case "strength":
                if (ClassSelectMenu.strength > strengthMin) {
                    ClassSelectMenu.strength -= 1;
                    ClassSelectMenu.sparePoints += 1;
                }
                break;
            case "dexterity":
                if (ClassSelectMenu.dexterity > dexterityMin) {
                    ClassSelectMenu.dexterity -= 1;
                    ClassSelectMenu.sparePoints += 1;
                }
                break;
            case "constitution":
                if (ClassSelectMenu.constitution > constitutionMin) {
                    ClassSelectMenu.constitution -= 1;
                    ClassSelectMenu.sparePoints += 1;
                }
                break;
            case "intelligence":
                if (ClassSelectMenu.intelligence > intelligenceMin) {
                    ClassSelectMenu.intelligence -= 1;
                    ClassSelectMenu.sparePoints += 1;
                }
                break;
            case "wisdom":
                if (ClassSelectMenu.wisdom > wisdomMin) {
                    ClassSelectMenu.wisdom -= 1;
                    ClassSelectMenu.sparePoints += 1;
                }
                break;
            case "luck":
                if (ClassSelectMenu.luck > luckMin) {
                    ClassSelectMenu.luck -= 1;
                    ClassSelectMenu.sparePoints += 1;
                }
                break;
        }
        UpdateBoxes();
    }

    public void MouseOverStat(string type) {
        descriptionText.text = statDescriptions[type];
    }

    public void Confirm() {
        if (ClassSelectMenu.sparePoints == 0 && nameField.text != "" && nameField.text != null) {
            appearanceSelector.SetActive(true);
            appearanceSelector.GetComponent<AppearanceSelector>().characterName = nameField.text;
        }
    }

    public void Cancel() {
        SceneManager.LoadScene("Character Selection");
    }
}
