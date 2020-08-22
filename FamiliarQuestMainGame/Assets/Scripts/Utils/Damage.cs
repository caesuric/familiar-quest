using System.Collections.Generic;
using UnityEngine;

public class Damage {
    public static void MeleeAttack(Character character, Collider other, string faction) {
        var otherCharacter = other.gameObject.GetComponent<Character>();
        if (otherCharacter != null && !otherCharacter.CompareTag(faction) && character.GetComponent<Attacker>().meleeAttackAbility != null) DamageCharacter(character, otherCharacter, character.GetComponent<Attacker>().meleeAttackAbility, character.GetComponent<Attacker>().meleeAttackDamage, character.GetComponent<Attacker>().meleeAttackAbility.element);
        else if (otherCharacter != null && !otherCharacter.CompareTag(faction) && character.GetComponent<Attacker>().meleeAttackAbility == null) DamageCharacter(character, otherCharacter, null, character.GetComponent<Attacker>().GetBaseDamage(BaseStat.strength), Element.none);
        else if (other.gameObject.GetComponent<LockedDoor>() != null) DamageDoor(character, other, character.GetComponent<Attacker>().GetBaseDamage(BaseStat.strength));
        else if (other.gameObject.GetComponent<LockedChest>() != null) DamageChest(character, other, character.GetComponent<Attacker>().GetBaseDamage(BaseStat.strength));
    }

    public static void TrapAttack(Collider other, int damage, GameObject gameObject) {
        bool destroy = true;
        var otherCharacter = other.gameObject.GetComponent<Character>();
        if (otherCharacter != null && !otherCharacter.CompareTag("Enemy") && damage != -1) {
            otherCharacter.GetComponent<SimulatedNoiseGenerator>().CmdMakeNoise(gameObject.transform.position, 22);
            otherCharacter.GetComponent<Health>().TakeDamageFromTrap(damage, Element.piercing);
        }
        else if (other.gameObject.CompareTag("Wall")) MakeNoiseByName("sfx_wall_hit2", 0);
        else if (other.gameObject.GetComponent<LockedDoor>() != null) DamageDoor(null, other, damage);
        else if (other.gameObject.GetComponent<LockedChest>() != null) DamageChest(null, other, damage);
        else destroy = false;
        if (destroy) gameObject.GetComponentInChildren<TrapArrowDealDamage>().armed = false;
    }

    public static void ProjectileAttack(Character character, Collider other, AttackAbility ability, int damage, float radius, float dotDamage, float dotTime, List<AbilityAttribute> attributes, GameObject gameObject, string faction) {
        var otherCharacter = other.gameObject.GetComponent<Character>();
        if (other.GetComponent<LockedDoor>() != null || other.GetComponent<LockedChest>() != null || (otherCharacter != null && !otherCharacter.CompareTag(faction)) || other.gameObject.CompareTag("Wall")) {
            if (ability != null && ability.FindAttribute("createDamageZone") != null) CreateDamageZone(character, gameObject, ability, radius, faction);
            else if (radius > 0) CreateAoe(character, gameObject, ability, attributes, damage, radius, faction);
            else if (otherCharacter != null && ability != null) DamageCharacterWithProjectile(character, otherCharacter, ability, gameObject, damage, ability.element);
            else if (otherCharacter != null && character.GetComponent<PlayerCharacter>() != null) DamageCharacterWithProjectile(character, otherCharacter, null, gameObject, damage, Element.none);
            else if (other.gameObject.GetComponent<LockedDoor>() != null) DamageDoor(character, other, damage);
            else if (other.gameObject.GetComponent<LockedChest>() != null) DamageChest(character, other, damage);
            else if (other.gameObject.CompareTag("Wall")) {
                Dictionary<Element, string> noises = new Dictionary<Element, string>() { { Element.acid, "sfx_acid_damage1" }, { Element.bashing, "sfx_wall_hit2" }, { Element.fire, "sfx_fire_damage2" }, { Element.ice, "sfx_ice_damage1" }, { Element.none, "sfx_rock_impact2" }, { Element.piercing, "sfx_wall_hit2" }, { Element.slashing, "sfx_wall_hit2" }, { Element.light, "sfx_holy_damage1" }, { Element.dark, "sfx_profane_damage1" } };
                var noise = "sfx_rock_impact2";
                if (ability != null && noises.ContainsKey(ability.element)) noise = noises[ability.element];
                if (noise != "") MakeNoiseByName(noise, 0, null, null);
            }
            gameObject.GetComponentInChildren<RangedHitboxDealDamage>().struck = true;
        }
    }

    public static void DamageCharacter(Character character, Character otherCharacter, AttackAbility ability, float damage, Element element) {
        if (ability != null) {
            var pushingEffects = character.gameObject.GetComponents<PushingEffect>();
            foreach (var pushingEffect in pushingEffects) GameObject.Destroy(pushingEffect, 0.01f);
            otherCharacter.GetComponent<Health>().TakeDamage(damage, ability.element, character, ability: ability);
        }
        else otherCharacter.GetComponent<Health>().TakeDamage(damage, Element.none, character);
        character.GetComponent<Attacker>().isAttacking = false;
    }

