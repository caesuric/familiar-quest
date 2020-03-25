using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUpdater : MonoBehaviour {

    public Text nameText;
    public Text descriptionText;
    public new string name = null;
    public string description = null;
    public string type = null;
    public string subtype = null;
    public float attackPower = 0;
    //public Item item = null;
    public Inventory inventory;
    private bool initialized = false;
    public int number;
    public int quality;
    public GameObject upArrow;
    public GameObject downArrow;
    public GameObject arrowContainer;
    public GameObject statTextContainer;
    public GameObject attributeTextObj;
    public GameObject statResultsContainer;
    public GameObject statTextObj;
    public Image image;
    public Image foldoutBackground;
    public GameObject details = null;
    public Vector3 hoverOffset;
    private static Dictionary<string, Sprite> images = new Dictionary<string, Sprite>();

    // Update is called once per frame
    void Update() {
        if (!details) return;
        if (!initialized && name != null) {
            initialized = true;
            details.SetActive(false);
            nameText.text = name;
            switch (quality) {
                case 0:
                    foldoutBackground.color = GetComponent<Image>().color = Color.white;
                    break;
                case 1:
                    foldoutBackground.color = GetComponent<Image>().color = Color.red;
                    break;
                case 2:
                    foldoutBackground.color = GetComponent<Image>().color = new Color(255, 165, 0);
                    break;
                case 3:
                    foldoutBackground.color = GetComponent<Image>().color = Color.yellow;
                    break;
                case 4:
                    foldoutBackground.color = GetComponent<Image>().color = Color.green;
                    break;
                case 5:
                    foldoutBackground.color = GetComponent<Image>().color = Color.blue;
                    break;
                case 6:
                    foldoutBackground.color = GetComponent<Image>().color = new Color(111, 0, 255);
                    break;
                case 7:
                default:
                    foldoutBackground.color = GetComponent<Image>().color = new Color(128, 0, 128);
                    break;
            }
            if (number < 0) return;
            foreach (Transform child in arrowContainer.transform) Destroy(child.gameObject);
            var es = PlayerCharacter.localPlayer.GetComponent<EquipmentSyncer>();
            var pc = PlayerCharacter.localPlayer.GetComponent<Character>();
            var si = inventory.sharedInventory;
            if (type == "weapon" && inventory.sharedInventory.inventoryMainStat[number] > es.attackPower) Instantiate(upArrow, arrowContainer.transform);
            else if (type == "weapon" && inventory.sharedInventory.inventoryMainStat[number] < es.attackPower) Instantiate(downArrow, arrowContainer.transform);
            if (type == "armor" || type == "shoes" || type == "hat") {
                if (inventory.sharedInventory.inventoryMainStat[number] > es.armor[EquipmentSyncer.slotKeys[type]]) Instantiate(upArrow, arrowContainer.transform);
                else if (inventory.sharedInventory.inventoryMainStat[number] < es.armor[EquipmentSyncer.slotKeys[type]]) Instantiate(downArrow, arrowContainer.transform);
            }
            if (!EquipmentSyncer.slotKeys.ContainsKey(type)) return;
            var intelligence = CharacterAttribute.attributes["intelligence"].instances[pc].BaseValue;
            var dexterity = CharacterAttribute.attributes["dexterity"].instances[pc].BaseValue;
            var strength = CharacterAttribute.attributes["strength"].instances[pc].BaseValue;
            if (intelligence > dexterity && intelligence > strength) {
                AddArrow(si.inventoryInt[number], es.intelligence[EquipmentSyncer.slotKeys[type]]);
                AddArrow(si.inventoryStr[number], es.strength[EquipmentSyncer.slotKeys[type]]);
                AddArrow(si.inventoryDex[number], es.dexterity[EquipmentSyncer.slotKeys[type]]);
                AddArrow(si.inventoryCon[number], es.constitution[EquipmentSyncer.slotKeys[type]]);
                AddArrow(si.inventoryWis[number], es.wisdom[EquipmentSyncer.slotKeys[type]]);
                AddArrow(si.inventoryLuc[number], es.luck[EquipmentSyncer.slotKeys[type]]);
            }
            else if (dexterity > intelligence && dexterity > strength) {
                AddArrow(si.inventoryDex[number], es.dexterity[EquipmentSyncer.slotKeys[type]]);
                AddArrow(si.inventoryStr[number], es.strength[EquipmentSyncer.slotKeys[type]]);
                AddArrow(si.inventoryCon[number], es.constitution[EquipmentSyncer.slotKeys[type]]);
                AddArrow(si.inventoryInt[number], es.intelligence[EquipmentSyncer.slotKeys[type]]);
                AddArrow(si.inventoryWis[number], es.wisdom[EquipmentSyncer.slotKeys[type]]);
                AddArrow(si.inventoryLuc[number], es.luck[EquipmentSyncer.slotKeys[type]]);
            }
            else {
                AddArrow(si.inventoryStr[number], es.strength[EquipmentSyncer.slotKeys[type]]);
                AddArrow(si.inventoryDex[number], es.dexterity[EquipmentSyncer.slotKeys[type]]);
                AddArrow(si.inventoryCon[number], es.constitution[EquipmentSyncer.slotKeys[type]]);
                AddArrow(si.inventoryInt[number], es.intelligence[EquipmentSyncer.slotKeys[type]]);
                AddArrow(si.inventoryWis[number], es.wisdom[EquipmentSyncer.slotKeys[type]]);
                AddArrow(si.inventoryLuc[number], es.luck[EquipmentSyncer.slotKeys[type]]);
            }
        }
    }

    private void AddArrow(int a, int b) {
        if (a > b) Instantiate(upArrow, arrowContainer.transform);
        else if (b > a) Instantiate(downArrow, arrowContainer.transform);
    }

    private void AddComparison(int a, int b) {
        var textObj = Instantiate(statTextObj, statResultsContainer.transform);
        var textObjText = textObj.GetComponent<Text>();
        textObjText.text = a.ToString();
        if (b > a) Instantiate(upArrow, statResultsContainer.transform);
        else if (b < a) Instantiate(downArrow, statResultsContainer.transform);
        else {
            textObj = Instantiate(statTextObj, statResultsContainer.transform);
            textObjText = textObj.GetComponent<Text>();
            textObjText.text = "=";
        }
        textObj = Instantiate(statTextObj, statResultsContainer.transform);
        textObjText = textObj.GetComponent<Text>();
        textObjText.text = b.ToString();
    }

    public void Equip() {
        inventory.EquipItem(number);
    }

    //public void Initialize(string name, string description, string type, Inventory inventory, int number, int quality, string icon, Item item) {
    public void Initialize(string name, string description, string type, Inventory inventory, int number, int quality, string icon, float attackPower = 0, string subtype = "", string displayType = "", string flavorText = "") {
        if (images.Count == 0) {
            images.Clear();
            var imagesTemp = Resources.LoadAll<Sprite>("Icons");
            foreach (var image in imagesTemp) images[image.name] = image;
        }
        this.name = name;
        this.description = description;
        this.attackPower = attackPower;
        this.type = type;
        this.subtype = subtype;
        this.inventory = inventory;
        details = inventory.inventoryDetails;
        foldoutBackground = inventory.inventoryDetailsBackground;
        descriptionText = inventory.inventoryDetailsDescriptionText;
        statTextContainer = inventory.inventoryDetailsStatTextContainer;
        statResultsContainer = inventory.inventoryDetailsStatResultsContainer;
        this.number = number;
        this.quality = quality;
        image.sprite = images[icon];
        var canvas = GetComponentInParent<Canvas>();
        //hoverOffset = new Vector3(GetComponent<RectTransform>().sizeDelta.x * canvas.GetComponent<CanvasScaler>().referencePixelsPerUnit, 0, 0);
        var rect = RectTransformUtility.PixelAdjustRect(GetComponent<RectTransform>(), canvas);
        hoverOffset = new Vector3(rect.width * 1.485f * Screen.width / 1600, 0, 0);
        var tooltip = GetComponent<DuloGames.UI.UITooltipShow>();
        tooltip.contentLines = new DuloGames.UI.UITooltipLineContent[] {
            new DuloGames.UI.UITooltipLineContent(),
            new DuloGames.UI.UITooltipLineContent(),
            new DuloGames.UI.UITooltipLineContent(),
            new DuloGames.UI.UITooltipLineContent(),
            new DuloGames.UI.UITooltipLineContent()
            // name
            // type
            // stats
            // comparison stats
            // flavor text
        };
        tooltip.contentLines[0].LineStyle = DuloGames.UI.UITooltipLines.LineStyle.Title;
        tooltip.contentLines[0].Content = name;
        tooltip.contentLines[1].LineStyle = DuloGames.UI.UITooltipLines.LineStyle.Description;
        tooltip.contentLines[1].Content = displayType;
        tooltip.contentLines[2].LineStyle = DuloGames.UI.UITooltipLines.LineStyle.Custom;
        tooltip.contentLines[2].CustomLineStyle = "ItemAttribute";
        tooltip.contentLines[2].Content = description.Replace("{{AttackPower}}", EquipmentNamer.GetAttackPowerNumberFromStat(attackPower, subtype).ToString() + " Attack");
        tooltip.contentLines[3].LineStyle = DuloGames.UI.UITooltipLines.LineStyle.Custom;
        tooltip.contentLines[3].CustomLineStyle = "ItemStat";
        tooltip.contentLines[3].Content = GetComparisonStats();
        tooltip.contentLines[4].LineStyle = DuloGames.UI.UITooltipLines.LineStyle.Custom;
        tooltip.contentLines[4].CustomLineStyle = "ItemDescription";
        tooltip.contentLines[4].Content = flavorText;
    }

    public string GetComparisonStats() {
        var output = "";
        if (number < 0) return output;
        var es = PlayerCharacter.localPlayer.GetComponent<EquipmentSyncer>();
        int comparisonSlot = 0;
        if (type == "bracelet") comparisonSlot = 3;
        else comparisonSlot = EquipmentSyncer.slotKeys[type];

        if (type == "weapon") output += AddComparisonStat(EquipmentNamer.GetAttackPowerNumberFromStat(es.attackPower, subtype), EquipmentNamer.GetAttackPowerNumberFromStat(inventory.sharedInventory.inventoryMainStat[number], subtype), "Attack");
        if (new List<string>() { "armor", "hat", "shoes" }.Contains(type)) output += AddComparisonStat(es.armor[comparisonSlot], (int)inventory.sharedInventory.inventoryMainStat[number], "Armor");
        output += AddComparisonStat(es.strength[comparisonSlot], inventory.sharedInventory.inventoryStr[number], "Strength");
        output += AddComparisonStat(es.dexterity[comparisonSlot], inventory.sharedInventory.inventoryDex[number], "Dexterity");
        output += AddComparisonStat(es.constitution[comparisonSlot], inventory.sharedInventory.inventoryCon[number], "Constitution");
        output += AddComparisonStat(es.intelligence[comparisonSlot], inventory.sharedInventory.inventoryInt[number], "Intelligence");
        output += AddComparisonStat(es.wisdom[comparisonSlot], inventory.sharedInventory.inventoryWis[number], "Wisdom");
        output += AddComparisonStat(es.luck[comparisonSlot], inventory.sharedInventory.inventoryLuc[number], "Luck");

        return output;
    }

    public string AddComparisonStat(int slotStat, int itemStat, string text) {
        var output = "";
        int numberChange = 0;
        if (slotStat > 0 || itemStat > 0) numberChange = itemStat - slotStat;
        if (numberChange > 0) output += "+";
        if (numberChange != 0) output += (numberChange).ToString() + " " + text + "\n";
        return output;
    }

    public void OnMouseEnter() {
        //details.SetActive(true);
        details.transform.position = transform.position + hoverOffset;
        details.transform.SetAsLastSibling();
        UpdateDetails();
    }

    public void OnMouseExit() {
        details.SetActive(false);
        initialized = false;
    }

    private void UpdateDetails() {
        foreach (Transform child in statTextContainer.transform) Destroy(child.gameObject);
        foreach (Transform child in statResultsContainer.transform) Destroy(child.gameObject);
        if (type == "weapon") descriptionText.text = description.Replace("{{AttackPower}}", "Attack Power: " + attackPower.ToString());
        else descriptionText.text = description;
        switch (quality) {
            case 0:
                foldoutBackground.color = GetComponent<Image>().color = Color.white;
                break;
            case 1:
                foldoutBackground.color = GetComponent<Image>().color = Color.red;
                break;
            case 2:
                foldoutBackground.color = GetComponent<Image>().color = new Color(255, 165, 0);
                break;
            case 3:
                foldoutBackground.color = GetComponent<Image>().color = Color.yellow;
                break;
            case 4:
                foldoutBackground.color = GetComponent<Image>().color = Color.green;
                break;
            case 5:
                foldoutBackground.color = GetComponent<Image>().color = Color.blue;
                break;
            case 6:
                foldoutBackground.color = GetComponent<Image>().color = new Color(111, 0, 255);
                break;
            case 7:
            default:
                foldoutBackground.color = GetComponent<Image>().color = new Color(128, 0, 128);
                break;
        }
        var pc = PlayerCharacter.localPlayer.GetComponent<Character>();
        var es = PlayerCharacter.localPlayer.GetComponent<EquipmentSyncer>();
        SharedInventory si = inventory.sharedInventory;
        int comparisonSlot = 0;
        if (type == "bracelet") comparisonSlot = 3;
        else comparisonSlot = EquipmentSyncer.slotKeys[type];
        if (number < 0) {
            AddSelfComparisons(es);
            return;
        }
        if (type == "weapon") {
            var statText = Instantiate(statTextObj, statTextContainer.transform);
            statText.GetComponent<Text>().text = "Attack:";
        }
        if (type == "armor" || type == "hat" || type == "shoes") {
            var statText = Instantiate(statTextObj, statTextContainer.transform);
            statText.GetComponent<Text>().text = "Armor:";
        }
        if (si.inventoryStr[number] > 0 || es.strength[comparisonSlot] > 0) {
            var statText = Instantiate(statTextObj, statTextContainer.transform);
            statText.GetComponent<Text>().text = "Strength:";
        }
        if (si.inventoryDex[number] > 0 || es.dexterity[comparisonSlot] > 0) {
            var statText = Instantiate(statTextObj, statTextContainer.transform);
            statText.GetComponent<Text>().text = "Dexterity:";
        }
        if (si.inventoryCon[number] > 0 || es.constitution[comparisonSlot] > 0) {
            var statText = Instantiate(statTextObj, statTextContainer.transform);
            statText.GetComponent<Text>().text = "Constitution:";
        }
        if (si.inventoryInt[number] > 0 || es.intelligence[comparisonSlot] > 0) {
            var statText = Instantiate(statTextObj, statTextContainer.transform);
            statText.GetComponent<Text>().text = "Intelligence:";
        }
        if (si.inventoryWis[number] > 0 || es.wisdom[comparisonSlot] > 0) {
            var statText = Instantiate(statTextObj, statTextContainer.transform);
            statText.GetComponent<Text>().text = "Wisdom:";
        }
        if (si.inventoryLuc[number] > 0 || es.luck[comparisonSlot] > 0) {
            var statText = Instantiate(statTextObj, statTextContainer.transform);
            statText.GetComponent<Text>().text = "Luck:";
        }
        if (type == "weapon") {
            var a = EquipmentNamer.GetAttackPowerNumberFromStat(es.attackPower, subtype);
            var b = EquipmentNamer.GetAttackPowerNumberFromStat(si.inventoryMainStat[number], subtype);
            AddComparison(a, b);
        }
        if (type == "armor" || type == "hat" || type == "shoes") {
            var a = es.armor[comparisonSlot];
            var b = (int)si.inventoryMainStat[number];
            AddComparison(a, b);
        }
        if (si.inventoryStr[number] > 0 || es.strength[comparisonSlot] > 0) {
            var a = es.strength[comparisonSlot];
            var b = si.inventoryStr[number];
            AddComparison(a, b);
        }
        if (si.inventoryDex[number] > 0 || es.dexterity[comparisonSlot] > 0) {
            var a = es.dexterity[comparisonSlot];
            var b = si.inventoryDex[number];
            AddComparison(a, b);
        }
        if (si.inventoryCon[number] > 0 || es.constitution[comparisonSlot] > 0) {
            var a = es.constitution[comparisonSlot];
            var b = si.inventoryCon[number];
            AddComparison(a, b);
        }
        if (si.inventoryInt[number] > 0 || es.intelligence[comparisonSlot] > 0) {
            var a = es.intelligence[comparisonSlot];
            var b = si.inventoryInt[number];
            AddComparison(a, b);
        }
        if (si.inventoryWis[number] > 0 || es.wisdom[comparisonSlot] > 0) {
            var a = es.wisdom[comparisonSlot];
            var b = si.inventoryWis[number];
            AddComparison(a, b);
        }
        if (si.inventoryLuc[number] > 0 || es.luck[comparisonSlot] > 0) {
            var a = es.luck[comparisonSlot];
            var b = si.inventoryLuc[number];
            AddComparison(a, b);
        }
    }

    private void AddSelfComparisons(EquipmentSyncer es) {
        var comparisonSlot = 0 - number;
        if (type == "weapon") {
            var statText = Instantiate(statTextObj, statTextContainer.transform);
            statText.GetComponent<Text>().text = "Attack:";
        }
        if (type == "armor" || type == "hat" || type == "shoes") {
            var statText = Instantiate(statTextObj, statTextContainer.transform);
            statText.GetComponent<Text>().text = "Armor:";
        }
        if (es.strength[comparisonSlot] > 0) {
            var statText = Instantiate(statTextObj, statTextContainer.transform);
            statText.GetComponent<Text>().text = "Strength:";
        }
        if (es.dexterity[comparisonSlot] > 0) {
            var statText = Instantiate(statTextObj, statTextContainer.transform);
            statText.GetComponent<Text>().text = "Dexterity:";
        }
        if (es.constitution[comparisonSlot] > 0) {
            var statText = Instantiate(statTextObj, statTextContainer.transform);
            statText.GetComponent<Text>().text = "Constitution:";
        }
        if (es.intelligence[comparisonSlot] > 0) {
            var statText = Instantiate(statTextObj, statTextContainer.transform);
            statText.GetComponent<Text>().text = "Intelligence:";
        }
        if (es.wisdom[comparisonSlot] > 0) {
            var statText = Instantiate(statTextObj, statTextContainer.transform);
            statText.GetComponent<Text>().text = "Wisdom:";
        }
        if (es.luck[comparisonSlot] > 0) {
            var statText = Instantiate(statTextObj, statTextContainer.transform);
            statText.GetComponent<Text>().text = "Luck:";
        }
        if (type == "weapon") {
            var a = EquipmentNamer.GetAttackPowerNumberFromStat(es.attackPower, subtype);
            AddComparison(a, a);
        }
        if (type == "armor" || type == "hat" || type == "shoes") {
            var a = es.armor[comparisonSlot];
            AddComparison(a, a);
        }
        if (es.strength[comparisonSlot] > 0) {
            var a = es.strength[comparisonSlot];
            AddComparison(a, a);
        }
        if (es.dexterity[comparisonSlot] > 0) {
            var a = es.dexterity[comparisonSlot];
            AddComparison(a, a);
        }
        if (es.constitution[comparisonSlot] > 0) {
            var a = es.constitution[comparisonSlot];
            AddComparison(a, a);
        }
        if (es.intelligence[comparisonSlot] > 0) {
            var a = es.intelligence[comparisonSlot];
            AddComparison(a, a);
        }
        if (es.wisdom[comparisonSlot] > 0) {
            var a = es.wisdom[comparisonSlot];
            AddComparison(a, a);
        }
        if (es.luck[comparisonSlot] > 0) {
            var a = es.luck[comparisonSlot];
            AddComparison(a, a);
        }
    }
}