using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class OverworldTerrainGenerator : MonoBehaviour {

    public float[,] elevation;
    public int mapSize = 257; //129
    private readonly float randomValueMax = 10f;
    private readonly int featureSize = 32;
    private float highest;
    private float newHighest;
    private List<Vector2> riverPoints = new List<Vector2>();
    private readonly Dictionary<Vector2, float> originalRiverHeights = new Dictionary<Vector2, float>();
    public float perlinScaleFactor = 30f;
    public float perlinFeatureSize = 100f;
    public float perlinMountainProportion = 0.1f, perlinWaterProportion = 0.1f;
    public int fractalPerlinDepth = 4;
    public GameObject dungeonEntrance;
    public GameObject[] trees;
    public GameObject[] bushes;
    public GameObject[] leaves;
    public GameObject[] grass;
    public GameObject[] rocks;
    public GameObject[] flowers;
    public Terrain terrain;
    public float landHeightDivisor;
    public float mountainHeightDivisor = 2f;
    public int riverDrunkenWalkFactor = 4;
    public int riverRadius = 1;
    public int numRivers = 40;
    public float treePercentage = 0.001f;
    public float bushPercentage = 0.004f;
    public float grassPercentage = 0.01f;
    public float flowersPercentage = 0.002f;
    public float leavesPercentage = 0.004f;
    public float rockPercentage = 0.004f;
    public static OverworldTerrainGenerator instance;
    public float randomCoordsMultiplier = 10f;
    public float landHeight = 0f;
    public float progress = 0f;
    public List<OverworldLandmark> landmarks = new List<OverworldLandmark>();
    public GameObject labPrefab;
    public GameObject dungeonPrefab;
    private string[,] floodFillGrid = null;
    private List<string> monsterTypes = new List<string>() {
        "Cyclops",
        "Dark Bishop",
        "Dark Knight",
        "Goblin Archer",
        "Goblin Rogue",
        "GOBLIN",
        "Ogre",
        "Troll",
        "Warlock",
        "Wolf"
    };
    private List<GameObject> monsterPrefabs = new List<GameObject>();
    public static bool loadedPreviouslyMadeWorld = false;
    public static float[,] loadedElevation;
    public static Vector2 loadedBaseCoords;
    public static List<Vector2> loadedDungeonCoords;
    
    // Use this for initialization
    void Start() {
        if (instance != null) {
            Destroy(gameObject);
            Destroy(this);
            return;
        }
        else instance = this;
        //DontDestroyOnLoad(gameObject);
        progress = 0f;
        LoadingProgressBar.StartSecondaryLoadPhase();
        var monsterList = Resources.LoadAll("Prefabs/Monsters");
        foreach (var monster in monsterList) monsterPrefabs.Add((GameObject)monster);
        StartCoroutine(GenerateTerrainFractalPerlinWithTerrainObject());
    }

    public void UpdateProgress(int phase, float percentage) {
        progress = (phase / 13f) + (percentage / 13f);
    }

    public IEnumerator GenerateTerrainFractalPerlinWithTerrainObject() {
        if (loadedPreviouslyMadeWorld) {
            elevation = loadedElevation;
            landmarks.Add(new OverworldLandmark() {
                type = "base",
                position = loadedBaseCoords
            });
            landmarks.Add(new OverworldLandmark() {
                type = "startingPosition",
                position = loadedBaseCoords
            });
            foreach (var dungeonPosition in loadedDungeonCoords) {
                landmarks.Add(new OverworldLandmark() {
                    type = "dungeon",
                    position = dungeonPosition
                });
            }
            highest = 0;
            for (int x = 0; x < mapSize; x++) {
                for (int y = 0; y < mapSize; y++) {
                    if (elevation[x, y] > highest) highest = elevation[x, y];
                }
            }
        }
        else {
            LoadingProgressBar.UpdateProgressText("Generating terrain heights");
            float randomX = Random.Range(0f, 1f) * randomCoordsMultiplier;
            float randomY = Random.Range(0f, 1f) * randomCoordsMultiplier;
            elevation = new float[mapSize, mapSize];
            for (int x = 0; x < mapSize; x++) {
                for (int y = 0; y < mapSize; y++) {
                    elevation[x, y] = 0;
                }
            }
            for (int x = 0; x < mapSize; x++) {
                for (int y = 0; y < mapSize; y++) {
                    var noise = Mathf.PerlinNoise(randomX + ((float)x / mapSize * perlinFeatureSize), randomY + ((float)y / mapSize * perlinFeatureSize));
                    for (int i = 1; i <= fractalPerlinDepth; i++) {
                        noise += Mathf.PerlinNoise(randomX + (x * Mathf.Pow(2, i) / mapSize * perlinFeatureSize), randomY + (y * Mathf.Pow(2, i) / mapSize * perlinFeatureSize));
                    }
                    UpdateProgress(0, ((float)x / mapSize) + ((float)y / mapSize / mapSize));
                    noise *= perlinScaleFactor;
                    elevation[x, y] = noise;
                }
            }
            yield return null;
            LoadingProgressBar.UpdateProgressText("Adding Rivers");
            yield return StartCoroutine(AddRivers());
            yield return null;
            LoadingProgressBar.UpdateProgressText("Widening Rivers");
            yield return StartCoroutine(WidenRivers());
            yield return null;

        }
        LoadingProgressBar.UpdateProgressText("Generating Terrain Object");
        yield return StartCoroutine(GenerateTerrainObject(loadedPreviouslyMadeWorld));
        yield return null;
        LoadingProgressBar.UpdateProgressText("Generating Alpha Map");
        yield return StartCoroutine(GenerateAlphaMap());
        yield return null;
        LoadingProgressBar.UpdateProgressText("Generating Terrain Details");
        yield return StartCoroutine(GenerateDetails());
        yield return null;
        LoadingProgressBar.UpdateProgressText("Generating Trees");
        yield return StartCoroutine(GenerateObjects(treePercentage, trees, 6));
        yield return null;
        LoadingProgressBar.UpdateProgressText("Generating Bushes");
        yield return StartCoroutine(GenerateObjects(bushPercentage, bushes, 7));
        yield return null;
        LoadingProgressBar.UpdateProgressText("Generating Leaves");
        yield return StartCoroutine(GenerateObjects(leavesPercentage, leaves, 8));
        yield return null;
        LoadingProgressBar.UpdateProgressText("Generating Rocks");
        yield return StartCoroutine(GenerateObjects(rockPercentage, rocks, 9));
        yield return null;
        LoadingProgressBar.UpdateProgressText("Generating Flowers");
        yield return StartCoroutine(GenerateObjects(flowersPercentage, flowers, 10));
        yield return null;
        if (loadedPreviouslyMadeWorld) {
            LoadLandmarksFromSave();
        }
        else {
            LoadingProgressBar.UpdateProgressText("Adding Dungeons");
            yield return StartCoroutine(AddLandmarks());
            yield return null;
        }
        LoadingProgressBar.UpdateProgressText("Adding Monsters");
        yield return StartCoroutine(AddMonsters());
        yield return null;
        SetStartGameCharacterPosition();
        GetComponent<NavMeshSurface>().BuildNavMesh();
        Camera.main.GetComponent<Desaturate>().enabled = false;
        LoadingProgressBar.EndLoad();
    }

    public IEnumerator AddMonsters() {
        UpdateProgress(12, 0f);
        for (int i = 0; i < 40; i++) {
            AddEncounter();
            if (i % 4 == 0) {
                UpdateProgress(12, i / 20f);
                yield return null;
            }
        }
    }

    public void AddEncounter() {
        var monsterTypes = GetMonsterTypesForEncounter();
        var numMobs = RNG.Int(3, 6);
        var position = GetValidRandomPosition();
        var baseLevel = PlayerCharacter.localPlayer.GetComponent<ExperienceGainer>().level;
        var minLevel = Mathf.Max(baseLevel - 6, 1);
        var maxLevel = Mathf.Max(baseLevel - 3, 1);
        for (int i=0; i<numMobs; i++) {
            var xRoll = RNG.Float(position.x - 3, position.x + 3);
            var yRoll = RNG.Float(position.z - 3, position.z + 3);
            var typeRoll = RNG.Int(0, monsterTypes.Count);
            var name = monsterTypes[typeRoll];
            int qualityRoll = RNG.Int(0, 100);
            int quality = 0;
            if (qualityRoll < 50) quality = 0;
            else if (qualityRoll < 80) quality = 1;
            else if (qualityRoll < 95) quality = 2;
            else quality = 3;
            var data = new MonsterData(name, name, RNG.Int(minLevel, maxLevel + 1), quality, null);
            InstantiateMonster(data, xRoll, yRoll);
        }
    }

    private List<string> GetMonsterTypesForEncounter() {
        var numTypes = RNG.Int(1, 3);
        var output = new List<string>();
        var baseLevel = PlayerCharacter.localPlayer.GetComponent<ExperienceGainer>().level;
        var minLevel = Mathf.Max(baseLevel - 6, 1);
        var maxLevel = Mathf.Max(baseLevel - 3, 1);
        var eligibleTypes = new List<string>();
        foreach (var monster in monsterPrefabs) if (monsterTypes.Contains(monster.name) && monster.GetComponent<Monster>().minLevel <= maxLevel && monster.GetComponent<Monster>().maxLevel >= minLevel) eligibleTypes.Add(monster.name);
        if (eligibleTypes.Count == 0) eligibleTypes = monsterTypes;
        for (int i=0; i<numTypes; i++) {
            var roll = RNG.Int(0, eligibleTypes.Count);
            output.Add(eligibleTypes[roll]);
        }
        return output;
    }

    private static void InstantiateMonster(MonsterData monster, float xRoll, float yRoll) {
        var prefab = GetMonsterPrefab(monster);
        if (prefab != null) {
            var obj = Instantiate(prefab, new Vector3(xRoll, 24, yRoll), new Quaternion());
            SetupMonster(obj, monster.level, monster.quality, PlayerCharacter.localPlayer.gameObject);
        }
    }

    private static GameObject GetMonsterPrefab(MonsterData monster) {
        return (GameObject)(Resources.Load("Prefabs/Monsters/" + monster.specificType));
    }

    private static void SetupMonster(GameObject obj, int level, int quality, GameObject player) {
        var mob = obj.GetComponent<Monster>();
        //mob.GetComponent<MonsterCombatant>().player = player;
        mob.GetComponent<MonsterScaler>().AdjustForLevel(level);
        mob.GetComponent<MonsterScaler>().quality = quality;
        mob.GetComponent<MonsterScaler>().numPlayers = 1;
        mob.GetComponent<MonsterScaler>().Scale();
        var billboard = obj.GetComponentInChildren<Billboard>();
        if (billboard != null) billboard.mainCamera = Camera.main;
        if (quality == 4) mob.gameObject.AddComponent<Boss>();
    }

    public void LoadLandmarksFromSave() {
        foreach (var landmark in landmarks) {
            if (landmark.type == "base") Instantiate(labPrefab, new Vector3(landmark.position.x, 24, landmark.position.y), new Quaternion());
            else if (landmark.type == "dungeon") {
                var obj = Instantiate(dungeonPrefab, new Vector3(landmark.position.x, 24, landmark.position.y), new Quaternion());
                obj.GetComponent<DungeonEntrance>().dungeonLevel = PlayerCharacter.localPlayer.GetComponent<ExperienceGainer>().level;
            }
        }
    }

    public IEnumerator AddLandmarks() {
        UpdateProgress(11, 0f);
        landmarks.Clear();
        FindPlayerStartingLocation();
        Instantiate(labPrefab, new Vector3(landmarks[0].position.x, 24, landmarks[0].position.y), new Quaternion());
        yield return StartCoroutine(AddDungeons(10));
        yield return null;
    }

    private void FindPlayerStartingLocation() {
        var startX = 0;
        var startY = 0;
        while (landmarks.Count == 0) {
            startX = Random.Range(0, 1024);
            startY = Random.Range(0, 1024);
            var height = terrain.SampleHeight(new Vector3(startX, 0, startY));
            if (height / newHighest < 1 - perlinMountainProportion && height > 0) {
                landmarks.Add(new OverworldLandmark() {
                    position = new Vector2(startX, startY),
                    type = "base"
                });
            }
        }
        while (landmarks.Count == 1) {
            var x = Random.Range(-1, 1);
            var y = Random.Range(-1, 1);
            var height = terrain.SampleHeight(new Vector3(startX + x, 0, startY + y));
            if (height / newHighest < 1 - perlinMountainProportion && height > 0) {
                landmarks.Add(new OverworldLandmark() {
                    position = new Vector2(startX + x, startY + y),
                    type = "startingPosition"
                });
            }
        }
    }

    private IEnumerator AddDungeons(int number) {
        for (int i = 0; i < number; i++) {
            var position = GetValidRandomPosition();
            landmarks.Add(new OverworldLandmark() {
                position = new Vector2(position.x, position.z),
                type = "dungeon"
            });
            var obj = Instantiate(dungeonPrefab, position, new Quaternion());
            obj.GetComponent<DungeonEntrance>().dungeonLevel = PlayerCharacter.localPlayer.GetComponent<ExperienceGainer>().level;
            UpdateProgress(11, (float)i / number);
            yield return null;
        }
    }

    private Vector3 GetValidRandomPosition() {
        var startingPosition = landmarks[0].position;
        if (floodFillGrid == null) {
            floodFillGrid = new string[1024, 1024];
            for (int x = 0; x < 1024; x++) {
                for (int y = 0; y < 1024; y++) {
                    var height = terrain.SampleHeight(new Vector3(x, 0, y));
                    if (height / newHighest < 1 - perlinMountainProportion && height > 0) floodFillGrid[x, y] = "y";
                    else floodFillGrid[x, y] = "n";
                }
            }
            FloodFill(floodFillGrid, startingPosition, "y", "r");
        }
        while (true) {
            var x = Random.Range(0, 1024);
            var y = Random.Range(0, 1024);
            if (floodFillGrid[x, y] == "r") return new Vector3(x, 24, y);
        }
    }

    private void FloodFill(string[,] floodFillGrid, Vector2 position, string targetLetter, string replacementLetter) {
        List<Vector2> queue = new List<Vector2> {
            position
        };
        floodFillGrid[(int)position.x, (int)position.y] = replacementLetter;
        while (queue.Count > 0) {
            position = queue[0];
            queue.Remove(position);
            var x = (int)position.x;
            var y = (int)position.y;
            if (x < 0 || y < 0 || x >= mapSize || y >= mapSize) continue;
            FloodFillStep(floodFillGrid, queue, new Vector2(position.x - 1, position.y), targetLetter, replacementLetter);
            FloodFillStep(floodFillGrid, queue, new Vector2(position.x + 1, position.y), targetLetter, replacementLetter);
            FloodFillStep(floodFillGrid, queue, new Vector2(position.x, position.y - 1), targetLetter, replacementLetter);
            FloodFillStep(floodFillGrid, queue, new Vector2(position.x, position.y + 1), targetLetter, replacementLetter);
        }
    }

    private void FloodFillStep(string [,] floodFillGrid, List<Vector2> queue, Vector2 position, string targetLetter, string replacementLetter) {
        var x = (int)position.x;
        var y = (int)position.y;
        if (x < 0 || y < 0 || x >= mapSize || y >= mapSize) return;
        if (floodFillGrid[x, y] == targetLetter && floodFillGrid[x, y] != replacementLetter) {
            floodFillGrid[x, y] = replacementLetter;
            queue.Add(position);
        }
    }

    public IEnumerator GenerateObjects(float objectPercentage, GameObject[] objects, int loadPhase) {
        for (int x = 0; x < mapSize; x++) {
            for (int y = 0; y < mapSize; y++) {
                var height = terrain.SampleHeight(new Vector3(x, 0, y));
                if (height / newHighest < 1 - perlinMountainProportion && height > 0) {
                    if (objectPercentage<1) {
                        float roll = Random.Range(0f, 1f);
                        if (roll <= objectPercentage) {
                            int whichObject = Random.Range(0, objects.Count());
                            var obj = Instantiate(objects[whichObject], new Vector3(x + Random.Range(-0.25f, 0.25f), height, y + Random.Range(-0.25f, 0.25f)), objects[whichObject].transform.rotation);
                            obj.transform.Rotate(0, Random.Range(0, 360), 0);
                            var scaleFactor = Random.Range(0.35f, 0.65f);
                            obj.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                        }
                    }
                    else {
                        int roll = (int)Random.Range(0, objectPercentage * 2);
                        for (int i=0; i<roll; i++) {
                            int whichObject = Random.Range(0, objects.Count());
                            var obj = Instantiate(objects[whichObject], new Vector3(x + Random.Range(-0.25f, 0.25f), height, y + Random.Range(-0.25f, 0.25f)), objects[whichObject].transform.rotation);
                            obj.transform.Rotate(0, Random.Range(0, 360), 0);
                            var scaleFactor = Random.Range(0.35f, 0.65f);
                            obj.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                        }
                    }
                }
            }
            if (x % 100 == 0) {
                UpdateProgress(loadPhase, (float)x / mapSize);
                yield return null;
            }
        }
    }
    
    public IEnumerator GenerateDetails() {
        for (int z = 0; z < 14; z++) {
            int[,] details = new int[terrain.terrainData.detailResolution, terrain.terrainData.detailResolution];
            for (int x = 0; x < terrain.terrainData.detailResolution; x++) {
                for (int y = 0; y < terrain.terrainData.detailResolution; y++) {
                    if (elevation[x, y] == landHeight) {
                        var random = Random.Range(0, 30);
                        if (random == 0) details[x, y] = Random.Range(0, 15);
                        else details[x, y] = 0;
                    }
                    else details[x, y] = 0;
                }
            }
            terrain.terrainData.SetDetailLayer(0, 0, z, details);
            if (z % 2 == 0) {
                UpdateProgress(5, z / 14f);
                yield return null;
            }
        }
    }

    public IEnumerator GenerateTerrainObject(bool loadedPreviouslyMadeWorld) {
        if (!loadedPreviouslyMadeWorld) {
            landHeight = (1 - perlinMountainProportion) / 2f / landHeightDivisor;
            for (int x = 0; x < mapSize; x++) {
                for (int y = 0; y < mapSize; y++) {
                    if (elevation[x, y] / highest <= perlinWaterProportion) elevation[x, y] = 0f;
                    else if (elevation[x, y] / highest < 1 - perlinMountainProportion) elevation[x, y] = landHeight;
                    else elevation[x, y] = elevation[x, y] / highest / mountainHeightDivisor;

                    //if (elevation[x, y] < perlinLandThreshold) elevation[x, y] = perlinLandThreshold / highest / terrainHeightDivisor / mountainHeightDivisor;
                    //else if (elevation[x, y] < perlinMountainThreshold) elevation[x, y] = perlinLandThreshold / highest / terrainHeightDivisor;
                    //else {
                    //    var heightAboveLand = elevation[x, y] / highest / terrainHeightDivisor - perlinLandThreshold / highest / terrainHeightDivisor;
                    //    elevation[x, y] = perlinLandThreshold / highest / terrainHeightDivisor + heightAboveLand / mountainHeightDivisor;
                    //}
                }
                if (x % 100 == 0) {
                    UpdateProgress(3, (float)x / mapSize);
                    yield return null;

                }
            }
        }
        terrain.terrainData.heightmapResolution = mapSize;
        terrain.terrainData.SetHeights(0, 0, elevation);
    }

    public IEnumerator GenerateAlphaMap() {
        TerrainData terrainData = terrain.terrainData;
        terrainData.alphamapResolution = mapSize;
        float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];
        newHighest = 0;
        for (int y=0; y<terrainData.heightmapResolution; y++) {
            for (int x=0; x<terrainData.heightmapResolution; x++) {
                var height = terrain.SampleHeight(new Vector3(x, 0, y));
                if (height > newHighest) newHighest = height;
            }
            if (y % 200 == 0) {
                UpdateProgress(4, ((float)y / terrainData.heightmapResolution / 2f));
                yield return null;
            }
        }
        for (int y = 0; y < terrainData.alphamapHeight; y++) {
            for (int x = 0; x < terrainData.alphamapWidth; x++) {
                float y_01 = y / (float)terrainData.alphamapHeight;
                float x_01 = x / (float)terrainData.alphamapWidth;
                float height = terrainData.GetHeight(Mathf.RoundToInt(y_01 * terrainData.heightmapResolution), Mathf.RoundToInt(x_01 * terrainData.heightmapResolution));
                //Vector3 normal = terrainData.GetInterpolatedNormal(y_01, x_01);
                //float steepness = terrainData.GetSteepness(y_01, x_01);

                float[] splatWeights = new float[terrainData.alphamapLayers];
                if (height / newHighest > (1 - perlinMountainProportion)) splatWeights[1] = 1f;
                else if (height == 0) splatWeights[2] = 1f;
                else splatWeights[0] = 1f;

                float z = splatWeights.Sum();
                for (int i = 0; i < terrainData.alphamapLayers; i++) {
                    splatWeights[i] /= z;
                    splatmapData[x, y, i] = splatWeights[i];
                }
            }
            if (y % 200 == 0) {
                UpdateProgress(4, 0.5f + ((float)y / terrainData.heightmapResolution / 2f));
                yield return null;
            }
        }
        terrainData.SetAlphamaps(0, 0, splatmapData);
    }

    private IEnumerator AddRivers() {
        highest = 0;
        for (int x = 0; x < mapSize; x++) {
            for (int y = 0; y < mapSize; y++) {
                if (elevation[x, y] > highest) highest = elevation[x, y];
            }
        }
        for (int i = 0; i < numRivers; i++) {
            Vector2[] points = new Vector2[20];
            for (int j = 0; j < 20; j++) {
                points[j].x = Random.Range(0, mapSize);
                points[j].y = Random.Range(0, mapSize);
            }
            var highest = FindHighestPoint(points);
            AddRiver((int)highest.x, (int)highest.y);
            if (i % 8 == 0) {
                UpdateProgress(1, (float)i / numRivers);
                yield return null;
            }
        }
    }

    private Vector2 FindHighestPoint(Vector2[] points) {
        float highestValue = 0;
        Vector2 highestPoint = new Vector2(0,0);
        foreach (var point in points) {
            if (elevation[(int)point.x, (int)point.y] > highestValue) {
                highestValue = elevation[(int)point.x, (int)point.y];
                highestPoint = point;
            }
        }
        return highestPoint;
    }
    private void AddRiver(int startingX, int startingY) {
        var roll = Random.Range(0f, 18000f);
        var coords = new Vector2(startingX, startingY);
        var length = 4000;
        var points = new List<float>();
        for (int j = 0; j < length; j++) points.Add(Mathf.PerlinNoise(roll + j, 0));
        var cursor = Random.Range(0f, 360f);
        var i = 0;
        while (InBounds((int)coords.x, (int)coords.y) && (elevation[(int)coords.x, (int)coords.y] == 0 || elevation[(int)coords.x, (int)coords.y] > perlinWaterProportion / highest)) {
            i++;
            if (i >= length) break;
            RegisterRiverPoint(coords);
            if ((int)coords.x == 0 || (int)coords.y == 0 || (int)coords.x == mapSize - 1 || (int)coords.y == mapSize - 1) return;
            var northElev = elevation[(int)coords.x, (int)coords.y - 1];
            var southElev = elevation[(int)coords.x, (int)coords.y + 1];
            var westElev = elevation[(int)coords.x - 1, (int)coords.y];
            var eastElev = elevation[(int)coords.x + 1, (int)coords.y];
            float minimum = 100;
            if (northElev < minimum && northElev != 0) minimum = northElev;
            if (southElev < minimum && southElev != 0) minimum = southElev;
            if (eastElev < minimum && eastElev != 0) minimum = eastElev;
            if (westElev < minimum && westElev != 0) minimum = westElev;
            if (northElev == minimum) coords = RiverWalkNorth(coords);
            else if (southElev == minimum) coords = RiverWalkSouth(coords);
            else if (westElev == minimum) coords = RiverWalkWest(coords);
            else if (eastElev == minimum) coords = RiverWalkEast(coords);

            RegisterRiverPoint(coords);

            var noisePoint = points[i];
            if (noisePoint < 0.5f) cursor -= (90 * (noisePoint * 2));
            else cursor += (90 * (noisePoint - 0.5f) * 2);
            if (cursor < 0) cursor += 360;
            else if (cursor >= 360) cursor -= 360;
            if (cursor >= 0 && cursor < 90 && InBounds((int)coords.x, (int)coords.y - 1) && elevation[(int)coords.x, (int)coords.y - 1] != 0) coords = new Vector2(coords.x, coords.y - 1);
            else if (cursor >= 90 && cursor < 180 && InBounds((int)coords.x + 1, (int)coords.y) && elevation[(int)coords.x + 1, (int)coords.y] != 0) coords = new Vector2(coords.x + 1, coords.y);
            else if (cursor >= 180 && cursor < 270 && InBounds((int)coords.x, (int)coords.y + 1) && elevation[(int)coords.x, (int)coords.y + 1] != 0) coords = new Vector2(coords.x, coords.y + 1);
            else if (cursor >= 270 && cursor < 360 && InBounds((int)coords.x - 1, (int)coords.y) && elevation[(int)coords.x - 1, (int)coords.y] != 0) coords = new Vector2(coords.x - 1, coords.y);
        }
    }

    private Vector2 RiverWalkNorth(Vector2 coords) {
        coords.y--;
        //RegisterRiverPoint(coords);
        //coords.y--;
        return coords;
    }

    private Vector2 RiverWalkSouth(Vector2 coords) {
        coords.y++;
        //RegisterRiverPoint(coords);
        //coords.y++;
        return coords;
    }

    private Vector2 RiverWalkEast(Vector2 coords) {
        coords.x++;
        //RegisterRiverPoint(coords);
        //coords.x++;
        return coords;
    }

    private Vector2 RiverWalkWest(Vector2 coords) {
        coords.x--;
        //RegisterRiverPoint(coords);
        //coords.x--;
        return coords;
    }

    private void RegisterRiverPoint(Vector2 coords) {
        if (!InBounds((int)coords.x, (int)coords.y)) return;
        originalRiverHeights[new Vector2((int)coords.x, (int)coords.y)] = elevation[(int)coords.x, (int)coords.y];
        elevation[(int)coords.x, (int)coords.y] = 0;
        riverPoints.Add(coords);
    }

    private IEnumerator WidenRivers() {
        int i = 0;
        foreach (var point in riverPoints) {
            for (int x = (int)point.x - riverRadius; x <= (int)point.x + riverRadius; x++) {
                for (int y = (int)point.y - riverRadius; y <= (int)point.y + riverRadius; y++) {
                    if (InBounds(x, y)) {
                        originalRiverHeights[new Vector2(x, y)] = elevation[x, y];
                        if (elevation[x, y] != 0) elevation[x, y] = 0;
                    }
                }
            }
            if (i % riverPoints.Count / 10 == 0) {
                UpdateProgress(2, (float)i / riverPoints.Count);
                yield return null;
            }
            i++;
        }
    }

    private bool InBounds(int x, int y) {
        if (x < 0) return false;
        if (y < 0) return false;
        if (x >= mapSize) return false;
        if (y >= mapSize) return false;
        return true;
    }

    private void SetStartGameCharacterPosition() {
        foreach (var item in landmarks) {
            if (item.type=="startingPosition") {
                PlayerCharacter.localPlayer.transform.position = new Vector3(item.position.x, 23.69529f, item.position.y);
            }
        }
    }
}

public class OverworldLandmark {
    public Vector2 position;
    public string type;
}