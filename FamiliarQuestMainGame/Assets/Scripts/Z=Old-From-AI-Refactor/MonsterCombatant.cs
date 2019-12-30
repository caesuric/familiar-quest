//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using UnityEngine.AI;
//using UnityEngine.Networking;

//class MonsterCombatant : MonoBehaviour {
//    public float timeSinceEngaged = 30f;
//    public string behaviorType;
//    public MonsterCombatBehavior behavior;
//    public List<ActiveAbility> baseAbilities = new List<ActiveAbility>();
//    private Vector3 lastSeen;
//    public GameObject player;
//    public float colliderSize = 0.25f;
//    private UnitFrame unitFrame = null;
//    private static bool debug = false;
//    public const float sightRange = 25f;

//    void Start() {
//        //if (NetworkServer.active) {
//            switch (behaviorType) {
//                case "melee":
//                case "stealth":
//                default:
//                    behavior = new MeleeBehavior();
//                    break;
//                case "ranged":
//                    behavior = new RangedBehavior();
//                    break;
//                case "random":
//                    behavior = new RandomBehavior();
//                    break;
//                case "thief":
//                    behavior = new ThiefBehavior();
//                    break;
//            }
//            lastSeen = transform.position;
//        //}
//    }

//    void Update() {
//        //if (NetworkServer.active) {
//            foreach (ActiveAbility ability in baseAbilities) {
//                ability.currentCooldown -= Time.deltaTime;
//                if (ability.currentCooldown < 0) ability.currentCooldown = 0;
//            }
//            UnityEngine.Profiling.Profiler.BeginSample("SeekAndDestroy");
//            SeekAndDestroy();
//            UnityEngine.Profiling.Profiler.EndSample();
//            if (player == null || player.GetComponent<StatusEffectHost>().CheckForEffect("stealth")) return;
//            if (CanSeePlayer()) {
//                if (timeSinceEngaged > 10) {
//                    GetComponentInChildren<AudioGenerator>().PlaySoundByName(GetComponent<MonsterSounds>().aggro);
//                    if (GetComponent<MonsterScaler>().quality == 3) GetComponentInChildren<AudioGenerator>().PlaySoundByName("fanfair_boss1");
//                }
//                timeSinceEngaged = 0;
//                ActivateMimicHealthbar();
//            }
//            else timeSinceEngaged += Time.deltaTime;
//            if (timeSinceEngaged >= 30) DeactivateMimicHealthbar();
//        //}
//    }

//    public bool CanHitPlayer() {
//        return CanSeePlayer(range: 1f);
//    }

//    public bool CanSeePlayer(float range = sightRange) {
//        foreach (var player in PlayerCharacter.players) if (CanSeeSpecificPlayer(player, range)) return true;
//        return false;
//    }

//    private bool CanSeeSpecificPlayer(PlayerCharacter player, float range) {
//        if (RaycastCheck(player, transform.position, range)) return true;
//        if (RaycastCheck(player, transform.position + new Vector3(-0.1f, 0, -0.1f), range)) return true;
//        if (RaycastCheck(player, transform.position + new Vector3(-0.1f, 0, 0.1f), range)) return true;
//        if (RaycastCheck(player, transform.position + new Vector3(0.1f, 0, -0.1f), range)) return true;
//        if (RaycastCheck(player, transform.position + new Vector3(0.1f, 0, 0.1f), range)) return true;
//        return false;
//    }

//    private bool RaycastCheck(PlayerCharacter player, Vector3 position, float range) {
//        var rayDirection = player.transform.position - position;
//        rayDirection.y = 0;
//        var hits = Physics.RaycastAll(position, rayDirection, range);
//        bool found = false;
//        foreach (var hit in hits) {
//            if (hit.transform.gameObject.CompareTag("Wall") && Vector3.Distance(hit.transform.position, position) < Vector3.Distance(player.transform.position, position)) return false;
//            if (hit.transform.gameObject.CompareTag("Player")) found = true;
//        }
//        return found;
//    }

//    private bool IsMimic() {
//        var mimic = GetComponent<Mimic>();
//        if (mimic == null) return false;
//        return true;
//    }

//    private void ActivateMimicHealthbar() {
//        var mimic = GetComponent<Mimic>();
//        if (mimic == null) return;
//        if (!mimic.active) mimic.ActivateHealthbar();
//    }

