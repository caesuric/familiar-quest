using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour {

    public float angleX;
    public float angleY;
    public float angleZ;
    public Character character;
    public PlayerCharacter pc;
    private new Light light;
    // Use this for initialization
    void Start () {
        light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(angleX, angleY, angleZ);
        if (character.GetComponent<StatusEffectHost>().GetComponent<PlayerSyncer>().blinded)
        {
            light.range = 5;
            light.intensity = 16;
            //light.spotAngle = 45;
        }
        else if (pc.litArea)
        {
            light.range = 5;
            if (!character.GetComponent<InputController>().stealthy) light.intensity = 2;
        }
        else {
            light.range = 100;
            if (!character.GetComponent<InputController>().stealthy) light.intensity = 2;
        }
	}
}
