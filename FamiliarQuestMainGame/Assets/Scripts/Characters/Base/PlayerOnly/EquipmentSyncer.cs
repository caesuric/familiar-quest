using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EquipmentSyncer : MonoBehaviour {

    //public SyncListString names = new SyncListString();
    //public SyncListString descriptions = new SyncListString();
    //public SyncListString types = new SyncListString();
    //public SyncListInt qualities = new SyncListInt();
    //public SyncListString icons = new SyncListString();
    //public SyncListInt armor = new SyncListInt();
    //public SyncListInt strength = new SyncListInt();
    //public SyncListInt dexterity = new SyncListInt();
    //public SyncListInt constitution = new SyncListInt();
    //public SyncListInt intelligence = new SyncListInt();
    //public SyncListInt wisdom = new SyncListInt();
    //public SyncListInt luck = new SyncListInt();
    public List<string> names = new List<string>();
    public List<string> descriptions = new List<string>();
    public List<string> types = new List<string>();
    public List<int> qualities = new List<int>();
    public List<string> icons = new List<string>();
    public List<int> armor = new List<int>();
    public List<int> strength = new List<int>();
    public List<int> dexterity = new List<int>();
    public List<int> constitution = new List<int>();
    public List<int> intelligence = new List<int>();
    public List<int> wisdom = new List<int>();
    public List<int> luck = new List<int>();

    //[SyncVar]
    public float attackPower = 0;
    //[SyncVar]
    public string weaponSubtype = "";

    PlayerCharacter player = null;

    public static Dictionary<string, int> slotKeys = new Dictionary<string, int>() {
        {"weapon", 0},
        {"armor", 1},
        {"belt", 2},
        {"bracelet1", 3},
        {"bracelet2", 4},
        {"bracelet3", 5},
        {"bracelet4", 6},
        {"cloak", 7},
        {"earring", 8},
        {"hat", 9},
        {"necklace", 10},
        {"shoes", 11}
    };

    // Use this for initialization
    void Start () {
        player = GetComponent<PlayerCharacter>();
        if (player == null) return;
        //if (player == null || !NetworkServer.active) return;
        attackPower = player.weapon.attackPower;
        AddItem("weapon", player.weapon);
        AddItem("armor", player.armor);
        AddItem("belt", player.belt);
        AddBracelet(player.bracelets, 0);
        AddBracelet(player.bracelets, 1);
        AddBracelet(player.bracelets, 2);
        AddBracelet(player.bracelets, 3);
        AddItem("cloak", player.cloak);
        AddItem("earring", player.earring);
        AddItem("hat", player.hat);
        AddItem("necklace", player.necklace);
        AddItem("shoes", player.shoes);
    }

    // Update is called once per frame
    void Update() {
        //if (player == null || !NetworkServer.active) return;
        if (player == null) return;
        if (player.weapon.attackPower != attackPower) attackPower = player.weapon.attackPower;
        DetermineWeaponSubtype();
        UpdateItem("weapon", player.weapon);
        UpdateItem("armor", player.armor);
        UpdateItem("belt", player.belt);
        UpdateBracelets();
        UpdateItem("cloak", player.cloak);
        UpdateItem("earring", player.earring);
        UpdateItem("hat", player.hat);
        UpdateItem("necklace", player.necklace);
        UpdateItem("shoes", player.shoes);
	}

    private void DetermineWeaponSubtype() {
        var subtype = "";
        if (player.weapon is MeleeWeapon) subtype = "strength";
        else if (player.weapon is RangedWeapon && ((RangedWeapon)(player.weapon)).usesInt) subtype = "intelligence";
        else if (player.weapon is RangedWeapon) subtype = "dexterity";
        if (weaponSubtype != subtype) weaponSubtype = subtype;
    }

    private void AddItem(string type, Equipment item) {
        if (item==null) {
            names.Add("");
            descriptions.Add("");
            types.Add("");
            qualities.Add(-1);
            icons.Add("");
            armor.Add(0);
            strength.Add(0);
            dexterity.Add(0);
            constitution.Add(0);
            intelligence.Add(0);
            wisdom.Add(0);
            luck.Add(0);
        }
        else {
            names.Add(item.name);
            descriptions.Add(item.description);
            types.Add(type);
            qualities.Add(item.quality);
            icons.Add(item.icon);
            if (item is Armor) armor.Add(item.armor);
            else armor.Add(0);
            strength.Add(item.strength);
            dexterity.Add(item.dexterity);
            constitution.Add(item.constitution);
            intelligence.Add(item.intelligence);
            wisdom.Add(item.wisdom);
            luck.Add(item.luck);
        }
    }

    private void AddBracelet(Bracelet[] bracelets, int num) {
        if (bracelets.Length > num && bracelets[num] != null) {
            var bracelet = bracelets[num];
            names.Add(bracelet.name);
            descriptions.Add(bracelet.description);
            types.Add("bracelet");
            qualities.Add(bracelet.quality);
            icons.Add(bracelet.icon);
            armor.Add(0);
            strength.Add(bracelet.strength);
            dexterity.Add(bracelet.dexterity);
            constitution.Add(bracelet.constitution);
            intelligence.Add(bracelet.intelligence);
            wisdom.Add(bracelet.wisdom);
            luck.Add(bracelet.luck);
        }
        else {
            names.Add("");
            descriptions.Add("");
            types.Add("");
            qualities.Add(-1);
            icons.Add("");
            armor.Add(0);
            strength.Add(0);
            dexterity.Add(0);
            constitution.Add(0);
            intelligence.Add(0);
            wisdom.Add(0);
            luck.Add(0);
        }
    }

    private void UpdateItem(string slot, Equipment item) {
        var index = slotKeys[slot];
        if (item == null) {
            if (names[index]!="") names[index] = "";
            if (descriptions[index] != "") descriptions[index] = "";
            if (types[index] != "") types[index] = "";
            if (qualities[index] != -1) qualities[index] = -1;
            if (icons[index] != "") icons[index] = "";
            if (armor[index] != 0) armor[index] = 0;
            if (strength[index] != 0) strength[index] = 0;
            if (dexterity[index] != 0) dexterity[index] = 0;
            if (constitution[index] != 0) constitution[index] = 0;
            if (intelligence[index] != 0) intelligence[index] = 0;
            if (wisdom[index] != 0) wisdom[index] = 0;
            if (luck[index] != 0) luck[index] = 0;
        }
        else {
            if (names[index] != item.name) names[index] = item.name;
            if (descriptions[index] != item.description) descriptions[index] = item.description;
            if (types[index] != slot) types[index] = slot;
            if (qualities[index] != item.quality) qualities[index] = item.quality;
            if (icons[index] != item.icon) icons[index] = item.icon;
            if (armor[index] != item.armor) armor[index] = item.armor;
            if (strength[index] != item.strength) strength[index] = item.strength;
            if (dexterity[index] != item.dexterity) dexterity[index] = item.dexterity;
            if (constitution[index] !=item.constitution) constitution[index] = item.constitution;
            if (intelligence[index] != item.intelligence) intelligence[index] = item.intelligence;
            if (wisdom[index] != item.wisdom) wisdom[index] = item.wisdom;
            if (luck[index] != item.luck) luck[index] = item.luck;
        }
    }

    private void UpdateBracelets() {
        for (var i=0; i<4; i++) {
            var index = i + 3;
            Equipment item = null;
            if (player.bracelets.Length > i) item = player.bracelets[i];
            if (item==null) {
                if (names[index] != "") names[index] = "";
                if (descriptions[index] != "") descriptions[index] = "";
                if (types[index] != "") types[index] = "";
                if (qualities[index] != -1) qualities[index] = -1;
                if (icons[index] != "") icons[index] = "";
                if (armor[index] != 0) armor[index] = 0;
                if (strength[index] != 0) strength[index] = 0;
                if (dexterity[index] != 0) dexterity[index] = 0;
                if (constitution[index] != 0) constitution[index] = 0;
                if (intelligence[index] != 0) intelligence[index] = 0;
                if (wisdom[index] != 0) wisdom[index] = 0;
                if (luck[index] != 0) luck[index] = 0;
            }
            else {
                if (names[index] != item.name) names[index] = item.name;
                if (descriptions[index] != item.description) descriptions[index] = item.description;
                if (types[index] != "bracelet") types[index] = "bracelet";
                if (qualities[index] != item.quality) qualities[index] = item.quality;
                if (icons[index] != item.icon) icons[index] = item.icon;
                if (armor[index] != item.armor) armor[index] = item.armor;
                if (strength[index] != item.strength) strength[index] = item.strength;
                if (dexterity[index] != item.dexterity) dexterity[index] = item.dexterity;
                if (constitution[index] != item.constitution) constitution[index] = item.constitution;
                if (intelligence[index] != item.intelligence) intelligence[index] = item.intelligence;
                if (wisdom[index] != item.wisdom) wisdom[index] = item.wisdom;
                if (luck[index] != item.luck) luck[index] = item.luck;
            }
        }
    }
}
