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
    public List<SavedSpirit> spirits = new List<SavedSpirit>();
    public List<SavedAbility> overflowAbilities = new List<SavedAbility>();
    [OptionalField]
    public List<SavedDust> dust = new List<SavedDust>();
    public int currentAbility;
    public int currentAltAbility;
    public string selectedClass;
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
            weapon = SavedWeapon.StartingWeapon(ClassSelectMenu.selectedClass),
            armor = null,
            necklace = null,
            belt = null
        };
        obj.bracelets[0] = null;
        obj.bracelets[1] = null;
        obj.bracelets[2] = null;
        obj.bracelets[3] = null;
        obj.cloak = null;
        obj.earring = null;
        obj.hat = null;
        obj.shoes = null;
        //obj.spirits.Add(SavedSpirit.ConvertFrom(Spirit.classDefaults[ClassSelectMenu.selectedClass]));
        obj.dust = new List<SavedDust>();
        obj.currentAbility = 0;
        obj.currentAltAbility = 1;
        obj.selectedClass = ClassSelectMenu.selectedClass;
        obj.sparePoints = 0;
        obj.strength = ClassSelectMenu.strength;
        obj.dexterity = ClassSelectMenu.dexterity;
        obj.constitution = ClassSelectMenu.constitution;
        obj.intelligence = ClassSelectMenu.intelligence;
        obj.wisdom = ClassSelectMenu.wisdom;
        obj.luck = ClassSelectMenu.luck;
        obj.resurrectionTimer = 0;
        obj.name = name;
        obj.furType = furType;
        return obj;
    }

    public static SavedCharacter ConvertFrom(GameObject go) {
        var pc = go.GetComponent<PlayerCharacter>();
        var character = go.GetComponent<Character>();
        var su = go.GetComponent<SpiritUser>();
        var obj = new SavedCharacter();
        var du = go.GetComponent<DustUser>();
        obj.xp = pc.GetComponent<ExperienceGainer>().xp;
        obj.level = pc.GetComponent<ExperienceGainer>().level;
        obj.xpToLevel = pc.GetComponent<ExperienceGainer>().xpToLevel;
        obj.gold = pc.gold;
        obj.weapon = SavedWeapon.ConvertFrom(pc.weapon);
        obj.armor = SavedArmor.ConvertFrom(pc.armor);
        obj.necklace = SavedNecklace.ConvertFrom(pc.necklace);
        obj.belt = SavedBelt.ConvertFrom(pc.belt);
        obj.bracelets[0] = SavedBracelet.ConvertFrom(pc.bracelets[0]);
        obj.bracelets[1] = SavedBracelet.ConvertFrom(pc.bracelets[1]);
        obj.bracelets[2] = SavedBracelet.ConvertFrom(pc.bracelets[2]);
        obj.bracelets[3] = SavedBracelet.ConvertFrom(pc.bracelets[3]);
        obj.cloak = SavedCloak.ConvertFrom(pc.cloak);
        obj.earring = SavedEarring.ConvertFrom(pc.earring);
        obj.hat = SavedHat.ConvertFrom(pc.hat);
        obj.shoes = SavedShoes.ConvertFrom(pc.shoes);
        foreach (var consumable in pc.consumables) obj.consumables.Add(SavedConsumable.ConvertFrom(consumable));
        foreach (var spirit in character.GetComponent<SpiritUser>().spirits) obj.spirits.Add(SavedSpirit.ConvertFrom(spirit));
        foreach (var ability in su.overflowAbilities) obj.overflowAbilities.Add(SavedAbility.ConvertFrom(ability));
        foreach (var dust in du.dust) obj.dust.Add(SavedDust.ConvertFrom(dust));
        obj.currentAbility = character.GetComponent<InputController>().currentAbility;
        obj.currentAltAbility = character.GetComponent<InputController>().currentAltAbility;
        obj.selectedClass = pc.selectedClass;
        obj.sparePoints = pc.GetComponent<ExperienceGainer>().sparePoints;
        //obj.strength = character.strength;
        //obj.dexterity = character.dexterity;
        //obj.constitution = character.constitution;
        //obj.intelligence = character.intelligence;
        //obj.wisdom = character.wisdom;
        //obj.luck = character.luck;
        obj.strength = (int)CharacterAttribute.attributes["strength"].instances[character].BaseValue;
        obj.dexterity = (int)CharacterAttribute.attributes["dexterity"].instances[character].BaseValue;
        obj.constitution = (int)CharacterAttribute.attributes["constitution"].instances[character].BaseValue;
        obj.intelligence = (int)CharacterAttribute.attributes["intelligence"].instances[character].BaseValue;
        obj.wisdom = (int)CharacterAttribute.attributes["wisdom"].instances[character].BaseValue;
        obj.luck = (int)CharacterAttribute.attributes["luck"].instances[character].BaseValue;
        obj.resurrectionTimer = character.GetComponent<SpiritUser>().resurrectionTimer;
        obj.name = pc.GetComponent<PlayerSyncer>().characterName;
        obj.furType = pc.GetComponent<PlayerSyncer>().furType;
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
        foreach (var spirit in spirits) character.GetComponent<SpiritUser>().spirits.Add(spirit.ConvertTo());
        foreach (var ability in overflowAbilities) character.GetComponent<SpiritUser>().overflowAbilities.Add(ability.ConvertTo());
        if (dust!=null) foreach (var dustItem in dust) character.GetComponent<DustUser>().dust.Add(dustItem.ConvertTo());
        character.GetComponent<InputController>().currentAbility = currentAbility;
        character.GetComponent<InputController>().currentAltAbility = currentAltAbility;
        pc.selectedClass = selectedClass;
        pc.GetComponent<ExperienceGainer>().sparePoints = sparePoints;
        //character.strength = strength;
        //character.dexterity = dexterity;
        //character.constitution = constitution;
        //character.intelligence = intelligence;
        //character.wisdom = wisdom;
        //character.luck = luck;
        CharacterAttribute.attributes["strength"].instances[character].BaseValue = strength;
        CharacterAttribute.attributes["dexterity"].instances[character].BaseValue = dexterity;
        CharacterAttribute.attributes["constitution"].instances[character].BaseValue = constitution;
        CharacterAttribute.attributes["intelligence"].instances[character].BaseValue = intelligence;
        CharacterAttribute.attributes["wisdom"].instances[character].BaseValue = wisdom;
        CharacterAttribute.attributes["luck"].instances[character].BaseValue = luck;
        character.GetComponent<SpiritUser>().resurrectionTimer = resurrectionTimer;
        pc.GetComponent<PlayerSyncer>().characterName = name;
        pc.GetComponent<PlayerSyncer>().furType = furType;
        pc.GetComponent<PlayerSyncer>().furTypeSet = true;
    }
}