//    private void DeactivateMimicHealthbar() {
//        var mimic = GetComponent<Mimic>();
//        if (mimic == null) return;
//        if (mimic.active) mimic.DeactivateHealthbar();
//    }

//    private void SeekAndDestroy() {
//        UnityEngine.Profiling.Profiler.BeginSample("CheckForStatusAilments");
//        if (GetComponent<StatusEffectHost>().CheckForEffect("paralysis")) {
//            DebugText("paralyzed");
//            if (GetComponent<NavMeshAgent>() != null) GetComponent<NavMeshAgent>().isStopped = true;
//            UnityEngine.Profiling.Profiler.EndSample();
//            return;
//        }
//        else if (GetComponent<StatusEffectHost>().CheckForEffect("immobilize")) {
//            DebugText("immobilized");
//            if (GetComponent<NavMeshAgent>() != null) GetComponent<NavMeshAgent>().isStopped = true;
//        }
//        else {
//            if (GetComponent<NavMeshAgent>() != null) {
//                GetComponent<NavMeshAgent>().isStopped = false;
//            }
//        }
//        UnityEngine.Profiling.Profiler.EndSample();
//        UnityEngine.Profiling.Profiler.BeginSample("GetTargetBasedOnAggro");
//        player = GetTargetBasedOnAggro();
//        UnityEngine.Profiling.Profiler.EndSample();
//        //if (player == null) return;
//        UnityEngine.Profiling.Profiler.BeginSample("RotateMonster");
//        if (player!=null && (GetComponent<NavMeshAgent>() == null || InCombat())) {
//            Quaternion wantedRotation = Quaternion.LookRotation(player.transform.position - transform.position);
//            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * 10.0f);
//        }
//        else if (GetComponent<NavMeshAgent>()!=null && GetComponent<NavMeshAgent>().remainingDistance > 0) {
//            Quaternion wantedRotation = Quaternion.LookRotation(GetComponent<NavMeshAgent>().destination - transform.position);
//            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * 10.0f);
//        }
//        else if (!IsMimic() && GetComponent<MonsterAnimationController>().rotationTimer > GetComponent<MonsterAnimationController>().rotationCount) {
//            GetComponent<MonsterAnimationController>().rotationTimer = 0;
//            GetComponent<MonsterAnimationController>().rotationCount = Random.Range(4, 8);
//            float randomRotation = Random.Range(-180, 180);
//            GetComponent<MonsterAnimationController>().rotationAngle = transform.rotation * Quaternion.Euler(0, randomRotation, 0);
//        }
//        else if (!IsMimic()) {
//            GetComponent<MonsterAnimationController>().rotationTimer += Time.deltaTime;
//            transform.rotation = Quaternion.Slerp(transform.rotation, GetComponent<MonsterAnimationController>().rotationAngle, GetComponent<MonsterAnimationController>().rotationTimer / GetComponent<MonsterAnimationController>().rotationCount);
//        }
//        UnityEngine.Profiling.Profiler.EndSample();

