using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldItem : MonoBehaviour {
    public Text text;
    private string worldName;
    private LobbyManager lobbyManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Initialize(string name, LobbyManager lobbyManager) {
        text.text = name;
        worldName = name;
        this.lobbyManager = lobbyManager;
    }

    public void OnClick() {
        lobbyManager.EnterWorld(worldName);
    }
}