[System.Serializable]
public class SavedItem
{
    public string name;
    public string description;
    public Dictionary<string, int> stats;
    public int armor = 0;
    public int quality;

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
}

[System.Serializable]
public class SavedWeapon : SavedItem {
    public float attackPower;
    public string icon;

    public static SavedWeapon StartingWeapon(string selectedClass) {
        SavedWeapon obj;
        switch (selectedClass)
        {
            case "paladin":
            case "fighter":
            case "rogue":
            case "cleric":
            default:
                obj = new SavedMeleeWeapon {
                    attackPower = 0.8437f,
                    description = "",
                    icon = "Weapon_01",
                    name = "Starting Sword"
                };
                obj.description = "{{AttackPower}}";
                break;
            case "infernoMage":
            case "warlock":
                obj = new SavedRangedWeapon {
                    attackPower = 0.8437f,
                    description = "",
                    icon = "Weapon_16",
                    name = "Starting Wand"
                };
                obj.description = "{{AttackPower}}";
                ((SavedRangedWeapon)obj).projectileModel = 1;
                ((SavedRangedWeapon)obj).usesInt = false;
                break;
            case "archer":
                obj = new SavedRangedWeapon {
                    attackPower = 0.8437f,
                    description = "",
                    icon = "bow_2",
                    name = "Starting Bow"
                };
                obj.description = "{{AttackPower}}";
                ((SavedRangedWeapon)obj).projectileModel = 0;
                ((SavedRangedWeapon)obj).usesInt = false;
                break;
        }
        return obj;
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
            description = weapon.description,
            attackPower = weapon.attackPower,
            icon = weapon.icon,
            quality = weapon.quality,
            stats = weapon.stats
        };
        return obj;
    }

    public static SavedWeapon ConvertFromRangedWeapon(RangedWeapon weapon) {
        var obj = new SavedRangedWeapon {
            name = weapon.name,
            description = weapon.description,
            attackPower = weapon.attackPower,
            icon = weapon.icon,
            range = weapon.range,
            projectileModel = weapon.projectileModel,
            usesInt = weapon.usesInt,
            quality = weapon.quality,
            stats = weapon.stats
        };
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
            description = description,
            attackPower = attackPower,
            icon = icon,
            quality = quality,
            stats = stats
        };
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
            description = description,
            attackPower = attackPower,
            icon = icon,
            range = range,
            projectileModel = projectileModel,
            usesInt = usesInt,
            stats = stats,
            quality = quality
        };
        return obj;
    }
}

