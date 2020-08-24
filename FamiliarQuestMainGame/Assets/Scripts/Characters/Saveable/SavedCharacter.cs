using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

[System.Serializable]
public class SavedCharacter {
    public int xp;
    public int level;
    public long xpToLevel;
    public int gold;
    public SavedWeapon weapon;
    public SavedArmor armor;
    public SavedNecklace necklace;
    public SavedBelt belt;
    public SavedBracelet[] bracelets = { null, null, null, null };
    public SavedCloak cloak;
    public SavedEarring earring;
    public SavedHat hat;
    public SavedShoes shoes;
    public List<SavedConsumable> consumables = new List<SavedConsumable>();
    public List<SavedAbility> soulGemActives = new List<SavedAbility>();
    public SavedAbility soulGemPassive;
    public List<SavedAbility> soulGemActivesOverflow = new List<SavedAbility>();
    public List<SavedAbility> soulGemPassivesOverflow = new List<SavedAbility>();
    public List<SavedAbility> overflowAbilities = new List<SavedAbility>();
    //[OptionalField]
    //public List<SavedDust> dust = new List<SavedDust>();
    public int currentAbility;
    public int currentAltAbility;
    public int sparePoints;
    public int strength;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int wisdom;
    public int luck;
    public float resurrectionTimer;
    public string name;
    public int furType;

    public static SavedCharacter BrandNewCharacter(string name, int furType) {
        var obj = new SavedCharacter {
            xp = 0,
            level = 1,
            xpToLevel = 200,
            gold = 0,
            weapon = SavedWeapon.StartingWeapon(),
            armor = null,
            necklace = null,
            belt = null,
            cloak = null,
            earring = null,
            hat = null,
            shoes = null,
            currentAbility = 0,
            currentAltAbility = 1,
            sparePoints = 0,
            strength = 10,
            dexterity = 10,
            constitution = 10,
            intelligence = 10,
            wisdom = 10,
            luck = 10,
            resurrectionTimer = 0,
            name = name,
            furType = furType
        };
        obj.bracelets[0] = null;
        obj.bracelets[1] = null;
        obj.bracelets[2] = null;
        obj.bracelets[3] = null;
        //obj.spirits.Add(SavedSpirit.ConvertFrom(Spirit.classDefaults[ClassSelectMenu.selectedClass]));
        return obj;
    }

    public static SavedCharacter ConvertFrom(GameObject go) {
        var pc = go.GetComponent<PlayerCharacter>();
        var character = go.GetComponent<Character>();
        var au = go.GetComponent<AbilityUser>();
        var obj = new SavedCharacter {
            xp = pc.GetComponent<ExperienceGainer>().xp,
            level = pc.GetComponent<ExperienceGainer>().level,
            xpToLevel = pc.GetComponent<ExperienceGainer>().xpToLevel,
            gold = pc.gold,
            weapon = SavedWeapon.ConvertFrom(pc.weapon),
            armor = SavedArmor.ConvertFrom(pc.armor),
            necklace = SavedNecklace.ConvertFrom(pc.necklace),
            belt = SavedBelt.ConvertFrom(pc.belt),
            cloak = SavedCloak.ConvertFrom(pc.cloak),
            earring = SavedEarring.ConvertFrom(pc.earring),
            hat = SavedHat.ConvertFrom(pc.hat),
            shoes = SavedShoes.ConvertFrom(pc.shoes),
            soulGemPassive = SavedAbility.ConvertFrom(au.soulGemPassive),
            currentAbility = character.GetComponent<InputController>().currentAbility,
            currentAltAbility = character.GetComponent<InputController>().currentAltAbility,
            sparePoints = pc.GetComponent<ExperienceGainer>().sparePoints,
            strength = (int)CharacterAttribute.attributes["strength"].instances[character].BaseValue,
            dexterity = (int)CharacterAttribute.attributes["dexterity"].instances[character].BaseValue,
            constitution = (int)CharacterAttribute.attributes["constitution"].instances[character].BaseValue,
            intelligence = (int)CharacterAttribute.attributes["intelligence"].instances[character].BaseValue,
            wisdom = (int)CharacterAttribute.attributes["wisdom"].instances[character].BaseValue,
            luck = (int)CharacterAttribute.attributes["luck"].instances[character].BaseValue,
            resurrectionTimer = au.resurrectionTimer,
            name = pc.GetComponent<PlayerSyncer>().characterName,
            furType = pc.GetComponent<PlayerSyncer>().furType
        };
        obj.bracelets[0] = SavedBracelet.ConvertFrom(pc.bracelets[0]);
        obj.bracelets[1] = SavedBracelet.ConvertFrom(pc.bracelets[1]);
        obj.bracelets[2] = SavedBracelet.ConvertFrom(pc.bracelets[2]);
        obj.bracelets[3] = SavedBracelet.ConvertFrom(pc.bracelets[3]);
        foreach (var consumable in pc.consumables) obj.consumables.Add(SavedConsumable.ConvertFrom(consumable));
        foreach (var ability in au.soulGemActives) obj.soulGemActives.Add(SavedAbility.ConvertFrom(ability));
        foreach (var ability in au.soulGemActivesOverflow) obj.soulGemActivesOverflow.Add(SavedAbility.ConvertFrom(ability));
        foreach (var ability in au.soulGemPassivesOverflow) obj.soulGemPassivesOverflow.Add(SavedAbility.ConvertFrom(ability));
        return obj;
    }

