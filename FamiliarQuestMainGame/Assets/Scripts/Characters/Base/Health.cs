using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(StatusEffectHost))]
[RequireComponent(typeof(ObjectSpawner))]
public class Health : MonoBehaviour {

    //[SyncVar]
    public float hp;
    //[SyncVar]
    public float maxHP;
    //public float healingMultiplier;
    private UnitFrame unitFrame = null;
    private bool isPlayer = false;
    private delegate void Effect(AbilityAttribute Attribute);
    private bool initialCalculationComplete = false;
    public float bossHpFactor = 1f;

    // Use this for initialization
    void Start() {
        Calculate();
        hp = maxHP;
        if (GetComponent<PlayerCharacter>() != null) isPlayer = true;
        else unitFrame = GetComponentInChildren<UnitFrame>();
    }

    // Update is called once per frame
    void Update() {
        if (!initialCalculationComplete) Calculate();
        if (maxHP == 0) return;
        if (hp <= 0) {
            hp = 0;
            if (GetComponent<Monster>() != null) GetComponent<MonsterMortal>().OnDeath();
        }
        else if (hp > maxHP) hp = maxHP;
        if (!isPlayer && unitFrame != null) unitFrame.SetHealthPercentage(hp / maxHP * 100f);
    }

    public void Calculate() {
        float hpFactor = 29;
        if (GetComponent<Monster>() != null) hpFactor = GetComponent<Monster>().hpFactor;
        else if (GetComponent<PlayerCharacter>() != null) hpFactor = GetHpFactor(hpFactor, GetComponent<ExperienceGainer>().level);
        //int newHP = (int)((float)GetComponent<Character>().constitution * hpFactor);
        int newHP = (int)(CharacterAttribute.attributes["bonusHp"].instances[GetComponent<Character>()].TotalValue * bossHpFactor);
        hp += (newHP - maxHP);
        maxHP = newHP;
        if (newHP != 0) initialCalculationComplete = true;
    }

    public float GetHpFactor(float baseFactor, int level) {
        for (var i = 1; i < level; i++) baseFactor *= 1.1f;
        return baseFactor;
    }

    public void Heal(float amount, bool silent = false, bool noEffect = false) {
        if (hp <= 0) return;
        //if (GetComponent<PlayerCharacter>()!=null) AggroTable.IncreaseAggroWithAll(gameObject, amount);
        hp = Mathf.Min(hp + amount, maxHP);
        var name = gameObject.name;
        if (name == "kittenCharacter(Clone)") name = "Player";
        if (!silent) GetComponent<ObjectSpawner>().CreateFloatingHealingText((int)amount, name + " healed for " + amount.ToString() + ".");
        if (!noEffect) GetComponent<ObjectSpawner>().CmdSpawnUnderParent(GetComponent<CacheGrabber>().spellEffects[11], gameObject);
    }

    public void TakeDamage(float amount, Element type, Character attacker, bool silent = false, AttackAbility ability = null, Transform projectileTransform = null) {
        var originalAmount = amount;
        if (GetComponent<MirrorImage>() != null) {
            Destroy(gameObject);
            return;
        }
        if (!silent) MakeDamageHitNoise(type);
        GetComponent<StatusEffectHost>().RemoveStealth();
        //if (GetComponent<Monster>() != null && attacker != null) GetComponent<AggroTable>().IncreaseAggro(attacker.gameObject, amount);
        ShowHitVisual(ability);
        var criticalRoll = UnityEngine.Random.Range(0f, 1f);
        amount = ModifyDamage(amount, criticalRoll, type, attacker, ability);
        if (BossImmunity()) amount = 0;
        if (ability == null || ability.FindAttribute("delay") == null || ability.FindAttribute("delay").priority < 50) hp -= amount;
        if (hp <= 0 && GetComponent<MonsterMortal>() != null) GetComponent<MonsterMortal>().killer = attacker.gameObject;
        if (amount >= 0 && ability != null) ApplyEffectsFromAttack(attacker, ability, amount, projectileTransform);
        if (amount > 0 && GetComponent<MonsterSounds>() != null) GetComponentInChildren<AudioGenerator>().PlaySoundByName(GetComponent<MonsterSounds>().onHit);
        if (amount > 0 && GetComponent<OchreJelly>() != null) GetComponent<OchreJelly>().Split();
        if (ability == null || ability.FindAttribute("delay") == null || ability.FindAttribute("delay").priority < 50) {
            if (!silent && amount == 0 && amount != originalAmount) CreateFloatingImmunityText(attacker);
            else if (!silent) CreateFloatingText(amount, criticalRoll, attacker, ability);
            ResurrectIfApplicable();
        }
    }

