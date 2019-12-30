//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
//using UnityEngine;
//using UnityEngine.AI;
//using UnityEngine.Networking;
//using UnityEngine.SceneManagement;

//[NetworkSettings(channel = 2)]
//public class InitializeLevel : MonoBehaviour {
//    public Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
//    private Dictionary<string, MapInitData> mapInitData = new Dictionary<string, MapInitData>();
//    public float dungeonMinutes;
//    public int dungeonFloors;
//    public static int currentFloor = 0;
//    public static bool goingDown = true;
//    public float secondsPerMonster;
//    public int minBranches;
//    public int maxBranches;
//    public int mapSize;
//    public GameObject character;
//    public List<GameObject> monsterList;
//    public Map map;
//    public static List<Map> maps = new List<Map>();
//    public static bool gameStarted = false;
//    public static Character charStats;
//    public static PlayerCharacter pcStats;
//    public bool setupDone = false;
//    private List<GameObject> dressingObjs = new List<GameObject>();
//    public static InitializeLevel instance = null;
//    public static int targetLevel = 1;
//    private bool syncingMap = false;
//    private int mapSyncX = 0;
//    private int mapSyncY = 0;
//    private int mapSyncSize = 0;
//    private string[,] mapSyncBlocks = null;
//    private int mapSyncEntireSize = 0;

//    public static void ResetGame() {
//        currentFloor = 1;
//        goingDown = true;
//        maps = new List<Map>();
//        gameStarted = false;
//        foreach (var player in PlayerCharacter.players) Destroy(player.gameObject);
//        PlayerCharacter.players.Clear();

//        CharacterSelectScreen.loadedCharacter = null;
//        CharacterSelectScreen.selectedCharacterName = "";
//        CharacterSelectScreen.characterByteArray = null;
//        instance.setupDone = false;
//        currentFloor = 0;
//        LobbyManager.Shutdown();
//    }


//    public void Start() {
//        if (instance != null) {
//            Destroy(gameObject);
//            Destroy(this);
//            return;
//        }
//        else instance = this;
//        var objs = Resources.LoadAll("Prefabs/Dungeon");
//        foreach (var obj in objs) prefabs.Add(obj.name, (GameObject)obj);
//        var data = TextReader.ReadSets("MapInitData");
//        foreach (var item in data) mapInitData.Add(item[0], new MapInitData(item[0], item[1], float.Parse(item[2]), float.Parse(item[3]), float.Parse(item[4]), float.Parse(item[5])));
//        DontDestroyOnLoad(gameObject);
//        SceneManager.sceneLoaded += OnSceneLoaded;
//    }

//    public void Update() {
//        if (!syncingMap || !NetworkServer.active) return;
//        SyncMap();
//    }

//    private void SyncMap() {
//        var oneDimBlocks = FoldBlocks(mapSyncBlocks, mapSyncX, mapSyncY, mapSyncSize);
//        RpcInstantiateMap(oneDimBlocks, mapSyncSize, mapSyncX, mapSyncY);
//        IncrementMapSync();
//    }

//    private void IncrementMapSync() {
//        if (mapSyncX + mapSyncSize < mapSyncEntireSize) {
//            mapSyncX += mapSyncSize;
//        }
//        else if (mapSyncY + mapSyncSize < mapSyncEntireSize) {
//            mapSyncX = 0;
//            mapSyncY += mapSyncSize;
//        }
//        else syncingMap = false;
//    }

//    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
//        if (instance == null || this==null || gameObject==null) return;
//        setupDone = false;
//        CmdOnCharacterConnect(targetLevel);
//        var lobbyCurtain = GameObject.FindGameObjectWithTag("LobbyCurtain");
//        if (lobbyCurtain != null) lobbyCurtain.SetActive(false);
//        var inventoryDetails = GameObject.FindGameObjectWithTag("InventoryDetails");
//        //if (inventoryDetails != null) inventoryDetails.SetActive(false);
//        var levelUpMenu = GameObject.FindGameObjectWithTag("LevelUpMenu");
//        if (levelUpMenu != null) levelUpMenu.SetActive(false);
//        //SceneManager.sceneLoaded -= OnSceneLoaded;
//    }

//    public void Unstuck() {
//        SetStartingPosition();
//        foreach (var player in PlayerCharacter.players) SceneInitializer.instance.CmdSetCharacterPosition(player.gameObject);
//    }

