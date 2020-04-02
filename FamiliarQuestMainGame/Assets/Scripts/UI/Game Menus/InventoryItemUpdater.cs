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
    public Item item = null;
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
    public Text costText;
    public GameObject costArea;
    private DuloGames.UI.UIWindow shop = null;
    public int cost;
    private static Dictionary<string, Sprite> images = new Dictionary<string, Sprite>();
    private static readonly Dictionary<string, string> displayTypes = new Dictionary<string, string>() {
        { "weapon", "Weapon" },
        { "armor", "Armor" },
        { "belt", "Belt" },
        { "bracelet", "Bracelet" },
        { "cloak", "Cloak" },
        { "earring", "Earring" },
        { "hat", "Hat" },
        { "necklace", "Necklace" },
        { "shoes", "Shoes" }
    };
    private static readonly List<string> qualityDisplayTypes = new List<string>() {
        "",
        "High Quality ",
        "Masterwork ",
        "Enchanted ",
        "Rare ",
        "Artifact ",
        "Legendary ",
        "Transcendent "
    };

    // Update is called once per frame
    void Update() {
        if (shop==null) shop = GameObject.FindGameObjectWithTag("ShopPane").GetComponent<DuloGames.UI.UIWindow>();
        if (shop.IsOpen && !costArea.activeSelf) costArea.SetActive(true);
        else if (!shop.IsOpen && costArea.activeSelf) costArea.SetActive(false);
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
            if (GetComponent<ShopItemController>() != null) {
                UpdateArrowsForShopItem();
                return;
            }
            var pc = PlayerCharacter.localPlayer.GetComponent<Character>();
            if (type == "weapon" && ((Weapon)inventory.items[number]).attackPower > inventory.player.weapon.attackPower) Instantiate(upArrow, arrowContainer.transform);
            else if (type == "weapon" && ((Weapon)inventory.items[number]).attackPower < inventory.player.weapon.attackPower) Instantiate(downArrow, arrowContainer.transform);
            if (type == "armor" || type == "shoes" || type == "hat") {
                Equipment item = null;
                if (type == "armor") item = inventory.player.armor;
                else if (type == "shoes") item = inventory.player.shoes;
                else item = inventory.player.hat;
                if (((Equipment)inventory.items[number]).armor > item.armor) Instantiate(upArrow, arrowContainer.transform);
                else if (((Equipment)inventory.items[number]).armor < item.armor) Instantiate(downArrow, arrowContainer.transform);
            }
            if (!(item is Equipment)) return;
            var equipment = item as Equipment;

            foreach (var kvp in CharacterAttribute.attributes) {
                if (equipment.stats.ContainsKey(kvp.Key) || (GetEquippedGear(equipment)!=null && GetEquippedGear(equipment).stats.ContainsKey(kvp.Key))) AddArrow(equipment.GetStatValue(kvp.Key), GetStatOnEquippedGear(equipment, kvp.Key));
            }
        }
    }

    private void UpdateArrowsForShopItem() {
        var shop = GetComponent<ShopItemController>().shop;
        number = shop.goods.IndexOf(item);
        var pc = PlayerCharacter.localPlayer.GetComponent<Character>();
        if (type == "weapon" && ((Weapon)shop.goods[number]).attackPower > inventory.player.weapon.attackPower) Instantiate(upArrow, arrowContainer.transform);
        else if (type == "weapon" && ((Weapon)shop.goods[number]).attackPower < inventory.player.weapon.attackPower) Instantiate(downArrow, arrowContainer.transform);
        if (type == "armor" || type == "shoes" || type == "hat") {
            Equipment item = null;
            if (type == "armor") item = inventory.player.armor;
            else if (type == "shoes") item = inventory.player.shoes;
            else item = inventory.player.hat;
            if (((Equipment)shop.goods[number]).armor > item.armor) Instantiate(upArrow, arrowContainer.transform);
            else if (((Equipment)shop.goods[number]).armor < item.armor) Instantiate(downArrow, arrowContainer.transform);
        }
        if (!(item is Equipment)) return;
        var equipment = item as Equipment;

        foreach (var kvp in CharacterAttribute.attributes) {
            if (equipment.stats.ContainsKey(kvp.Key) || (GetEquippedGear(equipment) != null && GetEquippedGear(equipment).stats.ContainsKey(kvp.Key))) AddArrow(equipment.GetStatValue(kvp.Key), GetStatOnEquippedGear(equipment, kvp.Key));
        }
    }

    private int GetStatOnEquippedGear(Equipment originalEquipment, string stat) {
        if (originalEquipment is Weapon && inventory.player.weapon != null) return inventory.player.weapon.GetStatValue(stat);
        else if (originalEquipment is Armor && inventory.player.armor != null) return inventory.player.armor.GetStatValue(stat);
        else if (originalEquipment is Belt && inventory.player.belt != null) return inventory.player.belt.GetStatValue(stat);
        else if (originalEquipment is Bracelet && inventory.player.bracelets[0] != null) return inventory.player.bracelets[0].GetStatValue(stat);
        else if (originalEquipment is Cloak && inventory.player.cloak != null) return inventory.player.cloak.GetStatValue(stat);
        else if (originalEquipment is Earring && inventory.player.earring != null) return inventory.player.earring.GetStatValue(stat);
        else if (originalEquipment is Hat && inventory.player.hat != null) return inventory.player.hat.GetStatValue(stat);
        else if (originalEquipment is Necklace && inventory.player.necklace != null) return inventory.player.necklace.GetStatValue(stat);
        else if (originalEquipment is Shoes && inventory.player.shoes != null) return inventory.player.shoes.GetStatValue(stat);
        else return 0;
    }

    private Equipment GetEquippedGear(Equipment originalEquipment) {
        if (originalEquipment is Weapon && inventory.player.weapon != null) return inventory.player.weapon;
        else if (originalEquipment is Armor && inventory.player.armor != null) return inventory.player.armor;
        else if (originalEquipment is Belt && inventory.player.belt != null) return inventory.player.belt;
        else if (originalEquipment is Bracelet && inventory.player.bracelets[0] != null) return inventory.player.bracelets[0];
        else if (originalEquipment is Cloak && inventory.player.cloak != null) return inventory.player.cloak;
        else if (originalEquipment is Earring && inventory.player.earring != null) return inventory.player.earring;
        else if (originalEquipment is Hat && inventory.player.hat != null) return inventory.player.hat;
        else if (originalEquipment is Necklace && inventory.player.necklace != null) return inventory.player.necklace;
        else if (originalEquipment is Shoes && inventory.player.shoes != null) return inventory.player.shoes;
        else return null;
    }

    private void AddArrow(int a, int b) {
        if (a > b) Instantiate(upArrow, arrowContainer.transform);
        else if (b > a) Instantiate(downArrow, arrowContainer.transform);
    }

    public void Click() {
        var characterSheet = GameObject.FindGameObjectWithTag("CharacterSheet").GetComponent<DuloGames.UI.UIWindow>();
        var shop = GameObject.FindGameObjectWithTag("ShopPane").GetComponent<DuloGames.UI.UIWindow>();
        if (!characterSheet.IsOpen && shop.IsOpen) shop.GetComponent<ShopPaneController>().SellItem(item);
        else Equip();
    }
