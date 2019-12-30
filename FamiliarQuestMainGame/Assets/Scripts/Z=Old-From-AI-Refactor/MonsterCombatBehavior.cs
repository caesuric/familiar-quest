//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using UnityEngine.AI;

//public interface MonsterCombatBehavior
//{
//    Vector3 CombatMovementBehavior(NavMeshAgent agent, Vector3 lastSeen, GameObject player, Transform transform);
//    float ClosingDistance();
//    bool IsRanged();
//}

//public class MeleeBehavior : MonsterCombatBehavior
//{
//    public Vector3 CombatMovementBehavior(NavMeshAgent agent, Vector3 lastSeen, GameObject player, Transform transform)
//    {
//        var mc = transform.GetComponent<MonsterCombatant>();
//        if (agent == null) return lastSeen;
//        if (player==null || player.GetComponent<StatusEffectHost>().CheckForEffect("stealth")) {
//            mc.DebugText("combat: player hidden");
//            return lastSeen;
//        }
//        if (Vector3.Distance(player.transform.position, agent.destination) > 0.25 && !agent.pathPending)
//        {
//            mc.DebugText("combat: setting path");
//            agent.destination = player.transform.position;
//            lastSeen = player.transform.position;
//        }
//        else if (agent.pathPending) {
//            mc.DebugText("combat: path pending");
//        }
//        else {
//            mc.DebugText("combat");
//        }
//        return lastSeen;
//    }
//    public float ClosingDistance()
//    {
//        return 1.0f;
//    }
//    public bool IsRanged()
//    {
//        return false;
//    }
//}

//public class RangedBehavior : MonsterCombatBehavior
//{
//    public Vector3 CombatMovementBehavior(NavMeshAgent agent, Vector3 lastSeen, GameObject player, Transform transform)
//    {
//        if (player==null || player.GetComponent<StatusEffectHost>().CheckForEffect("stealth")) return lastSeen;
//        lastSeen = player.transform.position;
//        var directionFromPlayer = transform.position - player.transform.position;
//        directionFromPlayer /= directionFromPlayer.magnitude;
//        var hit = new RaycastHit();
//        int layerMask = 1 << 10;
//        if (Physics.Raycast(transform.position, directionFromPlayer, out hit, maxDistance: 20, layerMask: layerMask)) agent.destination = hit.collider.transform.position;
//        else agent.destination = directionFromPlayer * 20;
//        return lastSeen;
//    }
//    public float ClosingDistance()
//    {
//        return 20f;
//    }
//    public bool IsRanged()
//    {
//        return true;
//    }
//}

//public class RandomBehavior : MonsterCombatBehavior {
//    float timer = 0;

//    public Vector3 CombatMovementBehavior(NavMeshAgent agent, Vector3 lastSeen, GameObject player, Transform transform) {
//        timer += Time.deltaTime;
//        var mc = transform.GetComponent<MonsterCombatant>();
//        if (agent == null) return lastSeen;
//        if (player.GetComponent<StatusEffectHost>().CheckForEffect("stealth")) {
//            mc.DebugText("combat: player hidden");
//            return lastSeen;
//        }
//        if (timer>=1) { //if (!agent.pathPending) {
//            mc.DebugText("combat: setting path");
//            timer = 0;
//            agent.destination = PickRandomLocationInSight(transform.position, player.transform.position);
//            lastSeen = player.transform.position;
//        }
//        else if (agent.pathPending) {
//            mc.DebugText("combat: path pending");
//        }
//        else {
//            mc.DebugText("combat");
//        }
//        return lastSeen;
//    }



//    public float ClosingDistance() {
//        return 20f;
//    }
//    public bool IsRanged() {
//        return true;
//    }

//    private Vector3 PickRandomLocationInSight(Vector3 position, Vector3 position2) {
//        var distance = 20;
//        var x = UnityEngine.Random.Range(position.x - distance, position.x + distance);
//        var y = position.y;
//        var z = UnityEngine.Random.Range(position.z - distance, position.z + distance);
//        var newPos = new Vector3(x, y, z);
//        int layerMask = 1 << 10;
//        var hit = Physics.Raycast(position, position - newPos, Vector3.Distance(position, newPos), layerMask);
//        if (hit) return PickRandomLocationInSight(position, position2);
//        hit = hit = Physics.Raycast(position2, position2 - newPos, Vector3.Distance(position2, newPos), layerMask);
//        if (hit) return PickRandomLocationInSight(position, position2);
//        return newPos;
//    }
//}

//public class ThiefBehavior : MonsterCombatBehavior {
//    float timer = 30;

//    public Vector3 CombatMovementBehavior(NavMeshAgent agent, Vector3 lastSeen, GameObject player, Transform transform) {
//        timer += Time.deltaTime;
//        var mc = transform.GetComponent<MonsterCombatant>();
//        if (agent == null || player == null) return lastSeen;
//        if (player.GetComponent<StatusEffectHost>().CheckForEffect("stealth")) {
//            mc.DebugText("combat: player hidden");
//            return lastSeen;
//        }
//        if (timer >= 30 || agent.pathStatus==NavMeshPathStatus.PathInvalid || agent.pathStatus==NavMeshPathStatus.PathPartial) {
//            mc.DebugText("combat: setting path");
//            timer = 0;
//            agent.destination = PickRandomRoom();
//            lastSeen = player.transform.position;
//        }
//        else if (agent.pathPending) {
//            mc.DebugText("combat: path pending");
//        }
//        else {
//            mc.DebugText("combat");
//        }
//        return lastSeen;
//    }



