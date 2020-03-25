using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpMenu : MonoBehaviour {

    public Text strengthText;
    public Text dexterityText;
    public Text constitutionText;
    public Text intelligenceText;
    public Text wisdomText;
    public Text luckText;
    public Text spareText;
    public Text descriptionText;
    public int strengthMin;
    public int dexterityMin;
    public int constitutionMin;
    public int intelligenceMin;
    public int wisdomMin;
    public int luckMin;
    public int sparePoints;
    public int strength;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int wisdom;
    public int luck;
    private Character character = null;
    private Dictionary<string, string> statDescriptions = new Dictionary<string, string>();

    // Use this for initialization
    void Start() {
        var data = TextReader.ReadSets("StatDescriptions");
        foreach (var item in data) statDescriptions.Add(item[0], item[1]);
    }

    public void Initialize(Character character) {
        this.character = character;
        sparePoints = this.character.GetComponent<ExperienceGainer>().sparePoints;
        //strength = strengthMin = this.character.strength;
        //dexterity = dexterityMin = this.character.dexterity;
        //constitution = constitutionMin = this.character.constitution;
        //intelligence = intelligenceMin = this.character.intelligence;
        //wisdom = wisdomMin = this.character.wisdom;
        //luck = luckMin = this.character.luck;
        strength = strengthMin = (int)CharacterAttribute.attributes["strength"].instances[character].BaseValue;
        dexterity = dexterityMin = (int)CharacterAttribute.attributes["dexterity"].instances[character].BaseValue;
        constitution = constitutionMin = (int)CharacterAttribute.attributes["constitution"].instances[character].BaseValue;
        intelligence = intelligenceMin = (int)CharacterAttribute.attributes["intelligence"].instances[character].BaseValue;
        wisdom = wisdomMin = (int)CharacterAttribute.attributes["wisdom"].instances[character].BaseValue;
        luck = luckMin = (int)CharacterAttribute.attributes["luck"].instances[character].BaseValue;
        UpdateBoxes();
    }

    private void UpdateBoxes() {
        strengthText.text = strength.ToString();
        dexterityText.text = dexterity.ToString();
        constitutionText.text = constitution.ToString();
        intelligenceText.text = intelligence.ToString();
        wisdomText.text = wisdom.ToString();
        luckText.text = luck.ToString();
        spareText.text = sparePoints.ToString();
    }

    public void Plus(string type) {
        switch (type) {
            case "strength":
                if (sparePoints > 0) {
                    strength += 1;
                    sparePoints -= 1;
                }
                break;
            case "dexterity":
                if (sparePoints > 0) {
                    dexterity += 1;
                    sparePoints -= 1;
                }
                break;
            case "constitution":
                if (sparePoints > 0) {
                    constitution += 1;
                    sparePoints -= 1;
                }
                break;
            case "intelligence":
                if (sparePoints > 0) {
                    intelligence += 1;
                    sparePoints -= 1;
                }
                break;
            case "wisdom":
                if (sparePoints > 0) {
                    wisdom += 1;
                    sparePoints -= 1;
                }
                break;
            case "luck":
                if (sparePoints > 0) {
                    luck += 1;
                    sparePoints -= 1;
                }
                break;
        }
        UpdateBoxes();
    }

    public void Minus(string type) {
        switch (type) {
            case "strength":
                if (strength > strengthMin) {
                    strength -= 1;
                    sparePoints += 1;
                }
                break;
            case "dexterity":
                if (dexterity > dexterityMin) {
                    dexterity -= 1;
                    sparePoints += 1;
                }
                break;
            case "constitution":
                if (constitution > constitutionMin) {
                    constitution -= 1;
                    sparePoints += 1;
                }
                break;
            case "intelligence":
                if (intelligence > intelligenceMin) {
                    intelligence -= 1;
                    sparePoints += 1;
                }
                break;
            case "wisdom":
                if (wisdom > wisdomMin) {
                    wisdom -= 1;
                    sparePoints += 1;
                }
                break;
            case "luck":
                if (luck > luckMin) {
                    luck -= 1;
                    sparePoints += 1;
                }
                break;
        }
        UpdateBoxes();
    }

    public void MouseOverStat(string type) {
        descriptionText.text = statDescriptions[type];
    }

    public void Confirm() {
        if (sparePoints == 0) {
            character.CmdSetStats(strength, dexterity, constitution, intelligence, wisdom, luck);
            gameObject.SetActive(false);
        }
    }

    public void Cancel() {
        gameObject.SetActive(false);
    }
}