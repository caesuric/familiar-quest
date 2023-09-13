using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Actions {
    public class LookAround : GoapAction {

        private bool started = false;
        private Vector3 originalRotation = new Vector3();
        private Vector3 startRotation = new Vector3();
        private Vector3 targetLookRotation = new Vector3();
        private int movePhase = 0;
        private float timer = 0f;
        private float turnTime = 0.25f;
        private float rotationAmount = 0f;

        public LookAround() {
            preconditions = new Dictionary<string, object>() {
                { "seePlayer", false }
            };
            effects = new Dictionary<string, object>() {
                { "awareOfSurroundings", true }
            };
            cost = 1f;
        }

        public override void Execute(GoapAgent agent) {
            if (!ActionPossible(agent)) {
                started = false;
                Fail(agent);
            }
            else if (!started) StartAction(agent);
            else ContinueAction(agent);
        }

        private void StartAction(GoapAgent agent) {
            started = true;
            isDone = false;
            movePhase = 0;
            startRotation = originalRotation = agent.transform.eulerAngles;
            rotationAmount = UnityEngine.Random.Range(30f, 90f);
            targetLookRotation = new Vector3(originalRotation.x, originalRotation.y - rotationAmount, originalRotation.z);
            timer = 0f;
            turnTime = UnityEngine.Random.Range(2f, 4f);
        }

        private void ContinueAction(GoapAgent agent) {
            timer += Time.deltaTime;
            agent.transform.rotation = Quaternion.Slerp(Quaternion.Euler(startRotation), Quaternion.Euler(targetLookRotation), timer / turnTime);
            if (timer >= turnTime) FinishTurn(agent);
        }

        private void FinishTurn(GoapAgent agent) {
            movePhase++;
            timer = 0f;
            var actions = new Dictionary<int, Action>() {
                { 1, () => CompleteMovePhase1(agent) },
                { 2, () => CompleteMovePhase2(agent) },
                { 3, () => CompleteMovePhase3(agent) }
            };
            actions[movePhase]();
        }

        private void CompleteMovePhase1(GoapAgent agent) {
            startRotation = agent.transform.eulerAngles;
            targetLookRotation = new Vector3(originalRotation.x, originalRotation.y + rotationAmount, originalRotation.z);
        }

        private void CompleteMovePhase2(GoapAgent agent) {
            startRotation = agent.transform.eulerAngles;
            targetLookRotation = originalRotation;
        }

        private void CompleteMovePhase3(GoapAgent agent) {
            started = false;
            isDone = true;
            agent.busy = false;
        }
    }
}
