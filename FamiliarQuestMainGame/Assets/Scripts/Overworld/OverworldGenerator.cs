using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OverworldGenerator : MonoBehaviour {

    public static OverworldGenerator instance;
    public static bool loadedPreviouslyMadeWorld = false;
    public static float[,] loadedElevation;
    public static Vector2 loadedBaseCoords;
    public static List<OverworldDungeon> loadedDungeons;

    public float[,] elevation;
    public int mapSize = 257; //129
    public float highest;
    public float newHighest;
    public float perlinScaleFactor = 1.25f;
    public float perlinFeatureSize = 0.3f;
    public int fractalPerlinDepth = 6;
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
        yield return StartCoroutine(OverworldEncounterGenerator.Generate());
        Camera.main.GetComponent<Desaturate>().enabled = false;
        LoadingProgressBar.EndLoad();
    }

    private IEnumerator GenerateElevations() {
        LoadingProgressBar.UpdateProgressText("Generating terrain heights");
        GeneratePerlinNoiseHeights();
        yield return null;
        LoadingProgressBar.UpdateProgressText("Adding Rivers");
        yield return StartCoroutine(OverworldRiverGenerator.AddRivers());
        yield return null;
        LoadingProgressBar.UpdateProgressText("Widening Rivers");
        yield return StartCoroutine(OverworldRiverGenerator.WidenRivers());
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

    private void SetStartGameCharacterPosition() {
        foreach (var item in OverworldLandmarkGenerator.landmarks) {
            if (item.type=="startingPosition") {
                PlayerCharacter.localPlayer.transform.position = new Vector3(item.position.x, 23.69529f, item.position.y);
            }
        }
    }
}