private void Equip() {
        inventory.EquipItem(number);
    }

    private string GetItemType(Item item) {
        if (item is Weapon) return "weapon";
        else if (item is Armor) return "armor";
        else if (item is Necklace) return "necklace";
        else if (item is Belt) return "belt";
        else if (item is Bracelet) return "bracelet";
        else if (item is Cloak) return "cloak";
        else if (item is Earring) return "earring";
        else if (item is Hat) return "hat";
        else if (item is Shoes) return "shoes";
        else return "item";
    }

    private string GetItemSubtype(Item item) {
        if (item is MeleeWeapon) return "sword";
        else if (item is RangedWeapon && ((RangedWeapon)item).usesInt) return "wand";
        else if (item is RangedWeapon) return "bow";
        else return "item";
    }

    //public void Initialize(string name, string description, string type, Inventory inventory, int number, int quality, string icon, Item item) {
    public void Initialize(Item item, Inventory inventory) {
        if (images.Count == 0) {
            images.Clear();
            var imagesTemp = Resources.LoadAll<Sprite>("Icons");
            foreach (var image in imagesTemp) images[image.name] = image;
        }
        cost = Shop.Appraise(item);
        costText.text = cost.ToString();
        costArea.SetActive(false);
        this.item = item;
        name = item.name;
        description = item.description;
        if (item is Weapon) attackPower = ((Weapon)item).attackPower;
        type = GetItemType(item);
        subtype = GetItemSubtype(item);
        this.inventory = inventory;
        details = inventory.inventoryDetails;
        foldoutBackground = inventory.inventoryDetailsBackground;
        descriptionText = inventory.inventoryDetailsDescriptionText;
        statTextContainer = inventory.inventoryDetailsStatTextContainer;
        statResultsContainer = inventory.inventoryDetailsStatResultsContainer;
        number = inventory.items.IndexOf(item);
        if (GetComponent<ShopItemController>() != null) number = GetComponent<ShopItemController>().shop.goods.IndexOf(item);
        if (item is Equipment) quality = ((Equipment)item).quality;
        image.sprite = images[item.icon];
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
        if (item is Equipment) {
            Equipment equipment = item as Equipment;
            tooltip.contentLines[1].Content = qualityDisplayTypes[equipment.quality] + displayTypes[type];
        }
        else tooltip.contentLines[1].Content = displayTypes[type];
        tooltip.contentLines[2].LineStyle = DuloGames.UI.UITooltipLines.LineStyle.Custom;
        tooltip.contentLines[2].CustomLineStyle = "ItemAttribute";
        tooltip.contentLines[2].Content = description.Replace("{{AttackPower}}", EquipmentNamer.GetAttackPowerNumberFromItem(item).ToString() + " Attack");
        tooltip.contentLines[3].LineStyle = DuloGames.UI.UITooltipLines.LineStyle.Custom;
        tooltip.contentLines[3].CustomLineStyle = "ItemStat";
        tooltip.contentLines[3].Content = GetComparisonStats();
        tooltip.contentLines[4].LineStyle = DuloGames.UI.UITooltipLines.LineStyle.Custom;
        tooltip.contentLines[4].CustomLineStyle = "ItemDescription";
        tooltip.contentLines[4].Content = item.flavorText;
    }

    public string GetComparisonStats() {
        var output = "";
        if (number < 0) return output;
        if (!(item is Equipment equipment)) return output;
        output = GetArmorAndAttackPowerComparisons(equipment);
        if (equipment.stats == null) return output;
        foreach (var kvp in CharacterAttribute.attributes) {
            if (equipment.stats.ContainsKey(kvp.Key) || (GetEquippedGear(equipment)!=null && GetEquippedGear(equipment).stats.ContainsKey(kvp.Key) && GetEquippedGear(equipment).GetStatValue(kvp.Key) > 0)) output += AddComparisonStat(GetStatOnEquippedGear(equipment, kvp.Key), equipment.GetStatValue(kvp.Key), kvp.Value.friendlyName);
        }
        return output;
    }

    private string GetArmorAndAttackPowerComparisons(Equipment equipment) {
        var output = "";
        var oldEquipment = GetEquippedGear(equipment);
        int numberChange = 0;
        if (equipment.armor != 0 || (oldEquipment != null && oldEquipment.armor != 0)) {
            if (equipment.armor > 0 || (oldEquipment != null && oldEquipment.armor > 0)) numberChange = equipment.armor - oldEquipment.armor;
            if (numberChange > 0) output += "+";
            if (numberChange != 0) output += numberChange.ToString() + " Armor\n";
        }
        if (!(equipment is Weapon)) return output;
        var weapon = equipment as Weapon;
        var oldWeapon = oldEquipment as Weapon;
        numberChange = EquipmentNamer.GetAttackPowerNumberFromItem(weapon) - EquipmentNamer.GetAttackPowerNumberFromItem(oldWeapon);
        if (numberChange > 0) output += "+";
        if (numberChange != 0) output += numberChange.ToString() + " Attack\n";
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
        int comparisonSlot = 0;
        if (type == "bracelet") comparisonSlot = 3;
        else comparisonSlot = Inventory.slotKeys[type];
    }
}