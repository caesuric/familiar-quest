using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUpdater : MonoBehaviour {

    public GameObject background;
    public Text levelText;
    public Text nameText;
    public Image qualityBackground;
    public MOBAEnergyBar healthBar;
    public List<Sprite> qualityBackgroundSprites = new List<Sprite>();

    private GameObject previousTarget = null;
    private Health targetHealth = null;    

    void Update() {
        if (Health.lastTargetHitByPlayer == null || Health.lastTargetHitByPlayer.GetComponent<Health>().hp <= 0) DisableHealthBar();
        else {
            EnableHealthBar();
            if (previousTarget != Health.lastTargetHitByPlayer) {
                UpdateLevelNumber();
                UpdateQualityBackground();
                UpdateName();
                targetHealth = Health.lastTargetHitByPlayer.GetComponent<Health>();
            }
            UpdateHealth();
            previousTarget = Health.lastTargetHitByPlayer;
        }
    }

    private void DisableHealthBar() {
        if (background.activeSelf) background.SetActive(false);
    }

    private void EnableHealthBar() {
        if (!background.activeSelf) background.SetActive(true);
    }

    private void UpdateLevelNumber() {
        levelText.text = Health.lastTargetHitByPlayer.GetComponent<MonsterScaler>().level.ToString();
    }

    private void UpdateQualityBackground() {
        var quality = Health.lastTargetHitByPlayer.GetComponent<MonsterScaler>().quality;
        qualityBackground.sprite = qualityBackgroundSprites[quality];
    }

    private void UpdateName() {
        var name = Health.lastTargetHitByPlayer.name;
        name = name.Replace("(Clone)", "");
        name = name.Replace(" (BOSS)", "");
        if (name == "GOBLIN") name = "Goblin";
        if (name == "SPIDER") name = "Spider";
        var quality = Health.lastTargetHitByPlayer.GetComponent<MonsterScaler>().quality;
        var qualityNames = new List<string> {
            "",
            "Strong",
            "Elite",
            "Miniboss",
            "Boss"
        };
        if (quality == 0) nameText.text = name;
        else nameText.text = qualityNames[quality] + " " + name;
    }

    private void UpdateHealth() {
        var value = 100f * targetHealth.hp / targetHealth.maxHP;
        if (healthBar.Value != value) healthBar.Value = value;
    }
}
