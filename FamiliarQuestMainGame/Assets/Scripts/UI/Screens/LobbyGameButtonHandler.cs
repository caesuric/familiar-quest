using UnityEngine;
using UnityEngine.UI;

public class LobbyGameButtonHandler : MonoBehaviour {
    private GameDescription game = null;
    private NewLobbyMenuManager manager = null;
    public Text text;

    public void OnClick() {
        manager.SelectGame(game);
    }

    public void Initialize(GameDescription game, NewLobbyMenuManager manager) {
        this.game = game;
        this.manager = manager;
        text.text = game.GameName;
    }
}
