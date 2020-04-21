using UnityEngine;
using System.Linq;
using System.Collections;

public static class OverworldTerrainGenerator {
    private static float landHeight = 0f;
    public static readonly float perlinMountainProportion = 0.21f;
    public static readonly float perlinWaterProportion = 0.6f;
    private static readonly float landHeightDivisor = 10f;
    private static readonly float mountainHeightDivisor = 15f;

    public static IEnumerator Generate() {
        LoadingProgressBar.UpdateProgressText("Generating Terrain Object");
        yield return OverworldGenerator.instance.StartCoroutine(GenerateTerrainObject(OverworldGenerator.loadedPreviouslyMadeWorld));
        yield return null;
        LoadingProgressBar.UpdateProgressText("Generating Alpha Map");
        yield return OverworldGenerator.instance.StartCoroutine(GenerateAlphaMap());
        yield return null;
        LoadingProgressBar.UpdateProgressText("Generating Terrain Details");
        yield return OverworldGenerator.instance.StartCoroutine(GenerateDetails());
        yield return null;
    }

    private static IEnumerator GenerateTerrainObject(bool loadedPreviouslyMadeWorld) {
        landHeight = (1 - perlinMountainProportion) / 2f / landHeightDivisor;
        if (!loadedPreviouslyMadeWorld) {
            for (int x = 0; x < OverworldGenerator.instance.mapSize; x++) {
                for (int y = 0; y < OverworldGenerator.instance.mapSize; y++) {
                    if (OverworldGenerator.instance.elevation[x, y] / OverworldGenerator.instance.highest <= perlinWaterProportion) OverworldGenerator.instance.elevation[x, y] = 0f;
                    else if (OverworldGenerator.instance.elevation[x, y] / OverworldGenerator.instance.highest < 1 - perlinMountainProportion) OverworldGenerator.instance.elevation[x, y] = landHeight;
                    else OverworldGenerator.instance.elevation[x, y] = OverworldGenerator.instance.elevation[x, y] / OverworldGenerator.instance.highest / mountainHeightDivisor;
                }
                if (x % 100 == 0) {
                    OverworldGenerator.instance.UpdateProgress(3, (float)x / OverworldGenerator.instance.mapSize);
                    yield return null;

                }
            }
        }
        OverworldGenerator.instance.terrain.terrainData.heightmapResolution = OverworldGenerator.instance.mapSize;
        OverworldGenerator.instance.terrain.terrainData.SetHeights(0, 0, OverworldGenerator.instance.elevation);
    }

    private static IEnumerator GenerateAlphaMap() {
        TerrainData terrainData = OverworldGenerator.instance.terrain.terrainData;
        terrainData.alphamapResolution = OverworldGenerator.instance.mapSize;
        float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];
        yield return OverworldGenerator.instance.StartCoroutine(SetNewHighest(terrainData));
        for (int y = 0; y < terrainData.alphamapHeight; y++) {
            for (int x = 0; x < terrainData.alphamapWidth; x++) GenerateSplatMapPoint(x, y, terrainData, splatmapData);
            if (y % 200 == 0) {
                OverworldGenerator.instance.UpdateProgress(4, 0.5f + ((float)y / terrainData.heightmapResolution / 2f));
                yield return null;
            }
        }
        terrainData.SetAlphamaps(0, 0, splatmapData);
    }

    private static IEnumerator SetNewHighest(TerrainData terrainData) {
        OverworldGenerator.instance.newHighest = 0;
        for (int y = 0; y < terrainData.heightmapResolution; y++) {
            for (int x = 0; x < terrainData.heightmapResolution; x++) {
                var height = OverworldGenerator.instance.terrain.SampleHeight(new Vector3(x, 0, y));
                if (height > OverworldGenerator.instance.newHighest) OverworldGenerator.instance.newHighest = height;
            }
            if (y % 200 == 0) {
                OverworldGenerator.instance.UpdateProgress(4, ((float)y / terrainData.heightmapResolution / 2f));
                yield return null;
            }
        }
    }

    private static void GenerateSplatMapPoint(int x, int y, TerrainData terrainData, float[,,] splatmapData) {
        float y_01 = y / (float)terrainData.alphamapHeight;
        float x_01 = x / (float)terrainData.alphamapWidth;
        float height = terrainData.GetHeight(Mathf.RoundToInt(y_01 * terrainData.heightmapResolution), Mathf.RoundToInt(x_01 * terrainData.heightmapResolution));
        //Vector3 normal = terrainData.GetInterpolatedNormal(y_01, x_01);
        //float steepness = terrainData.GetSteepness(y_01, x_01);

        float[] splatWeights = new float[terrainData.alphamapLayers];
        if (height / OverworldGenerator.instance.newHighest > (1 - perlinMountainProportion)) splatWeights[1] = 1f;
        else if (height == 0) splatWeights[2] = 1f;
        else splatWeights[0] = 1f;

        float z = splatWeights.Sum();
        for (int i = 0; i < terrainData.alphamapLayers; i++) {
            splatWeights[i] /= z;
            splatmapData[x, y, i] = splatWeights[i];
        }
    }

    private static IEnumerator GenerateDetails() {
        for (int z = 0; z < 14; z++) {
            int[,] details = new int[OverworldGenerator.instance.terrain.terrainData.detailResolution, OverworldGenerator.instance.terrain.terrainData.detailResolution];
            for (int x = 0; x < OverworldGenerator.instance.terrain.terrainData.detailResolution; x++) {
                for (int y = 0; y < OverworldGenerator.instance.terrain.terrainData.detailResolution; y++) {
                    if (OverworldGenerator.instance.elevation[x, y] == landHeight) {
                        var random = Random.Range(0, 30);
                        if (random == 0) details[x, y] = Random.Range(0, 15);
                        else details[x, y] = 0;
                    }
                    else details[x, y] = 0;
                }
            }
            OverworldGenerator.instance.terrain.terrainData.SetDetailLayer(0, 0, z, details);
            if (z % 2 == 0) {
                OverworldGenerator.instance.UpdateProgress(5, z / 14f);
                yield return null;
            }
        }
    }




}
