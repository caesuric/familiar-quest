using System.Collections.Generic;

public abstract class Equipment : Item {
    public List<EquipmentEffect> effects = new List<EquipmentEffect>();
    public int strength = 0;
    public int dexterity = 0;
    public int constitution = 0;
    public int intelligence = 0;
    public int wisdom = 0;
    public int luck = 0;
    public int armor = 0;
    public int quality = 0;
}

public class EquipmentEffect {
    //STUB: TODO
}
