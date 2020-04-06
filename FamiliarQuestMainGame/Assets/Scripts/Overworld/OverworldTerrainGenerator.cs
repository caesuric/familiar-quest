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
    private Dictionary<Vector2, float> originalRiverHeights = new Dictionary<Vector2, float>();
    public float perlinScaleFactor = 30f;
    public float perlinFeatureSize = 100f;
    public float perlinMountainThreshold = 14, perlinLandThreshold = 8;
    public int fractalPerlinDepth = 4;
    public GameObject dungeonEntrance;
    public GameObject[] trees;
    public Terrain terrain;
    public float terrainHeightDivisor;
    public float mountainHeightDivisor = 2f;
    public int riverDrunkenWalkFactor = 4;
    public int riverRadius = 1;
    public int numRivers = 40;
    public float treePercentage = 0.001f;
    public static OverworldTerrainGenerator instance;
    public float randomCoordsMultiplier = 10f;
    
    // Use this for initialization
    void Start() {
        if (instance != null) {
            Destroy(gameObject);
            Destroy(this);
            return;
        }
        else instance = this;
        DontDestroyOnLoad(gameObject);
        GenerateTerrainFractalPerlinWithTerrainObject();
    }

    public void GenerateTerrainFractalPerlinWithTerrainObject() {
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
                noise *= perlinScaleFactor;
                elevation[x, y] = noise;
                if (x==0 && y==0) Debug.Log(noise);
            }
        }

        AddRivers(perlinLandThreshold);
        WidenRivers();
        GenerateDetails();
        GenerateTerrainObject();
        GenerateAlphaMap();
        GenerateTrees();
    }

    public void GenerateTrees() {
        for (int x = 0; x < mapSize; x++) {
            for (int y = 0; y < mapSize; y++) {

                //if (height > perlinMountainThreshold * newHighest / highest) splatWeights[1] = 1f;

                var height = terrain.SampleHeight(new Vector3(x, 0, y));
                if (height < perlinMountainThreshold * newHighest / highest && height >= perlinLandThreshold * newHighest / highest) {
                    float roll = Random.Range(0f, 1f);
                    if (roll <= treePercentage) {
                        int whichTree = Random.Range(0, trees.Count());
                        var treeObj = Instantiate(trees[whichTree], new Vector3(x, height, y), trees[whichTree].transform.rotation);
                        treeObj.transform.Rotate(0, Random.Range(0, 360), 0);
                        var scaleFactor = Random.Range(0.35f, 0.65f);
                        treeObj.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                    }
                }
            }
        }
    }

    public void GenerateDetails() {
        int[,] details = new int[terrain.terrainData.detailResolution, terrain.terrainData.detailResolution];
        for (int x = 0; x < terrain.terrainData.detailResolution; x++) {
            for (int y = 0; y < terrain.terrainData.detailResolution; y++) {
                if (elevation[x, y] >= perlinLandThreshold && elevation[x, y] < perlinMountainThreshold) {
                    var random = Random.Range(0, 8);
                    if (random < 4) details[x, y] = 0;
                    else if (random < 6) details[x, y] = 1;
                    else details[x, y] = 2;
                }
                else details[x, y] = 0;
            }
        }
        terrain.terrainData.SetDetailLayer(0, 0, 0, details);
    }

    public void GenerateTerrainObject() {
        highest = 0;
        for (int x = 0; x < mapSize; x++) {
            for (int y = 0; y < mapSize; y++) {
                if (elevation[x, y] > highest) highest = elevation[x, y];
            }
        }
        for (int x = 0; x < mapSize; x++) {
            for (int y = 0; y < mapSize; y++) {
                if (elevation[x, y] < perlinLandThreshold) elevation[x, y] = perlinLandThreshold / highest / terrainHeightDivisor / mountainHeightDivisor;
                else if (elevation[x, y] < perlinMountainThreshold) elevation[x, y] = perlinLandThreshold / highest / terrainHeightDivisor;
                else {
                    var heightAboveLand = elevation[x, y] / highest / terrainHeightDivisor - perlinLandThreshold / highest / terrainHeightDivisor;
                    elevation[x, y] = perlinLandThreshold / highest / terrainHeightDivisor + heightAboveLand / mountainHeightDivisor;
                }

                //with divisors, no terracing
                //if (elevation[x, y] < perlinLandThreshold) elevation[x, y] = elevation[x, y] / highest / terrainHeightDivisor;
                //else if (elevation[x, y] < perlinMountainThreshold) elevation[x, y] = elevation[x, y] / highest / terrainHeightDivisor;
                //else {
                //    var heightAboveLand = elevation[x, y] / highest / terrainHeightDivisor - perlinLandThreshold / highest / terrainHeightDivisor;
                //    elevation[x, y] = perlinLandThreshold / highest / terrainHeightDivisor + heightAboveLand;
                //}

                //raw
                //elevation[x, y] = elevation[x, y] / highest;
            }
        }

        terrain.terrainData.heightmapResolution = mapSize;
        terrain.terrainData.SetHeights(0, 0, elevation);
    }

    public void GenerateAlphaMap() {
        TerrainData terrainData = terrain.terrainData;
        terrainData.alphamapResolution = mapSize;
        float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];
        newHighest = 0;
        for (int y=0; y<terrainData.heightmapResolution; y++) {
            for (int x=0; x<terrainData.heightmapResolution; x++) {
                var height = terrainData.GetHeight(x, y);
                if (height > newHighest) newHighest = height;
            }
        }
        for (int y = 0; y < terrainData.alphamapHeight; y++) {
            for (int x = 0; x < terrainData.alphamapWidth; x++) {
                float y_01 = (float)y / (float)terrainData.alphamapHeight;
                float x_01 = (float)x / (float)terrainData.alphamapWidth;
                float height = terrainData.GetHeight(Mathf.RoundToInt(y_01 * terrainData.heightmapResolution), Mathf.RoundToInt(x_01 * terrainData.heightmapResolution));
                //Vector3 normal = terrainData.GetInterpolatedNormal(y_01, x_01);
                //float steepness = terrainData.GetSteepness(y_01, x_01);

                float[] splatWeights = new float[terrainData.alphamapLayers];
                if (height > perlinMountainThreshold * newHighest / highest) splatWeights[1] = 1f;
                else splatWeights[0] = 1f;

                float z = splatWeights.Sum();
                for (int i = 0; i < terrainData.alphamapLayers; i++) {
                    splatWeights[i] /= z;
                    splatmapData[x, y, i] = splatWeights[i];
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, splatmapData);
    }

    private void AddRivers(float landThreshold) {
        for (int i = 0; i < numRivers; i++) {
            Vector2[] points = new Vector2[20];
            for (int j=0; j<20; j++) {
                points[j].x = Random.Range(0, mapSize);
                points[j].y = Random.Range(0, mapSize);
            }
            var highest = FindHighestPoint(points);
            AddRiver((int)highest.x, (int)highest.y, landThreshold);
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

    private void AddRiver(int startingX, int startingY, float landThreshold) {
        var coords = new Vector2(startingX, startingY);
        while (elevation[(int)coords.x, (int)coords.y] >= landThreshold) {
            originalRiverHeights[new Vector2((int)coords.x, (int)coords.y)] = elevation[(int)coords.x, (int)coords.y];
            elevation[(int)coords.x, (int)coords.y] = 0;
            riverPoints.Add(coords);
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
            if (northElev == minimum) coords = RiverWalkNorth(coords); //coords.y--;
            else if (southElev == minimum) coords = RiverWalkSouth(coords); //coords.y++;
            else if (westElev == minimum) coords = RiverWalkWest(coords); //coords.x--;
            else if (eastElev == minimum) coords = RiverWalkEast(coords); //coords.x++;
        }
    }

    private Vector2 RiverWalkNorth(Vector2 coords) {
        int random = Random.Range(0, riverDrunkenWalkFactor);
        if (random == 0) coords.x--;
        else if (random == 1) coords.x++;
        else coords.y--;
        return coords;
    }

    private Vector2 RiverWalkSouth(Vector2 coords) {
        int random = Random.Range(0, riverDrunkenWalkFactor);
        if (random == 0) coords.x--;
        else if (random == 1) coords.x++;
        else coords.y++;
        return coords;
    }

    private Vector2 RiverWalkEast(Vector2 coords) {
        int random = Random.Range(0, riverDrunkenWalkFactor);
        if (random == 0) coords.y--;
        else if (random == 1) coords.y++;
        else coords.x++;
        return coords;
    }

    private Vector2 RiverWalkWest(Vector2 coords) {
        int random = Random.Range(0, riverDrunkenWalkFactor);
        if (random == 0) coords.y--;
        else if (random == 1) coords.y++;
        else coords.x--;
        return coords;
    }

    private void WidenRivers() {
        foreach (var point in riverPoints) {
            for (int x = (int)point.x - riverRadius; x <= (int)point.x + riverRadius; x++) {
                for (int y = (int)point.y - riverRadius; y <= (int)point.y + riverRadius; y++) {
                    if (InBounds(x, y)) {
                        originalRiverHeights[new Vector2(x, y)] = elevation[x, y];
                        if (elevation[x, y] != 0) elevation[x, y] = 0;
                    }
                }
            }
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
        while (true) {
            int x = Random.Range(0, mapSize);
            int y = Random.Range(0, mapSize);
            if (elevation[x, y] > 8 && elevation[x, y] < 14) {
                SceneInitializer.instance.currentCharPosition = SceneInitializer.charPosition = new Vector3(x * 2, 145, y * 2);
                var go = Instantiate(dungeonEntrance);
                go.transform.position = new Vector3((x + 1) * 2, 145, y * 2);
                return;
            }
        }
    }
}