    private bool BossImmunity() {
        var boss = GetComponent<Boss>();
        if (boss == null) return false;
        if (boss.adds.Count > 0 && boss.addType == "bodyguard") return true;
        return false;
    }

    public void MakeDamageHitNoise(Element type) {
        Dictionary<Element, string> sounds = new Dictionary<Element, string>() { { Element.acid, "sfx_acid_damage1" }, { Element.bashing, "" }, { Element.dark, "" }, { Element.fire, "sfx_fire_damage2" }, { Element.ice, "sfx_ice_damage1" }, { Element.light, "" }, { Element.none, "" }, { Element.piercing, "" }, { Element.slashing, "" } };
        string sound = "";
        if (sounds.ContainsKey(type)) sound = sounds[type];
        if (sound != "") GetComponent<AudioGenerator>().PlaySoundByName(sound);
    }

    public void TakeDamageFromDot(float amount, Element type, Character attacker, bool silent = false, AttackAbility ability = null, Transform projectileTransform = null) {
        GetComponent<StatusEffectHost>().RemoveStealth();
        //if (GetComponent<Monster>() != null && attacker != null) GetComponent<AggroTable>().IncreaseAggro(attacker.gameObject, amount);
        ShowHitVisual(ability);
        var criticalRoll = UnityEngine.Random.Range(0f, 1f);
        amount = ModifyDamage(amount, criticalRoll, type, attacker, ability);
        hp -= amount;
        //if (amount >= 0 && ability != null) ApplyEffectsFromAttack(attacker, ability, amount, projectileTransform);
        if (hp <= 0 && GetComponent<MonsterMortal>() != null) GetComponent<MonsterMortal>().killer = attacker.gameObject;
        if (!silent) CreateFloatingText(amount, criticalRoll, attacker, ability);
        ResurrectIfApplicable();
    }

    private void ShowHitVisual(AttackAbility ability) {
        if (ability != null && !ability.isRanged) GetComponent<ObjectSpawner>().CmdSpawnWithPosition(GetComponent<CacheGrabber>().hitEffects[ability.hitEffect], transform.position, transform.rotation);
        else if (ability == null) GetComponent<ObjectSpawner>().CmdSpawnWithPosition(GetComponent<CacheGrabber>().hitEffects[1], transform.position, transform.rotation);
    }

    private void CreateFloatingText(float amount, float criticalRoll, Character attacker, AttackAbility ability) {
        if (attacker == null || gameObject == null) return;
        var name = gameObject.name;
        if (name == "kittenCharacter(Clone)") name = "Player";
        var attackerName = attacker.gameObject.name;
        if (attackerName == "kittenCharacter(Clone)") attackerName = "Player";
        //var critRate = attacker.GetComponent<Attacker>().critRate;
        var critRate = CharacterAttribute.attributes["criticalHitChance"].instances[attacker.GetComponent<Character>()].TotalValue / 100f;
        if (ability != null && ability.FindAttribute("increasedCritChance") != null) critRate += (float)ability.FindAttribute("increasedCritChance").FindParameter("degree").value;
        if (amount < 0) GetComponent<ObjectSpawner>().CreateFloatingHealingText((int)amount * -1, name + " healed for " + amount.ToString() + ".");
        else if (attacker != null && attacker.GetComponent<Attacker>() != null && GetComponent<ObjectSpawner>() != null && criticalRoll > critRate && amount >= maxHP / 100f && name == "Player") GetComponent<ObjectSpawner>().CreateFloatingDamageText((int)amount, attackerName, name);
        else if (attacker != null && attacker.GetComponent<Attacker>() != null && GetComponent<ObjectSpawner>() != null && criticalRoll > critRate && name != "Player") GetComponent<ObjectSpawner>().CreateFloatingDamageText((int)amount, attackerName, name);
        else if (name == "Player" && amount >= maxHP / 100f) GetComponent<ObjectSpawner>().CreateCriticalFloatingDamageText((int)amount, attackerName, name);
        else if (name != "Player") GetComponent<ObjectSpawner>().CreateCriticalFloatingDamageText((int)amount, attackerName, name);
    }