    public void ConvertTo(GameObject go) {
        var pc = go.GetComponent<PlayerCharacter>();
        var character = go.GetComponent<Character>();
        pc.GetComponent<ExperienceGainer>().xp = xp;
        pc.GetComponent<ExperienceGainer>().level = level;
        pc.GetComponent<ExperienceGainer>().xpToLevel = xpToLevel;
        pc.gold = gold;
        pc.weapon = weapon.ConvertTo();
        if (armor != null) pc.armor = armor.ConvertTo();
        else pc.armor = null;
        if (necklace != null) pc.necklace = necklace.ConvertTo();
        else pc.necklace = null;
        if (belt != null) pc.belt = belt.ConvertTo();
        else pc.belt = null;
        for (var i = 0; i < 4; i++) {
            if (bracelets[i] != null) pc.bracelets[i] = bracelets[i].ConvertTo();
            else pc.bracelets[i] = null;
        }
        if (cloak != null) pc.cloak = cloak.ConvertTo();
        else pc.cloak = null;
        if (earring != null) pc.earring = earring.ConvertTo();
        else pc.earring = null;
        if (hat != null) pc.hat = hat.ConvertTo();
        else pc.hat = null;
        if (shoes != null) pc.shoes = shoes.ConvertTo();
        else pc.shoes = null;
        foreach (var consumable in consumables) pc.consumables.Add(consumable.ConvertTo());
        var au = character.GetComponent<AbilityUser>();
        foreach (var ability in soulGemActives) au.soulGemActives.Add(((SavedActiveAbility)ability).ConvertTo());
        au.soulGemPassive = ((SavedPassiveAbility)soulGemPassive).ConvertTo();
        foreach (var ability in soulGemActivesOverflow) au.soulGemActives.Add(((SavedActiveAbility)ability).ConvertTo());
        foreach (var ability in soulGemPassivesOverflow) au.soulGemPassivesOverflow.Add(((SavedPassiveAbility)ability).ConvertTo());
        character.GetComponent<InputController>().currentAbility = currentAbility;
        character.GetComponent<InputController>().currentAltAbility = currentAltAbility;
        pc.GetComponent<ExperienceGainer>().sparePoints = sparePoints;
        CharacterAttribute.attributes["strength"].instances[character].BaseValue = strength;
        CharacterAttribute.attributes["dexterity"].instances[character].BaseValue = dexterity;
        CharacterAttribute.attributes["constitution"].instances[character].BaseValue = constitution;
        CharacterAttribute.attributes["intelligence"].instances[character].BaseValue = intelligence;
        CharacterAttribute.attributes["wisdom"].instances[character].BaseValue = wisdom;
        CharacterAttribute.attributes["luck"].instances[character].BaseValue = luck;
        ApplyItemValue(character, pc.weapon);
        ApplyItemValue(character, pc.armor);
        ApplyItemValue(character, pc.necklace);
        ApplyItemValue(character, pc.belt);
        ApplyItemValue(character, pc.cloak);
        ApplyItemValue(character, pc.earring);
        ApplyItemValue(character, pc.hat);
        ApplyItemValue(character, pc.shoes);
        for (int i=0; i<4; i++) ApplyItemValue(character, pc.bracelets[i]);
        character.GetComponent<AbilityUser>().resurrectionTimer = resurrectionTimer;
        pc.GetComponent<PlayerSyncer>().characterName = name;
        pc.GetComponent<PlayerSyncer>().furType = furType;
        pc.GetComponent<PlayerSyncer>().furTypeSet = true;
    }

