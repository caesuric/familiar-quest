using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectUser : MonoBehaviour {

    //public float checkFrequency = 0.1f;
    //public float checkTimer = 0f;
    public GameObject helpText = null;
    public bool isPlayer = false;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        //checkTimer += Time.deltaTime;
        //if (checkTimer>=checkFrequency) {
            //checkTimer = 0;
        if (isPlayer) CheckForObjects();
        //}
	}

    private void CheckForObjects() {
        var nearest = GetNearestUsableObject();
        if (nearest == null || nearest.hide) ClearHelpText();
        else SetHelpText(nearest);
    }

    private void ClearHelpText() {
        if (helpText == null) helpText = GameObject.FindGameObjectWithTag("ObjectHelpText");
        helpText.SetActive(false);
    }

    private void SetHelpText(UsableObject obj) {
        if (helpText==null) helpText = GameObject.FindGameObjectWithTag("ObjectHelpText");
        helpText.SetActive(true);
        helpText.GetComponentInChildren<TextMesh>().text = "SPACE: " + obj.helpText;
        helpText.transform.position = obj.transform.position + new Vector3(0, 5, 0);
    }

    public UsableObject GetNearestUsableObject() {
        var hits = Physics.OverlapSphere(transform.position, 2f);
        UsableObject nearestObj = null;
        float closestDistance = Mathf.Infinity;
        foreach (var hit in hits) {
            var usable = hit.gameObject.GetComponent<UsableObject>();
            if (usable == null) continue;
            var distance = Vector3.Distance(usable.transform.position, transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                nearestObj = usable;
            }
        }
        return nearestObj;
    }
}
