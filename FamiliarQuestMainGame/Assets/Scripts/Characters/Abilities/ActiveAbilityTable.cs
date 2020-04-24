using UnityEngine;

public static class ActiveAbilityTable {
    public static ActiveAbility Retrieve(int level=1) {
        int roll = Random.Range(1, 100);
        float points = 70f;
        if (level > 1) for (int i = 1; i < level; i++) points *= 1.05f;
        if (roll < 87) return AttackAbilityTable.Retrieve(points);
        else return UtilityAbilityTable.Retrieve(points);
    }
}
