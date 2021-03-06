﻿using UnityEngine;
using UnityEngine.AI;

class MonsterMortal : MonoBehaviour {
    public GameObject deathSound;
    public bool deathSoundCreated = false;
    public bool died = false;
    public GameObject killer = null;
    public bool diedToPlayer = true;
    public float fadeOutIntensity = 0f;
    public Shader shader;
    public Texture texture;
    public Texture gradientTexture;
    public float fadeOutTime = 5f;

    void Update() {
        if (!died) return;
        fadeOutIntensity += Time.deltaTime / fadeOutTime;
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers) {
            foreach (var material in renderer.materials) {
                material.SetFloat("_DissolveCutoff", fadeOutIntensity);
            }
        }
    }

    public void OnDeath() {
        if (died) return;
        died = true;
        if (diedToPlayer) {
            foreach (var player in PlayerCharacter.players) {
                player.GetComponent<ExperienceGainer>().GainXP(GetComponent<RewardGiver>().xpValue / PlayerCharacter.players.Count);
                player.GainGold(GetComponent<RewardGiver>().goldValue / PlayerCharacter.players.Count);
                GameLog.AddText("You gain " + (GetComponent<RewardGiver>().goldValue / PlayerCharacter.players.Count).ToString() + " gold.");
            }
            if (PlayerCharacter.players.Count > 0 && killer != null) {
                var pc = killer.GetComponent<PlayerCharacter>();
                var roll = RNG.Int(0, 2);
                if (pc != null) {
                    if (roll == 0 && GetComponent<AbilityUser>().soulGemPassive != null) pc.GainSoulGem(GetComponent<AbilityUser>().soulGemPassive);
                    else if (roll == 0 && GetComponent<AbilityUser>().soulGemActives.Count > 0) pc.GainSoulGem(GetComponent<AbilityUser>().soulGemActives[0]);
                    GetComponent<RewardGiver>().DropLoot(pc.GetComponent<Character>());
                }
            }
        }
        if (GetComponent<Boss>()!=null) {
            LevelGen.instance.bossFightActive = false;
            var boss = GetComponent<Boss>();
            Instantiate(boss.exitPortal, boss.originalLocation, boss.exitPortal.transform.rotation);
        }
        GetComponent<AudioGenerator>().CreateDeathSound();
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers) {
            foreach (var material in renderer.materials) {
                material.shader = shader;
                material.SetFloat("_DissolveCutoff", 0);
                material.SetFloat("_DissolveAlphaSource", 1);
                material.EnableKeyword("_DISSOLVEALPHASOURCE_CUSTOM_MAP");
                material.SetTexture("_DissolveMap1", texture);
                material.SetColor("_DissolveEdgeColor", new Color(1, 0.7f, 0, 1));
                material.SetFloat("_DissolveEdgeWidth", 0.125f);
                material.SetFloat("_DissolveEdgeShape", 1);
                material.SetFloat("_DissolveEdgeColorIntensity", 1);
                material.SetFloat("_DissolveEdgeTextureSource", 3);
                material.EnableKeyword("_DISSOLVEEDGETEXTURESOURCE_CUSTOM");
                material.SetTexture("_DissolveEdgeTexture", gradientTexture);
                material.SetFloat("_SmoothnessTextureChannel", 1);
            }
        }
        var colliders = GetComponentsInChildren<Collider>();
        foreach (var collider in colliders) collider.enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        Destroy(GetComponent<Monster>().unitFrame);
        Destroy(gameObject, fadeOutTime);
    }

    public void CreateDeathSound() {
        GetComponent<ObjectSpawner>().CmdSpawnWithPosition(deathSound, transform.position, transform.rotation);
    }
}
