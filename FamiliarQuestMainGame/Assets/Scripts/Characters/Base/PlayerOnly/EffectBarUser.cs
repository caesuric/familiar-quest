using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class EffectBarUser : MonoBehaviour {

    //public SyncListString effectIcons = new SyncListString();
    //public SyncListFloat effectDurations = new SyncListFloat();
    public List<string> effectIcons = new List<string>();
    public List<float> effectDurations = new List<float>();

    // Use this for initialization
    void Start() {
        var dependencies = new List<string>() { "PlayerCharacter", "Character", "StatusEffectHost" };
        Dependencies.Check(gameObject, dependencies);
    }

    // Update is called once per frame
    void Update() {
        //if (NetworkServer.active) {
            bool updateIcons = false;
            if (effectIcons.Count != GetComponent<StatusEffectHost>().statusEffects.Count) {
                effectIcons.Clear();
                effectDurations.Clear();
                updateIcons = true;
            }
            UpdateEffectIcons(updateIcons);
        //}
    }

    private void UpdateEffectIcons(bool updateIcons) {
        var i = -1;
        foreach (var effect in GetComponent<StatusEffectHost>().statusEffects) {
            i++;
            if (updateIcons) {
                effectIcons.Add(effect.type);
                if (effect.beginningDuration == Mathf.Infinity) effectDurations.Add(1);
                else effectDurations.Add(effect.duration / effect.beginningDuration);
            }
            else if (effect.beginningDuration == Mathf.Infinity) effectDurations[i] = 1f;
            else effectDurations[i] = effect.duration / effect.beginningDuration;
        }
    }
}
