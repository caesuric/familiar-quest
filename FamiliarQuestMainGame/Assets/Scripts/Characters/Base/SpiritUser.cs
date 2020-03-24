using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

public class SpiritUser : DependencyUser {

    public List<Spirit> spirits = new List<Spirit>();
    public List<Ability> overflowAbilities = new List<Ability>();
    public float resurrectionTimer = 0;
    Dictionary<string, Maintainer> passiveMethods;
    private delegate void Maintainer(AbilityAttribute attribute);

    // Use this for initialization
    void Start() {
        dependencies = new List<string>() { "{{PLAYER_OR_MONSTER}}", "Character", "StatusEffectHost", "ObjectSpawner", "Health", "Mana" };
        Dependencies.Check(this);
        passiveMethods = new Dictionary<string, Maintainer>() {
            { "damageEnemiesOnScreen", (AbilityAttribute attribute) => MaintainDamageEnemiesOnScreen(attribute) }
        };
    }

    // Update is called once per frame
    void Update() {
        //if (NetworkServer.active) {
            resurrectionTimer = Mathf.Max(0, resurrectionTimer - Time.deltaTime);
            foreach (Spirit spirit in spirits) foreach (ActiveAbility ability in spirit.activeAbilities) if (ability != null) ability.currentCooldown = Mathf.Max(0, ability.currentCooldown - Time.deltaTime);
            MaintainPassives();
        //}
    }

    public float ModifyDamageForSpirits(float amount, Element type) {
        float currentDamage = amount;
        float modifier = 0;
        foreach (var spirit in spirits) foreach (var affinity in spirit.elements) if (affinity.type == type) modifier -= affinity.amount;
        currentDamage *= (1 + (modifier / 100));
        return currentDamage;
    }

    public void MaintainPassives() {
        foreach (var spirit in spirits) foreach (var passive in spirit.passiveAbilities) if (passive != null) foreach (var attribute in passive.attributes) if (passiveMethods.ContainsKey(attribute.type)) passiveMethods[attribute.type](attribute);
    }

    private void MaintainDamageEnemiesOnScreen(AbilityAttribute attribute) {
        //if (GetComponent<PlayerCharacter>() != null) {
            //foreach (var monster in Monster.monsters) if (Vector3.Distance(transform.position, monster.transform.position) < 20f && monster.GetComponent<MonsterCombatant>().InCombat()) monster.GetComponent<Health>().TakeDamage(attribute.FindParameter("degree").floatVal * Time.deltaTime, Element.none, GetComponent<Character>(), silent: true);
        //}
        //else if (GetComponent<MonsterCombatant>().InCombat()) foreach (var player in PlayerCharacter.players) if (Vector3.Distance(transform.position, player.transform.position) < 20f) player.GetComponent<Health>().TakeDamage(attribute.FindParameter("degree").floatVal * Time.deltaTime, Element.none, GetComponent<Character>(), silent: true);
    }

    public void RemovePassive(PassiveAbility ability) {
        if (ability == null) return;
        var character = GetComponent<Character>();
        foreach (var attribute in ability.attributes) {
            if (attribute.type=="boostStat") {
                //switch (attribute.FindParameter("stat").stringVal) {
                //    case "strength":
                //        character.strength -= attribute.FindParameter("degree").intVal;
                //        break;
                //    case "dexterity":
                //        character.dexterity -= attribute.FindParameter("degree").intVal;
                //        break;
                //    case "constitution":
                //        character.constitution -= attribute.FindParameter("degree").intVal;
                //        break;
                //    case "intelligence":
                //        character.intelligence -= attribute.FindParameter("degree").intVal;
                //        break;
                //    case "wisdom":
                //        character.wisdom -= attribute.FindParameter("degree").intVal;
                //        break;
                //    case "luck":
                //        character.luck -= attribute.FindParameter("degree").intVal;
                //        break;
                //}
                CharacterAttribute.attributes[attribute.FindParameter("stat").stringVal].instances[GetComponent<Character>()].BuffValue -= attribute.FindParameter("degree").intVal;
            }
        }
        character.CalculateAll();
    }

