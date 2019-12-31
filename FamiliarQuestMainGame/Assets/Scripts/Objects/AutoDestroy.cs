using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AutoDestroy : MonoBehaviour {

    public float duration;
    // Use this for initialization
    void Start() {
        //if (!NetworkServer.active) return;
        Destroy(gameObject, duration);
    }
}
