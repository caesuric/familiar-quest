using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class AbilityEffects {
    public static void Lifeleech(Character attacker, Character target, float damage, AbilityAttribute attr) {
        var amount = damage * (float)attr.FindParameter("degree").value;
        attacker.GetComponent<Health>().Heal(amount);
        GameObject obj = GameObject.Instantiate(attacker.GetComponent<CacheGrabber>().spellEffects[0]);
        var script = obj.GetComponent<LifeleechScript>();
        script.vampire = attacker.transform;
        script.victim = target.transform;
    }

    public static void Paralyze(Character attacker, Character target, AbilityAttribute attr) {
        target.GetComponent<StatusEffectHost>().AddStatusEffect("paralysis", (float)attr.FindParameter("duration").value, inflicter: attacker);
        var name = target.gameObject.name;
        if (name == "kittenCharacter(Clone)") name = "Player";
        target.GetComponent<ObjectSpawner>().CreateFloatingStatusText("Paralyzed!", name + " has been paralyzed!");
    }

    public static void Blind(Character attacker, Character target, AbilityAttribute attr) {
        target.GetComponent<StatusEffectHost>().AddStatusEffect("blind", (float)attr.FindParameter("duration").value, inflicter: attacker);
        var name = target.gameObject.name;
        if (name == "kittenCharacter(Clone)") name = "Player";
        target.GetComponent<ObjectSpawner>().CreateFloatingStatusText("Blinded!", name + " has been blinded!");
    }

    public static void Knockback(Character attacker, Character target, AbilityAttribute attr) {
        float degree = (float)attr.FindParameter("degree").value;
        var obj = target.gameObject.AddComponent<PushingEffect>();
        obj.Initialize(target.transform.position + attacker.transform.forward * degree, 0.25f);
        target.GetComponent<AbilityUser>().AddPushingEffect(target.transform.position + attacker.transform.forward * degree, 0.25f);
    }

    public static void KnockbackDefault(Character attacker, Character target) {
        var degree = 5f;
        var obj = target.gameObject.AddComponent<PushingEffect>();
        obj.Initialize(target.transform.position + attacker.transform.forward * degree, 0.5f);
        target.GetComponent<AbilityUser>().RpcAddPushingEffect(target.transform.position + attacker.transform.forward * degree, 0.5f);
    }

    public static void JumpBack(Character attacker, Character target, AbilityAttribute attr) {
        if (attr.FindParameter("degree") == null) return;
        var degree = (float)attr.FindParameter("degree").value;
        var obj = attacker.gameObject.AddComponent<PushingEffect>();
        obj.Initialize(attacker.transform.position - attacker.transform.forward * degree, 0.5f);
        attacker.GetComponent<AbilityUser>().RpcAddPushingEffect(target.transform.position - attacker.transform.forward * degree, 0.5f);
    }

    public static void PullTowards(Character attacker, Character target, AbilityAttribute attr) {
        if (attr.FindParameter("degree") == null) return;
        float distance = (float)attr.FindParameter("degree").value;
        if (distance >= 1 && distance <= 10) {
            var obj = target.gameObject.AddComponent<PushingEffect>();
            obj.Initialize(target.transform.position - attacker.transform.forward * (distance - 1), 0.5f);
            target.GetComponent<AbilityUser>().RpcAddPushingEffect(target.transform.position - attacker.transform.forward * (distance - 1), 0.5f);
        }
    }

    public static void PullTowardsDefault(Character attacker, Character target) {
        var distance = 5f;
        if (distance >= 1 && distance <= 10) {
            var obj = target.gameObject.AddComponent<PushingEffect>();
            obj.Initialize(target.transform.position - attacker.transform.forward * (distance - 1), 0.5f);
            target.GetComponent<AbilityUser>().RpcAddPushingEffect(target.transform.position - attacker.transform.forward * (distance - 1), 0.5f);
        }
    }

    public static void MpOverTime(Character attacker, AbilityAttribute attr) {
        attacker.GetComponent<StatusEffectHost>().AddStatusEffect("mpOverTime", (float)attr.FindParameter("duration").value, (float)attr.FindParameter("degree").value, good: true);
    }

    public static void ElementalDamageBuff(Character attacker, AbilityAttribute attr) {
        attacker.GetComponent<StatusEffectHost>().AddStatusEffect((string)attr.FindParameter("element").value + "DamageBuff", (float)attr.FindParameter("duration").value, (float)attr.FindParameter("degree").value, good: true);
    }

    public static void Blunting(AttackAbility ability, Character attacker, Character target, AbilityAttribute attr) {
        float degree = (float)attr.FindParameter("degree").value;
        float multiplier;
        if (attacker.GetComponent<PlayerCharacter>() != null) multiplier = attacker.GetComponent<PlayerCharacter>().weapon.attackPower;
        else multiplier = attacker.GetComponent<Monster>().attackFactor;
        multiplier *= attacker.GetComponent<Attacker>().GetBaseDamage(ability.baseStat);
        target.GetComponent<StatusEffectHost>().AddStatusEffect("blunting", 60, degree * multiplier);
    }

    public static void InflictVulnerability(Character target, AbilityAttribute attr) {
        target.GetComponent<StatusEffectHost>().AddStatusEffect("vulnerability", (float)attr.FindParameter("duration").value, (float)attr.FindParameter("degree").value);
    }

    public static void Delay(Character attacker, Character target, AttackAbility ability, float damage, AbilityAttribute attr) {
        target.GetComponent<StatusEffectHost>().AddStatusEffect("dde", (float)attr.FindParameter("time").value, degree: damage, inflicter: attacker, ability: ability);
    }

    public static void DamageShield(Character attacker, AttackAbility ability, AbilityAttribute attr) {
        var shieldAmount = (float)attr.FindParameter("degree").value * attacker.GetComponent<Attacker>().GetBaseDamage(ability.baseStat);
        attacker.GetComponent<StatusEffectHost>().AddStatusEffect("shield", 60f, degree: shieldAmount, good: true);
    }

    public static void RestoreMp(Character attacker, AbilityAttribute attr) {
        attacker.GetComponent<Mana>().mp = Mathf.Min(attacker.GetComponent<Mana>().mp + (float)attr.FindParameter("degree").value, attacker.GetComponent<Mana>().maxMP);
        attacker.GetComponent<ObjectSpawner>().CmdSpawnUnderParent(attacker.GetComponent<CacheGrabber>().spellEffects[10], attacker.gameObject);
    }

    public static void RemoveDebuff(Character attacker, AttackAbility ability, AbilityAttribute attr) {
        attacker.GetComponent<StatusEffectHost>().RemoveAnyDebuff(ability, resetCDOnFail: false);
        attacker.GetComponent<ObjectSpawner>().CmdSpawnUnderParent(attacker.GetComponent<CacheGrabber>().spellEffects[11], attacker.gameObject);
    }

    public static void AddedDot(Character attacker, Character target, AttackAbility ability, AbilityAttribute attr) {
        target.GetComponent<StatusEffectHost>().AddStatusEffect("dot", (float)attr.FindParameter("duration").value, degree: attacker.GetComponent<Attacker>().GetBaseDamage(ability.baseStat) * (float)attr.FindParameter("degree").value / (float)attr.FindParameter("duration").value, inflicter: attacker, ability: ability);
    }

    public static void SpeedMinus(Character target, AbilityAttribute attr) {
        var degree = (float)attr.FindParameter("degree").value;
        var duration = (float)attr.FindParameter("duration").value;
        target.GetComponent<StatusEffectHost>().AddStatusEffect("speed-", duration, degree);
    }

    public static void AttrSteal(Character attacker, Character target) {
        if (attacker.GetComponent<Gremlin>().item != null) return;
        if (PlayerCharacter.localPlayer.inventory.items.Count == 0) return;

        int i = UnityEngine.Random.Range(0, PlayerCharacter.localPlayer.inventory.items.Count);
        attacker.GetComponent<Gremlin>().item = PlayerCharacter.localPlayer.inventory.items[i];
        PlayerCharacter.localPlayer.inventory.items.RemoveAt(i);
        target.GetComponent<ObjectSpawner>().CreateFloatingStatusText("ITEM STOLEN", "A gremlin stole one of your items!");
    }
}
