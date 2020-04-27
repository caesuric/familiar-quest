using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelGen : MonoBehaviour {

    public static OverworldDungeon dungeonData = null;
    public string levelName;
    public GameObject dungeonPrefab;
    public GameObject dungeonInstance;
    public GameObject roomLight;
    public GameObject nightstand;
    public GameObject darkRuinsLighting;
    public NavMeshSurface navMeshSurface;
    private static List<DungeonType> dungeonTypes = new List<DungeonType> {
        new DungeonType(DungeonSetting.INCIDENTAL, "cave"),
        new DungeonType(DungeonSetting.INCIDENTAL, "dungeon"),
        new DungeonType(DungeonSetting.INCIDENTAL, "mine"),
        new DungeonType(DungeonSetting.INCIDENTAL, "sewer"),
        new DungeonType(DungeonSetting.DESIGNED, "castle"),
        new DungeonType(DungeonSetting.DESIGNED, "temple"),
        new DungeonType(DungeonSetting.VAULT, "vault"),
        new DungeonType(DungeonSetting.VAULT, "tomb")
    };
    private static readonly Dictionary<string, Dictionary<string, Dictionary<string, GameObject>>> prefabs = LevelGenPrefabs.prefabs;
    private static readonly Dictionary<string, Dictionary<string, Dictionary<string, float>>> prefabProbability = LevelGenPrefabs.prefabProbability;
    public static Dictionary<string, Dictionary<string, string>> blockLookups = new Dictionary<string, Dictionary<string, string>> {
        { "castle", new Dictionary<string, string> {
            { "x", "hallwayFloor" },
            { "E", "hallwayFloor" },
            { "X", "floor" },
            { ">", "hallwayFloor" },
            { "<", "hallwayFloor" }
        } }
    };
    public static int targetLevel = 1;
    public static bool settingLevel = true;
    public static LevelGen instance = null;
    public DungeonType dungeonType = null;
    public bool resettled = false;
    private List<SocialStructure> socialStructures = new List<SocialStructure>();
    public Dungeon layout;
    public Vector3 entranceLocation = new Vector3(0, 0, 0);
    public float entranceAngle = 0;
    public Vector3 exitLocation = new Vector3(0, 0, 0);
    public float exitAngle = 0;
    public bool goingBack = false;
    public int floor = 0;
    public bool bossFightActive = false;
    public List<int> seeds = new List<int>();
    public List<GameObject> instantiatedObjects = new List<GameObject>();

    // Use this for initialization
    void Start() {
        if (settingLevel) CmdSetTargetLevel(targetLevel);
        //if (!NetworkServer.active) return;
        if (instance == null) instance = this;
        else {
            Destroy(gameObject);
            return;
        }
        foreach (var pair in prefabs) {
            foreach (var pair2 in pair.Value) {
                var keys = new List<string>();
                foreach (var pair3 in pair2.Value) keys.Add(pair3.Key);
                foreach (var item in keys) pair2.Value[item] = (GameObject)Resources.Load("Prefabs/Dungeon/" + item);
            }
        }
        int roll = Random.Range(0, dungeonTypes.Count);
        dungeonType = dungeonTypes[roll];

        //dungeonType = dungeonTypes[4]; // TEMP TO SEE MONSTERS
        dungeonType = dungeonTypes[6]; // TEMP FOR TESTING

        roll = Random.Range(0, 2);
        if (roll == 0) resettled = true;

        resettled = false; // TEMP FOR TESTING

        if (dungeonType.settingType == DungeonSetting.INCIDENTAL) resettled = true;
        if (dungeonData == null || !dungeonData.entered) GenerateDungeonLayout();
        else GenerateLoadedDungeonLayout();

        //TEMP TO VARY MUSIC UNTIL FULL LEVEL GEN IS FINISHED
        var types = new List<string>() {
            "castle",
            "mine",
            "sewer",
            "temple",
            "vault",
            "tomb"
        };
        var roll2 = Random.Range(0, 7);
        if (roll2 < 6) dungeonType = new DungeonType(DungeonSetting.DESIGNED, types[roll2]);
        else {
            dungeonType = new DungeonType(DungeonSetting.DESIGNED, types[Random.Range(0, 6)]);
            resettled = true;
        }
        //END TEMP TO VARY MUSIC
    }

    private void GenerateDungeonLayout() {
        switch (dungeonType.settingType) {
            case DungeonSetting.INCIDENTAL:
                GenerateIncidentalDungeonLayout();
                break;
            case DungeonSetting.DESIGNED:
                GenerateDesignedDungeonLayout();
                break;
            case DungeonSetting.VAULT:
                GenerateVaultDungeonLayout();
                break;
        }
    }

    private void GenerateIncidentalDungeonLayout() {
        switch (dungeonType.environmentType) {
            case "sewer":
                GenerateSewerLayout();
                break;
            case "cave":
                GenerateCaveLayout();
                break;
            case "mine":
                GenerateMineLayout();
                break;
            case "dungeon":
            default:
                GenerateCastleDungeonLayout();
                break;
        }
    }

    private void GenerateSewerLayout() {
        dungeonInstance = Instantiate(dungeonPrefab);
        navMeshSurface = dungeonInstance.GetComponentInChildren<NavMeshSurface>();
        layout = new Sewer();
        var sewerLayout = (Sewer)layout;
        float ruinLevel = Random.Range(0.25f, 0.5f);

        var numStructures = Random.Range(3, 11);
        for (int i = 0; i < numStructures; i++) socialStructures.Add(new SocialStructure(180 / numStructures, true));
        layout.ResettleRooms(socialStructures);
        layout.layout.ApplyRuin(ruinLevel);

        var obj = Instantiate(darkRuinsLighting);
        obj.transform.parent = dungeonInstance.transform;
        foreach (var socialStruture in socialStructures) Debug.Log(socialStruture.Print());
        Debug.Log(layout.PrintGrid());
        for (int i = 0; i < layout.numFloors; i++) seeds.Add(Random.Range(int.MinValue, int.MaxValue));
        InstantiateSewerLayout();
    }

    private void GenerateCaveLayout() {
        // STUB: TODO
    }

    private void GenerateMineLayout() {
        // STUB: TODO
    }

    private void GenerateCastleDungeonLayout() {
        // STUB: TODO
    }

    private void InstantiateIncidentalDungeonLayout() {
        // STUB: TODO
    }

    private void InstantiateSewerLayout() {
        // STUB: TODO
    }

    private void GenerateDesignedDungeonLayout() {
        dungeonInstance = Instantiate(dungeonPrefab);
        navMeshSurface = dungeonInstance.GetComponentInChildren<NavMeshSurface>();
        socialStructures.Add(new SocialStructure(180));
        layout = new DesignedBuilding(socialStructures[0]);
        float ruinLevel = Random.Range(0f, 0.25f);
        var designedLayout = (DesignedBuilding)layout;
        if (resettled) {
            socialStructures.Clear();
            var numStructures = Random.Range(3, 11);
            for (int i = 0; i < numStructures; i++) socialStructures.Add(new SocialStructure(180 / numStructures, true));
            designedLayout.ResettleRooms(socialStructures);
            designedLayout.layout.ApplyRuin(ruinLevel);
            var obj = Instantiate(darkRuinsLighting);
            obj.transform.parent = dungeonInstance.transform;
        }
        foreach (var socialStruture in socialStructures) Debug.Log(socialStruture.Print());
        Debug.Log(designedLayout.PrintGrid());
        for (int i = 0; i < layout.numFloors; i++) seeds.Add(Random.Range(int.MinValue, int.MaxValue));
        InstantiateDesignedDungeonLayout();
    }

    private void InstantiateDesignedDungeonLayout() {
        if (dungeonType.environmentType == "castle") {
            CastleGen.InstantiateLayout((DesignedBuilding)layout, floor, seeds[floor]);
            levelName = "Castle";
            if (!resettled) {
                foreach (var room in layout.rooms) {
                    if (room.floor == floor) CastleGen.AddDressing(room, (DesignedBuilding)layout);
                }
            }
            else levelName = "Ruined Castle";
        }
        SetPlayerLocation();
        navMeshSurface.BuildNavMesh();
        foreach (var socialStructure in socialStructures) CastleGen.InstantiateMonsters(floor, (DesignedBuilding)layout, socialStructure, this);
    }

    public void MoveFloorForward() {
        goingBack = false;
        ChangeFloors(1);
    }

    public void MoveFloorBackwards() {
        goingBack = true;
        ChangeFloors(-1);
    }

    private void ChangeFloors(int amount) {
        floor += amount;
        ClearWalls(floor);
        DeinstantiateLayout();
        dungeonInstance = Instantiate(dungeonPrefab);
        navMeshSurface = dungeonInstance.GetComponentInChildren<NavMeshSurface>();
        InstantiateLayout();
    }

    private void ClearWalls(int floor) {
        for (int x = 0; x < layout.maxDimensions; x++) {
            for (int y = 0; y < layout.maxDimensions; y++) {
                if (layout.grid[floor, x, y] == "w") layout.grid[floor, x, y] = " ";
            }
        }
    }

    private void DeinstantiateLayout() {
        Destroy(dungeonInstance);
    }

    private void InstantiateLayout() {
        switch (dungeonType.settingType) {
            case DungeonSetting.INCIDENTAL:
                InstantiateIncidentalDungeonLayout();
                break;
            case DungeonSetting.DESIGNED:
                InstantiateDesignedDungeonLayout();
                break;
            case DungeonSetting.VAULT:
                InstantiateVaultDungeonLayout();
                break;
        }
    }

    private void GenerateVaultDungeonLayout() {
        dungeonInstance = Instantiate(dungeonPrefab);
        navMeshSurface = dungeonInstance.GetComponentInChildren<NavMeshSurface>();
        if (dungeonType.environmentType == "vault") layout = new TreasureVault();
        else layout = new Tomb();
        var vaultLayout = (TreasureVault)layout;
        vaultLayout.Initialize();
        float ruinLevel = Random.Range(0f, 0.25f);
        if (resettled) {
            var numStructures = Random.Range(3, 11);
            for (int i = 0; i < numStructures; i++) socialStructures.Add(new SocialStructure(180 / numStructures, true));
            layout.ResettleRooms(socialStructures);
            layout.layout.ApplyRuin(ruinLevel);
        }
        else {
            vaultLayout.AddMonsters(targetLevel);
        }
        var obj = Instantiate(darkRuinsLighting);
        obj.transform.parent = dungeonInstance.transform;
        foreach (var socialStruture in socialStructures) Debug.Log(socialStruture.Print());
        //Debug.Log(layout.PrintGrid());
        for (int i = 0; i < layout.numFloors; i++) seeds.Add(Random.Range(int.MinValue, int.MaxValue));
        if (dungeonData != null) {
            dungeonData.entered = true;
            dungeonData.dungeonData = vaultLayout;
            dungeonData.seeds = seeds;
            SavedWorld.OverwriteDungeonData(dungeonData);
        }
        InstantiateVaultDungeonLayout();
    }

    private void GenerateLoadedDungeonLayout() {
        Debug.Log("hit2");
        dungeonInstance = Instantiate(dungeonPrefab);
        navMeshSurface = dungeonInstance.GetComponentInChildren<NavMeshSurface>();
        layout = dungeonData.dungeonData;
        var obj = Instantiate(darkRuinsLighting);
        obj.transform.parent = dungeonInstance.transform;
        seeds = dungeonData.seeds;
        floor = 0;
        InstantiateVaultDungeonLayout();
    }

    public void InstantiateVaultDungeonLayout() {
        VaultGen.InstantiateLayout((Vault)layout, floor, seeds[floor]);
        if (dungeonType.environmentType == "vault") {
            levelName = "Vault";
            if (resettled) levelName = "Ancient Vault";
        }
        else if (dungeonType.environmentType == "tomb") {
            levelName = "Tomb";
            if (resettled) levelName = "Decrepit Tomb";
        }
        SetPlayerLocation();
        navMeshSurface.BuildNavMesh();
        if (resettled) VaultGen.InstantiateResettledMonsters(floor, (Vault)layout, socialStructures);
        else VaultGen.InstantiateMonsters(floor, (Vault)layout, ((Vault)layout).monsters);
    }

    private void SetPlayerLocation() {
        foreach (var player in PlayerCharacter.players) {
            if (!goingBack) player.transform.position = entranceLocation;
            else player.transform.position = exitLocation;
            //player.transform.Rotate(0, entranceAngle, 0);
            //var rotation = player.transform.rotation;
            //rotation.eulerAngles = new Vector3(0, entranceAngle, 0);
        }
    }

    public void Unstuck() {
        SetPlayerLocation();
        //from when this was in InitializeLevel:
        //SetStartingPosition();
        //foreach (var player in PlayerCharacter.players) SceneInitializer.instance.CmdSetCharacterPosition(player.gameObject);
    }

    //[Command]
    public void CmdSetTargetLevel(int level) {
        targetLevel = level;
    }
}

public class DungeonType {
    public DungeonSetting settingType;
    public string environmentType;

    public DungeonType(DungeonSetting setting, string environment) {
        settingType = setting;
        environmentType = environment;
    }
}

public enum DungeonSetting {
    INCIDENTAL = 0,
    DESIGNED = 1,
    VAULT = 2
}
