using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Equipment {
    public float hp;
    public float mp;
    public Armor()
    {
        hp = 29;
        mp = 10;
        name = "Generic Armor";
        description = "{{HpAndMp}}";
        icon = "archi_arm_01_t";
        displayType = "Armor";
    }
}