    private void CreateFloatingImmunityText(Character attacker) {
        var name = gameObject.name;
        if (name == "kittenCharacter (Clone)") name = "Player";
        var attackerName = attacker.gameObject.name;
        if (attackerName == "kittenCharacter (Clone)") name = "Player";
        GetComponent<ObjectSpawner>().CreateFloatingImmunityText(name, attackerName);
    }

    private void ResurrectIfApplicable() {
        //if (hp <= 0 && GetComponent<AbilityUser>().HasPassive("resurrectOnDeath") && GetComponent<SpiritUser>().resurrectionTimer == 0) {
        //    hp = maxHP;
        //    GetComponent<AbilityUser>().resurrectionTimer = 60 * 60;
        //}
    }

    private float ModifyDamage(float amount, float criticalRoll, Element type, Character attacker, AttackAbility ability) {
        if (GetComponent<Monster>() != null) amount = GetComponent<Monster>().ModifyDamageForElements(amount, type);
        float critRate = 0;
        //if (attacker != null && attacker.GetComponent<Attacker>() != null) critRate = attacker.GetComponent<Attacker>().critRate;
        if (attacker != null && attacker.GetComponent<Attacker>() != null) critRate = CharacterAttribute.attributes["criticalHitChance"].instances[GetComponent<Character>()].TotalValue / 100f;
        if (ability != null && ability.FindAttribute("increasedCritChance") != null) critRate += (float)ability.FindAttribute("increasedCritChance").FindParameter("degree").value;
        float critMultiplier = 0;
        //if (attacker != null && attacker.GetComponent<Attacker>() != null) critMultiplier = attacker.GetComponent<Attacker>().critMultiplier;
        if (attacker != null && attacker.GetComponent<Attacker>() != null) critMultiplier = CharacterAttribute.attributes["criticalDamage"].instances[GetComponent<Character>()].TotalValue / 100f;
        if (ability != null && ability.FindAttribute("increasedCritDamage") != null) critMultiplier += (float)ability.FindAttribute("increasedCritDamage").FindParameter("degree").value;
        if (attacker != null && criticalRoll <= critRate) amount *= critMultiplier;
        amount = ModifyDamageForVulnerability(amount);
        amount = ModifyDamageForShield(amount);
        amount = ModifyDamageForArmor(amount, attacker);
        amount = ModifyDamageForLuck(amount, type);
        if (attacker != null) amount = ModifyDamageForDamageBoosts(ability, amount, attacker);
        amount = ModifyDamageForDamageReduction(ability, amount);
        if (attacker != null) amount = ModifyDamageForBlunting(amount, attacker);
        return amount;
    }

    private float ModifyDamageForDamageBoosts(AttackAbility ability, float amount, Character attacker) {
        var abilityUser = attacker.GetComponent<AbilityUser>();
        if (abilityUser.soulGemPassive == null) return amount;
        foreach (var attribute in abilityUser.soulGemPassive.attributes) {
            if (attribute.type == "boostDamage") {
                amount *= (1 + (float)attribute.FindParameter("degree").value);
            }
            else if (attribute.type == "boostElementalDamage" && (string)attribute.FindParameter("element").value == ability.element.ToString()) {
                amount *= (1 + (float)attribute.FindParameter("degree").value);
            }
        }
        return amount;
    }

