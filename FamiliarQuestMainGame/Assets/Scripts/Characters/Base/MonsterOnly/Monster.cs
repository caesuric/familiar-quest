using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class Monster : MonoBehaviour { //Hideable

    public float attackFactor;
    public float hpFactor;
    public float mpFactor;
    public static List<Monster> monsters = new List<Monster>();
    public int minLevel = 1;
    public int maxLevel = 50;
    public List<ElementalAffinity> elementalAffinities = new List<ElementalAffinity>();
    public GameObject unitFramePrefab;
    public GameObject unitFrame;
    public Renderer[] renderers;
    private MonsterAi monsterAi = null;
    //private MonsterCombatant monsterCombatant;

    // Use this for initialization

    void Start() {
        if (unitFramePrefab != null) unitFrame = GameObject.Instantiate(unitFramePrefab, transform);
        //monsterCombatant = GetComponent<MonsterCombatant>();
        //if (!NetworkServer.active && GetComponent<NavMeshAgent>() != null) GetComponent<NavMeshAgent>().enabled = false;
        //items.Add(this);
        //if (NetworkServer.active) {
        monsters.Add(this);
        if (GetComponent<RewardGiver>().generatedMonster) return;
        int spiritRoll = Random.Range(0, 200);
        if (spiritRoll < 10) {
            var ability = AbilityGenerator.Generate(GetComponent<MonsterScaler>().level);
            if (ability is PassiveAbility passiveAbility) GetComponent<AbilityUser>().soulGemPassive = passiveAbility;
            else GetComponent<AbilityUser>().soulGemActives.Add((ActiveAbility)ability);
            if (GetComponent<AbilityUser>().soulGemPassive != null) GetComponent<AbilityUser>().AddPassive(GetComponent<AbilityUser>().soulGemPassive);
        }
        renderers = GetComponentsInChildren<Renderer>();
        monsterAi = GetComponent<MonsterAi>();
        //}
    }

    private void Update() {
        var inCombat = (bool)monsterAi.state["seePlayer"];
        if (inCombat && !unitFrame.activeSelf) unitFrame.SetActive(true);
        else if (!inCombat && unitFrame.activeSelf) unitFrame.SetActive(false);
    }

    void OnDestroy() {
        monsters.Remove(this);
    }

    public float ModifyDamageForElements(float amount, Element type) {
        float currentDamage = amount;
        float modifier = 0;
        foreach (var affinity in elementalAffinities) if (affinity.type == type) modifier -= affinity.amount;
        currentDamage *= (1 + (modifier / 100));
        return currentDamage;
    }
}
