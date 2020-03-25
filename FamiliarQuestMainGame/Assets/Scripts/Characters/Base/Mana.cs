using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Mana : DependencyUser {

    //[SyncVar]
    public float mp;
    //[SyncVar]
    public float maxMP;
    
    // Use this for initialization
    void Start() {
        dependencies = new List<string>() { "{{PLAYER_OR_MONSTER}}", "Character" };
        Dependencies.Check(this);
        Calculate();
        mp = maxMP;
    }

    // Update is called once per frame
    void Update() {
        //if (NetworkServer.active) {
            int level = 1;
            if (GetComponent<ExperienceGainer>() != null) level = GetComponent<ExperienceGainer>().level;
            else level = GetComponent<MonsterScaler>().level;
            mp = Mathf.Min(mp + (CharacterAttribute.attributes["mpRegen"].instances[GetComponent<Character>()].TotalValue * Time.deltaTime), maxMP);
            //mp = Mathf.Min(mp + (SecondaryStatUtility.CalcMpRegenRate(GetComponent<Character>().wisdom, level) * Time.deltaTime), maxMP);
        //}
    }

    public void Calculate() {
        int level = 1;
        if (GetComponent<ExperienceGainer>() != null) level = GetComponent<ExperienceGainer>().level;
        else level = GetComponent<MonsterScaler>().level;
        //int newMP = SecondaryStatUtility.CalcMp(GetComponent<Character>().intelligence, level);
        int newMP = (int)CharacterAttribute.attributes["bonusMp"].instances[GetComponent<Character>()].TotalValue;
        mp += (newMP - maxMP);
        maxMP = newMP;
    }

    public float GetMpFactor(float baseFactor, int level) {
        for (var i = 1; i < level; i++) baseFactor *= 1.1f;
        return baseFactor;
    }
}
