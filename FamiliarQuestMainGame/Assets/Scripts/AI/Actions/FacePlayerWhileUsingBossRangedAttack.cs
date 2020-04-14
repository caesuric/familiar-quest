﻿using System.Collections.Generic;
using UnityEngine;

namespace AI.Actions {
    public class FacePlayerWhileUsingBossRangedAttack : GoapAction {

        private bool started = false;
        private Quaternion originalRotation = new Quaternion();
        private Quaternion targetLookRotation = new Quaternion();
        private float timer = 0f;
        private static readonly float turnTime = 0.1f;
        Boss boss = null;
        private MonsterAnimationController monsterAnimationController = null;

        public FacePlayerWhileUsingBossRangedAttack() {
            preconditions = new Dictionary<string, object>() {
                { "seePlayer", true },
                { "bossRangedAttackAvailable", true },
                { "playerAlive", true }
            };
            effects = new Dictionary<string, object>() {
                { "facingPlayer", true },
                { "facingPlayerPrecisely", true },
                { "playerHurt", true },
                { "gcdReady", false }
            };
            cost = 1f;
        }

        public override void Execute(GoapAgent agent) {
            if (!ActionPossible(agent)) agent.busy = false;
            else if (!started) StartAction(agent);
            else ContinueAction(agent);
        }

        private void StartAction(GoapAgent agent) {
            target = SelectTarget(agent);
            if (target == null) {
                agent.busy = false;
                return;
            }
            started = true;
            isDone = false;
            originalRotation = agent.transform.rotation;
            targetLookRotation = Quaternion.LookRotation(target.transform.position - agent.transform.position);
            timer = 0f;
        }

        private void ContinueAction(GoapAgent agent) {
            if (monsterAnimationController == null) monsterAnimationController = agent.GetComponent<MonsterAnimationController>();
            if (boss == null) boss = agent.GetComponent<Boss>();
            if (!ActionPossible(agent)) {
                monsterAnimationController.attacking = false;
            }
            else {
                monsterAnimationController.attacking = true;
                HitPlayer(agent);
            }
            timer += Time.deltaTime;
            agent.transform.rotation = Quaternion.Slerp(originalRotation, targetLookRotation, timer / turnTime);
            if (timer >= turnTime) {
                timer = 0f;
                started = false;
                ApplyEffects(agent);
            }
        }

        private GameObject SelectTarget(GoapAgent agent) {
            var fov = agent.memory["fieldOfVision"] as Data.FieldOfVision;
            if (fov.players.Count == 0) return null;
            else return fov.players[0];
        }

        private void HitPlayer(GoapAgent agent) {
            UseRangedAbility(agent);
        }

        private void UseRangedAbility(GoapAgent agent) {
            if (boss == null ) return;
            foreach (var ability in boss.bossAbilities) {
                if (IsRangedAbility(ability) && ability.currentCooldown == 0) {
                    agent.GetComponent<AbilityUser>().UseAbility(ability);
                    return;
                }
            }
        }

        private bool IsRangedAbility(ActiveAbility ability) {
            if (ability is AttackAbility) {
                var attackAbility = ability as AttackAbility;
                if (attackAbility.isRanged || attackAbility.FindAttribute("chargeTowards") != null) return true;
            }
            return false;
        }
    }
}