//        if (player == null) {
//            UnityEngine.Profiling.Profiler.BeginSample("GetTargetBasedOnAggro");
//            player = GetTargetBasedOnAggro();
//            UnityEngine.Profiling.Profiler.EndSample();
//        }
//        //if (player == null) {
//        //    return;
//        //}
//        if (InCombat()) {
//            UnityEngine.Profiling.Profiler.BeginSample("InCombatBehavior");
//            GetComponent<MonsterSenses>().sensedSomething = false;
//            lastSeen = behavior.CombatMovementBehavior(GetComponent<NavMeshAgent>(), lastSeen, player, transform);
//            UnityEngine.Profiling.Profiler.EndSample();
//        }
//        else if (GetComponent<MonsterSenses>().sensedSomething) {
//            UnityEngine.Profiling.Profiler.BeginSample("SensedSomething");
//            DebugText("sensedSomething");
//            if (GetComponent<NavMeshAgent>() != null) {
//                GetComponent<NavMeshAgent>().destination = GetComponent<MonsterSenses>().senseLocation;
//                GetComponent<MonsterSenses>().sensedSomething = false;
//            }
//            UnityEngine.Profiling.Profiler.EndSample();
//        }
//        else if (timeSinceEngaged < 10f && GetComponent<NavMeshAgent>() != null) {
//            DebugText("pursuing");
//            UnityEngine.Profiling.Profiler.BeginSample("Pursuing");
//            if (GetComponent<NavMeshAgent>() != null && GetComponent<NavMeshAgent>().destination != lastSeen) GetComponent<NavMeshAgent>().destination = lastSeen;
//            UnityEngine.Profiling.Profiler.EndSample();
//        }
//        else if (timeSinceEngaged >= 30f && GetComponent<NavMeshAgent>() != null && GetComponent<NavMeshAgent>().remainingDistance <= GetComponent<NavMeshAgent>().stoppingDistance && !IsMimic()) {
//            UnityEngine.Profiling.Profiler.BeginSample("IdleBehavior");
//            GetComponent<AggroTable>().aggroEntries.Clear();
//            GetComponent<MonsterIdler>().idleBehavior.IdleBehavior(GetComponent<NavMeshAgent>(), GetComponentInChildren<Animation>(), transform, GetComponent<MonsterAnimationController>().idleAnimation, GetComponent<MonsterAnimationController>().moveAnimation, GetComponent<Monster>());
//            UnityEngine.Profiling.Profiler.EndSample();
//        }
//        UnityEngine.Profiling.Profiler.BeginSample("TriggerAnimations");
//        if (GetComponent<NavMeshAgent>() != null && GetComponent<NavMeshAgent>().remainingDistance > behavior.ClosingDistance() + colliderSize) {
//            if (GetComponentInChildren<Animation>() != null) GetComponentInChildren<Animation>().CrossFade(GetComponent<MonsterAnimationController>().moveAnimation);
//            GetComponent<MonsterAnimationController>().moving = true;
//            GetComponent<MonsterAnimationController>().attacking = false;
//            if (GetComponentInChildren<Animator>() != null && GetComponent<AnimationController>().hasIsMoving) {
//                GetComponentInChildren<Animator>().SetBool("isMoving", true);
//            }
//        }
//        else if (!AllPlayersInStealth() && GetComponent<AbilityUser>().GCDTime > 0) {
//            if (GetComponentInChildren<Animation>() != null) GetComponentInChildren<Animation>().CrossFade(GetComponent<MonsterAnimationController>().attackAnimation);
//            GetComponent<MonsterAnimationController>().attacking = true;
//            GetComponent<MonsterAnimationController>().moving = false;
//            if (GetComponentInChildren<Animator>() != null && GetComponent<AnimationController>().hasIsMoving) {
//                GetComponentInChildren<Animator>().SetBool("isMoving", false);
//            }
//        }
//        UnityEngine.Profiling.Profiler.EndSample();
//        UnityEngine.Profiling.Profiler.BeginSample("UseBestAbility");
//        if (GetComponent<AbilityUser>().GCDTime == 0) UseBestAbility();
//        UnityEngine.Profiling.Profiler.EndSample();
//    }

//    private bool AllPlayersInStealth() {
//        foreach (var player in PlayerCharacter.players) {
//            if (!player.GetComponent<StatusEffectHost>().CheckForEffect("stealth")) return false;
//        }
//        return true;
//    }

//    public void UseBestAbility() {
//        if (player==null || player.GetComponent<StatusEffectHost>().CheckForEffect("stealth")) return;
//        AttackAbility bestAbility = null;
//        float bestDamage = 0;
//        foreach (var spirit in GetComponent<SpiritUser>().spirits) {
//            foreach (var ability in spirit.activeAbilities) {
//                float currentDamage = 0;
//                if (ability is UtilityAbility && ability.currentCooldown == 0 && ability.mpUsage <= GetComponent<Mana>().mp && (ability.FindAttribute("mirrorImage")!=null || HasBossAttribute(ability))) {
//                    GetComponent<AbilityUser>().UseAbility(ability);
//                    return;
//                }
//                if (HealingOpportunity(ability)) {
//                    GetComponent<AbilityUser>().UseAbility(ability);
//                    return;
//                }

