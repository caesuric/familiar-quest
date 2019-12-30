using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

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
                return;
            }
            if (!started) {
                started = true;
                isDone = false;
                movePhase = 0;
                startRotation = originalRotation = agent.transform.eulerAngles;
                rotationAmount = Random.Range(30f, 90f);
                targetLookRotation = new Vector3(originalRotation.x, originalRotation.y - rotationAmount, originalRotation.z);
                timer = 0f;
                turnTime = Random.Range(2f, 4f);
            }
            else {
                timer += Time.deltaTime;
                agent.transform.rotation = Quaternion.Slerp(Quaternion.Euler(startRotation), Quaternion.Euler(targetLookRotation), timer / turnTime);
                if (timer >= turnTime) {
                    movePhase++;
                    timer = 0f;
                    if (movePhase == 1) {
                        startRotation = agent.transform.eulerAngles;
                        targetLookRotation = new Vector3(originalRotation.x, originalRotation.y + rotationAmount, originalRotation.z);
                    }
                    else if (movePhase == 2) {
                        startRotation = agent.transform.eulerAngles;
                        targetLookRotation = originalRotation;
                    }
                    else if (movePhase == 3) {
                        started = false;
                        isDone = true;
                        agent.busy = false;
                    }
                }
            }
        }
    }
}
