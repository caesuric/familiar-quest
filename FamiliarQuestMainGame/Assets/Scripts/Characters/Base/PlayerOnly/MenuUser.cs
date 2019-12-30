using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class MenuUser : MonoBehaviour {

    //public SyncListString spiritDescriptions = new SyncListString();
    public List<string> spiritDescriptions = new List<string>();
    //[SyncVar]
    public string spiritAffinityText = "";
    //[SyncVar]
    public string weaponName;
    //[SyncVar]
    public string armorName;
    //[SyncVar]
    public string accessoryName;
    //[SyncVar]
    public string weaponDescription;
    //[SyncVar]
    public string armorDescription;
    //[SyncVar]
    public string accessoryDescription;

    // Use this for initialization
    void Start() {
        var dependencies = new List<string>() { "PlayerCharacter", "Character" };
        Dependencies.Check(gameObject, dependencies);
    }

    // Update is called once per frame
    void Update() {

    }
}
