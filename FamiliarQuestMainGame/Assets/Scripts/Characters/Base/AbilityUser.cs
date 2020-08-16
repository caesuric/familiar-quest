using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.AI;

//[NetworkSettings(sendInterval=0.01f)]
[RequireComponent(typeof(Character))]
[RequireComponent(typeof(StatusEffectHost))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Mana))]
[RequireComponent(typeof(Attacker))]
public class AbilityUser : MonoBehaviour {
    private delegate void Effect(ActiveAbility ability, AbilityAttribute attr);
    private Dictionary<string, Effect> attributeEffects;
    //[SyncVar]
    public float GCDTime = 0.0f; //when <0 onGCD would have been false
    public static float maxGCDTime = 0.5f;
    public GameObject mirrorImagePrefab;
    public GameObject teleportPrefab;

    private void Start() {
        attributeEffects = new Dictionary<string, Effect>() {
            { "damage+", (ActiveAbility ability, AbilityAttribute attr) => AttrDamagePlus(ability, attr) },
            { "speed+", (ActiveAbility ability, AbilityAttribute attr) => AttrSpeedPlus(ability, attr) },
            { "speed-", (ActiveAbility ability, AbilityAttribute attr) => AttrSpeedMinus(ability, attr) },
            { "paralyze", (ActiveAbility ability, AbilityAttribute attr) => AttrParalyze(ability, attr) },
            { "timestop", (ActiveAbility ability, AbilityAttribute attr) => AttrTimestop(ability, attr) },
            { "eatDebuff", (ActiveAbility ability, AbilityAttribute attr) => AttrEatDebuff(ability, attr) },
            { "removeDebuff", (ActiveAbility ability, AbilityAttribute attr) => AttrRemoveDebuff(ability, attr) },
            { "removeAllDebuffs", (ActiveAbility ability, AbilityAttribute attr) => AttrRemoveAllDebuffs(ability, attr) },
            { "heal", (ActiveAbility ability, AbilityAttribute attr) => AttrHeal(ability, attr) },
            { "healAll", (ActiveAbility ability, AbilityAttribute attr) => AttrHealAll(ability, attr) },
            { "shield", (ActiveAbility ability, AbilityAttribute attr) => AttrShield(ability, attr) },
            { "restoreMP", (ActiveAbility ability, AbilityAttribute attr) => AttrRestoreMp(ability, attr) },
            { "elementalDamageBuff", (ActiveAbility ability, AbilityAttribute attr) => AttrElementalDamageBuff(ability, attr) },
            { "hot", (ActiveAbility ability, AbilityAttribute attr) => AttrHot(ability, attr) },
            { "mpOverTime", (ActiveAbility ability, AbilityAttribute attr) => AttrMpOverTime(ability, attr) },
            { "grapplingHook", (ActiveAbility ability, AbilityAttribute attr) => AttrGrapplingHook(ability, attr) },
            { "disableDevice", (ActiveAbility ability, AbilityAttribute attr) => AttrDisableDevice(ability, attr) },
            { "selfDestruct", (ActiveAbility ability, AbilityAttribute attr) => AttrSelfDestruct(ability, attr) },
            { "mirrorImage", (ActiveAbility ability, AbilityAttribute attr) => AttrMirrorImage() },
            { "immobilizeSelf", (ActiveAbility ability, AbilityAttribute attr) => AttrImmobilizeSelf(attr) },
            { "bossRage", (ActiveAbility ability, AbilityAttribute attr) => AttrBossRage() },
            { "bossTeleport", (ActiveAbility ability, AbilityAttribute attr) => AttrBossTeleport() },
            { "bossEatMinion", (ActiveAbility ability, AbilityAttribute attr) => AttrBossEatMinion(ability) },
            { "bossSummonMinions", (ActiveAbility ability, AbilityAttribute attr) => AttrBossSummonMinions() }
        };
        teleportPrefab = Resources.Load("Prefabs/Teleport") as GameObject;
    }

    private void Update() {
        //if (NetworkServer.active) GCDTime = Mathf.Max(0, GCDTime - Time.deltaTime);
        GCDTime = Mathf.Max(0, GCDTime - Time.deltaTime);
    }

