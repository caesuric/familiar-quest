public abstract class GoapGoal {
    public string key = "";
    public object value = null;
    public float weight = 1f;

    public GoapGoal(float weight = 1) {
        this.weight = weight;
    }
}
