using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PlayerSyncer : DependencyUser {
    //[SyncVar]
    public float mp;
    //[SyncVar]
    public float maxMP;
    //[SyncVar]
    public int rangedObjNumber;
    //[SyncVar]
    public string characterName;
    //[SyncVar]
    public int furType = -1;
    //[SyncVar]
    public bool furTypeSet = false;
    //[SyncVar]
    public bool stealthy = false;
    //[SyncVar]
    public float speedMultiplier = 1;
    //[SyncVar]
    public bool blinded = false;

    private bool ready = false;

    // Use this for initialization
    void Start() {
        dependencies = new List<string>() { "PlayerCharacter", "Character" };
        Dependencies.Check(this);
    }

    // Update is called once per frame
    void Update() {
        if (!ready && GetComponent<CacheGrabber>().kittenFurCache.Count>0 && furTypeSet) {
            if (furType == -1) return;
            var renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var renderer in renderers) {
                if (renderer.gameObject.CompareTag("KittenBody")) {
                    renderer.material = GetComponent<CacheGrabber>().kittenFurCache[furType];
                    break;
                }
            }
            ready = true;
        }
        //if (NetworkServer.active) {
            if (GetComponent<StatusEffectHost>().CheckForEffect("blind") && !blinded) blinded = true;
            else if (!GetComponent<StatusEffectHost>().CheckForEffect("blind") && blinded) blinded = false;
            if (GetComponent<StatusEffectHost>().CheckForEffect("stealth")) stealthy = true;
            else stealthy = false;
            if (GetComponent<StatusEffectHost>().CheckForEffect("speed+")) speedMultiplier = 1 + GetComponent<StatusEffectHost>().GetEffect("speed+").degree;
            else if (GetComponent<StatusEffectHost>().CheckForEffect("speed-")) speedMultiplier = 1 - GetComponent<StatusEffectHost>().GetEffect("speed-").degree;
            else speedMultiplier = 1;
        //}
    }
}