using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthBarUpdater : MonoBehaviour {

    public Character attr = null;
    private RectTransform rectTransform;
	// Use this for initialization
	void Start () {
        rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        if (attr == null) {
            var players = PlayerCharacter.players;
            foreach (var item in players) {
                if (item.GetComponent<PlayerCharacter>().isMe) {
                    attr = item.GetComponent<Character>();
                }
            }
        }
        else {
            rectTransform.localScale = new Vector3(attr.GetComponent<Health>().hp / attr.GetComponent<Health>().maxHP * 4, 0.25f, 1);
        }
	}
}
