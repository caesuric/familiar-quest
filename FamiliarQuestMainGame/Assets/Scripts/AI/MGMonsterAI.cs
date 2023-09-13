using System.Collections.Generic;
using UnityEngine;
using MountainGoap;
using UnityEngine.AI;

public class MGMonsterAI : MonoBehaviour {
    public Agent Agent;

    // Start is called before the first frame update
    void Start() {
        var state = new Dictionary<string, object> {
            { "seePlayer", false },
            { "inMeleeRangeOfPlayer", false },
            { "facingPlayer", false },
            { "playerAlive", false },
            { "playerHurt", false },
            { "gcdReady", true },
            { "meleeAttackAvailable", false },
            { "rangedAttackAvailable", false },
            { "bossMeleeAttackAvailable", false },
            { "bossRangedAttackAvailable", false },
            { "bossUtilityAbilityAvailable", false },
            { "paralyzed", false },
            { "playersInFieldOfVision", new List<GameObject>() }
        };
        var hurtPlayerGoal = new Goal(
            weight: 1f,
            desiredState: new Dictionary<string, object> { { "playerHurt", true } }
        );
        var stayAlertGoal = new Goal(
            weight: 0.01f,
            desiredState: new Dictionary<string, object> { { "awareOfSurroundings", true } }
        );
        var facePlayerAction = new Action(
            name: "Face Player",
            cost: 1f,
            preconditions: new Dictionary<string, object> {
                { "seePlayer", true },
                { "paralyzed", false }
            },
            postconditions: new Dictionary<string, object> {
                { "facingPlayer", true },
                { "facingPlayerPrecisely", true }
            },
            permutationSelectors: new Dictionary<string, PermutationSelectorCallback> {
                { "target", PermutationSelectorGenerators.SelectFromCollectionInState<List<GameObject>>("playersInFieldOfVision") }
            },
            executor: GetFacePlayerExecutor()
        );
        var hitPlayerWithMeleeAttackAction = new Action(
            name: "Hit Player With Melee Attack",
            cost: 2f,
            preconditions: new Dictionary<string, object> {
                { "seePlayer", true },
                { "inMeleeRangeOfPlayer", true },
                { "meleeAttackAvailable", true },
                { "gcdReady", true },
                { "facingPlayer", true },
                { "playerAlive", true },
                { "paralyzed", false }
            },
            postconditions: new Dictionary<string, object> {
                { "playerHurt", true },
                { "gcdReady", false }
            },
            permutationSelectors: new Dictionary<string, PermutationSelectorCallback> {
                { "target", PermutationSelectorGenerators.SelectFromCollectionInState<List<GameObject>>("playersInFieldOfVision") }
            },
            executor: GetHitPlayerWithMeleeAttackExecutor()
        );
        var hitPlayerWithRangedAttackAction = new Action(
            name: "Hit Player With Ranged Attack",
            cost: 2f,
            preconditions: new Dictionary<string, object> {
                { "seePlayer", true },
                { "rangedAttackAvailable", true },
                { "gcdReady", true },
                { "facingPlayer", true },
                { "facingPlayerPrecisely", true },
                { "playerAlive", true },
                { "paralyzed", false }
            },
            postconditions: new Dictionary<string, object> {
                { "playerHurt", true },
                { "gcdReady", false }
            },
            permutationSelectors: new Dictionary<string, PermutationSelectorCallback> {
                { "target", PermutationSelectorGenerators.SelectFromCollectionInState<List<GameObject>>("playersInFieldOfVision") }
            },
            executor: GetHitPlayerWithRangedAttackExecutor()
        );
        var facePlayerWhileUsingRangedAttackAction = new Action(
            name: "Face Player While Using Ranged Attack",
            cost: 2f,
            preconditions: new Dictionary<string, object> {
                { "seePlayer", true },
                { "rangedAttackAvailable", true },
                { "playerAlive", true },
                { "paralyzed", false }
            },
            postconditions: new Dictionary<string, object> {
                { "facingPlayer", true },
                { "facingPlayerPrecisely", true },
                { "playerHurt", true },
                { "gcdReady", false }
            },
            permutationSelectors: new Dictionary<string, PermutationSelectorCallback> {
                { "target", PermutationSelectorGenerators.SelectFromCollectionInState<List<GameObject>>("playersInFieldOfVision") }
            },
            executor: GetFacePlayerWhileUsingRangedAttackExecutor()
        );
        var moveToPlayerAction = new Action(
            name: "Move To Player",
            preconditions: new Dictionary<string, object> {
                { "seePlayer", true },
                { "inMeleeRangeOfPlayer", false },
                { "playerAlive", true },
                { "paralyzed", false }
            },
            postconditions: new Dictionary<string, object> {
                { "inMeleeRangeOfPlayer", true },
                { "facingPlayer", true }
            },
            permutationSelectors: new Dictionary<string, PermutationSelectorCallback> {
                { "target", PermutationSelectorGenerators.SelectFromCollectionInState<List<GameObject>>("playersInFieldOfVision") }
            },
            executor: GetMoveToPlayerExecutor(),
            costCallback: GetMoveToPlayerCostCallback()
        );
        var waitForGcdAction = new Action(
            name: "Wait For GCD",
            preconditions: new Dictionary<string, object> {
                { "gcdReady", false }
            },
            postconditions: new Dictionary<string, object> {
                { "gcdReady", true }
            },
            executor: GetWaitForGcdExecutor()
        );
        var pursuePlayerAction = new Action(
            name: "Pursue Player",
            preconditions: new Dictionary<string, object> {
                { "seePlayer", false },
                { "haveSeenPlayer", true },
                { "playerAlive", true },
                { "paralyzed", false }
            },
            postconditions: new Dictionary<string, object> {
                { "seePlayer", true }
            },
            executor: GetPursuePlayerExecutor(),
            costCallback: GetPursuePlayerCostCallback()
        );
        var lookAroundAction = new Action(
            name: "Look Around",
            cost: 1f,
            preconditions: new Dictionary<string, object> {
                { "seePlayer", false },
                { "paralyzed", false }
            },
            postconditions: new Dictionary<string, object> {
                { "awareOfSurroundings", true }
            },
            executor: GetLookAroundExecutor()
        );
        var abilityUser = GetComponent<AbilityUser>();
        var monsterBaseAbilities = GetComponent<MonsterBaseAbilities>();
        var abilityTrackingSensor = new Sensor((agent) => {
            agent.State["meleeAttackAvailable"] = IsMeleeAttackAvailable(new List<List<ActiveAbility>> { abilityUser.soulGemActives, monsterBaseAbilities.baseAbilities });
            agent.State["rangedAttackAvailable"] = IsRangedAttackAvailable(new List<List<ActiveAbility>> { abilityUser.soulGemActives, monsterBaseAbilities.baseAbilities });
        });
        var gcdTrackingSensor = new Sensor((agent) => {
            agent.State["gcdReady"] = abilityUser.GCDTime <= 0;
        });
        var sightSensor = new Sensor((agent) => {
            UpdateFov(agent);
            var fov = agent.Memory["fieldOfVision"] as AI.Data.FieldOfVision;
            UpdateLastSeen(fov, agent);
            agent.State["seePlayer"] = (fov.players.Count > 0);
            if (fov.players.Count > 0) agent.State["haveSeenPlayer"] = true;
            agent.State["inMeleeRangeOfPlayer"] = CanHitPlayer(agent);
            agent.State["facingPlayer"] = IsFacingPlayer(agent);
            agent.State["facingPlayerPrecisely"] = IsFacingPlayer(agent, 10f);
            agent.State["playerAlive"] = IsPlayerAlive(agent, (bool)agent.State["playerAlive"]);
            agent.State["playerHurt"] = !(agent.State["playerAlive"].Equals(true));
            agent.State["awareOfSurroundings"] = false;
        });
        var memorySensor = new Sensor((agent) => {
            if (!agent.Memory.ContainsKey("character")) agent.Memory["characters"] = new AI.Data.MemoryOfCharacters();
            var moc = agent.Memory["characters"] as AI.Data.MemoryOfCharacters;
            var rememberAlive = false;
            foreach (var memory in moc.memories) {
                if (memory.isEnemy && memory.character.GetComponent<Health>().hp > 0) {
                    rememberAlive = true;
                    break;
                }
            }
            if (!rememberAlive && !agent.State["seePlayer"].Equals(true)) agent.State["playerAlive"] = false;
        });
        var statusEffectHost = GetComponent<StatusEffectHost>();
        var effectTrackingSensor = new Sensor((agent) => {
            agent.State["paralyzed"] = statusEffectHost.CheckForEffect("paralysis");
        });
        Agent = new Agent(
            goals: new List<BaseGoal> { hurtPlayerGoal, stayAlertGoal },
            actions: new List<Action> {
                facePlayerAction,
                hitPlayerWithMeleeAttackAction,
                hitPlayerWithRangedAttackAction,
                facePlayerWhileUsingRangedAttackAction,
                moveToPlayerAction,
                waitForGcdAction,
                pursuePlayerAction,
                lookAroundAction
            },
            sensors: new List<Sensor> {
                abilityTrackingSensor,
                gcdTrackingSensor,
                sightSensor,
                memorySensor,
                effectTrackingSensor
            },
            state: state
        );
    }

