using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : MonoBehaviour {
    public bool active = false;
    public GameObject healthbar;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActivateHealthbar() {
        healthbar.SetActive(true);
        active = true;
    }

    public void DeactivateHealthbar() {
        healthbar.SetActive(false);
        active = false;
    }
}