[System.Serializable]
public class SavedArmor : SavedItem
{
    public float hp;
    public float mp;

    public Armor ConvertTo()
    {
        var obj = new Armor {
            name = name,
            description = description,
            hp = hp,
            mp = mp,
            stats = stats,
            armor = armor,
            quality = quality
        };
        return obj;
    }

    public static SavedArmor ConvertFrom(Armor armor) {
        if (armor == null) return null;
        var obj = new SavedArmor {
            name = armor.name,
            description = armor.description,
            hp = armor.hp,
            mp = armor.mp,
            stats = armor.stats,
            armor = armor.armor,
            quality = armor.quality
        };
        return obj;
    }
}

[System.Serializable]
public class SavedNecklace : SavedItem {
    public Necklace ConvertTo() {
        var obj = new Necklace {
            name = name,
            description = description,
            stats = stats,
            quality = quality
        };
        return obj;
    }

    public static SavedNecklace ConvertFrom(Necklace accessory) {
        if (accessory == null) return null;
        var obj = new SavedNecklace {
            name = accessory.name,
            description = accessory.description,
            stats = accessory.stats,
            quality = accessory.quality
        };
        return obj;
    }
}

[System.Serializable]
public class SavedBelt: SavedItem {
    public Belt ConvertTo() {
        var obj = new Belt {
            name = name,
            description = description,
            stats = stats,
            quality = quality
        };
        return obj;
    }

    public static SavedBelt ConvertFrom(Belt accessory) {
        if (accessory == null) return null;
        var obj = new SavedBelt {
            name = accessory.name,
            description = accessory.description,
            stats = accessory.stats,
            quality = accessory.quality
        };
        return obj;
    }
}

[System.Serializable]
public class SavedBracelet : SavedItem {
    public Bracelet ConvertTo() {
        var obj = new Bracelet {
            name = name,
            description = description,
            stats = stats,
            quality = quality
        };
        return obj;
    }

    public static SavedBracelet ConvertFrom(Bracelet accessory) {
        if (accessory == null) return null;
        var obj = new SavedBracelet {
            name = accessory.name,
            description = accessory.description,
            stats = accessory.stats,
            quality = accessory.quality
        };
        return obj;
    }
}

[System.Serializable]
public class SavedCloak : SavedItem {
    public Cloak ConvertTo() {
        var obj = new Cloak {
            name = name,
            description = description,
            stats = stats,
            quality = quality
        };
        return obj;
    }

    public static SavedCloak ConvertFrom(Cloak accessory) {
        if (accessory == null) return null;
        var obj = new SavedCloak {
            name = accessory.name,
            description = accessory.description,
            stats = accessory.stats,
            quality = accessory.quality
        };
        return obj;
    }
}

