using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FusionAttributeItem : MonoBehaviour {

    public Text textBox;
    public AbilityAttribute attribute;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Initialize(AbilityAttribute attribute) {
        textBox.text = GetFriendlyName(attribute.type);
        this.attribute = attribute;
        transform.localScale = new Vector3(1, 1, 1);
    }

    private static string GetFriendlyName(string name) {
        return name;
    }
}
