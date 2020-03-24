using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class AutoHealer : DependencyUser {

    private float autoHealTimer = 3.0f;
    private Health health;
    private ExperienceGainer experienceGainer;
    private MonsterScaler monsterScaler;
    private Character character;
    private float timer;

    void Start() {
        health = GetComponent<Health>();
        experienceGainer = GetComponent<ExperienceGainer>();
        monsterScaler = GetComponent<MonsterScaler>();
        character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update() {
        //if (NetworkServer.active) {
            timer += Time.deltaTime;
            if (timer >= 1f) {
                if (health.hp < health.maxHP) {
                    ApplyInCombatRegen();
                    if (OutOfCombat()) {
                        autoHealTimer = Mathf.Max(autoHealTimer - timer, 0f);
                        if (autoHealTimer <= 0f && health.hp > 0) health.hp = Mathf.Min(health.hp + (60 * 0.375f * health.maxHP / 290f * timer), health.maxHP);
                    }
                    else autoHealTimer = 3.0f;
                }
                timer = 0f;
            }
        //}
    }

    public static bool OutOfCombat()
    {
        //foreach (var monster in Monster.monsters) if (monster.GetComponent<MonsterCombatant>().InCombat()) return false;
        return false; // temp
        return true;
    }

    private void ApplyInCombatRegen() {
        if (health.hp <= 0) return;
        int level = 1;
        if (experienceGainer != null) level = experienceGainer.level;
        else level = monsterScaler.level;
        //health.hp = Mathf.Min(health.hp + (timer * SecondaryStatUtility.CalcHpRegen(character.constitution, level)), health.maxHP);
        health.hp = Mathf.Min(health.hp + (timer * CharacterAttribute.attributes["hpRegen"].instances[character].TotalValue), health.maxHP);
    }
}
