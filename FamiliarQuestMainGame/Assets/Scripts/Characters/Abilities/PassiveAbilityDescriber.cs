using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class PassiveAbilityDescriber {
    private delegate string DescriberDelegate(Ability ability, AbilityAttribute attribute);
    private static readonly Dictionary<string, DescriberDelegate> descriptionTable;
    private static readonly Dictionary<string, string> simpleDescriptionTable;

    static PassiveAbilityDescriber() {
        simpleDescriptionTable = new Dictionary<string, string> {
            ["knockback"] = "Adds knockback to all attacks.\n",
            ["charge"] = "All melee attacks cause you to charge forward until you hit an enemy.\n",
            ["pullEnemies"] = "All ranged attacks pull enemies to you on hit.\n"
        };
        descriptionTable = new Dictionary<string, DescriberDelegate> {
            ["goldBoost"] = GoldBoostDescription,
            ["boostDamage"] = BoostDamageDescription,
            ["reduceDamage"] = ReduceDamageDescription,
            ["reduceElementalDamage"] = ReduceElementalDamageDescription,
            ["boostElementalDamage"] = BoostElementalDamageDescription,
            ["boostStat"] = BoostStatDescription,
            ["damageEnemiesOnScreen"] = DamageEnemiesOnScreenDescription,
            ["experienceBoost"] = ExperienceBoostDescription
        };
    }

    public static string Describe(PassiveAbility ability) {
        string description = "L" + ability.level.ToString() + " <b>Passive</b>\n";
        int count = 0;
        foreach (var attribute in ability.attributes) {
            if (attribute.priority < 50 || count > 3) description += "LATENT - ";
            if (simpleDescriptionTable.ContainsKey(attribute.type)) description += simpleDescriptionTable[attribute.type];
            else if (descriptionTable.ContainsKey(attribute.type)) description += descriptionTable[attribute.type](ability, attribute);
            else description += attribute.type + " - DESCRIPTION NOT FOUND.\n";
            count++;
        }
        return description;
    }

    private static string ExperienceBoostDescription(Ability ability, AbilityAttribute attribute) {
        return "Increases experience gained by " + ((int)(((float)attribute.FindParameter("degree").value) * 100f)).ToString() + "%.\n";
    }

    private static string DamageEnemiesOnScreenDescription(Ability ability, AbilityAttribute attribute) {
        return "Continously deals " + attribute.FindParameter("degree").value.ToString() + " damage per second to all enemies on screen.\n";
    }

    private static string BoostStatDescription(Ability ability, AbilityAttribute attribute) {
        switch ((string)attribute.FindParameter("stat").value) {
            case "strength":
                return "Increases Strength by " + attribute.FindParameter("degree").value.ToString() + ".\n";
            case "dexterity":
                return "Increases Dexterity by " + attribute.FindParameter("degree").value.ToString() + ".\n";
            case "constitution":
                return "Increases Constitution by " + attribute.FindParameter("degree").value.ToString() + ".\n";
            case "intelligence":
                return "Increases Intelligence by " + attribute.FindParameter("degree").value.ToString() + ".\n";
            case "wisdom":
                return "Increases Wisdom by " + attribute.FindParameter("degree").value.ToString() + ".\n";
            case "luck":
                return "Increases Luck by " + attribute.FindParameter("degree").value.ToString() + ".\n";
            default:
                return "Increases UNKNOWN ATTRIBUTE by " + attribute.FindParameter("degree").value.ToString() + ".\n";
        }
    }

    private static string BoostElementalDamageDescription(Ability ability, AbilityAttribute attribute) {
        return "Increase " + (string)attribute.FindParameter("element").value + " damage dealt by " + ((int)(((float)attribute.FindParameter("degree").value) * 100f)).ToString() + "%.\n";
    }

    private static string ReduceElementalDamageDescription(Ability ability, AbilityAttribute attribute) {
        return "Reduce " + (string)attribute.FindParameter("element").value + " damage taken by " + ((int)(((float)attribute.FindParameter("degree").value) * 100f)).ToString() + "%.\n";
    }

    private static string ReduceDamageDescription(Ability ability, AbilityAttribute attribute) {
        return "Decreases all damage taken by " + ((int)(((float)attribute.FindParameter("degree").value) * 100f)).ToString() + "%.\n";
    }

    private static string BoostDamageDescription(Ability ability, AbilityAttribute attribute) {
        return "Increases all damage dealt by " + ((int)(((float)attribute.FindParameter("degree").value) * 100f)).ToString() + "%.\n";
    }

    private static string GoldBoostDescription(Ability ability, AbilityAttribute attribute) {
        return "Increases gold found by " + ((int)(((float)attribute.FindParameter("degree").value) * 100f)).ToString() + "%.\n";
    }
}
