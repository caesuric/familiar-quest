using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomFungus : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.fire, -50));
        GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.slashing, -50));
        GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.acid, 50));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
