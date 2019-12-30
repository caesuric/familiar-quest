using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlordMainCam : MonoBehaviour {

    public Vector3 positionOffset;

    // Update is called once per frame
    void Update() {
        Vector3 position = new Vector3(0, 0, 0);
        transform.position = position + positionOffset;
    }
}
