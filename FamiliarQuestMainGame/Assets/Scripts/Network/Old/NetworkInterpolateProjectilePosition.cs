using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//[NetworkSettings(sendInterval=0.01f)]
public class NetworkInterpolateProjectilePosition : MonoBehaviour {

    //[SyncVar]
    public float syncRate; //how many seconds between syncs
    //[SyncVar]
    private Vector3 targetLocation;
    //[SyncVar]
    private Quaternion targetRotation;
    private float timer = 0;
    private bool initialized = false;
	// Use this for initialization
	void Start () {
        targetLocation = transform.position;
        targetRotation = transform.rotation;
	}
	
	// Update is called once per frame
    void Update () {
        //if (!NetworkServer.active && !initialized) Initialize();
        Initialize();
        //if (!NetworkServer.active)
        //{
            transform.position = Vector3.Lerp(transform.position, targetLocation, Time.deltaTime / syncRate);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime / syncRate);
        //}
        timer += Time.deltaTime;
        if (timer >= syncRate)
        {
            timer -= syncRate;
            //if (NetworkServer.active)
            //{
                targetLocation = transform.position;
                targetRotation = transform.rotation;
            //}
        }
	}

    private void Initialize()
    {
        transform.position = targetLocation;
        transform.rotation = targetRotation;
        initialized = true;
    }
}

