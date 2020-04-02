using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAttributesUpdater : MonoBehaviour {

    private Character character;
    private ExperienceGainer eg;
    public Text strength;
    public Text dexterity;
    public Text constitution;
    public Text intelligence;
    public Text wisdom;
    public Text luck;
    public Text sparePointsText;
    public GameObject strengthDownButton;
    public GameObject strengthUpButton;
    public GameObject dexterityDownButton;
    public GameObject dexterityUpButton;
    public GameObject constitutionDownButton;
    public GameObject constitutionUpButton;
    public GameObject intelligenceDownButton;
    public GameObject intelligenceUpButton;
    public GameObject wisdomDownButton;
    public GameObject wisdomUpButton;
    public GameObject luckDownButton;
    public GameObject luckUpButton;
    private int currentStrength, minStrength;
    private int currentDexterity, minDexterity;
    private int currentConstitution, minConstitution;
    private int currentIntelligence, minIntelligence;
    private int currentWisdom, minWisdom;
    private int currentLuck, minLuck;
    private int sparePoints;
    public List<GameObject> sparePointsInterface = new List<GameObject>();
    private bool levelUpModeActive = false;

    // Use this for initialization
    void Start() {
        character = PlayerCharacter.players[0].GetComponent<Character>();
        eg = character.GetComponent<ExperienceGainer>();
    }

    // Update is called once per frame
    void Update() {
        if (eg.sparePoints == 0) {
            //strength.text = character.strength.ToString();
            //dexterity.text = character.dexterity.ToString();
            //constitution.text = character.constitution.ToString();
            //intelligence.text = character.intelligence.ToString();
            //wisdom.text = character.wisdom.ToString();
            //luck.text = character.luck.ToString();
            strength.text = CharacterAttribute.attributes["strength"].instances[character].TotalValue.ToString();
            dexterity.text = CharacterAttribute.attributes["dexterity"].instances[character].TotalValue.ToString();
            constitution.text = CharacterAttribute.attributes["constitution"].instances[character].TotalValue.ToString();
            intelligence.text = CharacterAttribute.attributes["intelligence"].instances[character].TotalValue.ToString();
            wisdom.text = CharacterAttribute.attributes["wisdom"].instances[character].TotalValue.ToString();
            luck.text = CharacterAttribute.attributes["luck"].instances[character].TotalValue.ToString();
            strengthDownButton.GetComponent<Image>().enabled = false;
            strengthUpButton.GetComponent<Image>().enabled = false;
            dexterityDownButton.GetComponent<Image>().enabled = false;
            dexterityUpButton.GetComponent<Image>().enabled = false;
            constitutionDownButton.GetComponent<Image>().enabled = false;
            constitutionUpButton.GetComponent<Image>().enabled = false;
            intelligenceDownButton.GetComponent<Image>().enabled = false;
            intelligenceUpButton.GetComponent<Image>().enabled = false;
            wisdomDownButton.GetComponent<Image>().enabled = false;
            wisdomUpButton.GetComponent<Image>().enabled = false;
            luckDownButton.GetComponent<Image>().enabled = false;
            luckUpButton.GetComponent<Image>().enabled = false;
        }
        else if (!levelUpModeActive) {
            levelUpModeActive = true;
            //currentStrength = minStrength = character.strength;
            //currentDexterity = minDexterity = character.dexterity;
            //currentConstitution = minConstitution = character.constitution;
            //currentIntelligence = minIntelligence = character.intelligence;
            //currentWisdom = minWisdom = character.wisdom;
            //currentLuck = minLuck = character.luck;
            currentStrength = minStrength = (int)CharacterAttribute.attributes["strength"].instances[character].BaseValue;
            currentDexterity = minDexterity = (int)CharacterAttribute.attributes["dexterity"].instances[character].BaseValue;
            currentConstitution = minConstitution = (int)CharacterAttribute.attributes["constitution"].instances[character].BaseValue;
            currentIntelligence = minIntelligence = (int)CharacterAttribute.attributes["intelligence"].instances[character].BaseValue;
            currentWisdom = minWisdom = (int)CharacterAttribute.attributes["wisdom"].instances[character].BaseValue;
            currentLuck = minLuck = (int)CharacterAttribute.attributes["luck"].instances[character].BaseValue;
            sparePoints = eg.sparePoints;
        }
        else {
            strength.text = currentStrength.ToString();
            dexterity.text = currentDexterity.ToString();
            constitution.text = currentConstitution.ToString();
            intelligence.text = currentIntelligence.ToString();
            wisdom.text = currentWisdom.ToString();
            luck.text = currentLuck.ToString();
            sparePointsText.text = sparePoints.ToString();
            strengthDownButton.GetComponent<Image>().enabled = (currentStrength > minStrength);
            strengthUpButton.GetComponent<Image>().enabled = (sparePoints > 0);
            dexterityDownButton.GetComponent<Image>().enabled = (currentDexterity > minDexterity);
            dexterityUpButton.GetComponent<Image>().enabled = (sparePoints > 0);
            constitutionDownButton.GetComponent<Image>().enabled = (currentConstitution > minConstitution);
            constitutionUpButton.GetComponent<Image>().enabled = (sparePoints > 0);
            intelligenceDownButton.GetComponent<Image>().enabled = (currentIntelligence > minIntelligence);
            intelligenceUpButton.GetComponent<Image>().enabled = (sparePoints > 0);
            wisdomDownButton.GetComponent<Image>().enabled = (currentWisdom > minWisdom);
            wisdomUpButton.GetComponent<Image>().enabled = (sparePoints > 0);
            luckDownButton.GetComponent<Image>().enabled = (currentLuck > minLuck);
            luckUpButton.GetComponent<Image>().enabled = (sparePoints > 0);
        }
        foreach (var item in sparePointsInterface) item.SetActive(eg.sparePoints > 0);
    }

    public void StrengthDown() {
        if (currentStrength > minStrength) {
            currentStrength -= 1;
            sparePoints += 1;
        }
    }

    public void StrengthUp() {
        if (sparePoints > 0) {
            sparePoints -= 1;
            currentStrength += 1;
        }
    }

    public void DexterityDown() {
        if (currentDexterity > minDexterity) {
            currentDexterity -= 1;
            sparePoints += 1;
        }
    }

    public void DexterityUp() {
        if (sparePoints > 0) {
            sparePoints -= 1;
            currentDexterity += 1;
        }
    }

    public void ConstitutionDown() {
        if (currentConstitution > minConstitution) {
            currentConstitution -= 1;
            sparePoints += 1;
        }
    }

    public void ConstitutionUp() {
        if (sparePoints > 0) {
            sparePoints -= 1;
            currentConstitution += 1;
        }
    }

    public void IntelligenceDown() {
        if (currentIntelligence > minIntelligence) {
            currentIntelligence -= 1;
            sparePoints += 1;
        }
    }

    public void IntelligenceUp() {
        if (sparePoints > 0) {
            sparePoints -= 1;
            currentIntelligence += 1;
        }
    }

    public void WisdomDown() {
        if (currentWisdom > minWisdom) {
            currentWisdom -= 1;
            sparePoints += 1;
        }
    }

    public void WisdomUp() {
        if (sparePoints > 0) {
            sparePoints -= 1;
            currentWisdom += 1;
        }
    }

    public void LuckDown() {
        if (currentLuck > minLuck) {
            currentLuck -= 1;
            sparePoints += 1;
        }
    }

    public void LuckUp() {
        if (sparePoints > 0) {
            sparePoints -= 1;
            currentLuck += 1;
        }
    }

    public void ConfirmPoints() {
        eg.sparePoints = sparePoints;
        character.CmdSetStats(currentStrength, currentDexterity, currentConstitution, currentIntelligence, currentWisdom, currentLuck);
        levelUpModeActive = false;
    }
}