[System.Serializable]
public class SavedEarring : SavedItem {
    public Earring ConvertTo() {
        var obj = new Earring {
            name = name,
            description = description,
            stats = stats,
            quality = quality
        };
        return obj;
    }

    public static SavedEarring ConvertFrom(Earring accessory) {
        if (accessory == null) return null;
        var obj = new SavedEarring {
            name = accessory.name,
            description = accessory.description,
            stats = accessory.stats,
            quality = accessory.quality
        };
        return obj;
    }
}

[System.Serializable]
public class SavedHat : SavedItem {
    public Hat ConvertTo() {
        var obj = new Hat {
            name = name,
            description = description,
            stats = stats,
            armor = armor,
            quality = quality
        };
        return obj;
    }

    public static SavedHat ConvertFrom(Hat accessory) {
        if (accessory == null) return null;
        var obj = new SavedHat {
            name = accessory.name,
            description = accessory.description,
            stats = accessory.stats,
            armor = accessory.armor,
            quality = accessory.quality
        };
        return obj;
    }
}

[System.Serializable]
public class SavedShoes : SavedItem {
    public Shoes ConvertTo() {
        var obj = new Shoes {
            name = name,
            description = description,
            stats = stats,
            armor = armor,
            quality = quality
        };
        return obj;
    }