    // Update is called once per frame
    void Update() {
        Agent.Step();
    }

    private ExecutorCallback GetFacePlayerExecutor() {
        var started = false;
        var originalRotation = new Quaternion();
        var targetLookRotation = new Quaternion();
        var timer = 0f;
        var turnTime = 0.1f;
        return (agent, action) => {
            Debug.Log($"executing {action.Name}");
            if (!started) {
                var target = action.GetParameter("target") as GameObject;
                started = true;
                originalRotation = transform.rotation;
                targetLookRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                timer = 0f;
                return ExecutionStatus.Executing;
            }
            else {
                timer += Time.deltaTime;
                transform.rotation = Quaternion.Slerp(originalRotation, targetLookRotation, timer / turnTime);
                if (timer > turnTime) {
                    timer = 0f;
                    started = false;
                    return ExecutionStatus.Succeeded;
                }
                else return ExecutionStatus.Executing;
            }
        };
    }

    private ExecutorCallback GetHitPlayerWithMeleeAttackExecutor() {
        var monsterAnimationController = GetComponent<MonsterAnimationController>();
        var monsterBaseAbilities = GetComponent<MonsterBaseAbilities>();
        var abilityUser = GetComponent<AbilityUser>();
        return (agent, action) => {
            Debug.Log($"executing {action.Name}");
            if (monsterBaseAbilities == null || abilityUser == null) return ExecutionStatus.Failed;
            if (UseMeleeAttack(new List<List<ActiveAbility>>() { monsterBaseAbilities.baseAbilities, abilityUser.soulGemActives }, abilityUser)) {
                monsterAnimationController.attacking = true;
                return ExecutionStatus.Succeeded;
            }
            else return ExecutionStatus.Failed;
        };
    }