    public void UseAbility(ActiveAbility ability) {
        if (!AbilityUsable(ability)) return;
        GetComponent<Mana>().mp -= ability.mpUsage;
        AbilitySetCooldowns(ability);
        int count = 0;
        foreach (var attr in ability.attributes) {
            if (attr.priority >= 50 && count < 4 && attributeEffects.ContainsKey(attr.type)) attributeEffects[attr.type](ability, attr);
            count++;
        }
        count = 0;
        foreach (var attr in ability.attributes) {
            if (attr.priority >= 50 && count < 4 && attr.type == "selfDestruct") return;
            count++;
        }
        if (ability is AttackAbility) GetComponent<Attacker>().UseAttack((AttackAbility)ability);
        AbilityAffectStealth(ability);
    }

    public void AttrImmobilizeSelf(AbilityAttribute attr) {
        GetComponent<StatusEffectHost>().AddStatusEffect("immobilize", attr.FindParameter("duration").floatVal);
    }

    public void AttrSelfDestruct(ActiveAbility ability, AbilityAttribute attr) {
        var damage = GetComponent<Attacker>().CalculateAttackDamage((AttackAbility)ability) * ((AttackAbility)ability).damage;
        Damage.CreateAoe(GetComponent<Character>(), gameObject, (AttackAbility)ability, ability.attributes, (int)damage, 4, "Enemy");
        Destroy(gameObject, 0.2f);
    }

    public void AttrMirrorImage() {
        MakeMirrorImage(transform.position + transform.right);
        MakeMirrorImage(transform.position + transform.right * 2);
        MakeMirrorImage(transform.position + transform.right * -1);
        MakeMirrorImage(transform.position + transform.right * -2);
    }

    public void MakeMirrorImage(Vector3 position) {
        var obj = Instantiate(mirrorImagePrefab, position, transform.rotation);
        //NetworkServer.Spawn(obj);
        obj.GetComponent<MonsterScaler>().AdjustForLevel(GetComponent<MonsterScaler>().level);
        obj.GetComponent<MonsterScaler>().quality = GetComponent<MonsterScaler>().quality;
        obj.GetComponent<MonsterScaler>().numPlayers = 1;
        obj.GetComponent<MonsterScaler>().Scale();
        obj.GetComponent<MirrorImage>().creator = gameObject;
        var billboard = obj.GetComponentInChildren<Billboard>();
        if (billboard != null) billboard.mainCamera = Camera.main;
    }

    public void AttrDamagePlus(ActiveAbility ability, AbilityAttribute attr) {
        var degree = attr.FindParameter("degree").floatVal;
        var duration = attr.FindParameter("duration").floatVal;
        FindTarget().GetComponent<StatusEffectHost>().AddStatusEffect("damage+", duration, degree, good: true);
        FindTarget().CalculateAll();
    }

    public void AttrSpeedPlus(ActiveAbility ability, AbilityAttribute attr) {
        var degree = attr.FindParameter("degree").floatVal;
        var duration = attr.FindParameter("duration").floatVal;
        FindTarget().GetComponent<StatusEffectHost>().AddStatusEffect("speed+", duration, degree, good: true);
    }

    public void AttrSpeedMinus(ActiveAbility ability, AbilityAttribute attr) {
        if (!(ability is UtilityAbility)) return;
        var degree = attr.FindParameter("degree").floatVal;
        var duration = attr.FindParameter("duration").floatVal;
        float radius = 0;
        if (attr.FindParameter("radius") != null) radius = attr.FindParameter("radius").floatVal * 3;
        var utilityAbility = (UtilityAbility)ability;
        List<StatusEffectHost> targets = new List<StatusEffectHost>();
        if (utilityAbility.targetType == "none" && radius > 0) targets = GetTargetsWithinRadiusOfPoint(transform.position, radius);
        else if (utilityAbility.targetType == "point" && radius > 0 && GetMouseCursorTargetLocation() != Vector3.positiveInfinity) targets = GetTargetsWithinRadiusOfPoint(GetMouseCursorTargetLocation(), radius);
        else if (utilityAbility.targetType == "point" && radius == 0 && GetMouseCursorTargetCharacter() != null) targets.Add(GetMouseCursorTargetCharacter());
        foreach (var target in targets) target.AddStatusEffect("speed-", attr.FindParameter("duration").floatVal, attr.FindParameter("degree").floatVal, GetComponent<Character>(), false, ability);
    }

