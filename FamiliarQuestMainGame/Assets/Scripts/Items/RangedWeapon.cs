public class RangedWeapon : Weapon {
    public int range;
    public int projectileModel = 0;
    public bool usesInt = false;

    public RangedWeapon() {
        icon = "bow_2";
        attackPower = 0.8437f;
        name = "Basic Bow";
        description = "Attack power: {{AttackPower}}";
        displayType = "Weapon - Bow";
    }

    public static RangedWeapon Wand() {
        var rw = new RangedWeapon {
            icon = "Weapon_16",
            projectileModel = 1,
            usesInt = true,
            name = "Basic Wand",
            description = "Attack power: {{AttackPower}}",
            displayType = "Weapon - Wand"
        };
        return rw;
    }
}
