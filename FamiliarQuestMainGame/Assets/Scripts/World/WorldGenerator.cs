using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class WorldGenerator {
    public static List<string> worlds = new List<string>();

    public static void FetchWorlds() {
        worlds = new List<string>();
        if (!Directory.Exists(Application.persistentDataPath + "/worlds")) Directory.CreateDirectory(Application.persistentDataPath + "/worlds");
        if (File.Exists(Application.persistentDataPath + "/worlds/.world")) File.Delete(Application.persistentDataPath + "/worlds/.world");
        var files = Directory.GetFiles(Application.persistentDataPath + "/worlds");
        foreach (var filename in files) {
            var fi = new FileInfo(filename);
            worlds.Add(fi.Name.Replace(".world", ""));
        }
    }

    public static void CreateWorld(string name) {
        if (!Directory.Exists(Application.persistentDataPath + "/worlds")) Directory.CreateDirectory(Application.persistentDataPath + "/worlds");
        var saveObject = SavedWorld.BrandNewWorld(name);
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, saveObject);
        File.WriteAllBytes(Application.persistentDataPath + "/worlds/" + name + ".world", ms.ToArray());
    }
}
