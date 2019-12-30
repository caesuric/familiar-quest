using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

//[NetworkSettings(channel =3)]
public class AutoSaver : DependencyUser {

    //[SyncVar]
    public bool currentlySaving = false;
    private float autoSaveTimer = 0;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //if (NetworkServer.active) {
            autoSaveTimer += Time.deltaTime;
            if (autoSaveTimer >= 30) {
                autoSaveTimer = 0;
                SaveCharacter();
            }
        //}
    }

    public void SaveCharacter() {
        if (currentlySaving) return;
        currentlySaving = true;
        var saveObject = SavedCharacter.ConvertFrom(gameObject);
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, saveObject);
        try {
            RpcSaveCharacter(ms.ToArray());
        }
        catch (Exception e) {
            GameLog.AddText("<color=red>Autosave failed!</color>");
        }
    }

    //[Command]
    public void CmdSave() {
        SaveCharacter();
    }

    //[Command]
    public void CmdFinishedSaving() {
        currentlySaving = false;
    }

    //[ClientRpc]
    public void RpcSaveCharacter(byte[] data) {
        //if (isLocalPlayer) {
            if (!Directory.Exists(Application.persistentDataPath + "/characters")) Directory.CreateDirectory(Application.persistentDataPath + "/characters");
            File.WriteAllBytes(Application.persistentDataPath + "/characters/" + GetComponent<PlayerSyncer>().characterName + ".character", data);
            GameLog.AddText("<color=green>Autosaved successfully.</color>");
            CmdFinishedSaving();
        //}
    }
}
