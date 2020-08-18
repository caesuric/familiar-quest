using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class AbilityIconSelector {
    public static int Select(AttackAbility ability) {
        return AttackAbilityIconSelector.Select(ability);
    }

    public static int Select(UtilityAbility ability) {
        return UtilityAbilityIconSelector.Select(ability);
    }

    public static int Select(PassiveAbility ability) {
        return PassiveAbilityIconSelector.Select(ability);
    }
}
