﻿using System.Collections.Generic;

public class MonsterAi : GoapAgent {
    public MonsterAi() {
        goals = new List<GoapGoal> {
            new AI.Goals.HurtPlayer(1),
            new AI.Goals.StayAlert(0.01f)
        };
        availableActions = new List<GoapAction>() {
            new AI.Actions.FacePlayer(),
            new AI.Actions.HitPlayerWithMeleeAttack(),
            new AI.Actions.HitPlayerWithRangedAttack(),
            new AI.Actions.FacePlayerWhileUsingRangedAttack(),
            new AI.Actions.MoveToPlayer(),
            new AI.Actions.WaitForGcd(),
            new AI.Actions.PursuePlayer(),
            new AI.Actions.LookAround()
        };
        sensors = new List<GoapSensor>() {
            new AI.Sensors.AbilityTracking(),
            new AI.Sensors.GcdTracking(),
            new AI.Sensors.Sight(),
            new AI.Sensors.Memory(),
            new AI.Sensors.EffectTracking()
        };
        state = new Dictionary<string, object>() {
            { "seePlayer", false },
            { "inMeleeRangeOfPlayer", false },
            { "facingPlayer", false },
            { "playerAlive", false },
            { "playerHurt", false },
            { "gcdReady", true },
            { "meleeAttackAvailable", false },
            { "rangedAttackAvailable", false },
            { "bossMeleeAttackAvailable", false },
            { "bossRangedAttackAvailable", false },
            { "bossUtilityAbilityAvailable", false },
            { "paralyzed", false }
        };
    }
}