//    public static void SetStartingPosition() {
//        if (currentFloor == 0) SceneInitializer.charPosition = new Vector3(0, 145, 0);
//        else if (maps.Count>=currentFloor) SceneInitializer.charPosition = new Vector3((maps[currentFloor - 1].startingX * 2) - 120, 145, (maps[currentFloor - 1].startingY * 2) - 120);
//    }

//    public static void SetEndingPosition() {
//        SceneInitializer.charPosition = new Vector3((maps[currentFloor - 1].endingX * 2) - 120, 145, (maps[currentFloor - 1].endingY * 2) - 120);
//    }

//    public static void ClearObjectLists() {
//        Hideable.items.Clear();
//    }

//    private bool GenerateMap(NetworkGraph ng, int mapSize, int floor)
//    {
//        map = new Map(ng, mapSize, floor);
//        return map.valid;
//    }

//    [Command]
//    public void CmdInstantiateMap() {
//        map = maps[currentFloor - 1];
//        map.Cleanup();
//        for (int i = 0; i < map.mapSize; i++) {
//            for (int j = 0; j < map.mapSize; j++) {
//                var coordsX = (i * 2) - 120;
//                var coordsY = (j * 2) - 120;
//                if (map.blocks[i, j] == "B") map.AddBossObject(monsterList, character, coordsX, coordsY);
//                else if (!mapInitData.ContainsKey(map.blocks[i, j])) continue;
//                else InstantiateMapBlock(map.blocks[i, j], coordsX, coordsY);
//            }
//        }
//        InstantiateDressing();
//        CleanUpFloatingObjects();
//        //var dressing = GameObject.FindGameObjectsWithTag("DungeonDressing");
//        //foreach (var item in dressing) {
//        //    var collider = item.gameObject.GetComponentInChildren<Collider>();
//        //    collider.enabled = false;
//        //}
//        syncingMap = true;
//        mapSyncBlocks = map.blocks;
//        mapSyncEntireSize = map.mapSize;
//        mapSyncX = 0;
//        mapSyncY = 0;
//        mapSyncSize = 30;
//    }

//    [ClientRpc]
//    public void RpcInstantiateMap(string[] oneDimBlocks, int size, int x, int y) {
//        var blocks = UnfoldBlocks(oneDimBlocks, size);
//        if (NetworkServer.active) return;
//        for (int i = 0; i < size; i++) {
//            for (int j = 0; j < size; j++) {
//                var coordsX = ((i + x) * 2) - 120;
//                var coordsY = ((j + y) * 2) - 120;
//                if (!mapInitData.ContainsKey(blocks[i, j])) continue;
//                if (blocks[i, j] != "W") continue;
//                else InstantiateMapBlock(blocks[i, j], coordsX, coordsY, local: true);
//            }
//        }
//    }

//    private string[,] UnfoldBlocks(string[] oneDimBlocks, int size) {
//        var blocks = new string[size, size];
//        for (int i=0; i < size; i++) {
//            for (int j=0; j<size; j++) {
//                blocks[i, j] = oneDimBlocks[(j * size) + i];
//            }
//        }
//        return blocks;
//    }

//    private string[] FoldBlocks(string[,] blocks, int x, int y, int size) {
//        var oneDimBlocks = new string[size * size];
//        for (int i = 0; i < size; i++) {
//            for (int j = 0; j < size; j++) {
//                oneDimBlocks[(j * size) + i] = blocks[i + x, j + y];
//            }
//        }
//        return oneDimBlocks;
//    }

