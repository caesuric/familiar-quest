using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class MonsterAttributes : MonoBehaviour {

    public int strength;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int wisdom;
    public int luck;

    void Start() {
        var c = GetComponent<Character>();
        CharacterAttribute.attributes["strength"].instances[c].BaseValue = strength;
        CharacterAttribute.attributes["dexterity"].instances[c].BaseValue = dexterity;
        CharacterAttribute.attributes["constitution"].instances[c].BaseValue = constitution;
        CharacterAttribute.attributes["intelligence"].instances[c].BaseValue = intelligence;
        CharacterAttribute.attributes["wisdom"].instances[c].BaseValue = wisdom;
        CharacterAttribute.attributes["luck"].instances[c].BaseValue = luck;
    }
}