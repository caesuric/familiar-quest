﻿using System.Collections.Generic;
using UnityEngine;

namespace AI.Actions {
    public class FacePlayer : GoapAction {

        private bool started = false;
        private Quaternion originalRotation = new Quaternion();
        private Quaternion targetLookRotation = new Quaternion();
        private float timer = 0f;
        private static readonly float turnTime = 0.1f;

        public FacePlayer() {
            preconditions = new Dictionary<string, object>() {
                { "seePlayer", true },
                { "paralyzed", false }
            };
            effects = new Dictionary<string, object>() {
                { "facingPlayer", true },
                { "facingPlayerPrecisely", true }
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
    }
}