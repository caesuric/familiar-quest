using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDrop : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Initialize(Ability ability) {
        GetComponent<AbilityScreenIcon>().Initialize(ability);
    }

    public void OnClick() {
        DropsArea.OpenAbilities();
        DropsArea.ClearDrops();
    }
}