    private ExecutorCallback GetHitPlayerWithRangedAttackExecutor() {
        var monsterAnimationController = GetComponent<MonsterAnimationController>();
        var monsterBaseAbilities = GetComponent<MonsterBaseAbilities>();
        var abilityUser = GetComponent<AbilityUser>();
        return (agent, action) => {
            Debug.Log($"executing {action.Name}");
            if (monsterBaseAbilities == null || abilityUser == null) return ExecutionStatus.Failed;
            if (UseRangedAttack(new List<List<ActiveAbility>>() { monsterBaseAbilities.baseAbilities, abilityUser.soulGemActives }, abilityUser)) {
                monsterAnimationController.attacking = true;
                return ExecutionStatus.Succeeded;
            }
            else return ExecutionStatus.Failed;
        };
    }

    private ExecutorCallback GetFacePlayerWhileUsingRangedAttackExecutor() {
        var started = false;
        var originalRotation = new Quaternion();
        var targetLookRotation = new Quaternion();
        var timer = 0f;
        var turnTime = 0.1f;
        var monsterAnimationController = GetComponent<MonsterAnimationController>();
        var monsterBaseAbilities = GetComponent<MonsterBaseAbilities>();
        var abilityUser = GetComponent<AbilityUser>();
        return (agent, action) => {
            Debug.Log($"executing {action.Name}");
            if (!started) {
                var target = action.GetParameter("target") as GameObject;
                started = true;
                originalRotation = transform.rotation;
                targetLookRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                timer = 0f;
                return ExecutionStatus.Executing;
            }
            else {
                if (UseRangedAttack(new List<List<ActiveAbility>> { monsterBaseAbilities.baseAbilities, abilityUser.soulGemActives }, abilityUser)) monsterAnimationController.attacking = true;
                timer += Time.deltaTime;
                transform.rotation = Quaternion.Slerp(originalRotation, targetLookRotation, timer / turnTime);
                if (timer > turnTime) {
                    timer = 0f;
                    started = false;
                    return ExecutionStatus.Succeeded;
                }
                else return ExecutionStatus.Executing;
            }
        };
    }

    private ExecutorCallback GetMoveToPlayerExecutor() {
        var started = false;
        var navMeshAgent = GetComponent<NavMeshAgent>();
        var monsterAnimationController = GetComponent<MonsterAnimationController>();
        return (agent, action) => {
            Debug.Log($"executing {action.Name}");
            var target = action.GetParameter("target") as GameObject;
            if (!started) {
                started = true;
                SetDestination(navMeshAgent, target);
                return ExecutionStatus.Executing;
            }
            else {
                monsterAnimationController.moving = true;
                var distanceBetweenDestinationAndTarget = Vector3.Distance(navMeshAgent.destination, target.transform.position);
                if (!navMeshAgent.pathPending && distanceBetweenDestinationAndTarget > 0.25f) SetDestination(navMeshAgent, target);
                if (agent.State["inMeleeRangeOfPlayer"].Equals(true)) {
                    started = false;
                    monsterAnimationController.moving = false;
                    return ExecutionStatus.Succeeded;
                }
                else return ExecutionStatus.Executing;
            }
        };
    }

