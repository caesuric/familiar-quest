using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterNameUpdater : MonoBehaviour {

    private PlayerCharacter pc = null;
    private Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (PlayerCharacter.players.Count == 0) return;
        if (pc == null) pc = PlayerCharacter.players[0];
        text.text = pc.GetComponent<PlayerSyncer>().characterName.ToUpper();
    }
}
