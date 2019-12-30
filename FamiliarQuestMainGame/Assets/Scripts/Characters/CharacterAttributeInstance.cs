using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class CharacterAttributeInstance {
    public Character character = null;
    public event EventHandler ValueChanged;
    private float _baseValue = 0;
    public float BaseValue {
        get => _baseValue;
        set {
            _baseValue = value;
            TotalValue = _baseValue + _itemValue + _abilityValue + DerivedValue + _buffValue;
            ValueUpdated();
        }
    }
    private float _itemValue = 0;
    public float ItemValue {
        get => _itemValue;
        set {
            _itemValue = value;
            TotalValue = _baseValue + _itemValue + _abilityValue + DerivedValue + _buffValue;
            ValueUpdated();
        }
    }
    private float _abilityValue = 0;
    public float AbilityValue {
        get => _abilityValue;
        set {
            _abilityValue = value;
            TotalValue = _baseValue + _itemValue + _abilityValue + DerivedValue + _buffValue;
            ValueUpdated();
        }
    }
    private float _buffValue = 0;
    public float BuffValue {
        get => _buffValue;
        set {
            _buffValue = value;
            TotalValue = _baseValue + _itemValue + _abilityValue + DerivedValue + _buffValue;
            ValueUpdated();
        }
    }
    public float DerivedValue { get; private set; } = 0;
    public float TotalValue { get; private set; } = 0;
    public string name = "";

    public static void CreateAllAttributesForCharacter(Character character) {
        foreach (var kvp in CharacterAttribute.attributes) {
            var temp = new CharacterAttributeInstance(character, kvp.Key);
        }
    }

    public CharacterAttributeInstance(Character character, string name) {
        this.character = character;
        this.name = name;
        CharacterAttribute.attributes[name].instances[character] = this;
        if (CharacterAttribute.attributes[name].isSecondary) {
            foreach (var baseStat in CharacterAttribute.attributes[name].basedOn) {
                CharacterAttribute.attributes[baseStat].instances[character].ValueChanged += UpdateFromBaseStat;
            }
        }
    }

    private void UpdateFromBaseStat(object sender, EventArgs e) {
        var senderObj = (CharacterAttribute)sender;
        float derivedValueTotal = 0;
        foreach (var kvp in CharacterAttribute.attributes) {
            if (CharacterAttribute.attributes[name].basedOn.Contains(kvp.Key)) {
                derivedValueTotal += kvp.Value.instances[character].TotalValue;
            }
        }
        DerivedValue = CalculateDerivedValue(derivedValueTotal);
    }

    private float CalculateDerivedValue(float baseValue) {
        int level = SecondaryStatUtility.GetLevel(character);
        float percentage = SecondaryStatUtility.GetPercent((int)baseValue, level);
        float minimum = CharacterAttribute.attributes[name].GetMinimum(level);
        float average = CharacterAttribute.attributes[name].GetAverage(level);
        float maximum = CharacterAttribute.attributes[name].GetMaximum(level);
        if (percentage < 0.5) return ((average - minimum) * (percentage / 2) + minimum);
        else if (percentage == 0.5) return average;
        else return ((maximum - average) * ((percentage - 0.5f) * 2) + average);
    }

    protected void ValueUpdated() {
        var e = new EventArgs();
        ValueChanged?.Invoke(this, e);
    }
}
