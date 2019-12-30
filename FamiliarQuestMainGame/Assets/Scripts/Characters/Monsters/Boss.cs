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
    public List<GameObject> possibleAdds = new List<GameObject>();

    // Use this for initialization
    void Start() {
        //if (!NetworkServer.active) return;
        wall = (GameObject)Resources.Load("Prefabs/Dungeon/Old/Wall");
        exitPortal = (GameObject)Resources.Load("Prefabs/Dungeon/Old/Exit Portal");
        var addObjs = Resources.LoadAll("Prefabs/Monsters");
        foreach (var obj in addObjs) possibleAdds.Add((GameObject)obj);
        originalLocation = transform.position;
        AddRandomElementalStrengths();
        SetupBossPatterns();
        //GetComponent<MonsterCombatant>().behavior = new BossBehavior(GetComponent<MonsterCombatant>().behavior, this);
        gameObject.name += " (BOSS)";
    }

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
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
            player.GetComponent<ConfigGrabber>().questLog.quests[0].completed = true;
            //foreach (var door in doors) {
            //    NetworkServer.Destroy(door);
            //    Destroy(door);
            //}
            Instantiate(exitPortal, originalLocation, exitPortal.transform.rotation);
        }
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