    private void ApplyItemValue(Character character, Equipment equipment) {
        if (equipment == null) return;
        foreach (var kvp in equipment.stats) CharacterAttribute.attributes[kvp.Key].instances[character].ItemValue += kvp.Value;
    }
}

[System.Serializable]
public class SavedItem
{
    public string name;
    public string description;
    public List<string> statKeys = new List<string>();
    public List<int> statValues = new List<int>();
    public int armor = 0;
    public int quality;
    public int level = 0;

    public static SavedItem ConvertFrom(Item item) {
        if (item is Weapon) return SavedWeapon.ConvertFrom((Weapon)item);
        if (item is Armor) return SavedArmor.ConvertFrom((Armor)item);
        if (item is Belt) return SavedBelt.ConvertFrom((Belt)item);
        if (item is Bracelet) return SavedBracelet.ConvertFrom((Bracelet)item);
        if (item is Cloak) return SavedCloak.ConvertFrom((Cloak)item);
        if (item is Earring) return SavedEarring.ConvertFrom((Earring)item);
        if (item is Hat) return SavedHat.ConvertFrom((Hat)item);
        if (item is Necklace) return SavedNecklace.ConvertFrom((Necklace)item);
        if (item is Shoes) return SavedShoes.ConvertFrom((Shoes)item);
        return null;
    }

    public Item ConvertTo() {
        if (this is SavedWeapon) return ((SavedWeapon)this).ConvertTo();
        if (this is SavedArmor) return ((SavedArmor)this).ConvertTo();
        if (this is SavedBelt) return ((SavedBelt)this).ConvertTo();
        if (this is SavedBracelet) return ((SavedBracelet)this).ConvertTo();
        if (this is SavedCloak) return ((SavedCloak)this).ConvertTo();
        if (this is SavedEarring) return ((SavedEarring)this).ConvertTo();
        if (this is SavedHat) return ((SavedHat)this).ConvertTo();
        if (this is SavedNecklace) return ((SavedNecklace)this).ConvertTo();
        if (this is SavedShoes) return ((SavedShoes)this).ConvertTo();
        return null;
    }

    public Dictionary<string, int> ConvertStatsTo(List<string> statKeys, List<int> statValues) {
        var output = new Dictionary<string, int>();
        for (int i=0; i<statKeys.Count; i++) output.Add(statKeys[i], statValues[i]);
        return output;
    }
    public void ConvertStatsFrom(Dictionary<string, int> source) {
        statKeys.Clear();
        statValues.Clear();
        foreach (var kvp in source) {
            statKeys.Add(kvp.Key);
            statValues.Add(kvp.Value);
        }
    }
}

[System.Serializable]
public class SavedWeapon : SavedItem {
    public float attackPower;
    public string icon;

    public static SavedWeapon StartingWeapon() {
        return new SavedMeleeWeapon {
            attackPower = 0.8437f,
            description = "{{AttackPower}}",
            icon = "Weapon_01",
            name = "Starting Sword",
        };
    }

    public Weapon ConvertTo()
    {
        if (this is SavedMeleeWeapon) return ((SavedMeleeWeapon)this).ConvertTo();
        else if (this is SavedRangedWeapon) return ((SavedRangedWeapon)this).ConvertTo();
        else return null;
    }

