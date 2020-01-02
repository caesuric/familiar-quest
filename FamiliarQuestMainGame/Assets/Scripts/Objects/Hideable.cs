using UnityEngine;
using System.Collections.Generic;

public class Hideable : MonoBehaviour {
    public static List<Hideable> items = new List<Hideable>();
    //[SyncVar]
    public bool prune = false;
    //[SyncVar]
    public bool isSecret = false;

    private void OnDestroy() {
        if (items.Contains(this)) items.Remove(this);
    }
}
