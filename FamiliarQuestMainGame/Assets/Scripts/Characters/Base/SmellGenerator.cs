using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

class SmellGenerator : MonoBehaviour
{
    public GameObject smell;

    //[Command]
    public void CmdMakeSmell(Vector3 position, float intensity)
    {
        var obj = Instantiate(smell, position, transform.rotation);
        var smellObj = obj.GetComponent<Smell>();
        smellObj.intensity = intensity;
        //NetworkServer.Spawn(obj);
    }
}