    public void AttrParalyze(ActiveAbility ability, AbilityAttribute attr) {
        if (!(ability is UtilityAbility)) return;
        var duration = attr.FindParameter("duration").floatVal;
        float radius = 0;
        if (attr.FindParameter("radius") != null) radius = attr.FindParameter("radius").floatVal * 3;
        var utilityAbility = (UtilityAbility)ability;
        List<StatusEffectHost> targets = new List<StatusEffectHost>();
        if (utilityAbility.targetType == "none" && radius > 0) targets = GetTargetsWithinRadiusOfPoint(transform.position, radius);
        else if (utilityAbility.targetType == "point" && radius > 0 && GetMouseCursorTargetLocation() != Vector3.positiveInfinity) targets = GetTargetsWithinRadiusOfPoint(GetMouseCursorTargetLocation(), radius);
        else if (utilityAbility.targetType == "point" && radius == 0 && GetMouseCursorTargetCharacter() != null) targets.Add(GetMouseCursorTargetCharacter());
        foreach (var target in targets) target.AddStatusEffect("paralysis", attr.FindParameter("duration").floatVal, inflicter: GetComponent<Character>(), good: false, ability: ability);
    }

    public Vector3 GetMouseCursorTargetLocation() {
        if (GetComponent<PlayerCharacter>()!=null && InputMovement.isDragging) return Vector3.positiveInfinity;
        if (GetComponent<PlayerCharacter>() != null && !InputMovement.ClickIsOnUi()) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) return hit.point;
            else return Vector3.positiveInfinity;
        }
        else if (GetComponent<PlayerCharacter>() != null) return Vector3.positiveInfinity;
        else if (Vector3.Distance(transform.position, PlayerCharacter.players[0].transform.position) < 20f) return PlayerCharacter.players[0].transform.position;
        else return Vector3.positiveInfinity;
    }

    public StatusEffectHost GetMouseCursorTargetCharacter() {
        if (GetComponent<PlayerCharacter>()!=null) return GetNearestTargetToPoint(GetMouseCursorTargetLocation());
        else if (Vector3.Distance(transform.position, PlayerCharacter.players[0].transform.position) < 20f) return PlayerCharacter.players[0].GetComponent<StatusEffectHost>();
        else return null;
    }

    public StatusEffectHost GetNearestTargetToPoint(Vector3 point) {
        float smallestDistance = Mathf.Infinity;
        StatusEffectHost closestTarget = null;
        foreach (var monster in Monster.monsters) {
            var distance = Vector3.Distance(monster.transform.position, point);
            if (distance < smallestDistance && distance < 20f) {
                smallestDistance = distance;
                closestTarget = monster.GetComponent<StatusEffectHost>();
            }
        }
        if (GetComponent<Monster>() != null && Vector3.Distance(point, PlayerCharacter.players[0].transform.position) < 20f) return PlayerCharacter.players[0].GetComponent<StatusEffectHost>();
        return closestTarget;
    }

    public List<StatusEffectHost> GetTargetsWithinRadiusOfPoint(Vector3 point, float radius) {
        var output = new List<StatusEffectHost>();
        foreach (var monster in Monster.monsters) {
            var distance = Vector3.Distance(point, monster.transform.position);
            if (distance <= radius) output.Add(monster.GetComponent<StatusEffectHost>());
        }
        if (GetComponent<Monster>() != null && Vector3.Distance(point, PlayerCharacter.players[0].transform.position) < 20f) return new List<StatusEffectHost> { PlayerCharacter.players[0].GetComponent<StatusEffectHost>() };
        return output;
    }

    public void AttrTimestop(ActiveAbility ability, AbilityAttribute attr) {
        var duration = attr.FindParameter("duration").floatVal;
        var radius = ability.radius;
        string tag = GetComponent<Character>().oppositeFaction;
        AddStatusEffectToAllWithTag(tag, "paralysis", duration, distance: radius);
    }

    public void AttrEatDebuff(ActiveAbility ability, AbilityAttribute attr) {
        FindTarget().GetComponent<StatusEffectHost>().RemoveAnyDebuff(ability, eat: true);
    }
    public void AttrRemoveDebuff(ActiveAbility ability, AbilityAttribute attr) {
        FindTarget().GetComponent<StatusEffectHost>().RemoveAnyDebuff(ability);
    }
    public void AttrRemoveAllDebuffs(ActiveAbility ability, AbilityAttribute attr) {
        FindTarget().GetComponent<StatusEffectHost>().RemoveAllDebuffs(ability);
    }
    public void AttrHeal(ActiveAbility ability, AbilityAttribute attr) {
        float factor = 10;
        if (GetComponent<PlayerCharacter>() != null) factor *= GetComponent<PlayerCharacter>().weapon.attackPower;
        else factor *= GetComponent<Monster>().attackFactor;
        int level = 1;
        if (GetComponent<ExperienceGainer>() != null) level = GetComponent<ExperienceGainer>().level;
        else level = GetComponent<MonsterScaler>().level;
        //factor *= SecondaryStatUtility.CalcHealingMultiplier(GetComponent<Character>().wisdom, level);
        factor *= CharacterAttribute.attributes["healingMultiplier"].instances[GetComponent<Character>()].TotalValue / 100f;
        //var healingAmount = (int)(attr.FindParameter("degree").floatVal * factor * FindTarget().GetComponent<Health>().healingMultiplier);
        var healingAmount = (int)(attr.FindParameter("degree").floatVal * factor * CharacterAttribute.attributes["receivedHealing"].instances[FindTarget()].TotalValue / 100f);
        FindTarget(heal: true).GetComponent<Health>().Heal(healingAmount);
        GetComponent<AudioGenerator>().PlaySoundByName("sfx_magic_heal2");
    }
    public void AttrHealAll(ActiveAbility ability, AbilityAttribute attr) {
        float factor = 10;
        if (GetComponent<PlayerCharacter>() != null) factor *= GetComponent<PlayerCharacter>().weapon.attackPower;
        else factor *= GetComponent<Monster>().attackFactor;
        int level = 1;
        if (GetComponent<ExperienceGainer>() != null) level = GetComponent<ExperienceGainer>().level;
        else level = GetComponent<MonsterScaler>().level;
        //factor *= SecondaryStatUtility.CalcHealingMultiplier(GetComponent<Character>().wisdom, level);
        factor *= CharacterAttribute.attributes["healingMultiplier"].instances[GetComponent<Character>()].TotalValue / 100f;
        //var healingAmount = (int)(attr.FindParameter("degree").floatVal * factor * FindTarget().GetComponent<Health>().healingMultiplier);
        var healingAmount = (int)(attr.FindParameter("degree").floatVal * factor * CharacterAttribute.attributes["receivedHealing"].instances[FindTarget()].TotalValue / 100f);
        //foreach (var monster in Monster.monsters) if (monster.GetComponent<MonsterCombatant>().InCombat()) monster.GetComponent<Health>().Heal(healingAmount);
        foreach (var monster in Monster.monsters) monster.GetComponent<Health>().Heal(healingAmount); // temp
        GetComponent<AudioGenerator>().PlaySoundByName("sfx_magic_heal2");
    }
    public void AttrShield(ActiveAbility ability, AbilityAttribute attr) {
        var shieldAmount = (int)(attr.FindParameter("degree").floatVal * GetComponent<Attacker>().GetBaseDamage(ability.baseStat));
        FindTarget().GetComponent<StatusEffectHost>().AddStatusEffect("shield", 60f, degree: shieldAmount, good: true);
    }
    public void AttrRestoreMp(ActiveAbility ability, AbilityAttribute attr) {
        FindTarget().GetComponent<Mana>().mp = Mathf.Min(FindTarget().GetComponent<Mana>().maxMP, FindTarget().GetComponent<Mana>().mp + attr.FindParameter("degree").floatVal);
    }
    public void AttrElementalDamageBuff(ActiveAbility ability, AbilityAttribute attr) {
        FindTarget().GetComponent<StatusEffectHost>().AddStatusEffect(attr.FindParameter("element").stringVal + "DamageBuff", attr.FindParameter("duration").floatVal, attr.FindParameter("degree").floatVal, good: true);
    }
    public void AttrHot(ActiveAbility ability, AbilityAttribute attr) {
        FindTarget().GetComponent<StatusEffectHost>().AddStatusEffect("hot", attr.FindParameter("duration").floatVal, attr.FindParameter("degree").floatVal, good: true);
    }
    public void AttrMpOverTime(ActiveAbility ability, AbilityAttribute attr) {
        FindTarget().GetComponent<StatusEffectHost>().AddStatusEffect("mpOverTime", attr.FindParameter("duration").floatVal, attr.FindParameter("degree").floatVal, good: true);
    }
    public void AttrGrapplingHook(ActiveAbility ability, AbilityAttribute attr) {
        var obj = gameObject.AddComponent<GrapplingEffect>();
        RpcAddGrapplingEffect();
    }
    public void AttrDisableDevice(ActiveAbility ability, AbilityAttribute attr) {
        var items = Physics.OverlapSphere(transform.position, attr.FindParameter("radius").floatVal);
        foreach (var item in items) {
            var lockedDoor = item.GetComponent<LockedDoor>();
            if (lockedDoor != null) lockedDoor.Unlock(gameObject);
            var lockedChest = item.GetComponent<LockedChest>();
            if (lockedChest != null) lockedChest.Unlock(gameObject);
            if (item.gameObject.CompareTag("TrapTrigger")) {
                GetComponent<AudioGenerator>().PlaySoundByName("sfx_trap_disarm");
                Destroy(item.gameObject);
            }
        }
    }

    public void AttrBossRage() {
        var seh = GetComponent<StatusEffectHost>();
        if (seh.GetEffect("bossRage") == null) seh.AddStatusEffect("bossRage", 30, good: true);
    }

    public void AttrBossTeleport() {
        //if (GetComponent<MonsterCombatant>().behaviorType == "melee") BossTeleportToPlayer(GetComponent<MonsterCombatant>().player);
        //else BossTeleportToRandomSpot();
        CreateTeleportEffect();
        BossTeleportToRandomSpot();
    }

    private void CreateTeleportEffect() {
        var teleport = Instantiate(teleportPrefab);
        teleport.transform.position = transform.position;
    }

    public void BossTeleportToPlayer(GameObject player) {
        var loc = player.transform.position;
        var distance = GetComponent<NavMeshAgent>().stoppingDistance;
        var vector = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized;
        var dest = loc + (distance * vector);
        transform.position = dest;
        CreateTeleportEffect();
    }

    public void BossTeleportToRandomSpot() {
        var originalLocation = GetComponent<Boss>().originalLocation;
        var dest = new Vector3(Random.Range(originalLocation.x - 20, originalLocation.x + 20), 0, Random.Range(originalLocation.z - 20, originalLocation.z + 20));
        transform.position = dest;
        CreateTeleportEffect();
    }

    public void AttrBossEatMinion(ActiveAbility ability) {
        var health = GetComponent<Health>();
        if (!CloseToMinion() || health.hp >= health.maxHP) {
            ability.currentCooldown = 0;
            return;
        }
        if (GetComponent<Boss>().adds.Count == 0) return;
        GetComponent<ObjectSpawner>().CreateFloatingStatusText("ATE A MINION!", "The boss ate a minion to recover health!");
        KillNearestMinion();
        health.hp += health.maxHP / 4;
        if (health.hp > health.maxHP) health.hp = health.maxHP;
    }

    public void AttrBossSummonMinions() {
        var boss = GetComponent<Boss>();
        int numMinions = 0;
        if (boss.adds.Count < boss.numAdds) numMinions = boss.numAdds - boss.adds.Count;
        numMinions = Mathf.Max(2, numMinions);
        for (int i=0; i<numMinions; i++) boss.SpawnAdd();
    }

    public bool CloseToMinion() {
        var boss = GetComponent<Boss>();
        var loc = transform.position;
        foreach (var add in boss.adds) if (Vector3.Distance(add.transform.position, loc) < 8) return true;
        return false;
    }

    public void KillNearestMinion() {
        var boss = GetComponent<Boss>();
        var loc = transform.position;
        GameObject nearestAdd = null;
        var nearestDistance = Mathf.Infinity;
        foreach (var add in boss.adds) {
            var distance = Vector3.Distance(loc, add.transform.position);
            if (distance < nearestDistance) {
                nearestAdd = add;
                nearestDistance = distance;
            }
        }
        boss.adds.Remove(nearestAdd);
        nearestAdd.GetComponent<MonsterMortal>().diedToPlayer = false;
        Destroy(nearestAdd);
    }

    private bool AbilityUsable(ActiveAbility ability) {
        if (GCDTime > 0) return false;
        if (GetComponent<Health>().hp <= 0) return false;
        if (GetComponent<StatusEffectHost>().CheckForEffect("paralysis") && ability.FindAttribute("usableWhileParalyzed") == null) return false;
        if (GetComponent<Mana>().mp < ability.mpUsage || ability.currentCooldown > 0f) return false;
        return true;
    }

    private void AbilityAffectStealth(ActiveAbility ability) {
        var subtle = (ability.FindAttribute("stealthy") != null);
        var stealthAbility = (ability.FindAttribute("stealth") != null);
        if (GetComponent<StatusEffectHost>().CheckForEffect("stealth") && (stealthAbility || (!subtle))) GetComponent<StatusEffectHost>().RemoveEffectByName("stealth");
        //else if (stealthAbility && !MonsterCombatant.AnyInCombat()) GetComponent<StatusEffectHost>().AddStatusEffect("stealth", 3600, good: true, ability: ability);
        else if (stealthAbility) GetComponent<StatusEffectHost>().AddStatusEffect("stealth", Mathf.Infinity, good: true, ability: ability);
    }

    private void AbilitySetCooldowns(ActiveAbility ability) {
        int level = 1;
        if (GetComponent<ExperienceGainer>() != null) level = GetComponent<ExperienceGainer>().level;
        else level = GetComponent<MonsterScaler>().level;
        //GCDTime = maxGCDTime * (1 - SecondaryStatUtility.CalcCooldownReduction(GetComponent<Character>().wisdom, level));
        var cooldownReduction = CharacterAttribute.attributes["cooldownReduction"].instances[GetComponent<Character>()].TotalValue / 100f;
        GCDTime = maxGCDTime * (1 - cooldownReduction);
        if (ability.FindAttribute("offGCD") != null) GCDTime = 0f;
        ability.currentCooldown = ability.cooldown * (1 - cooldownReduction);
    }


    public void AddStatusEffectToAllWithTag(string tag, string effect, float duration, float degree = 0, float distance = -1) {
        var objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (var obj in objects) if ((distance == -1 || Vector3.Distance(transform.position, obj.transform.position) <= distance) && obj.GetComponent<Character>() != null) obj.GetComponent<StatusEffectHost>().AddStatusEffect(effect, duration, degree, inflicter: GetComponent<Character>());
    }

    private Character FindTarget(bool heal=false) {
        if (GetComponent<PlayerCharacter>() == null) return FindMonsterTarget(heal);
        //foreach (var player in PlayerCharacter.players) if (GetComponent<PlayerCharacter>().target == player.netId.Value) return player.GetComponent<Character>();
        return GetComponent<Character>();
    }

    private Character FindMonsterTarget(bool heal) {
        if (!heal) return GetComponent<Character>();
        float highestMissingHp = 0;
        Character target = GetComponent<Character>();
        foreach (var monster in Monster.monsters) {
            //if (!monster.GetComponent<MonsterCombatant>().InCombat()) continue;
            var health = monster.GetComponent<Health>();
            if (health.hp >= health.maxHP) continue;
            var missing = health.maxHP - health.hp;
            if (missing > highestMissingHp) {
                highestMissingHp = missing;
                target = monster.GetComponent<Character>();
            }
        }
        return target;
    }

    //[ClientRpc]
    public void RpcAddPushingEffect(Vector3 vector, float time) {
        var obj = gameObject.AddComponent<PushingEffect>();
        obj.Initialize(vector, time);
    }

    public void AddPushingEffect(Vector3 vector, float time) {
        var obj = gameObject.AddComponent<PushingEffect>();
        obj.Initialize(vector, time);
    }

    //[ClientRpc]
    public void RpcAddGrapplingEffect() {
        var obj = gameObject.AddComponent<GrapplingEffect>();
    }
}
