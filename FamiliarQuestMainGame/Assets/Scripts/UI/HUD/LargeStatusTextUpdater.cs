using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LargeStatusTextUpdater : MonoBehaviour {

    private Text text;
    public Character character;
    private bool saved = false;
    public GameObject quitMenu;
    public GameObject canvas;
    public GameObject objectHelpText;

    // Use this for initialization
    void Start() {
        text = GetComponent<Text>();
        text.text = "";
        if (quitMenu != null) quitMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (character == null) {
            var players = PlayerCharacter.players;
            foreach (var item in players) {
                if (item.isMe) {
                    character = item.GetComponent<Character>();
                }
            }
        }
        else {
            if (character.GetComponent<Health>().hp <= 0) {
                text.text = "<b>YOU ARE DEAD</b>";
                if (!saved) {
                    saved = true;
                    character.GetComponent<AutoSaver>().CmdSave();
                }
                //StartCoroutine(ResetGame());
                quitMenu.SetActive(true);
            }
            else if (character.GetComponent<StatusEffectHost>().CheckForEffect("paralysis")) text.text = "PARALYZED";
            else text.text = "";
        }
    }

    public void ResetGame() {
        foreach (var player in PlayerCharacter.players) Destroy(player.gameObject);
        PlayerCharacter.players.Clear();
        CharacterSelectScreen.loadedCharacter = null;
        CharacterSelectScreen.selectedCharacterName = "";
        CharacterSelectScreen.characterByteArray = null;
        Destroy(canvas);
        Destroy(objectHelpText);
        MusicController.instance.PlayMusic(MusicController.instance.menuMusic);
        SceneManager.LoadScene("New Character Selection");
        //NetworkManager.Shutdown();
    }

    //private IEnumerator ResetGame() {
    //    yield return new WaitForSeconds(5);
    //    InitializeLevel.ResetGame();
    //}
}