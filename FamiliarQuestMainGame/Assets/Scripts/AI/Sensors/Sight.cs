using UnityEngine;

namespace AI.Sensors {
    public class Sight : GoapSensor {

        private MonsterScaler monsterScaler = null;

        public override void Run(GoapAgent agent) {
            if (monsterScaler == null) monsterScaler = agent.GetComponent<MonsterScaler>();
            UpdateFov(agent);
            var fov = agent.memory["fieldOfVision"] as Data.FieldOfVision;
            UpdateLastSeen(fov, agent);
            agent.state["seePlayer"] = (fov.players.Count > 0);
            if (fov.players.Count > 0) agent.state["haveSeenPlayer"] = true;
            agent.state["inMeleeRangeOfPlayer"] = CanHitPlayer(agent);
            agent.state["facingPlayer"] = IsFacingPlayer(agent);
            agent.state["facingPlayerPrecisely"] = IsFacingPlayer(agent, 10f);
            agent.state["playerAlive"] = IsPlayerAlive(agent, (bool)agent.state["playerAlive"]);
            agent.state["playerHurt"] = !(agent.state["playerAlive"].Equals(true));
        }

        public void UpdateFov(GoapAgent agent) {
            var fov = new Data.FieldOfVision();
            foreach (var player in PlayerCharacter.players) if (CanSeeSpecificPlayer(agent, player)) fov.players.Add(player.gameObject);
            agent.memory["fieldOfVision"] = fov;
        }

        public void UpdateLastSeen(Data.FieldOfVision fov, GoapAgent agent) {
            var memChars = new Data.MemoryOfCharacters();
            if (agent.memory.ContainsKey("characters")) memChars = agent.memory["characters"] as Data.MemoryOfCharacters;
            else agent.memory.Add("characters", memChars);
            memChars.AddFovFrame(fov);
        }

        private bool IsFacingPlayer(GoapAgent agent, float angle = 30f) {
            var fov = agent.memory["fieldOfVision"] as Data.FieldOfVision;
            foreach (var player in fov.players) {
                if (Mathf.Abs(Vector3.Angle(agent.transform.forward, player.transform.position - agent.transform.position)) <= angle) return true;
            }
            return false;
        }

        private bool IsPlayerAlive(GoapAgent agent, bool previousState) {
            var fov = agent.memory["fieldOfVision"] as Data.FieldOfVision;
            foreach (var player in fov.players) if (player.GetComponent<Health>().hp > 0) return true;
            return previousState;
        }

        public bool CanHitPlayer(GoapAgent agent) {
            var fov = agent.memory["fieldOfVision"] as Data.FieldOfVision;
            foreach (var player in fov.players) if (CanSeeSpecificPlayer(agent, player.GetComponent<PlayerCharacter>(), range: monsterScaler.colliderSize + 2.5f)) return true;
            return false;
        }

        public bool CanSeePlayer(GoapAgent agent, float range = 25f) {
            foreach (var player in PlayerCharacter.players) if (CanSeeSpecificPlayer(agent, player, range)) return true;
            return false;
        }

        private bool CanSeeSpecificPlayer(GoapAgent agent, PlayerCharacter player, float range = 25f) {
            if (RaycastCheck(player, agent.transform.position, range)) return true;
            if (RaycastCheck(player, agent.transform.position + new Vector3(-0.1f, 0, -0.1f), range)) return true;
            if (RaycastCheck(player, agent.transform.position + new Vector3(-0.1f, 0, 0.1f), range)) return true;
            if (RaycastCheck(player, agent.transform.position + new Vector3(0.1f, 0, -0.1f), range)) return true;
            if (RaycastCheck(player, agent.transform.position + new Vector3(0.1f, 0, 0.1f), range)) return true;
            return false;
        }

        private bool RaycastCheck(PlayerCharacter player, Vector3 position, float range) {
            var rayDirection = player.transform.position - position;
            rayDirection.y = 0;
            var hits = Physics.RaycastAll(position, rayDirection, range);
            bool found = false;
            foreach (var hit in hits) {
                if (hit.transform.gameObject.CompareTag("Wall") && Vector3.Distance(hit.transform.position, position) < Vector3.Distance(player.transform.position, position)) return false;
                if (hit.transform.gameObject.CompareTag("Player")) found = true;
            }
            return found;
        }
    }
}