//    public void InstantiateDressing() {
//        foreach (var node in map.ng.members) {
//            for (var i = 0; i < 4; i++) {
//                if (node.dressing[i] == "") continue;
//                var coordsMinX = (node.left * 2) - 120;
//                var coordsMinY = (node.top * 2) - 120;
//                var coordsMaxX = (node.right * 2) - 120;
//                var coordsMaxY = (node.bottom * 2) - 120;
//                var prefab = (GameObject)(Resources.Load("Prefabs/Dungeon/Dressing/" + node.dressing[i]));
//                var table = new Dictionary<string, Action>() {
//                    {"center", () => AddCenterDressing(prefab, node.dressingCount[i], coordsMinX, coordsMinY, coordsMaxX, coordsMaxY) },
//                    {"wallsRandom", () => AddWallsRandomDressing(prefab, node.dressingCount[i], coordsMinX, coordsMinY, coordsMaxX, coordsMaxY) },
//                    {"middleRandom", () => AddMiddleRandomDressing(prefab, node.dressingCount[i], coordsMinX, coordsMinY, coordsMaxX, coordsMaxY) },
//                    {"oppositeWallsEven", () => AddOppositeWallsEvenDressing(prefab, node.dressingCount[i], coordsMinX, coordsMinY, coordsMaxX, coordsMaxY) },
//                    {"middleEven", () => AddMiddleEvenDressing(prefab, node.dressingCount[i], coordsMinX, coordsMinY, coordsMaxX, coordsMaxY) },
//                    {"allWallsEven", () => AddAllWallsEvenDressing(prefab, node.dressingCount[i], coordsMinX, coordsMinY, coordsMaxX, coordsMaxY) },
//                    {"oneWallEven", () => AddOneWallEvenDressing(prefab, node.dressingCount[i], coordsMinX, coordsMinY, coordsMaxX, coordsMaxY) }
//                };
//                table[node.dressingPlacement[i]]();
//            }
//            if (node.roomData != null) CleanRoom(node);
//        }
//    }

//    private void CleanRoom(GraphNode node) {
//        float x = (node.roomData.doorX * 2) - 120;
//        float y = (node.roomData.doorY * 2) - 120;
//        var overlaps = Physics.OverlapBox(new Vector3(x,145,y), new Vector3(4, 4, 4), new Quaternion(0, 0, 0, 0));
//        foreach (var obj in overlaps) if (obj.CompareTag("DungeonDressing")) Destroy(obj.gameObject);
//        foreach (var connection in node.connections) {
//            if (connection.roomData == null) continue;
//            x = (connection.roomData.doorX * 2) - 120;
//            y = (connection.roomData.doorY * 2) - 120;
//            overlaps = Physics.OverlapBox(new Vector3(x, 145, y), new Vector3(4, 4, 4), new Quaternion(0, 0, 0, 0));
//            foreach (var obj in overlaps) if (obj.CompareTag("DungeonDressing")) Destroy(obj.gameObject);
//        }
//    }

//    private void CleanUpFloatingObjects() {
//        var pruneList = new List<GameObject>();
//        foreach (var obj in dressingObjs) {
//            if (obj == null) continue;
//            var overlaps = Physics.OverlapBox(obj.transform.position, new Vector3(1.5f, 1.5f, 1.5f), new Quaternion(0, 0, 0, 0));
//            var wallFound = false;
//            foreach (var overlap in overlaps) {
//                if (overlap.gameObject.CompareTag("Wall")) {
//                    wallFound = true;
//                    break;
//                }
//            }
//            if (!wallFound) pruneList.Add(obj.gameObject);
//        }
//        foreach (var deader in pruneList) {
//            if (deader.name.StartsWith("torch") || deader.name.StartsWith("banner")) Destroy(deader);
//        }
//    }

//    private void AddCenterDressing(GameObject prefab, int count, float coordsMinX, float coordsMinY, float coordsMaxX, float coordsMaxY) {
//        var obj = Instantiate(prefab);
//        dressingObjs.Add(obj);
//        obj.transform.position = new Vector3((coordsMinX + coordsMaxX) / 2, 145, (coordsMinY + coordsMaxY) / 2);
//    }

//    private void AddWallsRandomDressing(GameObject prefab, int count, float coordsMinX, float coordsMinY, float coordsMaxX, float coordsMaxY) {
//        for (var i = 0; i < count; i++) {
//            int wallRoll = UnityEngine.Random.Range(0, 4);
//            float x = coordsMinX;
//            float y = coordsMinY;
//            int signX = 0;
//            int signY = 0;
//            if (wallRoll == 0 || wallRoll == 2) x = UnityEngine.Random.Range(coordsMinX, coordsMaxX);
//            if (wallRoll == 1 || wallRoll == 3) y = UnityEngine.Random.Range(coordsMinY, coordsMaxY);
//            switch (wallRoll) {
//                case 0:
//                    y = coordsMinY;
//                    signY = 1;
//                    break;
//                case 1:
//                    x = coordsMaxX;
//                    signX = -1;
//                    break;
//                case 2:
//                    y = coordsMaxY;
//                    signY = -1;
//                    break;
//                case 3:
//                    x = coordsMinX;
//                    signX = 1;
//                    break;
//            }
//            var obj = Instantiate(prefab);
//            dressingObjs.Add(obj);
//            var size = obj.GetComponentInChildren<Renderer>().bounds.size;
//            if (prefab.name=="banner" || prefab.name=="torch") {
//                signX = 0;
//                signY = 0;
//            }
//            obj.transform.position = new Vector3(x + (signX * size.x / 2), 145, y + (signY * size.x / 2));
//            RotateProperly(obj, wallRoll);
//        }
//    }