    public static SavedWeapon ConvertFrom(Weapon weapon) {
        if (weapon is MeleeWeapon) return ConvertFromMeleeWeapon((MeleeWeapon)weapon);
        if (weapon is RangedWeapon) return ConvertFromRangedWeapon((RangedWeapon)weapon);
        else return null;
    }

    public static SavedWeapon ConvertFromMeleeWeapon(MeleeWeapon weapon) {
        var obj = new SavedMeleeWeapon {
            name = weapon.name,
            level = weapon.level,
            description = weapon.description,
            attackPower = weapon.attackPower,
            icon = weapon.icon,
            quality = weapon.quality
        };
        obj.ConvertStatsFrom(weapon.stats);
        return obj;
    }

    public static SavedWeapon ConvertFromRangedWeapon(RangedWeapon weapon) {
        var obj = new SavedRangedWeapon {
            name = weapon.name,
            level = weapon.level,
            description = weapon.description,
            attackPower = weapon.attackPower,
            icon = weapon.icon,
            range = weapon.range,
            projectileModel = weapon.projectileModel,
            usesInt = weapon.usesInt,
            quality = weapon.quality
        };
        obj.ConvertStatsFrom(weapon.stats);
        return obj;
    }
}

[System.Serializable]
public class SavedMeleeWeapon : SavedWeapon
{
    public new MeleeWeapon ConvertTo()
    {
        var obj = new MeleeWeapon {
            name = name,
            level = level,
            description = description,
            attackPower = attackPower,
            icon = icon,
            quality = quality,
        };
        obj.stats = ConvertStatsTo(statKeys, statValues);
        return obj;
    }
}

[System.Serializable]
public class SavedRangedWeapon : SavedWeapon
{
    public int range;
    public int projectileModel;
    public bool usesInt;

    public new RangedWeapon ConvertTo()
    {
        var obj = new RangedWeapon {
            name = name,
            level = level,
            description = description,
            attackPower = attackPower,
            icon = icon,
            range = range,
            projectileModel = projectileModel,
            usesInt = usesInt,
            quality = quality
        };
        obj.stats = ConvertStatsTo(statKeys, statValues);
        return obj;
    }
}

[System.Serializable]
public class SavedArmor : SavedItem
{
    public Armor ConvertTo()
    {
        var obj = new Armor {
            name = name,
            level = level,
            description = description,
            armor = armor,
            quality = quality
        };
        obj.stats = ConvertStatsTo(statKeys, statValues);
        return obj;
    }

    public static SavedArmor ConvertFrom(Armor armor) {
        if (armor == null) return null;
        var obj = new SavedArmor {
            name = armor.name,
            level = armor.level,
            description = armor.description,
            armor = armor.armor,
            quality = armor.quality
        };
        obj.ConvertStatsFrom(armor.stats);
        return obj;
    }
}

[System.Serializable]
public class SavedNecklace : SavedItem {
    public Necklace ConvertTo() {
        var obj = new Necklace {
            name = name,
            level = level,
            description = description,
            quality = quality
        };
        obj.stats = ConvertStatsTo(statKeys, statValues);
        return obj;
    }

    public static SavedNecklace ConvertFrom(Necklace accessory) {
        if (accessory == null) return null;
        var obj = new SavedNecklace {
            name = accessory.name,
            level = accessory.level,
            description = accessory.description,
            quality = accessory.quality
        };
        obj.ConvertStatsFrom(accessory.stats);
        return obj;
    }
}

[System.Serializable]
public class SavedBelt: SavedItem {
    public Belt ConvertTo() {
        var obj = new Belt {
            name = name,
            level = level,
            description = description,
            quality = quality
        };
        obj.stats = ConvertStatsTo(statKeys, statValues);
        return obj;
    }

    public static SavedBelt ConvertFrom(Belt accessory) {
        if (accessory == null) return null;
        var obj = new SavedBelt {
            name = accessory.name,
            level = accessory.level,
            description = accessory.description,
            quality = accessory.quality
        };
        obj.ConvertStatsFrom(accessory.stats);
        return obj;
    }
}

