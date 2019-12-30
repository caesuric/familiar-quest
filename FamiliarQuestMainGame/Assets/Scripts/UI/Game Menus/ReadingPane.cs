using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadingPane : MonoBehaviour {

    public Text titleObj;
    public Text textObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		

	}

    public void SetText(string title, string text) {
        titleObj.text = title;
        textObj.text = text;
    }

    public void Close() {
        gameObject.SetActive(false);
    }
}

