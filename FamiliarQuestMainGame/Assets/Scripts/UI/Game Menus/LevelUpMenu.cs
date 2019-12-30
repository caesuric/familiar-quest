using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

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
    private Character attr = null;
    private Dictionary<string, string> statDescriptions = new Dictionary<string, string>();

    // Use this for initialization
    void Start () {
        var data = TextReader.ReadSets("StatDescriptions");
        foreach (var item in data) statDescriptions.Add(item[0], item[1]);
    }

    public void Initialize(Character character)
    {
        attr = character;
        sparePoints = attr.GetComponent<ExperienceGainer>().sparePoints;
        strength = strengthMin = attr.strength;
        dexterity = dexterityMin = attr.dexterity;
        constitution = constitutionMin = attr.constitution;
        intelligence = intelligenceMin = attr.intelligence;
        wisdom = wisdomMin = attr.wisdom;
        luck = luckMin = attr.luck;
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
        switch(type) {
            case "strength":
                if (strength>strengthMin) {
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
        if (sparePoints==0) {
            attr.CmdSetStats(strength, dexterity, constitution, intelligence, wisdom, luck);
            gameObject.SetActive(false);
        }
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }
}
