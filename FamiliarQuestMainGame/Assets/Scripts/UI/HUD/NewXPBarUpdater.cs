using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NewXPBarUpdater : MonoBehaviour {

    public Character attr = null;
    private MOBAEnergyBar bar;
    // Use this for initialization
    void Start() {
        bar = GetComponent<MOBAEnergyBar>();
    }

    // Update is called once per frame
    void Update() {
        if (attr == null) {
            var players = PlayerCharacter.players;
            foreach (var item in players) if (item.isMe) attr = item.GetComponent<Character>();
        }
        else bar.Value = attr.GetComponent<ExperienceGainer>().xpPercentage * 100;
    }
}