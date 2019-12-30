using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectCache : MonoBehaviour {

    public List<GameObject> backingItems = new List<GameObject>();
    public Dictionary<string, GameObject> items = new Dictionary<string, GameObject>();
	// Use this for initialization
	void Start () {
        var data = TextReader.ReadSets("CharacterEffectData");
        foreach (var item in data) items.Add(item[0], backingItems[int.Parse(item[1])]);
	}
}
