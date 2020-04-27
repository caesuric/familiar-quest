using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class OverworldLandmarkGenerator {
    public static List<OverworldLandmark> landmarks = new List<OverworldLandmark>();

    public static void Load() {
        foreach (var landmark in landmarks) {
            if (landmark.type == "base") Object.Instantiate(OverworldGenerator.instance.labPrefab, new Vector3(landmark.position.x, 24, landmark.position.y), new Quaternion());
            else if (landmark.type == "dungeon") {
                var obj = Object.Instantiate(OverworldGenerator.instance.dungeonPrefab, new Vector3(landmark.position.x, 24, landmark.position.y), new Quaternion());
                obj.GetComponent<DungeonEntrance>().dungeonLevel = PlayerCharacter.localPlayer.GetComponent<ExperienceGainer>().level;
                obj.GetComponent<DungeonEntrance>().dungeonData = (OverworldDungeon)landmark;
            }
        }
    }

    public static IEnumerator Generate() {
        LoadingProgressBar.UpdateProgressText("Adding Dungeons");
        yield return OverworldGenerator.instance.StartCoroutine(AddLandmarks());
        yield return null;
    }

    private static IEnumerator AddLandmarks() {
        OverworldGenerator.instance.UpdateProgress(11, 0f);
        landmarks.Clear();
        FindPlayerStartingLocation();
        Object.Instantiate(OverworldGenerator.instance.labPrefab, new Vector3(landmarks[0].position.x, 24, landmarks[0].position.y), new Quaternion());
        yield return OverworldGenerator.instance.StartCoroutine(AddDungeons(10));
        yield return null;
    }

    private static void FindPlayerStartingLocation() {
        var startX = 0;
        var startY = 0;
        while (landmarks.Count == 0) {
            startX = Random.Range(0, 1024);
            startY = Random.Range(0, 1024);
            var height = OverworldGenerator.instance.terrain.SampleHeight(new Vector3(startX, 0, startY));
            if (height / OverworldGenerator.instance.newHighest < 1 - OverworldTerrainGenerator.perlinMountainProportion && height > 0) {
                landmarks.Add(new OverworldLandmark() {
                    position = new Vector2(startX, startY),
                    type = "base"
                });
            }
        }
        AddStartingPosition(startX, startY);
    }

    private static void AddStartingPosition(int startX, int startY) {
        while (landmarks.Count == 1) {
            var x = Random.Range(-1, 1);
            var y = Random.Range(-1, 1);
            var height = OverworldGenerator.instance.terrain.SampleHeight(new Vector3(startX + x, 0, startY + y));
            if (height / OverworldGenerator.instance.newHighest < 1 - OverworldTerrainGenerator.perlinMountainProportion && height > 0) {
                landmarks.Add(new OverworldLandmark() {
                    position = new Vector2(startX + x, startY + y),
                    type = "startingPosition"
                });
            }
        }
    }

    private static IEnumerator AddDungeons(int number) {
        for (int i = 0; i < number; i++) {
            var position = OverworldPathfinding.GetValidRandomPosition();
            var owd = new OverworldDungeon() {
                position = new Vector2(position.x, position.z)
            };
            landmarks.Add(owd);
            var obj = Object.Instantiate(OverworldGenerator.instance.dungeonPrefab, position, new Quaternion());
            obj.GetComponent<DungeonEntrance>().dungeonLevel = PlayerCharacter.localPlayer.GetComponent<ExperienceGainer>().level;
            obj.GetComponent<DungeonEntrance>().dungeonData = owd;
            OverworldGenerator.instance.UpdateProgress(11, (float)i / number);
            yield return null;
        }
    }
}
