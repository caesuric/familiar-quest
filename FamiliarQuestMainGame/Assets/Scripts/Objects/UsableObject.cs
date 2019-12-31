using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UsableObject : MonoBehaviour {

    public UnityEvent onUse;
    public GameObject user;
    public string helpText = "";
    public bool hide = false;

    public void Use(GameObject user) {
        this.user = user;
        onUse.Invoke();
    }
}
