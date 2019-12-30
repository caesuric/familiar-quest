using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkInterpolatePlayerPosition : MonoBehaviour {

    public float syncRate;
	
	// Update is called once per frame
    void Update () {
        if (GetComponent<PlayerCharacter>().isMe) Destroy(this);
        Vector3 targetLocation = transform.position;
        Quaternion targetRotation = transform.rotation;
        //uint id = GetComponent<NetworkIdentity>().netId.Value;
        var ic = GetComponent<InputController>();
        targetLocation = new Vector3(ic.posX, 0, ic.posY);
        targetRotation = Quaternion.Euler(0, ic.rotation, 0);
        transform.position = Vector3.Lerp(transform.position, targetLocation, Time.deltaTime / syncRate);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime / syncRate);
	}
}

