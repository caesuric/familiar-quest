using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OverworldTerrainGenerator : MonoBehaviour {

    private float[,] elevation;
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
    public GameObject dungeonPrefab;
    
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
        StartCoroutine(GenerateTerrainFractalPerlinWithTerrainObject());
    }

    public void UpdateProgress(int phase, float percentage) {
        if (phase < 11) progress = ((phase / 12f) + (percentage / 12f)) / 4f;
        else progress = 0.25f + percentage * 0.75f;
    }

    public IEnumerator GenerateTerrainFractalPerlinWithTerrainObject() {
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
        LoadingProgressBar.UpdateProgressText("Generating Terrain Object");
        yield return StartCoroutine(GenerateTerrainObject());
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
        LoadingProgressBar.UpdateProgressText("Adding Dungeons");
        yield return StartCoroutine(AddLandmarks());
        yield return null;
        SetStartGameCharacterPosition();
        Camera.main.GetComponent<Desaturate>().enabled = false;
        LoadingProgressBar.EndLoad();
    }

    public IEnumerator AddLandmarks() {
        UpdateProgress(11, 0f);
        landmarks.Clear();
        FindPlayerStartingLocation();
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
        var grid = new bool[1024, 1024];
        for (int x = 0; x < 1024; x++) {
            for (int y = 0; y < 1024; y++) {
                var height = terrain.SampleHeight(new Vector3(x, 0, y));
                if (height / newHighest < 1 - perlinMountainProportion && height > 0) grid[x, y] = true;
                else grid[x, y] = false;
            }
        }
        var overworldGraph = new OverworldGraph(0, 1024, 1024, grid);
        for (int i = 0; i < number; i++) {
            try {
                var astar = new TerrainMapAstar();
                var position = GetValidDungeonPosition(astar, overworldGraph);
                overworldGraph.AddTeleport(new Coordinates((int)position.x, (int)position.z), new Coordinates((int)landmarks[0].position.x, (int)landmarks[0].position.y));
                var obj = Instantiate(dungeonPrefab);
                obj.transform.position = position;
            }
            catch {
                // do nothing
            }
            UpdateProgress(11, (float)i / number);
            yield return null;
        }
    }

    private Vector3 GetValidDungeonPosition(TerrainMapAstar astar, OverworldGraph overworldGraph) {
        var startingPosition = landmarks[0].position;
        var start = new Coordinates((int)startingPosition.x, (int)startingPosition.y);
        while (true) {
            var x = Random.Range(0, 1024);
            var y = Random.Range(0, 1024);
            var search = astar.SearchOpenAreas(overworldGraph, new Coordinates(x, y), start);
            if (SearchValid(search, new Coordinates(x, y), start)) return new Vector3(x, 24, y);
        }
    }

    private bool SearchValid(Dictionary<Coordinates, Coordinates> search, Coordinates startCoordinates, Coordinates endCoordinates) {
        var chain = new List<Coordinates>();
        var cursor = endCoordinates;
        while (search.ContainsKey(cursor) && search[cursor]!=cursor) {
            chain.Add(cursor);
            cursor = search[cursor];
        }
        if (search.ContainsKey(cursor)) chain.Add(search[cursor]);
        if (chain.Count > 0) chain.Reverse();
        if (chain.Count == 0 || chain[0] != startCoordinates) return false;
        return true;
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

    public IEnumerator GenerateTerrainObject() {
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