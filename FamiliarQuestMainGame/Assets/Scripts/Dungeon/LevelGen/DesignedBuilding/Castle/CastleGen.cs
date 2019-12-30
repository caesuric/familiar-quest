using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CastleGen : MonoBehaviour {

    private static Dictionary<string, Dictionary<string, Dictionary<string, GameObject>>> prefabs = LevelGenPrefabs.prefabs;
    private static Dictionary<string, Dictionary<string, Dictionary<string, float>>> prefabProbability = LevelGenPrefabs.prefabProbability;

    public static void InstantiateLayout(DesignedBuilding layout, int floor, int seed) {
        Random.InitState(seed);
        ActiveCastleGen.InstantiateLayout(layout, floor);
    }

    public static void InstantiateMonsters(int floor, DesignedBuilding layout, SocialStructure socialStructure, LevelGen levelGen) {
        ActiveCastleGen.InstantiateMonsters(floor, layout, socialStructure, levelGen);
    }

    public static void AddDressing(Room room, DesignedBuilding layout) {
        ActiveCastleGen.AddDressing(room, layout);
    }
}