//    private void AddMiddleRandomDressing(GameObject prefab, int count, float coordsMinX, float coordsMinY, float coordsMaxX, float coordsMaxY) {
//        for (var i = 0; i < count; i++) {
//            var obj = Instantiate(prefab);
//            dressingObjs.Add(obj);
//            obj.transform.position = new Vector3(UnityEngine.Random.Range(coordsMinX, coordsMaxX), 145, UnityEngine.Random.Range(coordsMinY, coordsMaxY));
//            obj.transform.Rotate(0, UnityEngine.Random.Range(0, 360), 0);
//        }
//    }

//    private void AddOppositeWallsEvenDressing(GameObject prefab, int count, float coordsMinX, float coordsMinY, float coordsMaxX, float coordsMaxY) {
//        int wallRoll = UnityEngine.Random.Range(0, 2);
//        if (wallRoll==0) {
//            for (int i=1; i<=count; i++) {
//                var x = ((coordsMaxX - coordsMinX) * i / (count + 1)) + coordsMinX;
//                var obj = Instantiate(prefab);
//                dressingObjs.Add(obj);
//                var size = obj.GetComponentInChildren<Renderer>().bounds.size;
//                obj.transform.position = new Vector3(x, 145, coordsMinY + (size.x / 2));
//                var obj2 = Instantiate(prefab);
//                dressingObjs.Add(obj2);
//                obj2.transform.position = new Vector3(x, 145, coordsMaxY - (size.x / 2));
//                RotateProperly(obj, wallRoll);
//                RotateProperly(obj2, wallRoll + 2);
//            }
//        }
//        else {
//            for (int i = 1; i <= count; i++) {
//                var y = ((coordsMaxY - coordsMinY) * i / (count + 1)) + coordsMinY;
//                var obj = Instantiate(prefab);
//                dressingObjs.Add(obj);
//                var size = obj.GetComponentInChildren<Renderer>().bounds.size;
//                obj.transform.position = new Vector3(coordsMinX + (size.x / 2), 145, y);
//                var obj2 = Instantiate(prefab);
//                dressingObjs.Add(obj2);
//                obj2.transform.position = new Vector3(coordsMaxX - (size.x / 2), 145, y);
//                RotateProperly(obj, wallRoll);
//                RotateProperly(obj2, wallRoll + 2);
//            }
//        }
//    }

//    private void AddMiddleEvenDressing(GameObject prefab, int count, float coordsMinX, float coordsMinY, float coordsMaxX, float coordsMaxY) {
//        if (count==2) {
//            var obj = Instantiate(prefab);
//            dressingObjs.Add(obj);
//            obj.transform.position = new Vector3(((coordsMaxX - coordsMinX) / 3) + coordsMinX, 145, ((coordsMaxY - coordsMinY) / 2) + coordsMinY);
//            var obj2 = Instantiate(prefab);
//            dressingObjs.Add(obj2);
//            obj2.transform.position = new Vector3(((coordsMaxX - coordsMinX) * 2 / 3) + coordsMinX, 145, ((coordsMaxY - coordsMinY) / 2) + coordsMinY);
//        }
//        else {
//            var obj = Instantiate(prefab);
//            dressingObjs.Add(obj);
//            obj.transform.position = new Vector3(((coordsMaxX - coordsMinX) / 3) + coordsMinX, 145, ((coordsMaxY - coordsMinY) / 3) + coordsMinY);
//            var obj2 = Instantiate(prefab);
//            dressingObjs.Add(obj2);
//            obj2.transform.position = new Vector3(((coordsMaxX - coordsMinX) / 3) + coordsMinX, 145, ((coordsMaxY - coordsMinY) * 2 / 3) + coordsMinY);
//            var obj3 = Instantiate(prefab);
//            dressingObjs.Add(obj3);
//            obj3.transform.position = new Vector3(((coordsMaxX - coordsMinX) * 2 / 3) + coordsMinX, 145, ((coordsMaxY - coordsMinY) / 3) + coordsMinY);
//            var obj4= Instantiate(prefab);
//            dressingObjs.Add(obj4);
//            obj4.transform.position = new Vector3(((coordsMaxX - coordsMinX) * 2 / 3) + coordsMinX, 145, ((coordsMaxY - coordsMinY) * 2 / 3) + coordsMinY);
//        }
//    }

