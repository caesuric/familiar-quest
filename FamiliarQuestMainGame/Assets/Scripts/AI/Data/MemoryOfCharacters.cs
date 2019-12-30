using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AI.Data {
    public class MemoryOfCharacters {
        public List<IndividualCharacterMemory> memories = new List<IndividualCharacterMemory>();

        public void AddFovFrame(FieldOfVision fov) {
            foreach (var player in fov.players) AddPlayerMemory(player);
        }

        private void AddPlayerMemory(GameObject player) {
            foreach (var memory in memories) {
                if (memory.character==player) {
                    memory.time = DateTime.Now;
                    memory.position = player.transform.position;
                    return;
                }
            }
            memories.Add(new IndividualCharacterMemory(player, player.transform.position, true));
        }

        public IndividualCharacterMemory GetClosestPlayerMemory(GoapAgent agent) {
            float distance = Mathf.Infinity;
            IndividualCharacterMemory target = null;
            foreach (var memory in memories) {
                if (memory.isEnemy) {
                    var distanceToPlayerPosition = Vector3.Distance(agent.transform.position, memory.position);
                    if (distanceToPlayerPosition<distance) {
                        distance = distanceToPlayerPosition;
                        target = memory;
                    }
                }
            }
            return target;
        }
    }

    public class IndividualCharacterMemory {
        public GameObject character = null;
        public DateTime time = DateTime.Now;
        public Vector3 position = new Vector3();
        public bool isEnemy = false;

        public IndividualCharacterMemory(GameObject character, Vector3 position, bool isEnemy) {
            this.character = character;
            this.position = position;
            this.isEnemy = isEnemy;
        }
    }
}
