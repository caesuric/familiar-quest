using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NewHealthBarUpdater : MonoBehaviour {

    public Character attr = null;
    private Image image;
    // Use this for initialization
    void Start() {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {
        if (attr == null) {
            var players = PlayerCharacter.players;
            foreach (var item in players) if (item.isMe) attr = item.GetComponent<Character>();
        }
        else image.fillAmount = attr.GetComponent<Health>().hp / attr.GetComponent<Health>().maxHP;
    }
}