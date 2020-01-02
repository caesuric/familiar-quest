using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class WorldAutoSaver : MonoBehaviour {
    private float autoSaveTimer = 0;
    public string worldName = "";

    // Update is called once per frame
    void Update() {
        autoSaveTimer += Time.deltaTime;
        if (autoSaveTimer >= 30) {
            autoSaveTimer = 0;
            SaveWorld();
        }
    }

    public void SaveWorld() {
        GameLog.AddText("<color=green>Autosaved world state.</color>");
        var saveObject = SavedWorld.ConvertFrom(gameObject);
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, saveObject);
        if (!Directory.Exists(Application.persistentDataPath + "/worlds")) Directory.CreateDirectory(Application.persistentDataPath + "/worlds");
        File.WriteAllBytes(Application.persistentDataPath + "/worlds/" + worldName + ".world", ms.ToArray());
    }
}
