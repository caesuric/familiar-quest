using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraFollow : MonoBehaviour {

    public GameObject player = null;
    public Vector3 positionOffset;
    public static GameObject instance = null;

    private void Start() {
        if (instance == null) {
            DontDestroyOnLoad(gameObject);
            instance = gameObject;
        }
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update () {
        if (player == null) {
            var players = PlayerCharacter.players;
            foreach (var item in players) if (item.isMe) player = item.gameObject;
        }
        if (player != null) transform.position = player.transform.position + positionOffset;
	}
}
