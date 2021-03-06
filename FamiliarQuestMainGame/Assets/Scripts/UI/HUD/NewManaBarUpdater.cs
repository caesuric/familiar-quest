﻿using UnityEngine;
using UnityEngine.UI;

public class NewManaBarUpdater : MonoBehaviour {

    public Character character = null;
    private Image image;
    // Use this for initialization
    void Start() {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {
        if (character == null) {
            var players = PlayerCharacter.players;
            foreach (var item in players) if (item.isMe) character = item.GetComponent<Character>();
        }
        else image.fillAmount = character.GetComponent<Mana>().mp / character.GetComponent<Mana>().maxMP;
    }
}