//    public float ClosingDistance() {
//        return 20f;
//    }
//    public bool IsRanged() {
//        return true;
//    }

//    private Vector3 PickRandomRoom() {
//        var rooms = LevelGen.instance.layout.rooms;
//        int roomRoll = UnityEngine.Random.Range(0, rooms.Count);
//        var room = rooms[roomRoll];
//        float x = UnityEngine.Random.Range(room.x, room.x + room.xSize - 1);
//        float y = UnityEngine.Random.Range(room.y, room.y + room.ySize - 1);
//        x = (x * 5);
//        y = (y * 5);
//        return new Vector3(x, 0, y);
//    }
//}

//public class BossBehavior : MonsterCombatBehavior {
//    private MonsterCombatBehavior baseBehavior = null;
//    private Boss boss;
//    private List<ActiveAbility> baseAbilities = new List<ActiveAbility>();
//    private List<float> healthPercentageThresholds = new List<float>();
//    private Element element;
//    private bool hasChangedPhases = false;

//    public BossBehavior(MonsterCombatBehavior behavior, Boss boss) {
//        baseBehavior = behavior;
//        this.boss = boss;
//        element = Spirit.RandomElement();
//        if (!boss.phasesTimeBased) SetupHealthThresholds();
//    }

//    public void SetupHealthThresholds() {
//        healthPercentageThresholds.Clear();
//        int slices = boss.phases.Count * boss.phaseCycles;
//        var health = boss.GetComponent<Health>();
//        float sliceSize = health.maxHP / slices;
//        float amount = health.maxHP;
//        while (amount>0) {
//            amount -= sliceSize;
//            healthPercentageThresholds.Add(amount);
//        }
//    }
    
//    public Vector3 CombatMovementBehavior(NavMeshAgent agent, Vector3 lastSeen, GameObject player, Transform transform) {
//        boss.fightTime += Time.deltaTime;
//        var phase = DeterminePhase();
//        if (phase != boss.phases[boss.currentPhase]) ChangePhases(phase);
//        return baseBehavior.CombatMovementBehavior(agent, lastSeen, player, transform);
//    }
//    public float ClosingDistance() {
//        return baseBehavior.ClosingDistance();
//    }
//    public bool IsRanged() {
//        return baseBehavior.IsRanged();
//    }

//    private BossPhase DeterminePhase() {
//        if (boss.phasesTimeBased) return DetermineTimeBasedPhase();
//        else return DetermineDamageBasedPhase();
//    }

//    private BossPhase DetermineTimeBasedPhase() {
//        int phase = boss.currentPhase;
//        if (boss.fightTime>=boss.phaseTime) {
//            boss.fightTime = 0;
//            phase += 1;
//            if (phase >= boss.phases.Count) phase = 0;
//        }
//        return boss.phases[phase];
//    }

//    private BossPhase DetermineDamageBasedPhase() {
//        int phaseNumber = 0;
//        int index = 0;
//        var health = boss.GetComponent<Health>().hp;
//        while (healthPercentageThresholds[index]>health) {
//            phaseNumber++;
//            if (phaseNumber >= boss.phases.Count) phaseNumber = 0;
//            index++;
//        }
//        return boss.phases[phaseNumber];
//    }

//    private void ChangePhases(BossPhase phase) {
//        if (!hasChangedPhases) {
//            foreach (var ability in boss.GetComponent<MonsterCombatant>().baseAbilities) baseAbilities.Add(ability);
//            hasChangedPhases = true;
//        }
//        boss.currentPhase = boss.phases.IndexOf(phase);
//        var mc = boss.GetComponent<MonsterCombatant>();
//        mc.baseAbilities.Clear();
//        foreach (var ability in baseAbilities) mc.baseAbilities.Add(ability);
//        SetupPhase(phase);
//    }

//    private void SetupPhase(BossPhase phase) {
//        foreach (var mechanic in phase.mechanics) AddAbilitiesForMechanic(mechanic);
//    }