    private ExecutorCallback GetWaitForGcdExecutor() {
        return (agent, action) => {
            Debug.Log($"executing {action.Name}");
            if (agent.State["gcdReady"].Equals(true)) return ExecutionStatus.Succeeded;
            return ExecutionStatus.Executing;
        };
    }

    private ExecutorCallback GetPursuePlayerExecutor() {
        var monsterAnimationController = GetComponent<MonsterAnimationController>();
        var navMeshAgent = GetComponent<NavMeshAgent>();
        var started = false;
        return (agent, action) => {
            Debug.Log($"executing {action.Name}");
            var playerMemory = agent.Memory["characters"] as AI.Data.MemoryOfCharacters;
            var player = playerMemory.GetClosestPlayerMemory(transform.position);
            if (player == null) return ExecutionStatus.Failed;
            if (!started) {
                started = true;
                if (navMeshAgent.isOnNavMesh) SetDestination(navMeshAgent, player.character);
                return ExecutionStatus.Executing;
            }
            else {
                monsterAnimationController.moving = true;
                if (Vector3.Distance(transform.position, navMeshAgent.destination) < 1f) {
                    started = false;
                    monsterAnimationController.moving = false;
                    return ExecutionStatus.Succeeded;
                }
                return ExecutionStatus.Executing;
            }
        };
    }

    private ExecutorCallback GetLookAroundExecutor() {
        var started = false;
        var originalRotation = new Vector3();
        var startRotation = new Vector3();
        var targetLookRotation = new Vector3();
        var movePhase = 0;
        var timer = 0f;
        var turnTime = 0.25f;
        var rotationAmount = 0f;
        System.Action completeMovePhase1 = () => {
            startRotation = transform.eulerAngles;
            targetLookRotation = new Vector3(originalRotation.x, originalRotation.y + rotationAmount, originalRotation.z);
        };
        System.Action completeMovePhase2 = () => {
            startRotation = transform.eulerAngles;
            targetLookRotation = originalRotation;
        };
        System.Action completeMovePhase3 = () => {
            started = false;
        };
        var actions = new Dictionary<int, System.Action>() {
            { 1,  () => completeMovePhase1() },
            { 2,  () => completeMovePhase2() },
            { 3,  () => completeMovePhase3() }
        };
        System.Action finishTurn = () => {
            movePhase++;
            timer = 0f;
            actions[movePhase]();
        };
        return (agent, action) => {
            Debug.Log($"executing {action.Name}");
            if (!started) {
                started = true;
                movePhase = 0;
                startRotation = originalRotation = transform.eulerAngles;
                rotationAmount = Random.Range(30f, 90f);
                targetLookRotation = new Vector3(originalRotation.x, originalRotation.y - rotationAmount, originalRotation.z);
                timer = 0f;
                turnTime = Random.Range(2f, 4f);
                return ExecutionStatus.Executing;
            }
            else {
                timer += Time.deltaTime;
                transform.rotation = Quaternion.Slerp(Quaternion.Euler(startRotation), Quaternion.Euler(targetLookRotation), timer / turnTime);
                if (timer >= turnTime) finishTurn();
                if (movePhase >= 3) {
                    Debug.Log("terminating look around");
                    started = false;
                    return ExecutionStatus.Succeeded;
                }
                return ExecutionStatus.Executing;
            }
        };
    }

    private CostCallback GetPursuePlayerCostCallback() {
        var navMeshAgent = GetComponent<NavMeshAgent>();
        return (action, state) => {
            var distance = Vector3.Distance(transform.position, navMeshAgent.destination);
            return 1f + (distance / 2f);
        };
    }

    private CostCallback GetMoveToPlayerCostCallback() {
        var navMeshAgent = GetComponent<NavMeshAgent>();
        return (action, state) => {
            var distanceToTarget = Vector3.Distance(transform.position, navMeshAgent.destination);
            return 1f + (distanceToTarget / 2f);
        };
    }

    private void SetDestination(NavMeshAgent navMeshAgent, GameObject target) {
        if (navMeshAgent.isOnNavMesh) {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(target.transform.position);
        }
    }