[System.Serializable]
public class SavedBracelet : SavedItem {
    public Bracelet ConvertTo() {
        var obj = new Bracelet {
            name = name,
            level = level,
            description = description,
            quality = quality
        };
        obj.stats = ConvertStatsTo(statKeys, statValues);
        return obj;
    }

    public static SavedBracelet ConvertFrom(Bracelet accessory) {
        if (accessory == null) return null;
        var obj = new SavedBracelet {
            name = accessory.name,
            level = accessory.level,
            description = accessory.description,
            quality = accessory.quality
        };
        obj.ConvertStatsFrom(accessory.stats);
        return obj;
    }
}

[System.Serializable]
public class SavedCloak : SavedItem {
    public Cloak ConvertTo() {
        var obj = new Cloak {
            name = name,
            level = level,
            description = description,
            quality = quality
        };
        obj.stats = ConvertStatsTo(statKeys, statValues);
        return obj;
    }

    public static SavedCloak ConvertFrom(Cloak accessory) {
        if (accessory == null) return null;
        var obj = new SavedCloak {
            name = accessory.name,
            level = accessory.level,
            description = accessory.description,
            quality = accessory.quality
        };
        obj.ConvertStatsFrom(accessory.stats);
        return obj;
    }
}

[System.Serializable]
public class SavedEarring : SavedItem {
    public Earring ConvertTo() {
        var obj = new Earring {
            name = name,
            level = level,
            description = description,
            quality = quality
        };
        obj.stats = ConvertStatsTo(statKeys, statValues);
        return obj;
    }

    public static SavedEarring ConvertFrom(Earring accessory) {
        if (accessory == null) return null;
        var obj = new SavedEarring {
            name = accessory.name,
            level = accessory.level,
            description = accessory.description,
            quality = accessory.quality
        };
        obj.ConvertStatsFrom(accessory.stats);
        return obj;
    }
}

[System.Serializable]
public class SavedHat : SavedItem {
    public Hat ConvertTo() {
        var obj = new Hat {
            name = name,
            level = level,
            description = description,
            armor = armor,
            quality = quality
        };
        obj.stats = ConvertStatsTo(statKeys, statValues);
        return obj;
    }

    public static SavedHat ConvertFrom(Hat accessory) {
        if (accessory == null) return null;
        var obj = new SavedHat {
            name = accessory.name,
            level = accessory.level,
            description = accessory.description,
            armor = accessory.armor,
            quality = accessory.quality
        };
        obj.ConvertStatsFrom(accessory.stats);
        return obj;
    }
}

[System.Serializable]
public class SavedShoes : SavedItem {
    public Shoes ConvertTo() {
        var obj = new Shoes {
            name = name,
            level = level,
            description = description,
            armor = armor,
            quality = quality
        };
        obj.stats = ConvertStatsTo(statKeys, statValues);
        return obj;
    }

    public static SavedShoes ConvertFrom(Shoes accessory) {
        if (accessory == null) return null;
        var obj = new SavedShoes {
            name = accessory.name,
            level = accessory.level,
            description = accessory.description,
            armor = accessory.armor,
            quality = accessory.quality
        };
        obj.ConvertStatsFrom(accessory.stats);
        return obj;
    }
}

[System.Serializable]
public class SavedConsumable : SavedItem
{
    public ConsumableType type;
    public float degree;
    public int quantity;

    public Consumable ConvertTo()
    {
        return new Consumable(type, degree, quantity);
    }

    public static SavedConsumable ConvertFrom(Consumable consumable) {
        if (consumable == null) {
            var obj = new SavedConsumable {
                type = ConsumableType.none,
                degree = 0,
                quantity = 0
            };
            return obj;
        }
        else {
            var obj = new SavedConsumable {
                type = consumable.type,
                degree = consumable.degree,
                quantity = consumable.quantity
            };
            return obj;
        }
    }
}

[System.Serializable]
public class SavedAbility {

