using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiritItem : MonoBehaviour {

    public Text nameText;
    public Text descriptionText;
    public new string name = null;
    public string description = null;
    public Spirit spirit = null;
    public SpiritScreen spiritScreen;
    private bool initialized = false;
    public int number;

    // Update is called once per frame
    void Update() {
        if (!initialized && name != null) {
            initialized = true;
            nameText.text = name;
            descriptionText.text = description;
        }
    }

    public void Equip(int slotNumber) {
        spiritScreen.EquipSpirit(number, slotNumber);
    }
}

