using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MonsterInitializer
{
    public static bool Initialize(GameObject gameObject, params ActiveAbility[] abilities)
    {
        var monster = gameObject.GetComponent<Monster>();
        if (monster != null)
        {
            foreach (var ability in abilities) monster.GetComponent<MonsterBaseAbilities>().baseAbilities.Add(ability);
            return true;
        }
        else return false;
    }
}
