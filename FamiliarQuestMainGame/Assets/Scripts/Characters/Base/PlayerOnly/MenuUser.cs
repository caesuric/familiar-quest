using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(PlayerCharacter))]
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
}
