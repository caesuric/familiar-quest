using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class CharacterAttribute {
    private const int minLevel = 1;
    private const int maxLevel = 50;
    public static Dictionary<string, CharacterAttribute> attributes = new Dictionary<string, CharacterAttribute>();
    public Dictionary<Character, CharacterAttributeInstance> instances = new Dictionary<Character, CharacterAttributeInstance>();
    public string name = "";
    public string friendlyName = "";
    public bool isSecondary = false;
    public List<string> basedOn = new List<string>();
    public float minValueAtMinLevel = 0;
    public float avgValueAtMinLevel = 0;
    public float maxValueAtMinLevel = 0;
    public float minValueAtMaxLevel = 0;
    public float avgValueAtMaxLevel = 0;
    public float maxValueAtMaxLevel = 0;
    public bool isPercentage = false;

    public CharacterAttribute(string name, string friendlyName, bool isSecondary, List<string> basedOn = null, float minValueAtMinLevel = 0, float avgValueAtMinLevel = 0, float maxValueAtMinLevel = 0, float minValueAtMaxLevel = 0, float avgValueAtMaxLevel = 0, float maxValueAtMaxLevel = 0, bool isPercentage = false) {
        this.name = name;
        this.friendlyName = friendlyName;
        this.isSecondary = isSecondary;
        this.basedOn = basedOn;
        this.minValueAtMinLevel = minValueAtMinLevel;
        this.avgValueAtMinLevel = avgValueAtMinLevel;
        this.maxValueAtMinLevel = maxValueAtMinLevel;
        this.minValueAtMaxLevel = minValueAtMaxLevel;
        this.avgValueAtMaxLevel = avgValueAtMaxLevel;
        this.maxValueAtMaxLevel = maxValueAtMaxLevel;
        this.isPercentage = isPercentage;
        attributes[name] = this;
    }

    public float GetMinimum(int level) {
        return ((minValueAtMaxLevel - minValueAtMinLevel) * (level - minLevel) / (maxLevel - minLevel)) + minValueAtMinLevel;
    }
    public float GetAverage(int level) {
        return ((avgValueAtMaxLevel - avgValueAtMinLevel) * (level - minLevel) / (maxLevel - minLevel)) + avgValueAtMinLevel;
    }
    public float GetMaximum(int level) {
        return ((maxValueAtMaxLevel - maxValueAtMinLevel) * (level - minLevel) / (maxLevel - minLevel)) + maxValueAtMinLevel;
    }
}