//    private void AddAllWallsEvenDressing(GameObject prefab, int count, float coordsMinX, float coordsMinY, float coordsMaxX, float coordsMaxY) {
//        for (int i=1; i<=count; i++) {
//            var obj = Instantiate(prefab);
//            dressingObjs.Add(obj);
//            var size = obj.GetComponentInChildren<Renderer>().bounds.size;
//            obj.transform.position = new Vector3(((coordsMaxX - coordsMinX) * i / (count + 1)) + coordsMinX, 145, coordsMinY + (size.x/2));
//            RotateProperly(obj, 0);
//            var obj2 = Instantiate(prefab);
//            dressingObjs.Add(obj2);
//            obj2.transform.position = new Vector3(coordsMinX + (size.x / 2), 145, ((coordsMaxY - coordsMinY) * i / (count + 1)) + coordsMinY);
//            RotateProperly(obj2, 1);
//            var obj3 = Instantiate(prefab);
//            dressingObjs.Add(obj3);
//            obj3.transform.position = new Vector3(((coordsMaxX - coordsMinX) * i / (count + 1)) + coordsMinX, 145, coordsMaxY - (size.x / 2));
//            RotateProperly(obj3, 2);
//            var obj4 = Instantiate(prefab);
//            dressingObjs.Add(obj4);
//            obj4.transform.position = new Vector3(coordsMaxX, 145, ((coordsMaxY - coordsMinY) * i / (count + 1)) + coordsMinY + (size.x / 2));
//            RotateProperly(obj4, 3);
//        }
//    }

//    private void AddOneWallEvenDressing(GameObject prefab, int count, float coordsMinX, float coordsMinY, float coordsMaxX, float coordsMaxY) {
//        int wallRoll = UnityEngine.Random.Range(0, 4);
//        Vector3 size;
//        for (int i = 1; i <= count; i++) {
//            switch (wallRoll) {
//                case 0:
//                    var obj = Instantiate(prefab);
//                    dressingObjs.Add(obj);
//                    size = obj.GetComponentInChildren<Renderer>().bounds.size;
//                    obj.transform.position = new Vector3(((coordsMaxX - coordsMinX) * i / (count + 1)) + coordsMinX, 145, coordsMinY + (size.x / 2));
//                    RotateProperly(obj, 0);
//                    break;
//                case 1:
//                    var obj2 = Instantiate(prefab);
//                    dressingObjs.Add(obj2);
//                    size = obj2.GetComponentInChildren<Renderer>().bounds.size;
//                    obj2.transform.position = new Vector3(coordsMinX + (size.x / 2), 145, ((coordsMaxY - coordsMinY) * i / (count + 1)) + coordsMinY);
//                    RotateProperly(obj2, 1);
//                    break;
//                case 2:
//                    var obj3 = Instantiate(prefab);
//                    dressingObjs.Add(obj3);
//                    size = obj3.GetComponentInChildren<Renderer>().bounds.size;
//                    obj3.transform.position = new Vector3(((coordsMaxX - coordsMinX) * i / (count + 1)) + coordsMinX, 145, coordsMaxY - (size.x / 2));
//                    RotateProperly(obj3, 2);
//                    break;
//                case 3:
//                    var obj4 = Instantiate(prefab);
//                    dressingObjs.Add(obj4);
//                    size = obj4.GetComponentInChildren<Renderer>().bounds.size;
//                    obj4.transform.position = new Vector3(coordsMaxX, 145, ((coordsMaxY - coordsMinY) * i / (count + 1)) + coordsMinY + (size.x / 2));
//                    RotateProperly(obj4, 3);
//                    break;
//            }
//        }
//    }

