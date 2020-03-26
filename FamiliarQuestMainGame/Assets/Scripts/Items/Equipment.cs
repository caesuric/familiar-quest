using System.Collections.Generic;

public abstract class Equipment : Item {
    public List<EquipmentEffect> effects = new List<EquipmentEffect>();
    public int armor = 0;
    public int quality = 0;
    public Dictionary<string, int> stats = new Dictionary<string, int>();

    public void AddStat(string stat, int value) {
        if (stats.ContainsKey(stat)) stats[stat] += value;
        else stats[stat] = value;
    }

    public int GetStatValue(string stat) {
        if (stats.ContainsKey(stat)) return stats[stat];
        return 0;
    }

    public string GetHighestStat() {
        var highest = "";
        var highestValue = 0;
        foreach (var kvp in stats) {
            if (kvp.Value > highestValue) {
                highestValue = kvp.Value;
                highest = kvp.Key;
            }
        }
        return highest;
    }
}

public class EquipmentEffect {
    //STUB: TODO
}
