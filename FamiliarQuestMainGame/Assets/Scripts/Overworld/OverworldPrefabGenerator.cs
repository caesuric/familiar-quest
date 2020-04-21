using UnityEngine;
using System.Collections;
using System.Linq;

public static class OverworldPrefabGenerator {
    private static readonly float treePercentage = 0.001f;
    private static readonly float bushPercentage = 0.001f;
    private static readonly float grassPercentage = 0.01f;
    private static readonly float flowersPercentage = 0.004f;
    private static readonly float leavesPercentage = 0.004f;
    private static readonly float rockPercentage = 0.004f;

    public static IEnumerator Generate() {
        LoadingProgressBar.UpdateProgressText("Generating Trees");
        yield return OverworldGenerator.instance.StartCoroutine(GenerateObjects(treePercentage, OverworldGenerator.instance.trees, 6));
        yield return null;
        LoadingProgressBar.UpdateProgressText("Generating Bushes");
        yield return OverworldGenerator.instance.StartCoroutine(GenerateObjects(bushPercentage, OverworldGenerator.instance.bushes, 7));
        yield return null;
        LoadingProgressBar.UpdateProgressText("Generating Leaves");
        yield return OverworldGenerator.instance.StartCoroutine(GenerateObjects(leavesPercentage, OverworldGenerator.instance.leaves, 8));
        yield return null;
        LoadingProgressBar.UpdateProgressText("Generating Rocks");
        yield return OverworldGenerator.instance.StartCoroutine(GenerateObjects(rockPercentage, OverworldGenerator.instance.rocks, 9));
        yield return null;
        LoadingProgressBar.UpdateProgressText("Generating Flowers");
        yield return OverworldGenerator.instance.StartCoroutine(GenerateObjects(flowersPercentage, OverworldGenerator.instance.flowers, 10));
        yield return null;
    }

    private static IEnumerator GenerateObjects(float objectPercentage, GameObject[] objects, int loadPhase) {
        for (int x = 0; x < OverworldGenerator.instance.mapSize; x++) {
            for (int y = 0; y < OverworldGenerator.instance.mapSize; y++) {
                var height = OverworldGenerator.instance.terrain.SampleHeight(new Vector3(x, 0, y));
                if (height / OverworldGenerator.instance.newHighest < 1 - OverworldTerrainGenerator.perlinMountainProportion && height > 0) {
                    if (objectPercentage < 1) {
                        float roll = Random.Range(0f, 1f);
                        if (roll <= objectPercentage) {
                            int whichObject = Random.Range(0, objects.Count());
                            var obj = Object.Instantiate(objects[whichObject], new Vector3(x + Random.Range(-0.25f, 0.25f), height, y + Random.Range(-0.25f, 0.25f)), objects[whichObject].transform.rotation);
                            obj.transform.Rotate(0, Random.Range(0, 360), 0);
                            var scaleFactor = Random.Range(0.35f, 0.65f);
                            obj.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                        }
                    }
                    else {
                        int roll = (int)Random.Range(0, objectPercentage * 2);
                        for (int i = 0; i < roll; i++) {
                            int whichObject = Random.Range(0, objects.Count());
                            var obj = Object.Instantiate(objects[whichObject], new Vector3(x + Random.Range(-0.25f, 0.25f), height, y + Random.Range(-0.25f, 0.25f)), objects[whichObject].transform.rotation);
                            obj.transform.Rotate(0, Random.Range(0, 360), 0);
                            var scaleFactor = Random.Range(0.35f, 0.65f);
                            obj.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                        }
                    }
                }
            }
            if (x % 100 == 0) {
                OverworldGenerator.instance.UpdateProgress(loadPhase, (float)x / OverworldGenerator.instance.mapSize);
                yield return null;
            }
        }
    }

}
