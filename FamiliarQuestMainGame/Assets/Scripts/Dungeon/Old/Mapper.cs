using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Mapper : MonoBehaviour {

    //public SyncListString blocks = new SyncListString();
    public List<string> blocks = new List<string>();
    public static List<Vector2> newHits = new List<Vector2>();
    public static List<string> newHitsItems = new List<string>();
    private float timer = 0;
    private readonly float checkFrequency = 0.1f;

    // Use this for initialization
	void Start () {
        for (int i = 0; i < 120 * 120; i++) blocks.Add(" ");
    }
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<PlayerCharacter>().isMe) {
            timer += Time.deltaTime;
            if (timer >= checkFrequency) CheckForMappables();
        }
	}

    public void Reset() {
        for (int i = 0; i < 120 * 120; i++) blocks[i] = " ";
        List<Vector2> newHits = new List<Vector2>();
        List<string> newHitsItems = new List<string>();
    }

private void CheckForMappables()
    {
        timer = 0;
        int layerMask = (1 << 10) + (1 << 11) + (1 << 12);
        var raycastPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        var hits = Physics.OverlapSphere(raycastPos, 32, layerMask);
        foreach (var hit in hits)
        {
            var mappable = hit.gameObject.GetComponent<Mappable>();
            if (mappable != null)
            {
                var hit2 = new RaycastHit();
                Physics.Raycast(raycastPos, hit.transform.position - raycastPos, out hit2, Mathf.Infinity, layerMask);
                if (hit2.transform == hit.transform) {
                    if (!mappable.unmappable) AddToMap(hit.transform, mappable.value, mappable);
                    else mappable.mapperRef = this;
                }
                Physics.Raycast(raycastPos - new Vector3(-0.25f, 0, -0.25f), hit.transform.position - raycastPos, out hit2, Mathf.Infinity, layerMask);
                if (hit2.transform == hit.transform) {
                    if (!mappable.unmappable) AddToMap(hit.transform, mappable.value, mappable);
                    else mappable.mapperRef = this;
                }
                Physics.Raycast(raycastPos - new Vector3(-0.25f, 0, 0.25f), hit.transform.position - raycastPos, out hit2, Mathf.Infinity, layerMask);
                if (hit2.transform == hit.transform) {
                    if (!mappable.unmappable) AddToMap(hit.transform, mappable.value, mappable);
                    else mappable.mapperRef = this;
                }
                Physics.Raycast(raycastPos - new Vector3(0.25f, 0, -0.25f), hit.transform.position - raycastPos, out hit2, Mathf.Infinity, layerMask);
                if (hit2.transform == hit.transform) {
                    if (!mappable.unmappable) AddToMap(hit.transform, mappable.value, mappable);
                    else mappable.mapperRef = this;
                }
                Physics.Raycast(raycastPos - new Vector3(0.25f, 0, 0.25f), hit.transform.position - raycastPos, out hit2, Mathf.Infinity, layerMask);
                if (hit2.transform == hit.transform) {
                    if (!mappable.unmappable) AddToMap(hit.transform, mappable.value, mappable);
                    else mappable.mapperRef = this;
                }
            }
        }
    }

    public void RemoveFromMap(Transform transform, Mappable mappable) {
        //var x = (int)((transform.position.x + 120 + mappable.xOffset) / 2);
        //var y = (int)((transform.position.z + 120 + mappable.yOffset) / 2);
        var x = (int)((transform.position.x + mappable.xOffset) / 5);
        var y = (int)((transform.position.z + mappable.yOffset) / 5);
        if (blocks[x + y * 120] != " ") CmdUpdateMap(x, y, " ");
    }

    private void AddToMap(Transform transform, string value, Mappable mappable)
    {
        //var x = (int)((transform.position.x + 120 + mappable.xOffset) / 2);
        //var y = (int)((transform.position.z + 120 + mappable.yOffset) / 2);
        var x = (int)((transform.position.x + mappable.xOffset) / 5);
        var y = (int)((transform.position.z + mappable.yOffset) / 5);
        if (blocks[x + y * 120] != value) CmdUpdateMap(x, y, value);
    }

    //[Command]
    public void CmdUpdateMap(int x, int y, string value) {
        if (blocks[x + y * 120] != value) {
            blocks[x + y * 120] = value;
            newHits.Add(new Vector2(x, y));
            newHitsItems.Add(value);
            RpcAddNewHit(x, y, value);
        }
    }

    //[ClientRpc]
    private void RpcAddNewHit(int x, int y, string value) {
        newHits.Add(new Vector2(x, y));
        newHitsItems.Add(value);
    }
}
