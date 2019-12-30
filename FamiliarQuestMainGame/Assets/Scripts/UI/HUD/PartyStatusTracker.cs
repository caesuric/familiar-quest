using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PartyStatusTracker : MonoBehaviour {

    //public SyncListFloat hp = new SyncListFloat();
    //public SyncListFloat posX = new SyncListFloat();
    //public SyncListFloat posY = new SyncListFloat();
    //public SyncListUInt id = new SyncListUInt();
    public List<float> hp = new List<float>();
    public List<float> posX = new List<float>();
    public List<float> posY = new List<float>();
    public List<uint> id = new List<uint>();
    public List<bool> isTargeted = new List<bool>();
    public PlayerCharacter localPlayer = null;
    //public SyncListString names = new SyncListString();
    //public SyncListInt furTypes = new SyncListInt();
    public List<string> names = new List<string>();
    public List<int> furTypes = new List<int>();

    // Update is called once per frame
    void Update () {
        //if (localPlayer == null) Initialize();
        //else if (id.Count != PlayerCharacter.players.Count || isTargeted.Count != PlayerCharacter.players.Count) RecreateData();
        //else UpdateData();
	}

    private void Initialize()
    {
        //var players = PlayerCharacter.players;
        //foreach (var item in players) if (item.isMe) localPlayer = item.GetComponent<PlayerCharacter>();
    }

    private void RecreateData()
    {
        //if (NetworkServer.active) ClearAll();
        ClearAll();
        isTargeted.Clear();
        foreach (var player in PlayerCharacter.players)
        {
            //if (NetworkServer.active) AddAll(player);
            AddAll(player);
            //isTargeted.Add(player.netId.Value == localPlayer.target);
        }
    }

    private void ClearAll()
    {
        hp.Clear();
        id.Clear();
        posX.Clear();
        posY.Clear();
        names.Clear();
        furTypes.Clear();
    }

    private void AddAll(PlayerCharacter player)
    {
        hp.Add(player.GetComponent<Health>().hp / player.GetComponent<Health>().maxHP);
        posX.Add(player.transform.position.x);
        posY.Add(player.transform.position.z);
        //id.Add(player.netId.Value);
        names.Add(player.GetComponent<PlayerSyncer>().characterName);
        furTypes.Add(player.GetComponent<PlayerSyncer>().furType);
        //player.RpcSetColor(player.GetComponent<PlayerSyncer>().furType);
    }

    private void UpdateData()
    {
        for (int i = 0; i < PlayerCharacter.players.Count; i++)
        {
            //if (NetworkServer.active) UpdateAll(i);
            UpdateAll(i);
            //isTargeted[i] = (PlayerCharacter.players[i].netId.Value == localPlayer.target);
        }
    }

    private void UpdateAll(int i)
    {
        hp[i] = PlayerCharacter.players[i].GetComponent<Health>().hp / PlayerCharacter.players[i].GetComponent<Health>().maxHP;
        //id[i] = PlayerCharacter.players[i].netId.Value;
        posX[i] = PlayerCharacter.players[i].transform.position.x;
        posY[i] = PlayerCharacter.players[i].transform.position.z;
        names[i] = PlayerCharacter.players[i].GetComponent<PlayerSyncer>().characterName;
        furTypes[i] = PlayerCharacter.players[i].GetComponent<PlayerSyncer>().furType;
    }
}