    public static SavedAbility ConvertFrom(Ability ability) {
        if (ability is ActiveAbility) return SavedActiveAbility.ConvertFrom((ActiveAbility)ability);
        else return SavedPassiveAbility.ConvertFrom((PassiveAbility)ability);
    }

    public virtual Ability ConvertTo() {
        if (this is SavedActiveAbility) return ((SavedActiveAbility)this).ConvertTo();
        else return ((SavedPassiveAbility)this).ConvertTo();
    }
}

[System.Serializable]
public class SavedActiveAbility : SavedAbility
{
    public List<SavedAbilityAttribute> attributes = new List<SavedAbilityAttribute>();
    public int icon;
    public float cooldown = 0f;
    public float mpUsage = 0;
    public float radius = 0;
    public string name;
    public string description;
    public string targetType = "";
    public BaseStat baseStat;
    public float points = 70;
    public int level = 1;
    public long xp = 0;
    public float baseMpUsage = 0;
    public SavedSkillTree skillTree;

    public static SavedActiveAbility ConvertFrom(ActiveAbility ability)
    {
        if (ability is AttackAbility) return SavedAttackAbility.ConvertAttackFrom((AttackAbility)ability);
        var obj = new SavedActiveAbility();
        if (ability == null) {
            return null;
        }
        obj.targetType = ((UtilityAbility)ability).targetType;
        obj.icon = ability.icon;
        obj.cooldown = ability.cooldown;
        obj.mpUsage = ability.mpUsage;
        obj.baseMpUsage = ability.baseMpUsage;
        obj.radius = ability.radius;
        obj.name = ability.name;
        obj.description = ability.description;
        obj.baseStat = ability.baseStat;
        obj.points = ability.points;
        obj.xp = ability.xp;
        obj.level = ability.level;
        obj.skillTree = SavedSkillTree.ConvertFrom(ability.skillTree);
        foreach (var attribute in ability.attributes) obj.attributes.Add(SavedAbilityAttribute.ConvertFrom(attribute));
        return obj;
    }

    public virtual ActiveAbility ConvertTo()
    {
        if (this is SavedAttackAbility) return ((SavedAttackAbility)this).ConvertTo();
        var obj = new UtilityAbility {
            name = name,
            description = description,
            targetType = targetType,
            icon = icon,
            cooldown = cooldown,
            mpUsage = mpUsage
        };
        if (baseMpUsage == 0) baseMpUsage = mpUsage;
        obj.baseMpUsage = baseMpUsage;
        obj.radius = radius;
        obj.baseStat = baseStat;
        if (points == 0) points = 70;
        obj.points = points;
        obj.xp = xp;
        obj.level = level;
        obj.skillTree = skillTree.ConvertTo();
        foreach (var list in obj.skillTree.nodesByLayer) foreach (var item in list) item.ability = obj;
        foreach (var attribute in attributes) obj.attributes.Add(attribute.ConvertTo());
        return obj;
    }
}

[System.Serializable]
public class SavedAttackAbility : SavedActiveAbility
{
    public float damage = 0;
    public float dotDamage = 0;
    public float dotTime = 1;
    public Element element;
    public bool isRanged = false;
    public int rangedProjectile = 0;
    public int hitEffect = 0;
    public int aoe = 0;

    public static SavedAttackAbility ConvertAttackFrom(AttackAbility ability)
    {
        var obj = new SavedAttackAbility {
            icon = ability.icon,
            cooldown = ability.cooldown,
            mpUsage = ability.mpUsage,
            baseMpUsage = ability.baseMpUsage,
            radius = ability.radius,
            name = ability.name,
            description = ability.description,
            baseStat = ability.baseStat,
            damage = ability.damage,
            dotDamage = ability.dotDamage,
            dotTime = ability.dotTime,
            element = ability.element,
            isRanged = ability.isRanged,
            rangedProjectile = ability.rangedProjectile,
            hitEffect = ability.hitEffect,
            aoe = ability.aoe,
            points = ability.points,
            xp = ability.xp,
            level = ability.level
        };
        foreach (var attribute in ability.attributes) obj.attributes.Add(SavedAbilityAttribute.ConvertFrom(attribute));
        return obj;
    }

