using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitFrame : MonoBehaviour {
    public MOBAEnergyBar healthBar;
    public Image levelCircle;
    public TextMesh levelText;
    public TextMesh debugText;
    public List<Sprite> levelCircleSprites;

    public void SetHealthPercentage(float percentage) {
        healthBar.Value = percentage;
    }

    public void SetLevelCircleType(int quality) {
        levelCircle.sprite = levelCircleSprites[quality];
    }

    public void SetLevel(int level) {
        levelText.text = level.ToString();
    }

    public void SetDebugText(string text) {
        debugText.text = text;
    }

    public void SetScale(GameObject go) {
        if (go.GetComponent<Spider>() != null) gameObject.transform.localScale *= 2f;
    }
}
