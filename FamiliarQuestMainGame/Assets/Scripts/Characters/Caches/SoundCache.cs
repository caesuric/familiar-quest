using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCache : MonoBehaviour {

    public List<AudioClip> items = new List<AudioClip>();
    public Dictionary<string, AudioClip> itemsByName = new Dictionary<string, AudioClip>();
    public void Start() {
        var loaded = Resources.LoadAll<AudioClip>("SFX");
        foreach (var item in loaded) itemsByName[item.name] = item;
    }
}
