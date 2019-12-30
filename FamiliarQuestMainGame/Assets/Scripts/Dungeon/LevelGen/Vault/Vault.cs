using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Vault : Dungeon {

    public abstract void AddMonsters(int targetLevel);
    public List<MonsterData> monsters = new List<MonsterData>();
}
