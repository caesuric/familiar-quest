using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClassSelectMenu : MonoBehaviour {

    public GameObject descriptionPanel;
    public Text strengthText;
    public Text dexterityText;
    public Text constitutionText;
    public Text intelligenceText;
    public Text wisdomText;
    public Text luckText;
    private Text text;
    public static int strength = 10;
    public static int dexterity = 10;
    public static int constitution = 10;
    public static int intelligence = 10;
    public static int wisdom = 10;
    public static int luck = 10;
    public static int sparePoints = 6;
    public static Weapon weapon = new MeleeWeapon();
    public static Spirit spirit;
    public static Dictionary<string, string> classDescriptions = new Dictionary<string, string>();
    public static Dictionary<string, int[]> classStats = new Dictionary<string, int[]>();
    public static Dictionary<string, Weapon> classWeapon = new Dictionary<string, Weapon>();
    public static string selectedClass = "";

    private static void AddWeapon(string[] item) {
        switch (item[1])
        {
            case "sword":
                classWeapon.Add(item[0], new MeleeWeapon());
                break;
            case "bow":
                classWeapon.Add(item[0], new RangedWeapon());
                break;
            case "wand":
                classWeapon.Add(item[0], RangedWeapon.Wand());
                break;
        }
    }

    // Use this for initialization
    void Start () {
        spirit = new Spirit(1);
        spirit = Spirit.classDefaults["thief"];
        classDescriptions.Clear();
        classStats.Clear();
        classWeapon.Clear();
        var items = TextReader.ReadSets("ClassDescriptions");
        foreach (var item in items) classDescriptions.Add(item[0], item[1] + "\n\n" + item[2]);
        items = TextReader.ReadSets("ClassStats");
        foreach (var item in items) classStats.Add(item[0], new int[] { int.Parse(item[1]), int.Parse(item[2]), int.Parse(item[3]), int.Parse(item[4]), int.Parse(item[5]), int.Parse(item[6]) });
        items = TextReader.ReadSets("ClassWeapons");
        foreach (var item in items) AddWeapon(item);
        strength = 10;
        dexterity = 10;
        constitution = 10;
        intelligence = 10;
        wisdom = 10;
        luck = 10;
        sparePoints = 6;
        selectedClass = "";
        text = descriptionPanel.GetComponent<Text>();
    }

    public void MouseOverButton(string type) {
        SetGuiText(type);
        strength = classStats[type][0];
        dexterity = classStats[type][1];
        constitution = classStats[type][2];
        intelligence = classStats[type][3];
        wisdom = classStats[type][4];
        luck = classStats[type][5];
        weapon = classWeapon[type];
        spirit = Spirit.classDefaults[type];
        selectedClass = type;
    }

    private void SetGuiText(string type)
    {
        text.text = classDescriptions[type];
        strengthText.text = classStats[type][0].ToString();
        dexterityText.text = classStats[type][1].ToString();
        constitutionText.text = classStats[type][2].ToString();
        intelligenceText.text = classStats[type][3].ToString();
        wisdomText.text = classStats[type][4].ToString();
        luckText.text = classStats[type][5].ToString();
    }

    public void SelectClass() {
        PlayerPrefs.SetString("class", selectedClass);
        SceneManager.LoadScene("Stat Selection Screen");
    }
}