    private bool UseMeleeAttack(List<List<ActiveAbility>> abilityLists, AbilityUser abilityUser) {
        foreach (var list in abilityLists) {
            foreach (var ability in list) {
                if (IsMeleeAttack(ability) && ability is AttackAbility attackAbility && attackAbility.currentCooldown == 0) {
                    abilityUser.UseAbility(attackAbility);
                    return true;
                }
            }
        }
        return false;
    }

    private bool UseRangedAttack(List<List<ActiveAbility>> abilityLists, AbilityUser abilityUser) {
        foreach (var list in abilityLists) {
            foreach (var ability in list) {
                if (IsRangedAttack(ability) && ability is AttackAbility attackAbility && attackAbility.currentCooldown == 0) {
                    abilityUser.UseAbility(attackAbility);
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsMeleeAttackAvailable(List<List<ActiveAbility>> lists) {
        foreach (var list in lists) {
            foreach (var ability in list) {
                if (IsMeleeAttack(ability)) return true;
            }
        }
        return false;
    }

    private bool IsRangedAttackAvailable(List<List<ActiveAbility>> lists) {
        foreach (var list in lists) {
            foreach (var ability in list) {
                if (IsRangedAttack(ability)) return true;
            }
        }
        return false;
    }

    private bool IsMeleeAttack(Ability ability) {
        if (ability is AttackAbility attackAbility) return !attackAbility.isRanged;
        return false;
    }

    private bool IsRangedAttack(Ability ability) {
        if (ability is AttackAbility attackAbility) return attackAbility.isRanged;
        return false;
    }

    private void UpdateFov(Agent agent) {
        var fov = new AI.Data.FieldOfVision();
        foreach (var player in PlayerCharacter.players) if (CanSeeSpecificPlayer(player)) fov.players.Add(player.gameObject);
        agent.Memory["fieldOfVision"] = fov;
    }

    private void UpdateLastSeen(AI.Data.FieldOfVision fov, Agent agent) {
        var memChars = new AI.Data.MemoryOfCharacters();
        if (agent.Memory.ContainsKey("characters")) memChars = agent.Memory["characters"] as AI.Data.MemoryOfCharacters;
        else agent.Memory.Add("characters", memChars);
        memChars.AddFovFrame(fov);
    }

    private bool IsFacingPlayer(Agent agent, float angle = 30f) {
        var fov = agent.Memory["fieldOfVision"] as AI.Data.FieldOfVision;
        foreach (var player in fov.players) {
            if (Mathf.Abs(Vector3.Angle(transform.forward, player.transform.position - transform.position)) <= angle) return true;
        }
        return false;
    }

    private bool IsPlayerAlive(Agent agent, bool previousState) {
        var fov = agent.Memory["fieldOfVision"] as AI.Data.FieldOfVision;
        foreach (var player in fov.players) if (player.GetComponent<Health>().hp > 0) return true;
        return previousState;
    }

    private bool CanHitPlayer(Agent agent) {
        var fov = agent.Memory["fieldOfVision"] as AI.Data.FieldOfVision;
        foreach (var player in fov.players) if (CanSeeSpecificPlayer(player.GetComponent<PlayerCharacter>(), range: GetComponent<MonsterScaler>().colliderSize + 2.5f)) return true;
        return false;
    }

    private bool CanSeePlayer(Agent agent, float range = 25f) {
        foreach (var player in PlayerCharacter.players) if (CanSeeSpecificPlayer(player, range)) return true;
        return false;
    }

    private bool CanSeeSpecificPlayer(PlayerCharacter player, float range = 25f) {
        if (RaycastCheck(player, transform.position, range)) return true;
        if (RaycastCheck(player, transform.position + new Vector3(-0.1f, 0, -0.1f), range)) return true;
        if (RaycastCheck(player, transform.position + new Vector3(-0.1f, 0, 0.1f), range)) return true;
        if (RaycastCheck(player, transform.position + new Vector3(0.1f, 0, -0.1f), range)) return true;
        if (RaycastCheck(player, transform.position + new Vector3(0.1f, 0, 0.1f), range)) return true;
        return false;
    }

    private bool RaycastCheck(PlayerCharacter player, Vector3 position, float range) {
        var rayDirection = player.transform.position - position;
        rayDirection.y = 0;
        var hits = Physics.RaycastAll(position, rayDirection, range);
        bool found = false;
        foreach (var hit in hits) {
            if (hit.transform.gameObject.CompareTag("Wall") && Vector3.Distance(hit.transform.position, position) < Vector3.Distance(player.transform.position, position)) return false;
            if (hit.transform.gameObject.CompareTag("Player")) found = true;
        }
        return found;
    }
}
