using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Boss : MonoBehaviour {

    public bool fightStarted = false;
    public List<Vector3> doorLocations = new List<Vector3>();
    public GameObject wall;
    public GameObject exitPortal;
    public List<GameObject> doors = new List<GameObject>();
    public Vector3 originalLocation;
    public List<BossPhase> phases = new List<BossPhase>();
    public int phaseCycles = 0;
    public bool phasesTimeBased = false;
    public float phaseTime;
    public List<GameObject> adds = new List<GameObject>();
    public GameObject addPrefab = null;
    public int numAdds = 0;
    public int currentPhase = 0;
    public string addType = "";
    public float fightTime = 0;
    private float lowestHealth = 0f;
    public List<GameObject> possibleAdds = new List<GameObject>();
    public List<ActiveAbility> bossAbilities = new List<ActiveAbility>();
    private List<float> healthPhaseThresholds = new List<float>();
    private Element element;

    // Use this for initialization
    void Start() {
        //if (!NetworkServer.active) return;
        wall = (GameObject)Resources.Load("Prefabs/Dungeon/Old/Wall");
        exitPortal = (GameObject)Resources.Load("Prefabs/Dungeon/Old/Exit Portal");
        var addObjs = Resources.LoadAll("Prefabs/Monsters");
        foreach (var obj in addObjs) if (obj.name != "Mirror Mage Mirror Image" && obj.name != "Energy Wisplet") possibleAdds.Add((GameObject)obj);
        originalLocation = transform.position;
        AddRandomElementalStrengths();
        SetupBossPatterns();
        gameObject.name += " (BOSS)";
        var tm = GetComponent<TestMonster>();
        tm.sensors.Add(new AI.Sensors.BossAbilityTracking());
        tm.availableActions.Add(new AI.Actions.HitPlayerWithBossMeleeAttack());
        tm.availableActions.Add(new AI.Actions.HitPlayerWithBossRangedAttack());
        tm.availableActions.Add(new AI.Actions.FacePlayerWhileUsingBossRangedAttack());
        tm.availableActions.Add(new AI.Actions.UseBossUtilityAbility());
        element = Spirit.RandomElement();
        if (!phasesTimeBased) SetupHealthThresholds();
    }

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        var canSeePlayer = GetComponent<TestMonster>().state["seePlayer"].Equals(true);
        if (canSeePlayer && !fightStarted) {
            if (PlayersInRoom() && Vector3.Distance(transform.position, originalLocation) < 50f) {
                fightStarted = true;
                fightTime = 0;
                MusicController.instance.PlayMusic(MusicController.instance.bossMusic);
                LevelGen.instance.bossFightActive = true;
                SpawnAdds();
            }
        }
        else if (canSeePlayer) {
            fightTime += Time.deltaTime;
            var phase = DeterminePhase();
            if (phase != phases[currentPhase]) ChangePhases(phase);
        }
        else {
            transform.position = originalLocation;
            GetComponent<Health>().hp = GetComponent<Health>().maxHP;
            fightTime = 0;
        }

        foreach (ActiveAbility ability in bossAbilities) {
            ability.currentCooldown -= Time.deltaTime;
            if (ability.currentCooldown < 0) ability.currentCooldown = 0;
        }

        //if (GetComponent<MonsterCombatant>().timeSinceEngaged == 0 && !fightStarted) {
        //    if (PlayersInRoom() && Vector3.Distance(transform.position, originalLocation) < 14) {
        //        fightStarted = true;
        //        fightTime = 0;
        //        //GetComponent<AudioGenerator>().PlaySoundByName("fanfair_boss2");
        //        MusicController.instance.PlayMusic(MusicController.instance.bossMusic);
        //        LevelGen.instance.bossFightActive = true;
        //        SpawnAdds();
        //        //foreach (var coords in doorLocations) {
        //        //    var obj = Instantiate(wall);
        //        //    obj.transform.position = new Vector3(coords.x, coords.y + 2.5f, coords.z);
        //        //    obj.AddComponent<NetworkIdentity>();
        //        //    NetworkServer.Spawn(obj);
        //        //    doors.Add(obj);
        //        //}
        //    }
        //    else GetComponent<Health>().hp = GetComponent<Health>().maxHP;
        //}
        //else fightTime += Time.deltaTime;
    }

    public void OnDestroy() {
        foreach (var player in PlayerCharacter.players) {
            LevelGen.instance.bossFightActive = false;
            //player.GetComponent<ConfigGrabber>().questLog.quests[0].completed = true;
            //foreach (var door in doors) {
            //    NetworkServer.Destroy(door);
            //    Destroy(door);
            //}
            Instantiate(exitPortal, originalLocation, exitPortal.transform.rotation);
        }
    }

    private BossPhase DeterminePhase() {
        if (phasesTimeBased) return DetermineTimeBasedPhase();
        else return DetermineDamageBasedPhase();
    }

    private BossPhase DetermineTimeBasedPhase() {
        int phase = currentPhase;
        if (fightTime >= phaseTime) {
            fightTime = 0;
            phase += 1;
            if (phase >= phases.Count) phase = 0;
        }
        return phases[phase];
    }

    private BossPhase DetermineDamageBasedPhase() {
        int phaseNumber = 0;
        int index = 0;
        lowestHealth = Mathf.Min(lowestHealth, GetComponent<Health>().hp);
        while (healthPhaseThresholds[index] > lowestHealth) {
            phaseNumber++;
            if (phaseNumber >= phases.Count) phaseNumber = 0;
            index++;
        }
        return phases[phaseNumber];
    }

    private void ChangePhases(BossPhase phase) {
        currentPhase = phases.IndexOf(phase);
        bossAbilities.Clear();
        SetupPhase(phase);
    }

    private void SetupPhase(BossPhase phase) {
        foreach (var mechanic in phase.mechanics) AddAbilitiesForMechanic(mechanic);
    }

    private void AddAbilitiesForMechanic(BossMechanic mechanic) {
        var abilities = bossAbilities;
        var baseStat = GetBestStat();
        var proj = GetProjectile(element);
        var hitEffect = GetHitEffect(element);
        var aoe = GetAoeEffect(element);
        Debug.Log("switching to mechanic: " + mechanic.type);
        switch (mechanic.type) {
            case "damageZones":
                if (mechanic.options.Contains("heals")) abilities.Add(new AttackAbility("Healing Damage Zone", "Damage zone that heals the caster.", 0f, element, baseStat, dotDamage: 4f, dotTime: 10, isRanged: true, cooldown: 10, radius: 4, aoe: aoe, attributes: new AbilityAttribute("bossHealingDamageZone")));
                else abilities.Add(new AttackAbility("Damage Zone", "Damage zone.", 0f, element, baseStat, dotDamage: 4f, dotTime: 10, isRanged: true, cooldown: 10, radius: 4, aoe: aoe, attributes: new AbilityAttribute("bossDamageZone")));
                break;
            case "circleAoe":
                abilities.Add(new AttackAbility("Circle AOE", "Circular AOE that targets player.", 4f, element, baseStat, isRanged: true, cooldown: 5, radius: 4, aoe: aoe, attributes: new AbilityAttribute("bossCircleAoe")));
                break;
            case "lineAoe":
                abilities.Add(new AttackAbility("Line AOE", "Line AOE that targets player.", 4f, element, baseStat, isRanged: true, cooldown: 5, aoe: aoe, attributes: new AbilityAttribute("bossLineAoe")));
                break;
            case "rage":
                abilities.Add(new UtilityAbility("Rage", "Become enraged.", cooldown: 30, attributes: new AbilityAttribute("bossRage")));
                break;
            case "bulletHell":
                abilities.Add(new AttackAbility("Bullet Hell", "Creates a living hell of projectiles.", 2f, element, baseStat, isRanged: true, rangedProjectile: proj, hitEffect: hitEffect, attributes: new AbilityAttribute("bossBulletHell")));
                break;
            case "homingProjectiles":
                abilities.Add(new AttackAbility("Homing Projectile", "Fires a homing projectile.", 2f, element, baseStat, isRanged: true, rangedProjectile: proj, hitEffect: hitEffect, attributes: new AbilityAttribute("bossHomingProjectile")));
                break;
            case "projectileSpreads":
                abilities.Add(new AttackAbility("Projectile Spread", "Fires a projectile spread.", 2f, element, baseStat, isRanged: true, rangedProjectile: proj, hitEffect: hitEffect, attributes: new AbilityAttribute("projectileSpread")));
                break;
            case "jumpAndShoot":
                abilities.Add(new AttackAbility("Jump and fire", "Jumps and fires.", 2.1f, element, baseStat, cooldown: 1.5f, isRanged: true, rangedProjectile: proj, hitEffect: hitEffect, attributes: new AbilityAttribute("bossJumpAndShoot")));
                abilities.Add(new AttackAbility("Fire", "Ranged attack.", 2f, element, baseStat, isRanged: true, rangedProjectile: proj, hitEffect: hitEffect));
                break;
            case "charges":
                abilities.Add(new AttackAbility("Charge", "Charges the enemy.", 4f, element, baseStat, cooldown: 5, hitEffect: hitEffect, attributes: new AbilityAttribute("chargeTowards")));
                break;
            case "teleports":
                abilities.Add(new UtilityAbility("Teleport", "Teleport somewhere useful.", cooldown: 5, attributes: new AbilityAttribute("bossTeleport")));
                break;
            case "eatMinions":
                abilities.Add(new UtilityAbility("Eat Minion", "Eats a minion.", cooldown: 30, attributes: new AbilityAttribute("bossEatMinion")));
                break;
            case "spawnAdds":
                abilities.Add(new UtilityAbility("Summon", "Summons minions.", cooldown: 30, attributes: new AbilityAttribute("bossSummonMinions")));
                break;
            default:
                break;
        }
    }

    private BaseStat GetBestStat() {
        var c = GetComponent<Character>();
        //if (c.strength > c.dexterity && c.strength > c.intelligence) return BaseStat.strength;
        //else if (c.dexterity > c.intelligence) return BaseStat.dexterity;
        //return BaseStat.intelligence;
        var strength = CharacterAttribute.attributes["strength"].instances[c].TotalValue;
        var dexterity = CharacterAttribute.attributes["dexterity"].instances[c].TotalValue;
        var intelligence = CharacterAttribute.attributes["intelligence"].instances[c].TotalValue;
        if (strength > dexterity && strength > intelligence) return BaseStat.strength;
        else if (dexterity > intelligence) return BaseStat.dexterity;
        return BaseStat.intelligence;
    }

    private int GetProjectile(Element element) {
        var dict = new Dictionary<Element, int> { { Element.bashing, 3 }, { Element.piercing, 0 }, { Element.slashing, 4 }, { Element.fire, 1 }, { Element.ice, 5 }, { Element.acid, 2 }, { Element.light, 6 }, { Element.dark, 7 }, { Element.none, 8 } };
        return dict[element];
    }

    private int GetHitEffect(Element element) {
        var dict = new Dictionary<Element, int> { { Element.bashing, 1 }, { Element.piercing, 2 }, { Element.slashing, 0 }, { Element.fire, 3 }, { Element.ice, 4 }, { Element.acid, 5 }, { Element.light, 6 }, { Element.dark, 7 }, { Element.none, 8 } };
        return dict[element];
    }

    private int GetAoeEffect(Element element) {
        var dict = new Dictionary<Element, int> { { Element.bashing, 2 }, { Element.piercing, 1 }, { Element.slashing, 0 }, { Element.fire, 3 }, { Element.ice, 4 }, { Element.acid, 5 }, { Element.light, 6 }, { Element.dark, 7 }, { Element.none, 8 } };
        return dict[element];
    }

    private void SetupHealthThresholds() {
        healthPhaseThresholds.Clear();
        int slices = phases.Count * phaseCycles;
        var health = GetComponent<Health>();
        float sliceSize = health.maxHP / slices;
        float amount = health.maxHP;
        while (amount > 0) {
            amount -= sliceSize;
            healthPhaseThresholds.Add(amount);
        }
        lowestHealth = health.maxHP;
    }

    private void AddRandomElementalStrengths() {
        int number = Random.Range(1, 3);
        for (int i = 0; i < number; i++) AddRandomElementalStrength();
    }

    private void AddRandomElementalStrength() {
        var element = Spirit.RandomElement();
        foreach (var affinity in GetComponent<Monster>().elementalAffinities) {
            if (affinity.type==element) {
                affinity.amount += RandomElementalStrengthAmount();
                return;
            }
        }
        GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(element, RandomElementalStrengthAmount()));
    }

    private int RandomElementalStrengthAmount() {
        int roll = Random.Range(0, 3);
        switch(roll) {
            case 0:
            default:
                return 50;
            case 1:
                return 100;
            case 2:
                return 200;
        }
    }

    private void SetupBossPatterns() {
        int numPhases = Random.Range(2, 5);
        phaseCycles = Random.Range(2, 4);
        int phaseTriggerRoll = Random.Range(0, 2);
        int hasAddsRoll = Random.Range(0, 2);
        phasesTimeBased = (phaseTriggerRoll == 1);
        phaseTime = (60f / numPhases / phaseCycles);
        for (int i = 0; i < numPhases; i++) phases.Add(new BossPhase(i));
        if (HasAddsBasedPhase()) hasAddsRoll = 0;
        addPrefab = GetRandomMonster(); //EncounterBuilder.GetRandomMonster(InitializeLevel.instance.monsterList, GetComponent<MonsterScaler>().level);
        if (hasAddsRoll==0) {
            numAdds = Random.Range(2, 7);
            int addTypeRoll = Random.Range(0, 4);
            if (addTypeRoll == 3 && !HasAddsBasedPhase()) addType = "bodyguard";
            else if (addTypeRoll == 4) addType = "boostsOnDeath";
        }
    }

    private GameObject GetRandomMonster() {
        var mob = possibleAdds[Random.Range(0, possibleAdds.Count)];
        var mobStats = mob.GetComponent<Monster>();
        if (mobStats.maxLevel < GetComponent<MonsterScaler>().level) return GetRandomMonster();
        else if (mobStats.minLevel > GetComponent<MonsterScaler>().level) return GetRandomMonster();
        else return mob;
    }

    private bool HasAddsBasedPhase() {
        foreach (var phase in phases) foreach (var mechanic in phase.mechanics) if (mechanic.type == "eatMinions") return true;
        return false;
    }

    private void SpawnAdds() {
        for (int i = 0; i < numAdds; i++) SpawnAdd();
    }

    public void SpawnAdd() {
        var x = originalLocation.x + Random.Range(-14, 14);
        var z = originalLocation.z + Random.Range(-14, 14);
        var obj = Instantiate(addPrefab, new Vector3(x, originalLocation.y, z), addPrefab.transform.rotation);
        var bossAdd = obj.AddComponent<BossAdd>();
        bossAdd.boss = this;
        //NetworkServer.Spawn(obj);
        SetupMonster(obj, PlayerCharacter.players[0].gameObject, GetComponent<MonsterScaler>().level, 0);
        string barText = "MINION";
        if (addType == "bodyguard") barText = "GUARD";
        else if (addType == "boostsOnDeath") barText = "SACRIFICE";
        obj.GetComponent<MonsterScaler>().UpdateBarText(barText);
        adds.Add(obj);
    }

    private void SetupMonster(GameObject obj, GameObject player, int level, int quality) {
        var mob = obj.GetComponent<Monster>();
        //mob.GetComponent<MonsterCombatant>().player = player;
        mob.GetComponent<MonsterScaler>().AdjustForLevel(level);
        mob.GetComponent<MonsterScaler>().quality = quality;
        mob.GetComponent<MonsterScaler>().numPlayers = 1;
        mob.GetComponent<MonsterScaler>().Scale();
        var billboard = obj.GetComponentInChildren<Billboard>();
        if (billboard != null) billboard.mainCamera = Camera.main;
    }

    public void LevelUp() {
        var ms = GetComponent<MonsterScaler>();
        ms.level += 1;
        ms.Scale();
    }

    public bool PlayersInRoom() {
        foreach (var player in PlayerCharacter.players) if (Vector3.Distance(player.transform.position, originalLocation) >= 14) return false;
        return true;
    }
}

public class BossPhase {
    public List<BossMechanic> mechanics = new List<BossMechanic>();
    public BossPhase(int index) {
        if (index == 0) return;
        int numMechanicsRoll = Random.Range(1, 3);
        for (int i = 0; i < numMechanicsRoll; i++) mechanics.Add(new BossMechanic());
    }
}

public class BossMechanic {
    public string type;
    public List<string> options = new List<string>();

    public BossMechanic() {
        type = TableRoller.Roll("BossMechanics");
        if (type=="damageZones") {
            int healRoll = Random.Range(0, 2);
            if (healRoll == 0) options.Add("heals");
        }
    }
}