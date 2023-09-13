namespace AI.Goals {
    public class StayAlert : GoapGoal {
        public StayAlert(float weight = 1) {
            key = "awareOfSurroundings";
            value = true;
            this.weight = weight;
        }
    }
}