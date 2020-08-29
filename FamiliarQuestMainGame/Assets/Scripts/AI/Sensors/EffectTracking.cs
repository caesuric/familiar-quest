namespace AI.Sensors {
    public class EffectTracking : GoapSensor {

        StatusEffectHost statusEffectHost = null;

        public override void Run(GoapAgent agent) {
            if (statusEffectHost == null) statusEffectHost = agent.GetComponent<StatusEffectHost>();
            agent.state["paralyzed"] = IsParalyzed();
        }

        private bool IsParalyzed() {
            return statusEffectHost.CheckForEffect("paralysis");
        }
    }
}
