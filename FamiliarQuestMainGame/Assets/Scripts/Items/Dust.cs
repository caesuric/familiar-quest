﻿public class Dust : Item {
    public string type;
    public float quantity = 1;

    public Dust(string type, float quantity) {
        this.type = type;
        this.quantity = quantity;
        displayType = "Dust";
    }
}
