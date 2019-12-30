using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoginScreen : MonoBehaviour {

    public InputField nameField;
    public InputField passwordField;
    //[SyncVar]
    public bool successful = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Login()
    {
        //CmdLogin(nameField.text, passwordField.text);
    }

    public void Cancel()
    {
        Application.Quit();
    }
}
