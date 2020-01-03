using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectIconUpdater : MonoBehaviour {

    public IconCache iconCache;
    public Image image;
    private EffectIcons icons;
    public float duration = 0;
    public string icon;

    // Use this for initialization
    void Start() {
        var config = GameObject.FindGameObjectsWithTag("ConfigObject");
        iconCache = config[0].GetComponent<IconCache>();
        image = GetComponent<Image>();
        var parentImage = transform.parent.GetComponent<Image>();
        icons = new EffectIcons();
        parentImage.sprite = icons.icons[icon];
    }

    // Update is called once per frame
    void Update() {
        image.fillAmount = duration;
    }

    public void Initialize(string icon, float duration, Image image, float fillAmount) {
        this.icon = icon;
        this.duration = duration;
        this.image = image;
        this.image.fillAmount = fillAmount;
    }
}

public class EffectIcons {
    public static IconCache iconCache;
    public Dictionary<string, Sprite> icons = new Dictionary<string, Sprite>();
    public EffectIcons() {
        var config = GameObject.FindGameObjectsWithTag("ConfigObject");
        iconCache = config[0].GetComponent<IconCache>();
        var data = TextReader.ReadSets("EffectIcons");
        foreach (var item in data) icons.Add(item[0], iconCache.icons[int.Parse(item[1])]);
    }
}