    public override ActiveAbility ConvertTo()
    {
        if (points == 0) points = 70;
        var obj = new AttackAbility {
            name = name,
            description = description,
            damage = damage,
            element = element,
            baseStat = baseStat,
            icon = icon,
            dotDamage = dotDamage,
            dotTime = dotTime,
            isRanged = isRanged,
            rangedProjectile = rangedProjectile,
            cooldown = cooldown,
            mpUsage = mpUsage,
            baseMpUsage = baseMpUsage,
            radius = radius,
            hitEffect = hitEffect,
            aoe = aoe,
            points = points,
            level = level,
            xp = xp
        };
        foreach (var attribute in attributes) obj.attributes.Add(attribute.ConvertTo());
        return obj;
    }
}

[System.Serializable]
public class SavedPassiveAbility : SavedAbility {
    public List<SavedAbilityAttribute> attributes = new List<SavedAbilityAttribute>();
    public int icon;
    public string name;
    public string description;
    public float points = 70;
    public int level = 1;
    public long xp = 0;
    public int baseMpUsage = 0;
    public SavedSkillTree skillTree;

    public static SavedPassiveAbility ConvertFrom(PassiveAbility ability) {
        var obj = new SavedPassiveAbility();
        if (ability == null) {
            return null;
        }
        obj.icon = ability.icon;
        obj.name = ability.name;
        obj.description = ability.description;
        obj.points = ability.points;
        obj.level = ability.level;
        obj.xp = ability.xp;
        obj.skillTree = SavedSkillTree.ConvertFrom(ability.skillTree);
        foreach (var attribute in ability.attributes) obj.attributes.Add(SavedAbilityAttribute.ConvertFrom(attribute));
        return obj;
    }

    public PassiveAbility ConvertTo() {
        var obj = new PassiveAbility {
            name=name,
            description=description,
            icon = icon
        };
        if (points == 0) points = 70;
        obj.points = points;
        obj.level = level;
        obj.xp = xp;
        obj.skillTree = skillTree.ConvertTo();
        foreach (var list in obj.skillTree.nodesByLayer) foreach (var item in list) item.ability = obj;
        foreach (var attribute in attributes) obj.attributes.Add(attribute.ConvertTo());
        return obj;
    }
}

[System.Serializable]
public class SavedSkillTree {
    public List<List<SavedSkillTreeNode>> nodes = new List<List<SavedSkillTreeNode>>();

    public static SavedSkillTree ConvertFrom(AbilitySkillTree skillTree) {
        var nodes = new List<List<SavedSkillTreeNode>>();
        foreach (var list in skillTree.nodesByLayer) {
            var newList = new List<SavedSkillTreeNode>();
            nodes.Add(newList);
            foreach (var item in list) {
                newList.Add(SavedSkillTreeNode.ConvertFrom(item));
            }
        }
        return new SavedSkillTree {
            nodes = nodes
        };
    }

    public AbilitySkillTree ConvertTo() {
        var ast = new AbilitySkillTree();
        foreach (var list in nodes) {
            var outputList = new List<AbilitySkillTreeNode>();
            ast.nodesByLayer.Add(outputList);
            foreach (var item in list) {
                outputList.Add(item.ConvertTo());
            }
        }
        ast.baseNodes = ast.nodesByLayer[0];
        return ast;
    }
}

public class SavedSkillTreeNode {
    public List<SavedSkillTreeNode> children = new List<SavedSkillTreeNode>();
    public List<SavedSoulGemEnhancement> effects = new List<SavedSoulGemEnhancement>();
    public bool clickable = false;
    public bool active = false;

    public static SavedSkillTreeNode ConvertFrom(AbilitySkillTreeNode node) {
        var sstn = new SavedSkillTreeNode() {
            clickable = node.clickable,
            active = node.active
        };
        foreach (var child in node.children) sstn.children.Add(ConvertFrom(child));
        foreach (var effect in node.effects) sstn.effects.Add(SavedSoulGemEnhancement.ConvertFrom(effect));
        return sstn;
    }

