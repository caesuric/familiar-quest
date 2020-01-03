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
    public float perlinScaleFactor = 30f;
    public float perlinFeatureSize = 100f;
    public float perlinMountainThreshold = 14, perlinLandThreshold = 8;
    public int fractalPerlinDepth = 4;
    public GameObject prefab;
    public GameObject grassPrefab;
    public GameObject waterPrefab;
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
        float randomX = Random.Range(0f, 1f) * perlinFeatureSize;
        float randomY = Random.Range(0f, 1f) * perlinFeatureSize;
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
                    noise += Mathf.PerlinNoise(randomX + ((float)x * Mathf.Pow(2, i) / mapSize / Mathf.Pow(2, i) * perlinFeatureSize), randomY + ((float)y * Mathf.Pow(2, i) / mapSize / Mathf.Pow(2, i) * perlinFeatureSize));
                }
                noise *= perlinScaleFactor;
                elevation[x, y] = noise;
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

                var height = terrain.SampleHeight(new Vector3(x, 138, y));
                if (height < perlinMountainThreshold * newHighest / highest && height >= perlinLandThreshold * newHighest / highest) {
                    float roll = Random.Range(0f, 1f);
                    if (roll <= treePercentage) {
                        int whichTree = Random.Range(0, trees.Count());
                        var treeObj = Instantiate(trees[whichTree], new Vector3(x, 146.5f, y), trees[whichTree].transform.rotation);
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

    public void GenerateTerrainPerlin() {
        float randomX = Random.Range(0f, 1f) * perlinFeatureSize;
        float randomY = Random.Range(0f, 1f) * perlinFeatureSize;
        elevation = new float[mapSize, mapSize];
        for (int x = 0; x < mapSize; x++) {
            for (int y = 0; y < mapSize; y++) {
                elevation[x, y] = 0;
            }
        }
        for (int x = 0; x < mapSize; x++) {
            for (int y = 0; y < mapSize; y++) {
                var noise = Mathf.PerlinNoise(randomX + ((float)x / mapSize * perlinFeatureSize), randomY + ((float)y / mapSize * perlinFeatureSize)) * perlinScaleFactor;
                elevation[x, y] = noise;
            }
        }
        AddRivers(perlinLandThreshold);
        GenerateTerrainPrefabs(perlinMountainThreshold, perlinLandThreshold);
    }

    public void GenerateTerrainFractalPerlin() {
        float randomX = Random.Range(0f, 1f) * perlinFeatureSize;
        float randomY = Random.Range(0f, 1f) * perlinFeatureSize;
        elevation = new float[mapSize, mapSize];
        for (int x = 0; x < mapSize; x++) {
            for (int y = 0; y < mapSize; y++) {
                elevation[x, y] = 0;
            }
        }
        for (int x = 0; x < mapSize; x++) {
            for (int y = 0; y < mapSize; y++) {
                var noise = Mathf.PerlinNoise(randomX + ((float)x / mapSize * perlinFeatureSize), randomY + ((float)y / mapSize * perlinFeatureSize));
                for (int i=1; i<= fractalPerlinDepth; i++) {
                    noise += Mathf.PerlinNoise(randomX + ((float)x * Mathf.Pow(2, i) / mapSize / Mathf.Pow(2, i) * perlinFeatureSize), randomY + ((float)y * Mathf.Pow(2, i) / mapSize / Mathf.Pow(2, i) * perlinFeatureSize));
                }
                noise *= perlinScaleFactor;
                elevation[x, y] = noise;
            }
        }
        AddRivers(perlinLandThreshold);
        GenerateTerrainPrefabs(perlinMountainThreshold, perlinLandThreshold);
    }

    public void GenerateTerrainPrefabs(float mountainThreshold, float landThreshold) {
        for (int x = 0; x < mapSize; x++) {
            for (int y = 0; y < mapSize; y++) {
                //var go = Instantiate(prefab);
                //go.transform.position = new Vector3(x * 2f, 145f + ((elevation[x, y])), y * 2f);
                //go.transform.localScale = new Vector3(2, ((elevation[x, y])) * 2f, 2);
                if (elevation[x, y] > mountainThreshold) {
                    var go = Instantiate(prefab);
                    go.transform.position = new Vector3(x * 2f, 145f + ((elevation[x, y] - 14f) * 2f) + 0.5f, y * 2f);
                    go.transform.localScale = new Vector3(2, ((elevation[x, y] - 14f) + 0.5f) * 4f, 2);
                }
                else if (elevation[x, y] > landThreshold) {
                    var go = Instantiate(grassPrefab);
                    go.transform.position = new Vector3(x * 2f, 145, y * 2f);
                }
                else {
                    var go = Instantiate(waterPrefab);
                    go.transform.position = new Vector3(x * 2f, 145, y * 2f);
                }
            }
        }
        //SetStartGameCharacterPosition();
    }

    public void GenerateTerrainDiamondSquare() {
        //Random.InitState(0);
        elevation = new float[mapSize, mapSize];
        for (int x = 0; x < mapSize; x += featureSize) {
            for (int y = 0; y < mapSize; y += featureSize) {
                elevation[x, y] = Random.Range(0, 1);
            }
        }
        var stepSize = (float)(featureSize);
        var scale = 1f;
        while (stepSize >= 1) {
            PerformSquareStep((int)stepSize, scale);
            PerformDiamondStep((int)stepSize, scale);
            scale /= 2f;
            stepSize /= 2;
        }
        AddRivers(8);
        GenerateTerrainPrefabs(14, 8);
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

    private void GenerateDoubleTerrain() {
        //Random.InitState(0);
        SetStartGameCharacterPosition();
        elevation = new float[mapSize*2, mapSize];
        for (int x = 0; x < mapSize*2; x += featureSize) {
            for (int y = 0; y < mapSize; y += featureSize) {
                elevation[x, y] = Random.Range(0, 1);
            }
        }
        var stepSize = (float)(featureSize);
        var scale = 1f;
        while (stepSize >= 1) {
            PerformSquareStep((int)stepSize, scale);
            PerformDiamondStep((int)stepSize, scale);
            scale /= 2f;
            stepSize /= 2;
        }

        for (int y=0; y<mapSize; y+= 1) elevation[mapSize + 1, y] = elevation[mapSize, y];
        stepSize = (float)(featureSize);
        scale = 1f;
        while (stepSize >= 1) {
            PerformSquareStepSecondTime((int)stepSize, scale);
            PerformDiamondStepSecondTime((int)stepSize, scale);
            scale /= 2f;
            stepSize /= 2;
        }


        //for (int i = 0; i < 40; i++) {
        //    int x = Random.Range(0, mapSize);
        //    int y = Random.Range(0, mapSize);
        //    AddRiver(x, y);
        //}
        for (int x = 0; x < mapSize*2; x++) {
            for (int y = 0; y < mapSize; y++) {
                //var go = Instantiate(prefab);
                //go.transform.position = new Vector3(x * 2f, 145f + ((elevation[x, y])), y * 2f);
                //go.transform.localScale = new Vector3(2, ((elevation[x, y])) * 2f, 2);
                if (elevation[x, y] > 14) {
                    var go = Instantiate(prefab);
                    go.transform.position = new Vector3(x * 2f, 145f + ((elevation[x, y] - 14f) * 2f) + 0.5f, y * 2f);
                    go.transform.localScale = new Vector3(2, ((elevation[x, y] - 14f) + 0.5f) * 4f, 2);
                }
                else if (elevation[x, y] > 8) {
                    var go = Instantiate(grassPrefab);
                    go.transform.position = new Vector3(x * 2f, 145, y * 2f);
                }
                //else {
                //    var go = Instantiate(waterPrefab);
                //    go.transform.position = new Vector3(x * 2f, 145, y * 2f);
                //}
            }
        }
    }

    private void AddRiver(int startingX, int startingY, float landThreshold) {
        var coords = new Vector2(startingX, startingY);
        while (elevation[(int)coords.x, (int)coords.y] >= landThreshold) {
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
                    if (InBounds(x,y)) elevation[x, y] = 0;
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

    private void PerformDiamondStep(int stepSize, float scale) {
        for (int y = 0; y < mapSize; y += stepSize) {
            for (int x = 0; x < mapSize; x += stepSize) {
                if (x + stepSize / 2 < mapSize) DiamondStep(x + stepSize / 2, y, stepSize, scale);
                if (y + stepSize / 2 < mapSize) DiamondStep(x, y + stepSize / 2, stepSize, scale);
            }
        }
    }

    private void PerformSquareStep(int stepSize, float scale) {
        var halfStep = stepSize / 2;
        for (int y = halfStep; y < mapSize; y += stepSize) {
            for (int x = halfStep; x < mapSize; x += stepSize) {
                if (x >= mapSize || y >= mapSize) continue;
                SquareStep(x, y, stepSize, scale);
            }
        }
    }

    private void PerformDiamondStepSecondTime(int stepSize, float scale) {
        for (int y = 0; y < mapSize; y += stepSize) {
            for (int x = mapSize; x < mapSize*2; x += stepSize) {
                if (x + stepSize / 2 < mapSize*2) DiamondStep(x + stepSize / 2, y, stepSize, scale);
                if (y + stepSize / 2 < mapSize) DiamondStep(x, y + stepSize / 2, stepSize, scale);
            }
        }
    }

    private void PerformSquareStepSecondTime(int stepSize, float scale) {
        var halfStep = stepSize / 2;
        for (int y = halfStep; y < mapSize; y += stepSize) {
            for (int x = halfStep + mapSize; x < mapSize*2; x += stepSize) {
                if (x >= mapSize*2 || y >= mapSize) continue;
                SquareStepSecond(x, y, stepSize, scale);
            }
        }
    }

    private void DiamondStep(int x, int y, int stepSize, float scale) {
        float total = 0;
        float divisor = 0;
        if (x - stepSize / 2 >= 0) {
            total += elevation[x - stepSize / 2, y];
            divisor += 1;
        }
        if (x + stepSize / 2 < mapSize) {
            total += elevation[x + stepSize / 2, y];
            divisor += 1;
        }
        if (y - stepSize / 2 >= 0) {
            total += elevation[x, y - stepSize / 2];
            divisor += 1;
        }
        if (y + stepSize / 2 < mapSize) {
            total += elevation[x, y + stepSize / 2];
            divisor += 1;
        }
        elevation[x, y] = (total / divisor) + Random.Range(0, randomValueMax * scale);
    }

    private void SquareStep(int x, int y, int stepSize, float scale) {
        float total = 0;
        float divisor = 0;
        if (x - stepSize / 2 >= 0 && y - stepSize / 2 >= 0) {
            total += elevation[x - stepSize / 2, y - stepSize / 2];
            divisor += 1;
        }
        if (x-stepSize/2>=0 && y+stepSize/2<mapSize) {
            total += elevation[x - stepSize / 2, y + stepSize / 2];
            divisor += 1;
        }
        if (x+stepSize/2<mapSize && y-stepSize/2>=0) {
            total += elevation[x + stepSize / 2, y - stepSize / 2];
            divisor += 1;
        }
        if (x+stepSize/2<mapSize && y+stepSize/2<mapSize) {
            total += elevation[x + stepSize / 2, y + stepSize / 2];
            divisor += 1;
        }
        elevation[x, y] = (total / divisor) + Random.Range(0, randomValueMax * scale);
    }

    private void SquareStepSecond(int x, int y, int stepSize, float scale) {
        float total = 0;
        float divisor = 0;
        if (x - stepSize / 2 >= 0 && y - stepSize / 2 >= 0) {
            total += elevation[x - stepSize / 2, y - stepSize / 2];
            divisor += 1;
        }
        if (x - stepSize / 2 >= 0 && y + stepSize / 2 < mapSize) {
            total += elevation[x - stepSize / 2, y + stepSize / 2];
            divisor += 1;
        }
        if (x + stepSize / 2 < mapSize * 2 && y - stepSize / 2 >= 0) {
            total += elevation[x + stepSize / 2, y - stepSize / 2];
            divisor += 1;
        }
        if (x + stepSize / 2 < mapSize * 2 && y + stepSize / 2 < mapSize) {
            total += elevation[x + stepSize / 2, y + stepSize / 2];
            divisor += 1;
        }
        elevation[x, y] = (total / divisor) + Random.Range(0, randomValueMax * scale);
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