    private float ModifyDamageForDamageReduction(AttackAbility ability, float amount) {
        var abilityUser = GetComponent<AbilityUser>();
        if (abilityUser.soulGemPassive == null) return amount;
        foreach (var attribute in abilityUser.soulGemPassive.attributes) {
            if (attribute.type == "reduceDamage") {
                amount *= (1 - (float)attribute.FindParameter("degree").value);
            }
            else if (attribute.type == "reduceElementalDamage" && (string)attribute.FindParameter("element").value == ability.element.ToString()) {
                amount *= (1 - (float)attribute.FindParameter("degree").value);
            }
        }
        return amount;
    }

    private float ModifyDamageForLuck(float amount, Element type) {
        //if (type == Element.bashing || type == Element.piercing || type == Element.slashing || type == Element.none) return amount;
        //var level = 1;
        //if (GetComponent<ExperienceGainer>() != null) level = GetComponent<ExperienceGainer>().level;
        //else level = GetComponent<MonsterScaler>().level;
        //return amount * (1 - SecondaryStatUtility.CalcElementalResistanceFromLuck(GetComponent<Character>().luck, level));
        if (type == Element.none) return amount;
        var resistances = new Dictionary<Element, string> {
            {  Element.acid, "acidResistance" },
            {  Element.fire, "fireResistance" },
            {  Element.ice, "iceResistance" },
            {  Element.light, "lightResistance" },
            {  Element.dark, "darkResistance" },
            {  Element.piercing, "piercingResistance" },
            {  Element.bashing, "bashingResistance" },
            {  Element.slashing, "slashingResistance" }
        };
        var resistance = resistances[type];
        return amount * (1 - CharacterAttribute.attributes[resistance].instances[GetComponent<Character>()].TotalValue / 100f);
    }

    private float ModifyDamageForArmor(float amount, Character attacker) {
        var pc = GetComponent<PlayerCharacter>();
        if (pc == null || attacker == null) return amount;
        var scaler = attacker.GetComponent<MonsterScaler>();
        if (scaler == null) return amount;
        var attackerLevel = scaler.level;
        var armorValue = 0;
        if (pc.armor != null) armorValue += pc.armor.armor;
        if (pc.shoes != null) armorValue += pc.shoes.armor;
        if (pc.hat != null) armorValue += pc.hat.armor;
        var multiplier = 1f - (armorValue / (armorValue + 400f + (85f * attackerLevel)));
        var prevented = 1f - multiplier;
        //prevented *= SecondaryStatUtility.CalcArmorMultiplier(GetComponent<Character>().constitution, GetComponent<ExperienceGainer>().level);
        prevented *= CharacterAttribute.attributes["armorMultiplier"].instances[GetComponent<Character>()].TotalValue / 100f;
        multiplier = 1f - prevented;
        return amount * multiplier;
    }

    private float ModifyDamageForVulnerability(float amount) {
        var vulnerability = GetComponent<StatusEffectHost>().GetEffect("vulnerability");
        if (vulnerability != null) amount *= (1 + (vulnerability.degree / 100));
        return amount;
    }

    private float ModifyDamageForShield(float amount) {
        var shield = GetComponent<StatusEffectHost>().GetEffect("shield");
        if (shield != null) {
            var originalAmount = amount;
            amount = Mathf.Max(0, amount - (int)shield.degree);
            shield.degree -= originalAmount;
            if (shield.degree <= 0) GetComponent<StatusEffectHost>().RemoveEffect(shield);
        }
        return amount;
    }

    private float ModifyDamageForBlunting(float amount, Character attacker) {
        var blunting = attacker.GetComponent<StatusEffectHost>().GetEffect("blunting");
        if (blunting != null) {
            var originalAmount = amount;
            amount = Mathf.Max(0, amount - (int)blunting.degree);
            blunting.degree -= originalAmount;
            if (blunting.degree <= 0) GetComponent<StatusEffectHost>().RemoveEffect(blunting);
        }
        return amount;
    }

