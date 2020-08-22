using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbility: Ability {
    public float cooldown = 0f;
    public float currentCooldown = 0f;
    public float mpUsage = 0;
    public float baseMpUsage = 0;
    public float radius = 0;
    public BaseStat baseStat;
}
