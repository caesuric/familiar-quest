namespace AI.Goals {
    public class BeBoss : GoapGoal {
        public BeBoss(float weight = 1000) {
            key = "beingABoss";
            value = true;
            this.weight = weight;
        }
    }
}