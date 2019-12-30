using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class Effects {
    public static void Lifeleech(Character attacker, Character target, float damage, AbilityAttribute attr) {
        var amount = damage * attr.FindParameter("degree").floatVal;
        attacker.GetComponent<Health>().Heal(amount);
        GameObject obj = GameObject.Instantiate(attacker.GetComponent<CacheGrabber>().spellEffects[0]);
        //NetworkServer.Spawn(obj);
        var script = obj.GetComponent<LifeleechScript>();
        script.vampire = attacker.transform;
        script.victim = target.transform;
    }

    public static void Paralyze(Character attacker, Character target, AbilityAttribute attr) {
        target.GetComponent<StatusEffectHost>().AddStatusEffect("paralysis", attr.FindParameter("duration").floatVal, inflicter: attacker);
        var name = target.gameObject.name;
        if (name == "kittenCharacter(Clone)") name = "Player";
        target.GetComponent<ObjectSpawner>().CreateFloatingStatusText("Paralyzed!", name + " has been paralyzed!");
    }

    public static void Blind(Character attacker, Character target, AbilityAttribute attr) {
        target.GetComponent<StatusEffectHost>().AddStatusEffect("blind", attr.FindParameter("duration").floatVal, inflicter: attacker);
        var name = target.gameObject.name;
        if (name == "kittenCharacter(Clone)") name = "Player";
        target.GetComponent<ObjectSpawner>().CreateFloatingStatusText("Blinded!", name + " has been blinded!");
    }

    public static void Knockback(Character attacker, Character target, AbilityAttribute attr) {
        float degree = attr.FindParameter("degree").floatVal;
        var obj = target.gameObject.AddComponent<PushingEffect>();
        obj.Initialize(target.transform.position + attacker.transform.forward * degree, 0.25f);
        target.GetComponent<AbilityUser>().AddPushingEffect(target.transform.position + attacker.transform.forward * degree, 0.25f);
        //target.GetComponent<AbilityUser>().RpcAddPushingEffect(target.transform.position + attacker.transform.forward * degree, 0.25f);
    }

    public static void KnockbackDefault(Character attacker, Character target) {
        var degree = 5f;
        var obj = target.gameObject.AddComponent<PushingEffect>();
        obj.Initialize(target.transform.position + attacker.transform.forward * degree, 0.25f);
        target.GetComponent<AbilityUser>().RpcAddPushingEffect(target.transform.position + attacker.transform.forward * degree, 0.25f);
    }

    public static void JumpBack(Character attacker, Character target, AbilityAttribute attr) {
        if (attr.FindParameter("degree") == null) return;
        var degree = attr.FindParameter("degree").floatVal;
        var obj = attacker.gameObject.AddComponent<PushingEffect>();
        obj.Initialize(attacker.transform.position - attacker.transform.forward * degree, 0.25f);
        attacker.GetComponent<AbilityUser>().RpcAddPushingEffect(target.transform.position - attacker.transform.forward * degree, 0.25f);
    }

    public static void PullTowards(Character attacker, Character target, AbilityAttribute attr) {
        if (attr.FindParameter("degree") == null) return;
        float distance = attr.FindParameter("degree").floatVal;
        if (distance >= 1 && distance <= 10) {
            var obj = target.gameObject.AddComponent<PushingEffect>();
            obj.Initialize(target.transform.position - attacker.transform.forward * (distance - 1), 0.25f);
            target.GetComponent<AbilityUser>().RpcAddPushingEffect(target.transform.position - attacker.transform.forward * (distance - 1), 0.25f);
        }
    }

    public static void PullTowardsDefault(Character attacker, Character target) {
        var distance = 5f;
        if (distance >= 1 && distance <= 10) {
            var obj = target.gameObject.AddComponent<PushingEffect>();
            obj.Initialize(target.transform.position - attacker.transform.forward * (distance - 1), 0.25f);
            target.GetComponent<AbilityUser>().RpcAddPushingEffect(target.transform.position - attacker.transform.forward * (distance - 1), 0.25f);
        }
    }

    public static void MpOverTime(Character attacker, AbilityAttribute attr) {
        attacker.GetComponent<StatusEffectHost>().AddStatusEffect("mpOverTime", attr.FindParameter("duration").floatVal, attr.FindParameter("degree").floatVal, good: true);
    }

    public static void ElementalDamageBuff(Character attacker, AbilityAttribute attr) {
        attacker.GetComponent<StatusEffectHost>().AddStatusEffect(attr.FindParameter("element").stringVal + "DamageBuff", attr.FindParameter("duration").floatVal, attr.FindParameter("degree").floatVal, good: true);
    }

    public static void Blunting(Character target, AbilityAttribute attr) {
        target.GetComponent<StatusEffectHost>().AddStatusEffect("blunting", 60, attr.FindParameter("degree").floatVal);
    }

    public static void InflictVulnerability(Character target, AbilityAttribute attr) {
        target.GetComponent<StatusEffectHost>().AddStatusEffect("vulnerability", attr.FindParameter("duration").floatVal, attr.FindParameter("degree").floatVal);
    }

    public static void Delay(Character attacker, Character target, AttackAbility ability, float damage, AbilityAttribute attr) {
        target.GetComponent<StatusEffectHost>().AddStatusEffect("dde", attr.FindParameter("time").floatVal, degree: damage, inflicter: attacker, ability: ability);
    }

    public static void DamageShield(Character attacker, AttackAbility ability, AbilityAttribute attr) {
        var shieldAmount = attr.FindParameter("degree").floatVal * attacker.GetComponent<Attacker>().GetBaseDamage(ability.baseStat);
        attacker.GetComponent<StatusEffectHost>().AddStatusEffect("shield", 60f, degree: shieldAmount, good: true);
    }

    public static void RestoreMp(Character attacker, AbilityAttribute attr) {
        attacker.GetComponent<Mana>().mp = Mathf.Min(attacker.GetComponent<Mana>().mp + attr.FindParameter("degree").floatVal, attacker.GetComponent<Mana>().maxMP);
        attacker.GetComponent<ObjectSpawner>().CmdSpawnUnderParent(attacker.GetComponent<CacheGrabber>().spellEffects[10], attacker.gameObject);
    }

    public static void RemoveDebuff(Character attacker, AttackAbility ability, AbilityAttribute attr) {
        attacker.GetComponent<StatusEffectHost>().RemoveAnyDebuff(ability, resetCDOnFail: false);
        attacker.GetComponent<ObjectSpawner>().CmdSpawnUnderParent(attacker.GetComponent<CacheGrabber>().spellEffects[11], attacker.gameObject);
    }

    public static void AddedDot(Character attacker, Character target, AttackAbility ability, AbilityAttribute attr) {
        target.GetComponent<StatusEffectHost>().AddStatusEffect("dot", attr.FindParameter("duration").floatVal, degree: attacker.GetComponent<Attacker>().GetBaseDamage(ability.baseStat) * attr.FindParameter("degree").floatVal / attr.FindParameter("duration").floatVal, inflicter: attacker, ability: ability);
    }

    public static void SpeedMinus(Character target, AbilityAttribute attr) {
        var degree = attr.FindParameter("degree").floatVal;
        var duration = attr.FindParameter("duration").floatVal;
        target.GetComponent<StatusEffectHost>().AddStatusEffect("speed-", duration, degree);
    }

    public static void AttrSteal(Character attacker, Character target) {
        if (attacker.GetComponent<Gremlin>().item != null) return;
        if (SharedInventory.instance.inventory.Count == 0) return;
        int i = UnityEngine.Random.Range(0, SharedInventory.instance.inventory.Count);
        attacker.GetComponent<Gremlin>().item = SharedInventory.instance.inventory[i];
        SharedInventory.instance.inventory.RemoveAt(i);
        target.GetComponent<ConfigGrabber>().sharedInventory.CmdRefresh();
        //attacker.GetComponent<MonsterCombatant>().behaviorType = "thief";
        //attacker.GetComponent<MonsterCombatant>().behavior = new ThiefBehavior();
        target.GetComponent<ObjectSpawner>().CreateFloatingStatusText("ITEM STOLEN", "A gremlin stole one of your items!");
    }
}
