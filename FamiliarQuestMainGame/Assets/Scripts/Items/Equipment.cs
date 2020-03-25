using System.Collections.Generic;

public abstract class Equipment : Item {
    public List<EquipmentEffect> effects = new List<EquipmentEffect>();
    //public int strength = 0;
    //public int dexterity = 0;
    //public int constitution = 0;
    //public int intelligence = 0;
    //public int wisdom = 0;
    //public int luck = 0;
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
}

public class EquipmentEffect {
    //STUB: TODO
}