//    private void RotateProperly(GameObject obj, int wall) {
//        if (wall != 3) obj.transform.Rotate(0, (wall + 1) * 90, 0);
//        var directions = new List<Vector3> { new Vector3(0, 0, -1), new Vector3(-1, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0) };
//        obj.transform.position += directions[wall];
//    }

//    private void InstantiateMapBlock(string blockValue, float coordsX, float coordsY, bool local = false) {
//        var obj = Instantiate(prefabs[mapInitData[blockValue].prefabValue]);
//        if (blockValue=="D" || blockValue=="LD") {
//            int flip = UnityEngine.Random.Range(0, 2);
//            if (flip == 0) {
//                coordsY += 0.5f;
//                obj.GetComponent<Mappable>().yOffset = -0.5f;
//            }
//            else {
//                coordsY -= 0.5f;
//                //if (obj.GetComponent<Door>() != null) obj.GetComponent<Door>().reverseHinge = true;
//                //else obj.GetComponent<LockedDoor>().reverseHinge = true;
//                obj.GetComponent<Mappable>().yOffset = 0.5f;
//            }
//        }
//        else if (blockValue=="RD" || blockValue=="RLD") {
//            int flip = UnityEngine.Random.Range(0, 2);
//            if (flip == 0) {
//                coordsX += 0.5f;
//                obj.GetComponent<Mappable>().xOffset = -0.5f;
//            }
//            else {
//                coordsX -= 0.5f;
//                //if (obj.GetComponent<Door>() != null) obj.GetComponent<Door>().reverseHinge = true;
//                //else obj.GetComponent<LockedDoor>().reverseHinge = true;
//                obj.GetComponent<Mappable>().xOffset = 0.5f;
//            }
//        }
//        obj.transform.position = new Vector3(coordsX, 145 + mapInitData[blockValue].height, coordsY);
//        obj.transform.localScale = mapInitData[blockValue].scale;
//        if (blockValue != "W" && !local) NetworkServer.Spawn(obj);
//    }

//    [Command]
//    public void CmdOnCharacterConnect(int level) {
//        if (currentFloor == 0) return;
//        if (setupDone) return;
//        int avgMobsEncountered = (int)(dungeonMinutes * 60 / dungeonFloors / secondsPerMonster);
//        while (maps.Count < dungeonFloors) GenerateMap();
//        ClearEncounterFlags();
//        SetStartGameCharacterPosition();
//        CmdInstantiateMap();
//        SetMapLevels(level);
//        while (avgMobsEncountered > 0) avgMobsEncountered = map.AddEncounter(avgMobsEncountered, monsterList, character);
//        RoomFeatureBuilder.AddTraps(map, this, currentFloor);
//        setupDone = true;
//    }

//    private void ClearEncounterFlags() {
//        foreach (var room in map.ng.members) room.hasEncounter = false;
//    }

//    [Command]
//    public void CmdSetTargetLevel(int level) {
//        targetLevel = level;
//    }

//    [Command]
//    public void CmdSetCurrentFloor(int floor) {
//        currentFloor = floor;
//        RpcSetCurrentFloor(floor);
//    }

//    [ClientRpc]
//    public void RpcSetCurrentFloor(int floor) {
//        currentFloor = floor;
//    }

//    private void SetMapLevels(int level) {
//        int i = 0;
//        foreach (var map in maps) {
//            map.intendedLevel = level + i;
//            i++;
//        }
//    }

//    private void SetStartGameCharacterPosition() {
//        if (!gameStarted) {
//            SceneInitializer.instance.currentCharPosition = SceneInitializer.charPosition = new Vector3((maps[0].startingX * 2) - 120, 145, (maps[0].startingY * 2) - 120);
//            gameStarted = true;
//        }
//    }

//    private void GenerateMap() {
//        int branches = UnityEngine.Random.Range(minBranches, maxBranches + 1);
//        var ng = new NetworkGraph(branches, finalFloor: (maps.Count == dungeonFloors - 1));
//        bool mapGenerated = false;
//        while (!mapGenerated) mapGenerated = GenerateMap(ng, mapSize, maps.Count + 1);
//        maps.Add(map);
//    }
//}