    public static SavedShoes ConvertFrom(Shoes accessory) {
        if (accessory == null) return null;
        var obj = new SavedShoes {
            name = accessory.name,
            description = accessory.description,
            stats = accessory.stats,
            armor = accessory.armor,
            quality = accessory.quality
        };
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
public class SavedSpirit
{
    public List<SavedActiveAbility> activeAbilities = new List<SavedActiveAbility>();
    public List<SavedPassiveAbility> passiveAbilities = new List<SavedPassiveAbility>();
    public List<SavedElementalAffinity> elements = new List<SavedElementalAffinity>();
    public List<Element> types = new List<Element>();
    public string name;
    public string description = "";

    public static SavedSpirit ConvertFrom(Spirit spirit)
    {
        var obj = new SavedSpirit {
            types = spirit.types,
            name = spirit.name,
            description = spirit.description
        };
        foreach (var affinity in spirit.elements) obj.elements.Add(SavedElementalAffinity.ConvertFrom(affinity));
        //foreach (var ability in spirit.activeAbilities) if (ability != null) obj.activeAbilities.Add(SavedActiveAbility.ConvertFrom(ability));
        foreach (var ability in spirit.activeAbilities) obj.activeAbilities.Add(SavedActiveAbility.ConvertFrom(ability));
        foreach (var ability in spirit.passiveAbilities) obj.passiveAbilities.Add(SavedPassiveAbility.ConvertFrom(ability));
        return obj;
    }

    public Spirit ConvertTo()
    {
        var obj = new Spirit(1);
        obj.activeAbilities.Clear();
        obj.passiveAbilities.Clear();
        obj.elements.Clear();
        obj.types.Clear();
        foreach (var item in types) obj.types.Add(item);
        obj.name = name;
        obj.description = description;
        foreach (var ability in activeAbilities) {
            if (ability == null) obj.activeAbilities.Add(null);
            else obj.activeAbilities.Add(ability.ConvertTo());
        }
        foreach (var ability in passiveAbilities) {
            if (ability != null) obj.passiveAbilities.Add(ability.ConvertTo());
            else obj.passiveAbilities.Add(null);
        }
        foreach (var element in elements) obj.elements.Add(element.ConvertTo());
        return obj;
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
    public int mpUsage = 0;
    public float radius = 0;
    public string name;
    public string description;
    public string targetType = "";
    public BaseStat baseStat;
    [OptionalField]
    public int points = 70;
    [OptionalField]
    public int baseMpUsage = 0;

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
        foreach (var attribute in ability.attributes) obj.attributes.Add(SavedAbilityAttribute.ConvertFrom(attribute));
        return obj;
    }

    public virtual ActiveAbility ConvertTo()
    {
        if (this is SavedAttackAbility) return ((SavedAttackAbility)this).ConvertTo();
        var obj = new UtilityAbility {
            targetType = targetType,
            icon = icon,
            cooldown = cooldown,
            mpUsage = mpUsage
        };
        if (baseMpUsage == 0) baseMpUsage = mpUsage;
        obj.baseMpUsage = baseMpUsage;
        obj.radius = radius;
        obj.name = name;
        obj.description = description;
        obj.baseStat = baseStat;
        if (points == 0) points = 70;
        obj.points = points;
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
            points = ability.points
        };
        foreach (var attribute in ability.attributes) obj.attributes.Add(SavedAbilityAttribute.ConvertFrom(attribute));
        return obj;
    }

    public override ActiveAbility ConvertTo()
    {
        if (points == 0) points = 70;
        var obj = new AttackAbility(name, description, damage, element, baseStat, icon, dotDamage, dotTime, isRanged, rangedProjectile, cooldown, mpUsage, baseMpUsage, radius, hitEffect, aoe, points: points);
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
    [OptionalField]
    public int points = 70;
    [OptionalField]
    public int baseMpUsage = 0;

    public static SavedPassiveAbility ConvertFrom(PassiveAbility ability) {
        var obj = new SavedPassiveAbility();
        if (ability == null) {
            return null;
        }
        obj.icon = ability.icon;
        obj.name = ability.name;
        obj.description = ability.description;
        obj.points = ability.points;
        foreach (var attribute in ability.attributes) obj.attributes.Add(SavedAbilityAttribute.ConvertFrom(attribute));
        return obj;
    }

    public PassiveAbility ConvertTo() {
        var obj = new PassiveAbility(name, description) {
            icon = icon
        };
        if (points == 0) points = 70;
        obj.points = points;
        foreach (var attribute in attributes) obj.attributes.Add(attribute.ConvertTo());
        return obj;
    }
}

[System.Serializable]
public class SavedElementalAffinity
{
    public Element type;
    public int amount;

    public static SavedElementalAffinity ConvertFrom(ElementalAffinity affinity)
    {
        var obj = new SavedElementalAffinity {
            type = affinity.type,
            amount = affinity.amount
        };
        return obj;
    }

    public ElementalAffinity ConvertTo()
    {
        var obj = new ElementalAffinity(type) {
            amount = amount
        };
        return obj;
    }
}

[System.Serializable]
public class SavedAbilityAttribute
{
    public string type;
    public List<SavedAbilityParameter> parameters = new List<SavedAbilityParameter>();

    public static SavedAbilityAttribute ConvertFrom(AbilityAttribute attribute)
    {
        var obj = new SavedAbilityAttribute {
            type = attribute.type
        };
        foreach (var parameter in attribute.parameters) obj.parameters.Add(SavedAbilityParameter.ConvertFrom(parameter));
        return obj;
    }

    public AbilityAttribute ConvertTo()
    {
        var paramList = new AbilityParameter[parameters.Count];
        int i = -1;
        foreach (var parameter in parameters)
        {
            i++;
            paramList[i] = parameter.ConvertTo();
        }
        return new AbilityAttribute(type, paramList);
    }
}

[System.Serializable]
public class SavedUtilityAbility : SavedActiveAbility {
    //placeholder
}

[System.Serializable]
public class SavedAbilityParameter {
    public string name;
    public DataType type;
    public int intVal;
    public float floatVal;
    public string stringVal;

    public static SavedAbilityParameter ConvertFrom(AbilityParameter parameter) {
        var obj = new SavedAbilityParameter {
            name = parameter.name,
            type = parameter.type,
            intVal = parameter.intVal,
            floatVal = parameter.floatVal,
            stringVal = parameter.stringVal
        };
        return obj;
    }

    public AbilityParameter ConvertTo() {
        return new AbilityParameter(name, type, intVal, floatVal, stringVal);
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