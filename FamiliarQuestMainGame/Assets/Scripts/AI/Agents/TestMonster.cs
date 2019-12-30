using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace AI.Agents {
    public class TestMonster : GoapAgent {
        public TestMonster() {
            goals = new List<GoapGoal> {
                new AI.Goals.HurtPlayer(1),
                new AI.Goals.StayAlert(0.01f)
            };
            availableActions = new List<GoapAction>() {
                new AI.Actions.FacePlayer(),
                new AI.Actions.HitPlayerWithMeleeAttack(),
                new AI.Actions.HitPlayerWithRangedAttack(),
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
            };
            state = new Dictionary<string, object>() {
                { "seePlayer", false },
                { "inMeleeRangeOfPlayer", false },
                { "facingPlayer", false },
                { "playerAlive", false },
                { "playerHurt", false },
                { "gcdReady", true },
                { "meleeAttackAvailable", false }
            };
        }
    }
//}