//    private void AddAbilitiesForMechanic(BossMechanic mechanic) {
//        var abilities = boss.GetComponent<MonsterCombatant>().baseAbilities;
//        var baseStat = GetBestStat();
//        var proj = GetProjectile(element);
//        var hitEffect = GetHitEffect(element);
//        var aoe = GetAoeEffect(element);
//        Debug.Log("switching to mechanic: " + mechanic.type);
//        switch (mechanic.type) {
//            case "damageZones":
//                if (mechanic.options.Contains("heals")) abilities.Add(new AttackAbility("Healing Damage Zone", "Damage zone that heals the caster.", 0f, element, baseStat, dotDamage: 4f, dotTime: 10, isRanged: true, cooldown: 10, radius: 4, aoe: aoe, attributes: new AbilityAttribute("bossHealingDamageZone")));
//                else abilities.Add(new AttackAbility("Damage Zone", "Damage zone.", 0f, element, baseStat, dotDamage: 4f, dotTime: 10, isRanged: true, cooldown: 10, radius: 4, aoe: aoe, attributes: new AbilityAttribute("bossDamageZone")));
//                break;
//            case "circleAoe":
//                abilities.Add(new AttackAbility("Circle AOE", "Circular AOE that targets player.", 4f, element, baseStat, isRanged: true, cooldown: 5, radius: 4, aoe: aoe, attributes: new AbilityAttribute("bossCircleAoe")));
//                break;
//            case "lineAoe":
//                abilities.Add(new AttackAbility("Line AOE", "Line AOE that targets player.", 4f, element, baseStat, isRanged: true, cooldown: 5, aoe: aoe, attributes: new AbilityAttribute("bossLineAoe")));
//                break;
//            case "rage":
//                abilities.Add(new UtilityAbility("Rage", "Become enraged.", cooldown: 30, attributes: new AbilityAttribute("bossRage")));
//                break;
//            case "bulletHell":
//                abilities.Add(new AttackAbility("Bullet Hell", "Creates a living hell of projectiles.", 2f, element, baseStat, isRanged: true, rangedProjectile: proj, hitEffect: hitEffect, attributes: new AbilityAttribute("bossBulletHell")));
//                break;
//            case "homingProjectiles":
//                abilities.Add(new AttackAbility("Homing Projectile", "Fires a homing projectile.", 2f, element, baseStat, isRanged: true, rangedProjectile: proj, hitEffect: hitEffect, attributes: new AbilityAttribute("bossHomingProjectile")));
//                break;
//            case "projectileSpreads":
//                abilities.Add(new AttackAbility("Projectile Spread", "Fires a projectile spread.", 2f, element, baseStat, isRanged: true, rangedProjectile: proj, hitEffect: hitEffect, attributes: new AbilityAttribute("projectileSpread")));
//                break;
//            case "jumpAndShoot":
//                abilities.Add(new AttackAbility("Jump and fire", "Jumps and fires.", 2.1f, element, baseStat, cooldown: 1.5f, isRanged: true, rangedProjectile: proj, hitEffect: hitEffect, attributes: new AbilityAttribute("bossJumpAndShoot")));
//                abilities.Add(new AttackAbility("Fire", "Ranged attack.", 2f, element, baseStat, isRanged: true, rangedProjectile: proj, hitEffect: hitEffect));
//                break;
//            case "charges":
//                abilities.Add(new AttackAbility("Charge", "Charges the enemy.", 4f, element, baseStat, cooldown: 5, hitEffect: hitEffect, attributes: new AbilityAttribute("chargeTowards")));
//                break;
//            case "teleports":
//                abilities.Add(new UtilityAbility("Teleport", "Teleport somewhere useful.", cooldown: 5, attributes: new AbilityAttribute("bossTeleport")));
//                break;
//            case "eatMinions":
//                abilities.Add(new UtilityAbility("Eat Minion", "Eats a minion.", cooldown: 30, attributes: new AbilityAttribute("bossEatMinion")));
//                break;
//            case "spawnAdds":
//                abilities.Add(new UtilityAbility("Summon", "Summons minions.", cooldown: 30, attributes: new AbilityAttribute("bossSummonMinions")));
//                break;
//            default:
//                break;
//        }
//    }

//    private BaseStat GetBestStat() {
//        var c = boss.GetComponent<Character>();
//        if (c.strength > c.dexterity && c.strength > c.intelligence) return BaseStat.strength;
//        else if (c.dexterity > c.intelligence) return BaseStat.dexterity;
//        return BaseStat.intelligence;
//    }

//    private int GetProjectile(Element element) {
//        var dict = new Dictionary<Element, int> { { Element.bashing, 3 }, { Element.piercing, 0 }, { Element.slashing, 4 }, { Element.fire, 1 }, { Element.ice, 5 }, { Element.acid, 2 }, { Element.light, 6 }, { Element.dark, 7 }, { Element.none, 8 } };
//        return dict[element];
//    }

//    private int GetHitEffect(Element element) {
//        var dict = new Dictionary<Element, int> { { Element.bashing, 1 }, { Element.piercing, 2 }, { Element.slashing, 0 }, { Element.fire, 3 }, { Element.ice, 4 }, { Element.acid, 5 }, { Element.light, 6 }, { Element.dark, 7 }, { Element.none, 8 } };
//        return dict[element];
//    }

//    private int GetAoeEffect(Element element) {
//        var dict = new Dictionary<Element, int> { { Element.bashing, 2 }, { Element.piercing, 1 }, { Element.slashing, 0 }, { Element.fire, 3 }, { Element.ice, 4 }, { Element.acid, 5 }, { Element.light, 6 }, { Element.dark, 7 }, { Element.none, 8 } };
//        return dict[element];
//    }
//}