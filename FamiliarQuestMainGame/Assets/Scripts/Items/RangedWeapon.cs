using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public int range;
    public int projectileModel = 0;
    public bool usesInt = false;

    public RangedWeapon() {
        icon = "bow_2";
        attackPower = 0.8437f;
        name = "Basic Bow";
        description = "Attack power: {{AttackPower}}";
        displayType = "Weapon - Bow";
    }

    public static RangedWeapon Wand () {
        var rw = new  RangedWeapon();
        rw.icon = "Weapon_16";
        rw.projectileModel = 1;
        rw.usesInt = true;
        rw.name = "Basic Wand";
        rw.description = "Attack power: {{AttackPower}}";
        rw.displayType = "Weapon - Wand";
        return rw;
    }
}
