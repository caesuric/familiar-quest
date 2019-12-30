using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Troll : MonoBehaviour {

    private Character character = null;
	// Use this for initialization
	void Start () {
        GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.fire, -50));
        GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.slashing, -50));
    }
	
	// Update is called once per frame
	void Update () {
        //if (!NetworkServer.active) return;
        if (character==null) character = GetComponent<Character>();
        if (character!=null) character.GetComponent<Health>().hp = Mathf.Min(character.GetComponent<Health>().maxHP, character.GetComponent<Health>().hp + (Time.deltaTime * character.GetComponent<Health>().maxHP / 5));
	}
}
