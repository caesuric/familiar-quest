using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OverlordPlayerCam : MonoBehaviour {

    public Vector3 positionOffset;

    // Update is called once per frame
    void Update() {
        Vector3 position = new Vector3(0, 0, 0);
        foreach (var player in PlayerCharacter.players) position += player.transform.position;
        position /= PlayerCharacter.players.Count;
        transform.position = position + positionOffset;
    }
}