    public void AddPassive(PassiveAbility ability) {
        var character = GetComponent<Character>();
        foreach (var attribute in ability.attributes) {
            if (attribute.type == "boostStat") {
                //switch (attribute.FindParameter("stat").stringVal) {
                //    case "strength":
                //        character.strength += attribute.FindParameter("degree").intVal;
                //        break;
                //    case "dexterity":
                //        character.dexterity += attribute.FindParameter("degree").intVal;
                //        break;
                //    case "constitution":
                //        character.constitution += attribute.FindParameter("degree").intVal;
                //        break;
                //    case "intelligence":
                //        character.intelligence += attribute.FindParameter("degree").intVal;
                //        break;
                //    case "wisdom":
                //        character.wisdom += attribute.FindParameter("degree").intVal;
                //        break;
                //    case "luck":
                //        character.luck += attribute.FindParameter("degree").intVal;
                //        break;
                //}
                CharacterAttribute.attributes[attribute.FindParameter("stat").stringVal].instances[GetComponent<Character>()].BuffValue += attribute.FindParameter("degree").intVal;
            }
        }
        character.CalculateAll();
    }

    //private void MaintainHotPassive() {
    //    GetComponent<Health>().hp = Mathf.Min(GetComponent<Health>().hp + (Time.deltaTime), GetComponent<Health>().maxHP);
    //}

    //private void MaintainMpOverTimePassive() {
    //    GetComponent<Mana>().mp = Mathf.Min(GetComponent<Mana>().mp + (Time.deltaTime), GetComponent<Mana>().maxMP);
    //}

    //private void MaintainFindSecretDoorsPassive() {
    //    if (GetComponent<PlayerCharacter>() != null) FindSecretDoors();
    //}

    //public void FindSecretDoors() {
    //    foreach (var item in Hideable.items) CheckHideable(item);
    //}

    //private void CheckHideable(Hideable item) {
    //    if (item == null) return;
    //    var hit = RaycastHideable(item);
    //    //if (item != null && hit.transform != null && hit.transform.gameObject.CompareTag("Wall") && hit.transform.gameObject.GetComponent<Door>() != null && hit.transform.gameObject.GetComponent<Door>().isSecret) {
    //    //    GetComponent<ObjectSpawner>().CmdSpawnWithPosition(GetComponent<ObjectSpawner>().previouslySecretDoor, hit.transform.position, hit.transform.rotation);
    //    //    NetworkServer.Destroy(hit.transform.gameObject);
    //    //}
    //    //else if (item != null && hit.transform != null && hit.transform.gameObject.CompareTag("TrapTrigger")) {
    //    //    var mesh = hit.transform.gameObject.GetComponent<MeshRenderer>();
    //    //    mesh.enabled = true;
    //    //    RpcEnableMesh(hit.transform.gameObject);
    //    //}
    //    if (item != null && hit.transform != null && hit.transform.gameObject.CompareTag("TrapTrigger")) {
    //        var mesh = hit.transform.gameObject.GetComponent<MeshRenderer>();
    //        mesh.enabled = true;
    //        RpcEnableMesh(hit.transform.gameObject);
    //    }
    //}

    //private RaycastHit RaycastHideable(Hideable item) {
    //    RaycastHit hit;
    //    var rayDirection = item.transform.position - transform.position;
    //    int layerMask = 1 << 9;
    //    layerMask = ~layerMask;
    //    Physics.Raycast(transform.position, rayDirection, out hit, maxDistance: Mathf.Infinity, layerMask: layerMask);
    //    return hit;
    //}

    public bool HasPassive(string type) {
        foreach (var spirit in spirits) foreach (var passive in spirit.passiveAbilities) if (passive != null) foreach (var attribute in passive.attributes) if (attribute.type == type) return true;
        return false;
    }

    //[ClientRpc]
    //public void RpcEnableMesh(GameObject go) {
    //    if (go == null) return;
    //    var mesh = go.GetComponent<MeshRenderer>();
    //    if (mesh == null) return;
    //    mesh.enabled = true;
    //}
}
