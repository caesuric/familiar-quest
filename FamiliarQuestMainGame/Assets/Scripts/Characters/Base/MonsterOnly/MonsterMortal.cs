using UnityEngine;
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
            }
            if (PlayerCharacter.players.Count > 0 && killer != null) {
                var pc = killer.GetComponent<PlayerCharacter>();
                if (pc != null) {
                    pc.GainSpirits(GetComponent<SpiritUser>().spirits);
                    GetComponent<RewardGiver>().DropLoot(pc.GetComponent<Character>());
                }
            }
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
