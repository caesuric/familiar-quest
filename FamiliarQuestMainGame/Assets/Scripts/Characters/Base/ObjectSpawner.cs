using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class ObjectSpawner : DependencyUser {

    public GameObject floatingTextPrefab;
    public GameObject previouslySecretDoor;
    public new Camera camera;
    public RangedObjectCache rangedObjectCache = null;
    public HitEffectCache hitEffectCache = null;
    public AoeCache aoeCache = null;
    public DamageZoneCache damageZones = null;
    public CharacterEffectCache statusEffectCache = null;
    public SpellEffectCache spellEffectCache = null;
    public GameObject smell;

    public void CreateFloatingDamageText(int amount, string source, string target) {
        PlayerCharacter.players[0].GetComponent<ObjectSpawner>().CmdCreateFloatingTextWithPosition(amount.ToString(), Color.red, 90, source + " hits " + target + " for " + amount.ToString() + " damage.", transform.position);
    }

    public void CreateFloatingHealingText(int amount, string text) {
        PlayerCharacter.players[0].GetComponent<ObjectSpawner>().CmdCreateFloatingTextWithPosition(amount.ToString(), Color.green, 90, text, transform.position);
    }
    public void CreateFloatingTrapText(string str, string text) {
        PlayerCharacter.players[0].GetComponent<ObjectSpawner>().CmdCreateFloatingTextWithPosition(str, new Color(255, 165, 0), 90, text, transform.position);
    }

    public void CreateFloatingStatusText(string str, string text) {
        PlayerCharacter.players[0].GetComponent<ObjectSpawner>().CmdCreateFloatingTextWithPosition(str, Color.magenta, 90, text, transform.position);
    }

    public void CreateFloatingSaveText(string str, string target) {
        PlayerCharacter.players[0].GetComponent<ObjectSpawner>().CmdCreateFloatingTextWithPosition(str, Color.cyan, 90, target + " resisted!", transform.position);
    }

    public void CreateCriticalFloatingDamageText(int amount, string source, string target) {
        PlayerCharacter.players[0].GetComponent<ObjectSpawner>().CmdCreateFloatingTextWithPosition(amount.ToString(), Color.red, 180, "CRITICAL HIT! " + source + " hits " + target + " for " + amount.ToString() + " damage.", transform.position);
    }

    //[Command]
    public void CmdCreateFloatingText(string str, Color color, int size, string logText) {
        var text = Instantiate(floatingTextPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        var billboard = text.GetComponent<Billboard>();
        billboard.mainCamera = GetComponent<Camera>();
        var floatingText = text.GetComponent<FloatingText>();
        floatingText.text = str;
        floatingText.color = color;
        if (color == Color.magenta) text.transform.Translate(0, -1, 0);
        else if (color == Color.cyan) text.transform.Translate(0, -2, 0);
        floatingText.size = size;
        logText = AddColor(logText, color);
        GameLog.AddText(logText);
        //RpcCreateFloatingText(str, color, size, logText);
    }

    //[Command]
    public void CmdCreateFloatingTextWithPosition(string str, Color color, int size, string logText, Vector3 position) {
        var text = Instantiate(floatingTextPrefab, position, Quaternion.Euler(0, 0, 0));
        var billboard = text.GetComponent<Billboard>();
        billboard.mainCamera = GetComponent<Camera>();
        var floatingText = text.GetComponent<FloatingText>();
        floatingText.text = str;
        floatingText.color = color;
        if (color == Color.magenta) text.transform.Translate(0, -1, 0);
        else if (color == Color.cyan) text.transform.Translate(0, -2, 0);
        floatingText.size = size;
        logText = AddColor(logText, color);
        GameLog.AddText(logText);
        //RpcCreateFloatingText(str, color, size, logText);
    }

    private string AddColor(string text, Color color) {
        var colorText = "";
        if (color == Color.red) colorText = "red";
        else if (color == Color.cyan) colorText = "cyan";
        else if (color == Color.magenta) colorText = "magenta";
        else return text;
        return "<color=" + colorText + ">" + text + "</color>";
    }

    //[ClientRpc]
    public void RpcCreateFloatingText(string str, Color color, int size, string logText) {
        //if (NetworkServer.active) return;
        var text = Instantiate(floatingTextPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        var billboard = text.GetComponent<Billboard>();
        billboard.mainCamera = GetComponent<Camera>();
        var floatingText = text.GetComponent<FloatingText>();
        floatingText.text = str;
        floatingText.color = color;
        if (color == Color.magenta) text.transform.Translate(0, -1, 0);
        floatingText.size = size;
        GameLog.AddText(logText);
    }

    //[Command]
    public void CmdSpawn(GameObject obj)
    {
        if (obj == null) Debug.Log("Attempted to spawn null object");
        //else if (obj.GetComponent<NetworkIdentity>() == null) Debug.Log("Failed to spawn:" + obj.ToString());
        //else NetworkServer.Spawn(Instantiate(obj));
        else Instantiate(obj);
    }

    //[Command]
    public void CmdSpawnUnderParent(GameObject obj, GameObject parent)
    {
        if (obj == null) Debug.Log("Attempted to spawn null object");
        //else if (obj.GetComponent<NetworkIdentity>() == null) Debug.Log("Failed to spawn:" + obj.ToString());
        //else NetworkServer.Spawn(Instantiate(obj, parent.transform));
        else Instantiate(obj, parent.transform);
    }

    //[Command]
    public void CmdSpawnWithPosition(GameObject obj, Vector3 position, Quaternion rotation)
    {
        if (obj == null) Debug.Log("Attempted to spawn null object");
        //else if (obj.GetComponent<NetworkIdentity>() == null) Debug.Log("Failed to spawn: " + obj.ToString());
        //else NetworkServer.Spawn(Instantiate(obj, position, rotation));
        else Instantiate(obj, position, rotation);
    }

    internal void CreateFloatingImmunityText(string target, string source) {
        PlayerCharacter.players[0].GetComponent<ObjectSpawner>().CmdCreateFloatingTextWithPosition("IMMUNE", Color.red, 180, source + " hits " + target + ". " + target + " is IMMUNE.", transform.position);
    }
}
