using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IconCache : MonoBehaviour {

    public List<Sprite> icons = new List<Sprite>();
    public Dictionary<string, Sprite> newIcons = new Dictionary<string, Sprite>();

    public void Start() {
        var iconList = Resources.LoadAll("New Icons");
        foreach (var icon in iconList) {
            if (icon is Sprite) newIcons[icon.name] = (Sprite)icon;
        }
    }
}