    public void TakeDamageFromTrap(float amount, Element type, bool silent = false) {
        TakeDamage(amount, type, null, silent);
    }

    public void ApplyEffectsFromAttack(Character attacker, AttackAbility ability, float damage, Transform projectileTransform) {
        Dictionary<string, Effect> effects = SetUpEffectDictionary(attacker, ability, damage, projectileTransform);
        int count = 0;
        foreach (var attribute in ability.attributes) {
            if (effects.ContainsKey(attribute.type) && attribute.priority >= 50 && count < 4) effects[attribute.type](attribute);
            count++;
        }
        if (attacker != null && attacker.GetComponent<AbilityUser>().HasPassive("knockback")) AbilityEffects.KnockbackDefault(attacker, GetComponent<Character>());
        if (attacker != null && attacker.GetComponent<AbilityUser>().HasPassive("pullEnemies")) AbilityEffects.PullTowardsDefault(attacker, GetComponent<Character>());
        if (ability.FindAttribute("createDamageZone") == null && ability.dotDamage > 0) GetComponent<StatusEffectHost>().AddStatusEffect("dot", ability.dotTime, degree: ability.CalculateDotDamage(attacker), inflicter: attacker, ability: ability);
    }

    private Dictionary<string, Effect> SetUpEffectDictionary(Character attacker, AttackAbility ability, float damage, Transform projectileTransform) {
        return new Dictionary<string, Effect>() {
            { "lifeleech", (AbilityAttribute attribute) => AbilityEffects.Lifeleech(attacker, GetComponent<Character>(), damage, attribute) },
            { "paralyze", (AbilityAttribute attribute) => AbilityEffects.Paralyze(attacker, GetComponent<Character>(), attribute) },
            { "blind", (AbilityAttribute attribute) => AbilityEffects.Blind(attacker, GetComponent<Character>(), attribute) },
            { "knockback", (AbilityAttribute attribute) => AbilityEffects.Knockback(attacker, GetComponent<Character>(), attribute) },
            { "jumpBack", (AbilityAttribute attribute) => AbilityEffects.JumpBack(attacker, GetComponent<Character>(), attribute) },
            { "pullTowards", (AbilityAttribute attribute) => AbilityEffects.PullTowards(attacker, GetComponent<Character>(), attribute) },
            { "mpOverTime", (AbilityAttribute attribute) => AbilityEffects.MpOverTime(attacker, attribute) },
            { "elementalDamageBuff", (AbilityAttribute attribute) => AbilityEffects.ElementalDamageBuff(attacker, attribute) },
            { "blunting", (AbilityAttribute attribute) => AbilityEffects.Blunting(ability, attacker, GetComponent<Character>(), attribute) },
            { "inflictVulnerability", (AbilityAttribute attribute) => AbilityEffects.InflictVulnerability(GetComponent<Character>(), attribute) },
            { "delay", (AbilityAttribute attribute) => AbilityEffects.Delay(attacker, GetComponent<Character>(), ability, damage, attribute) },
            { "damageShield", (AbilityAttribute attribute) => AbilityEffects.DamageShield(attacker, ability, attribute) },
            { "restoreMP", (AbilityAttribute attribute) => AbilityEffects.RestoreMp(attacker, attribute) },
            { "removeDebuff", (AbilityAttribute attribute) => AbilityEffects.RemoveDebuff(attacker, ability, attribute) },
            { "addedDot", (AbilityAttribute attribute) => AbilityEffects.AddedDot(attacker, GetComponent<Character>(), ability, attribute) },
            { "speed-", (AbilityAttribute attribute) => AbilityEffects.SpeedMinus(GetComponent<Character>(), attribute) },
            { "steal", (AbilityAttribute attribute) => AbilityEffects.AttrSteal(attacker, GetComponent<Character>()) }
        };
    }
}
