using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public float hitboxWidth; //STUB: TODO
    public float hitboxDepth; //STUB: TODO
    public MeleeWeapon() {
        attackPower = 0.8437f;
        name = "Basic Sword";
        icon = "Weapon_01";
        description = "Attack power: {{AttackPower}}";
        displayType = "Weapon - Sword";
    }
}
