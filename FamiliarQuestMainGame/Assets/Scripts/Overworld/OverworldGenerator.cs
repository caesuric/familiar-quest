using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class OverworldGenerator : MonoBehaviour {

    public static OverworldGenerator instance;
    public static bool loadedPreviouslyMadeWorld = false;
    public static float[,] loadedElevation;
    public static Vector2 loadedBaseCoords;
    public static List<Vector2> loadedDungeonCoords;

    public float[,] elevation;
    public int mapSize = 257; //129
    public float highest;
    public float newHighest;
    public float perlinScaleFactor = 1.25f;
    public float perlinFeatureSize = 0.3f;
    public int fractalPerlinDepth = 6;
    public int riverRadius = 2;
    public int numRivers = 80;
    public float randomCoordsMultiplier = 10f;
    public float progress = 0f;
    public Terrain terrain;
    public GameObject labPrefab;
    public GameObject dungeonPrefab;
    public GameObject[] trees;
    public GameObject[] bushes;
    public GameObject[] leaves;
    public GameObject[] rocks;
    public GameObject[] flowers;
    private readonly float randomValueMax = 10f;
    private readonly int featureSize = 32;
    private List<Vector2> riverPoints = new List<Vector2>();
    private readonly Dictionary<Vector2, float> originalRiverHeights = new Dictionary<Vector2, float>();

    // Use this for initialization
    void Start() {
        if (instance != null) {
            Destroy(gameObject);
            Destroy(this);
            return;
        }
        else instance = this;
        progress = 0f;
        LoadingProgressBar.StartSecondaryLoadPhase();
        StartCoroutine(Generate());
    }

    public void UpdateProgress(int phase, float percentage) {
        progress = (phase / 13f) + (percentage / 13f);
    }

    public IEnumerator Generate() {
        if (loadedPreviouslyMadeWorld) OverworldLoader.Load();
        else yield return StartCoroutine(GenerateElevations());
        yield return StartCoroutine(OverworldTerrainGenerator.Generate());
        yield return StartCoroutine(OverworldPrefabGenerator.Generate());
        if (loadedPreviouslyMadeWorld) OverworldLandmarkGenerator.Load();
        else yield return StartCoroutine(OverworldLandmarkGenerator.Generate());
        SetStartGameCharacterPosition();
        GetComponent<NavMeshSurface>().BuildNavMesh();
        Camera.main.GetComponent<Desaturate>().enabled = false;
        LoadingProgressBar.EndLoad();
    }

    private IEnumerator GenerateElevations() {
        LoadingProgressBar.UpdateProgressText("Generating terrain heights");
        GeneratePerlinNoiseHeights();
        yield return null;
        LoadingProgressBar.UpdateProgressText("Adding Rivers");
        yield return StartCoroutine(AddRivers());
        yield return null;
        LoadingProgressBar.UpdateProgressText("Widening Rivers");
        yield return StartCoroutine(WidenRivers());
        yield return null;
    }

    private void GeneratePerlinNoiseHeights() {
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
        while (InBounds((int)coords.x, (int)coords.y) && (elevation[(int)coords.x, (int)coords.y] == 0 || elevation[(int)coords.x, (int)coords.y] > OverworldTerrainGenerator.perlinWaterProportion / highest)) {
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
        foreach (var item in OverworldLandmarkGenerator.landmarks) {
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