    public static void DamageCharacterWithProjectile(Character character, Character otherCharacter, AttackAbility ability, GameObject gameObject, float damage, Element element) {
        if (character == null || otherCharacter == null) return;
        character.GetComponent<SimulatedNoiseGenerator>().CmdMakeNoise(gameObject.transform.position, 22);
        DamageCharacter(character, otherCharacter, ability, damage, element);
        if (ability != null) {
            var attr = ability.FindAttribute("knockback");
            if (attr != null && attr.priority >= 50) otherCharacter.transform.position += gameObject.transform.forward * (float)attr.FindParameter("degree").value;
        }
    }

    public static void DamageDoor(Character character, Collider other, float damage) {
        other.gameObject.GetComponent<LockedDoor>().TakeDamage(damage, character.gameObject);
        character.GetComponent<SimulatedNoiseGenerator>().CmdMakeNoise(other.transform.position, 27);
    }

    public static void DamageChest(Character character, Collider other, float damage) {
        other.gameObject.GetComponent<LockedChest>().TakeDamage(damage, character.gameObject);
        character.GetComponent<SimulatedNoiseGenerator>().CmdMakeNoise(other.transform.position, 27);
    }

    public static void MakeHitNoise(int volume, Character character = null) {
        GameObject go = null;
        if (character) go = character.gameObject;
        MakeNoise(2, volume, go, character);
    }

    public static void MakeExplosionNoise(float volume, GameObject gameObject, Character character = null) {
        MakeNoise(1, volume, gameObject, character);
    }

    public static void MakeNoise(int noise, float volume, GameObject gameObject = null, Character character = null) {
        if (character == null) character = PlayerCharacter.localPlayer.GetComponent<Character>();
        if (gameObject == null) gameObject = character.gameObject;
        character.GetComponent<AudioGenerator>().PlaySound(noise);
        character.GetComponent<SimulatedNoiseGenerator>().CmdMakeNoise(gameObject.transform.position, volume);
    }

    public static void MakeNoiseByName(string noise, float volume, GameObject gameObject = null, Character character = null) {
        if (character == null) character = PlayerCharacter.localPlayer.GetComponent<Character>();
        if (gameObject == null) gameObject = character.gameObject;
        character.GetComponent<AudioGenerator>().PlaySoundByName(noise);
        if (volume > 0) character.GetComponent<SimulatedNoiseGenerator>().CmdMakeNoise(gameObject.transform.position, volume);
    }

    public static void CreateDamageZone(Character character, GameObject gameObject, AttackAbility ability, float radius, string faction) {
        var obj = GameObject.Instantiate(character.GetComponent<CacheGrabber>().damageZones[ability.aoe], gameObject.transform.position, gameObject.transform.rotation);
        obj.transform.Rotate(-90f, 0f, 0f);
        obj.GetComponent<DamageZoneDealDamage>().size = radius;
        var damageZoneDealDamage = obj.GetComponent<DamageZoneDealDamage>();
        if (damageZoneDealDamage != null) {
            damageZoneDealDamage.ability = ability;
            damageZoneDealDamage.character = character;
            damageZoneDealDamage.faction = faction;
        }
        var noises = new Dictionary<Element, string>() { { Element.acid, "sfx_acid_damage_aoe" }, { Element.bashing, "sfx_bashing_damage1" }, { Element.fire, "sfx_fire_damage_aoe2" }, { Element.ice, "sfx_ice_damage_aoe1" }, { Element.none, "sfx_bashing_damage1" }, { Element.piercing, "sfx_bashing_damage1" }, { Element.slashing, "sfx_slashing_damage1" }, { Element.light, "sfx_holy_damage_aoe1" }, { Element.dark, "sfx_profane_damage_aoe1" } };
        var noise = "";
        if (noises.ContainsKey(ability.element)) noise = noises[ability.element];
        if (noise != "") MakeNoiseByName(noise, 32, gameObject, character);
    }

    public static void CreateAoe(Character character, GameObject gameObject, AttackAbility ability, List<AbilityAttribute> attributes, int damage, float radius, string faction) {
        var obj = GameObject.Instantiate(character.GetComponent<CacheGrabber>().aoes[ability.aoe], gameObject.transform.position, gameObject.transform.rotation);
        obj.GetComponent<AOEDealDamage>().size = radius;
        var ps = obj.GetComponentInChildren<ParticleSystem>();
        if (ps != null) {
            var main = ps.main;
            main.startSize = radius;
            ps.Play();
        }
        obj.GetComponent<AOEDealDamage>().Initialize(damage, radius, attributes, faction, character, ability);
        var noises = new Dictionary<Element, string>() { { Element.acid, "sfx_acid_damage_aoe" }, { Element.bashing, "sfx_bashing_damage1" }, { Element.fire, "sfx_fire_damage_aoe2" }, { Element.ice, "sfx_ice_damage_aoe1" }, { Element.none, "sfx_bashing_damage1" }, { Element.piercing, "sfx_bashing_damage1" }, { Element.slashing, "sfx_slashing_damage1" }, { Element.light, "sfx_holy_damage_aoe1" }, { Element.dark, "sfx_profane_damage_aoe1" } };
        var noise = "";
        if (noises.ContainsKey(ability.element)) noise = noises[ability.element];
        if (noise != "") MakeNoiseByName(noise, 32, gameObject, character);
    }
}
