using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectBarUpdater : MonoBehaviour {

    public Character character;
    public GameObject effectIcon;

    // Update is called once per frame
    void Update() {
        if (character == null) Initialize();
        else if (character.GetComponent<PlayerCharacter>().effectIconObjects.Count != character.GetComponent<EffectBarUser>().effectIcons.Count) RecreateEffectBar();
        else UpdateEffectBar();
    }

    private void Initialize() {
        var players = PlayerCharacter.players;
        foreach (var item in players) if (item.isMe) character = item.GetComponent<Character>();
    }

    private void RecreateEffectBar() {
        List<GameObject> objs = new List<GameObject>();
        foreach (var obj in character.GetComponent<PlayerCharacter>().effectIconObjects) Destroy(obj);
        character.GetComponent<PlayerCharacter>().effectIconObjects.Clear();
        for (int i = 0; i < character.GetComponent<EffectBarUser>().effectIcons.Count; i++) CreateEffectIcon(i);
    }

    private void CreateEffectIcon(int i) {
        GameObject obj;
        if (character.GetComponent<PlayerCharacter>().effectIconObjects.Count > i) Destroy(character.GetComponent<PlayerCharacter>().effectIconObjects[i]);
        obj = Instantiate(effectIcon);
        character.GetComponent<PlayerCharacter>().effectIconObjects.Add(obj);
        obj.transform.SetParent(transform, false);
        var rect = obj.GetComponent<RectTransform>();
        var updater = obj.GetComponentInChildren<EffectIconUpdater>();
        updater.Initialize(character.GetComponent<EffectBarUser>().effectIcons[i], character.GetComponent<EffectBarUser>().effectDurations[i], updater.GetComponent<Image>(), updater.duration);
    }

    private void UpdateEffectBar() {
        for (int i = 0; i < character.GetComponent<EffectBarUser>().effectIcons.Count; i++) {
            GameObject obj;
            if (character.GetComponent<PlayerCharacter>().effectIconObjects.Count > i) obj = character.GetComponent<PlayerCharacter>().effectIconObjects[i];
            else return;
            if (obj == null) return;
            var updater = obj.GetComponentInChildren<EffectIconUpdater>();
            updater.duration = character.GetComponent<EffectBarUser>().effectDurations[i];
            updater.image = updater.GetComponent<Image>();
            updater.image.fillAmount = updater.duration;
        }
    }
}