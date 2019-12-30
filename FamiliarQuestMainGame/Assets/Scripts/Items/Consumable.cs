using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Consumable : Item
{
    public ConsumableType type;
    public float degree;
    public int quantity = 1;

    public Consumable(ConsumableType type, float degree, int quantity)
    {
        this.type = type;
        this.degree = degree;
        this.quantity = quantity;
        displayType = "Potion";
    }
}

public enum ConsumableType
{
    health,
    mana,
    none
}