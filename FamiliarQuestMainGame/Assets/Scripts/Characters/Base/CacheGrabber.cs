using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CacheGrabber : MonoBehaviour {

    public List<GameObject> aoes = null;
    public List<GameObject> spellEffects = null;
    public List<GameObject> hitEffects = null;
    public List<GameObject> projectiles = null;
    public List<GameObject> damageZones = null;
    public List<Material> kittenFurCache = null;
    public List<Sprite> iconCache = null;
    public List<AudioClip> sounds = null;
    public Dictionary<string, AudioClip> soundsByName = null;
    public Dictionary<string, GameObject> statusEffects = null;
    public List<GameObject> rawStatusEffects = null;
    private bool initialized = false;
    private Dictionary<Element, int> dotSpellEffects = new Dictionary<Element, int>();

    void Start()
    {
        var data = TextReader.ReadSets("DotSpellEffects");
        foreach (var item in data) dotSpellEffects.Add(AbilityCalculator.StringToElement(item[0]), int.Parse(item[1]));
    }

    // Update is called once per frame
    void Update() {
        if (!initialized) {
            var config = GameObject.FindGameObjectWithTag("ConfigObject");
            if (config == null) return;
            aoes = config.GetComponent<AoeCache>().items;
            spellEffects = config.GetComponent<SpellEffectCache>().items;
            statusEffects = config.GetComponent<CharacterEffectCache>().items;
            rawStatusEffects = config.GetComponent<CharacterEffectCache>().backingItems;
            hitEffects = config.GetComponent<HitEffectCache>().items;
            projectiles = config.GetComponent<RangedObjectCache>().projectiles;
            sounds = config.GetComponent<SoundCache>().items;
            soundsByName = config.GetComponent<SoundCache>().itemsByName;
            damageZones = config.GetComponent<DamageZoneCache>().items;
            iconCache = config.GetComponent<IconCache>().icons;
            kittenFurCache = config.GetComponent<KittenFurCache>().skins;
            initialized = true;
        }
    }

    public GameObject GetStatusEffectVisual(string type, Character inflicter, ActiveAbility ability)
    {
        if (statusEffects.ContainsKey(type)) return Instantiate(statusEffects[type], transform);
        else if (type == "dot") return GetDotVisual((AttackAbility)(ability), inflicter);
        else return null;
    }

    private GameObject GetDotVisual(AttackAbility ability, Character inflicter)
    {
        if (dotSpellEffects.ContainsKey(ability.element)) return Instantiate(rawStatusEffects[dotSpellEffects[ability.element]]);
        return null;
    }
}