    public AbilitySkillTreeNode ConvertTo() {
        var obj = new AbilitySkillTreeNode {
            active = active,
            clickable = clickable
        };
        foreach (var child in children) obj.children.Add(child.ConvertTo());
        foreach (var effect in effects) obj.effects.Add(effect.ConvertTo());
        return obj;
    }
}

public class SavedSoulGemEnhancement {
    public string name = "";
    public string description = "";
    public string icon = "";
    public string generalType = "";
    public string type = "";
    public string target = "";
    public string subTarget = "";
    public float effect = 0f;

    public static SavedSoulGemEnhancement ConvertFrom(SoulGemEnhancement sge) {
        return new SavedSoulGemEnhancement {
            name = sge.name,
            description = sge.description,
            icon = sge.icon,
            generalType = sge.generalType,
            type = sge.type,
            target = sge.target,
            subTarget = sge.subTarget,
            effect = sge.effect
        };
    }

    public SoulGemEnhancement ConvertTo() {
        return new SoulGemEnhancement {
            name = name,
            description = description,
            icon = icon,
            generalType = generalType,
            type = type,
            target = target,
            subTarget = subTarget,
            effect = effect
        };
    }
}

//[System.Serializable]
//public class SavedElementalAffinity
//{
//    public Element type;
//    public int amount;

//    public static SavedElementalAffinity ConvertFrom(ElementalAffinity affinity)
//    {
//        var obj = new SavedElementalAffinity {
//            type = affinity.type,
//            amount = affinity.amount
//        };
//        return obj;
//    }

//    public ElementalAffinity ConvertTo()
//    {
//        var obj = new ElementalAffinity(type) {
//            amount = amount
//        };
//        return obj;
//    }
//}

[System.Serializable]
public class SavedAbilityAttribute
{
    public string type;
    public float points = 0;
    public float priority = 75f;
    public List<SavedAbilityParameter> parameters = new List<SavedAbilityParameter>();

    public static SavedAbilityAttribute ConvertFrom(AbilityAttribute attribute)
    {
        var obj = new SavedAbilityAttribute {
            type = attribute.type,
            points = attribute.points,
            priority = attribute.priority
        };
        foreach (var parameter in attribute.parameters) obj.parameters.Add(SavedAbilityParameter.ConvertFrom(parameter));
        return obj;
    }

    public AbilityAttribute ConvertTo()
    {
        var paramList = new AbilityAttributeParameter[parameters.Count];
        int i = -1;
        foreach (var parameter in parameters)
        {
            i++;
            paramList[i] = parameter.ConvertTo();
        }
        return new AbilityAttribute {
            type = type,
            points = points,
            priority = priority,
            parameters = paramList.ToList()
        };
    }
}

[System.Serializable]
public class SavedUtilityAbility : SavedActiveAbility {
    //placeholder
}

[System.Serializable]
public class SavedAbilityParameter {
    public string name;
    public string type;
    public int intVal;
    public float floatVal;
    public string stringVal;

    public static SavedAbilityParameter ConvertFrom(AbilityAttributeParameter parameter) {
        var obj = new SavedAbilityParameter {
            name = parameter.name
        };
        if (parameter.value is int) {
            obj.type = "int";
            obj.intVal = (int)parameter.value;
        }
        else if (parameter.value is float) {
            obj.type = "float";
            obj.floatVal = (float)parameter.value;
        }
        else {
            obj.type = "string";
            obj.stringVal = (string)parameter.value;
        }
        return obj;
    }

    public AbilityAttributeParameter ConvertTo() {
        var aap = new AbilityAttributeParameter {
            name=name
        };
        if (type == "int") aap.value = intVal;
        else if (type == "float") aap.value = floatVal;
        else aap.value = stringVal;
        return aap;
    }
}

[System.Serializable]
public class SavedDust {
    public string type;
    public float quantity;

    public static SavedDust ConvertFrom(Dust dust) {
        var obj = new SavedDust {
            type = dust.type,
            quantity = dust.quantity
        };
        return obj;
    }

    public Dust ConvertTo() {
        return new Dust(type, quantity);
    }
}