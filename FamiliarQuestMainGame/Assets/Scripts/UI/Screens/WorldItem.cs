using UnityEngine;
using UnityEngine.UI;

public class WorldItem : MonoBehaviour {
    public Text text;
    private string worldName;
    private LobbyManager lobbyManager;

    public void Initialize(string name, LobbyManager lobbyManager) {
        text.text = name;
        worldName = name;
        this.lobbyManager = lobbyManager;
    }

    public void OnClick() {
        lobbyManager.EnterWorld(worldName);
    }
}
