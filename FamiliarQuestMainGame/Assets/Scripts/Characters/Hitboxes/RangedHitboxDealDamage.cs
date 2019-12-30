using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RangedHitboxDealDamage : MonoBehaviour {

    // Use this for initialization
    public GameObject projectile;
    public Character character = null;
    public string faction;
    public int damage = -1;
    public float dotDamage = 0;
    public float dotTime = 1;
    public float radius = 0;
    public List<AbilityAttribute> attributes = new List<AbilityAttribute>();
    public AttackAbility ability = null;
    public bool struck = false;
    public float fxDuration = 1f;

    private void Update() {
        //if (!NetworkServer.active) return;
        if (struck) fxDuration -= Time.deltaTime;
        //if (fxDuration <= 0) NetworkServer.Destroy(projectile);
        if (fxDuration <= 0) Destroy(projectile);
    }

    private void OnTriggerStay(Collider other) {
        //if (!NetworkServer.active || struck) return;
        if (struck) return;
        Damage.ProjectileAttack(character, other, ability, damage, radius, dotDamage, dotTime, attributes, projectile, faction);
    }
    public void MakeNoise(float volume)
    {
        if (character.GetComponent<PlayerCharacter>() != null)
        {
            character.GetComponent<SimulatedNoiseGenerator>().CmdMakeNoise(transform.position, volume);
        }
    }

    public void Initialize(Character character, int damage, List<AbilityAttribute> attributes, float radius, float dotDamage, float dotTime, string faction, AttackAbility ability) {
        this.character = character;
        this.damage = damage;
        this.attributes = attributes;
        this.radius = radius;
        this.dotDamage = dotDamage;
        this.dotTime = dotTime;
        this.faction = faction;
        this.ability = ability;
    }
}
