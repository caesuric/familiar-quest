using System.Collections.Generic;
using UnityEngine;

public class Hider : MonoBehaviour //: NetworkBehaviour
{
    public static Hider instance = null;

    private void Start() {
        instance = this;
    }

    // Update is called once per frame
    void Update() {
        {
            var items = Hideable.items;
            var pruneList = new List<Hideable>();
            foreach (var item in items) if (item == null || item.prune) pruneList.Add(item);
            foreach (var item in pruneList) {
                items.Remove(item);
                if (item != null && item is Monster) Destroy(item.gameObject, 0.1f);
            }
            foreach (var item in items) DisableItemIfNecessary(item);
        }
    }

    private void DisableItemIfNecessary(Hideable item) {
        var renderers = item.gameObject.GetComponentsInChildren<Renderer>();
        if (!IsSeen(item)) foreach (var renderer in renderers) renderer.enabled = false;
        else foreach (var renderer in renderers) renderer.enabled = true;
        var lights = item.gameObject.GetComponentsInChildren<Light>();
        if (!IsSeen(item)) foreach (var light in lights) light.enabled = false;
        else foreach (var light in lights) light.enabled = true;
    }

    public bool IsSeen(Hideable item) {
        if (item.GetComponent<GoblinRogue>() != null) {
            var gr = item.GetComponent<GoblinRogue>();
            if (gr.hidden) return false;
        }
        var renderers = item.gameObject.GetComponentsInChildren<Renderer>();
        int ignoreLayer;
        if (item.GetComponent<Monster>() != null) ignoreLayer = 8;
        else ignoreLayer = 11;
        foreach (var player in PlayerCharacter.players) if (IsSeenByPlayer(item, player, ignoreLayer)) return true;
        return false;
    }

    private bool IsSeenByPlayer(Hideable item, PlayerCharacter player, int ignoreLayer) {
        if (item == null) return false;
        if (item.isSecret) return true;
        if (RaycastCheck(player, item.transform.position, ignoreLayer)) return true;
        if (RaycastCheck(player, item.transform.position + new Vector3(-0.1f, 0, -0.1f), ignoreLayer)) return true;
        if (RaycastCheck(player, item.transform.position + new Vector3(-0.1f, 0, 0.1f), ignoreLayer)) return true;
        if (RaycastCheck(player, item.transform.position + new Vector3(0.1f, 0, -0.1f), ignoreLayer)) return true;
        if (RaycastCheck(player, item.transform.position + new Vector3(0.1f, 0, 0.1f), ignoreLayer)) return true;
        return false;
    }

    private bool RaycastCheck(PlayerCharacter player, Vector3 position, int ignoreLayer) {
        var rayDirection = player.transform.position - position;
        var hits = Physics.RaycastAll(position, rayDirection, Mathf.Infinity);
        foreach (var hit in hits) {
            if (hit.transform.gameObject.CompareTag("Wall") && Vector3.Distance(hit.transform.position, position) < Vector3.Distance(player.transform.position, position)) return false;
        }
        return true;
    }
}