//                if (ability is AttackAbility) {
//                    currentDamage = ((AttackAbility)ability).damage * GetComponent<Attacker>().GetBaseDamage(((AttackAbility)ability).baseStat) + (((AttackAbility)ability).dotDamage * GetComponent<Attacker>().GetBaseDamage(((AttackAbility)ability).baseStat));
//                    for (int i = 0; i < ability.attributes.Count; i++) {
//                        currentDamage *= 2;
//                    }
//                }
//                if (ability.currentCooldown == 0 && ability.mpUsage <= GetComponent<Mana>().mp && currentDamage > bestDamage && WouldHit((AttackAbility)ability)) {
//                    bestAbility = (AttackAbility)ability;
//                    bestDamage = currentDamage;
//                }
//            }
//        }
//        foreach (var ability in baseAbilities) {
//            float currentDamage = 0;
//            if (ability is UtilityAbility && ability.currentCooldown == 0 && ability.mpUsage <= GetComponent<Mana>().mp && (ability.FindAttribute("mirrorImage") != null || HasBossAttribute(ability))) {
//                GetComponent<AbilityUser>().UseAbility(ability);
//                return;
//            }
//            if (HealingOpportunity(ability)) {
//                GetComponent<AbilityUser>().UseAbility(ability);
//                return;
//            }

//            if (ability is AttackAbility) {
//                currentDamage = ((AttackAbility)ability).damage * GetComponent<Attacker>().GetBaseDamage(((AttackAbility)ability).baseStat) + (((AttackAbility)ability).dotDamage * GetComponent<Attacker>().GetBaseDamage(((AttackAbility)ability).baseStat));
//                for (int i = 0; i < ability.attributes.Count; i++) {
//                    currentDamage *= 2;
//                }
//            }
//            if (ability.currentCooldown == 0 && ability.mpUsage <= GetComponent<Mana>().mp && currentDamage > bestDamage && WouldHit((AttackAbility)ability)) {
//                bestAbility = (AttackAbility)ability;
//                bestDamage = currentDamage;
//            }
//        }
//        if (bestAbility != null) {
//            int roll = Random.Range(0, 3);
//            if (roll==0) GetComponentInChildren<AudioGenerator>().PlaySoundByName(GetComponent<MonsterSounds>().attack);
//            GetComponent<AbilityUser>().UseAbility(bestAbility);
//        }
//    }

//    private bool HasBossAttribute(ActiveAbility ability) {
//        foreach (var attribute in ability.attributes) if (attribute.type.StartsWith("boss")) return true;
//        return false;
//    }

//    private bool HealingOpportunity(ActiveAbility ability) {
//        if (!IsNearbyMonsterDamaged()) return false;
//        if (ability.currentCooldown > 0) return false;
//        if (ability is UtilityAbility && ability.FindAttribute("heal") != null) return true;
//        if (ability is UtilityAbility && ability.FindAttribute("healAll") != null) return true;
//        return false;
//    }

//    private bool WouldHit(AttackAbility ability) {
//        var range = ability.isRanged ? sightRange : colliderSize + 2;
//        if (!ability.isRanged && GetComponent<MonsterScaler>().quality == 4) {
//            range *= 2;
//        }
//        if (ability.FindAttribute("chargeTowards") != null) range = 10;
//        return WouldHit(range);
//    }

//    private bool IsNearbyMonsterDamaged() {
//        foreach (var monster in Monster.monsters) if (monster.GetComponent<MonsterCombatant>().InCombat() && monster.GetComponent<Health>().hp < monster.GetComponent<Health>().maxHP) return true;
//        return false;
//    }

//    private bool WouldHit(float range) {
//        RaycastHit hit;
//        var rayDirection = transform.forward;
//        int layerMask = (1 << 9) + (1 << 10);
//        if (Physics.Raycast(transform.position, rayDirection, out hit, maxDistance: range, layerMask: layerMask)) {
//            if (hit.transform == player.transform) {
//                return true;
//            }
//        }
//        return false;
//    }

//    public bool InCombat() {
//        if (GetComponent<GoblinRogue>()!=null) {
//            var hits = Physics.OverlapSphere(transform.position, 2);
//            foreach (var hit in hits) if (hit.gameObject.GetComponent<PlayerCharacter>() != null) return true;
//            return false;
//        }
//        return (timeSinceEngaged == 0f);
//    }

//    public GameObject GetTargetBasedOnAggro() {
//        if (PlayerCharacter.players.Count == 0) return null;
//        var target = GetComponent<AggroTable>().FindTarget();
//        if (target != null) return target.obj;
//        else return null;
//    }

//    public static bool AnyInCombat() {
//        foreach (var monster in Monster.monsters) if (monster.GetComponent<MonsterCombatant>().InCombat()) return true;
//        return false;
//    }

//    public void DebugText(string text) {
//        if (!debug) return;
//        if (unitFrame == null) unitFrame = GetComponentInChildren<UnitFrame>();
//        if (unitFrame != null) unitFrame.SetDebugText(text);
//    }
//}