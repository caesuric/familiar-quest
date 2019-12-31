using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;

public class LobbyServerItem : MonoBehaviour {

    private LobbyManager lobbyManager;
    private NetworkID networkID;
    public Text nameText;
    public Text playersText;

    public void Initialize(string name, int numPlayers, NetworkID networkID, LobbyManager lobbyManager) {
        this.lobbyManager = lobbyManager;
        this.networkID = networkID;
        nameText.text = name;
        playersText.text = "Players: " + numPlayers.ToString();
    }
    
    public void Click() {
        lobbyManager.searchScreen.SetActive(false);
        lobbyManager.ActivateHud(false);
    }
}
