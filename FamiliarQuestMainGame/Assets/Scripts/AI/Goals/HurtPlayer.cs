namespace AI.Goals {
    public class HurtPlayer : GoapGoal {
        public HurtPlayer(float weight = 1) {
            key = "playerHurt";
            value = true;
            this.weight = weight;
        }
    }
}